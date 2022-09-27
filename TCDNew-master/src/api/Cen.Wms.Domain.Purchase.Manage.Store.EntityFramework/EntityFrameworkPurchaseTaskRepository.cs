using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.Data.Sql.Extensions;
using Cen.Common.Errors;
using Cen.Wms.Data.Context;
using Cen.Wms.Data.Models.Purchase;
using Cen.Wms.Data.Models.Purchase.Enums;
using Cen.Wms.Domain.Purchase.Abstract;
using Cen.Wms.Domain.Purchase.Models;
using Cen.Wms.Domain.Sync.Abstract;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace Cen.Wms.Domain.Purchase.Manage.Store.EntityFramework
{
    public class EntityFrameworkPurchaseTaskRepository: IPurchaseTaskRepository
    {
        private readonly IClock _clock;
        private readonly IDbConnection _connection;
        private readonly WmsContext _wmsContext;

        public EntityFrameworkPurchaseTaskRepository(IClock clock, IDbConnection connection, WmsContext wmsContext)
        {
            _clock = clock;
            _connection = connection;
            _wmsContext = wmsContext;
        }

        public async Task<RpcResponse<Guid>> PurchaseTaskCreateEmpty(Guid facilityId, Guid userId, bool isPubliclyAvailable)
        {
            var changeInstant = _clock.GetCurrentInstant();
            var purchaseTaskId = NewId.NextGuid();
            var purchaseTaskCodeSeqVal = _connection.SequenceNextval(PurchaseTaskHeadRow.ClassSequenceName);
            
            var purchaseTaskHeadRow = new PurchaseTaskHeadRow();
            purchaseTaskHeadRow.Id = purchaseTaskId;
            purchaseTaskHeadRow.Code = purchaseTaskCodeSeqVal.ToString();
            purchaseTaskHeadRow.FacilityId = facilityId;
            purchaseTaskHeadRow.CreatedByUserId = userId;
            purchaseTaskHeadRow.CreatedAt = changeInstant;
            purchaseTaskHeadRow.ChangedAt = changeInstant;
            purchaseTaskHeadRow.IsPubliclyAvailable = isPubliclyAvailable;

            _wmsContext.Add(purchaseTaskHeadRow);
            
            return RpcResponse<Guid>.WithSuccess(purchaseTaskId);
        }

        public async Task<RpcResponse<bool>> PurchaseTaskPacInclude(Guid purchaseTaskId, PacHeadEditModel pacHeadEditModel)
        {
            var changeInstant = _clock.GetCurrentInstant();
            var purchaseTask = await _wmsContext.PurchaseTaskHead
                .Include(e => e.Lines)
                .Include(e => e.Pallets)
                .Include(e => e.PacHeads)
                .FirstOrDefaultAsync(e => e.Id == purchaseTaskId);
            
            if (purchaseTask == null)
                return RpcResponse<bool>.WithError(false, CommonErrors.NotFound("задание"));
            
            if (purchaseTask.PurchaseTaskState != PurchaseTaskState.Created && purchaseTask.PurchaseTaskState != PurchaseTaskState.InProgress)
                return RpcResponse<bool>.WithError(false, CommonErrors.InvalidOperation);
            
            var isPurchaseTaskPacHeadExists = purchaseTask.PacHeads
                .Any(e => e.PurchaseTaskHeadId == purchaseTaskId && e.PacHeadId == pacHeadEditModel.Id);

            if (!isPurchaseTaskPacHeadExists)
            {
                var purchaseTaskPacHeadId = NewId.NextGuid();
                var purchaseTaskPacHeadRow = new PurchaseTaskPacHeadRow
                {
                    Id = purchaseTaskPacHeadId, 
                    PurchaseTaskHeadId = purchaseTaskId, 
                    PacHeadId = pacHeadEditModel.Id
                };
                await _wmsContext.PurchaseTaskPacHead.AddAsync(purchaseTaskPacHeadRow);
                await _wmsContext.SaveChangesAsync();
                
                foreach (var pacLineEditModel in pacHeadEditModel.Lines)
                {
                    var purchaseTaskLineRow =
                        purchaseTask.Lines.FirstOrDefault(e => e.ProductExtId == pacLineEditModel.ProductId);
                    if (purchaseTaskLineRow == null)
                    {
                        var purchaseTaskLineRowId = NewId.NextGuid(); 
                        purchaseTaskLineRow = new PurchaseTaskLineRow
                        {
                            Id = purchaseTaskLineRowId,
                            PurchaseTaskHeadId = purchaseTaskId,
                            Quantity = pacLineEditModel.QtyExpected,
                            ProductExtId = pacLineEditModel.ProductId,
                            ProductName = pacLineEditModel.ProductName,
                            ProductBarcodes = pacLineEditModel.ProductBarcodes.ToArray(),
                            ProductAbc = pacLineEditModel.ProductAbc,
                            ChangedAt = changeInstant
                        };
                        await _wmsContext.PurchaseTaskLine.AddAsync(purchaseTaskLineRow);
                        await _wmsContext.SaveChangesAsync();

                        var purchaseTaskLineStateRow = new PurchaseTaskLineStateRow
                        {
                            Id = NewId.NextGuid(),
                            PurchaseTaskLineId = purchaseTaskLineRowId,
                            ChangedAt = changeInstant
                        };
                        await _wmsContext.PurchaseTaskLineState.AddAsync(purchaseTaskLineStateRow);
                    }
                    else
                    {
                        purchaseTaskLineRow.Quantity += pacLineEditModel.QtyExpected;
                        purchaseTaskLineRow.ChangedAt = changeInstant;
                        
                        _wmsContext.PurchaseTaskLine.Update(purchaseTaskLineRow);
                    }
                }

                purchaseTask.ChangedAt = changeInstant;
                _wmsContext.PurchaseTaskHead.Update(purchaseTask);
            }

            return RpcResponse<bool>.WithSuccess(true);
        }

        public async Task<RpcResponse<bool>> PurchaseTaskPacExclude(Guid purchaseTaskId, PacHeadEditModel pacHeadEditModel)
        {
            var changeInstant = _clock.GetCurrentInstant();
            var purchaseTask = await _wmsContext.PurchaseTaskHead
                .Include(e => e.Lines)
                .Include(e => e.Pallets)
                .Include(e => e.PacHeads)
                .FirstOrDefaultAsync(e => e.Id == purchaseTaskId);

            if (purchaseTask == null)
                return RpcResponse<bool>.WithError(false, CommonErrors.NotFound("задание"));
            
            if (purchaseTask.PurchaseTaskState != PurchaseTaskState.Created)
                return RpcResponse<bool>.WithError(false, CommonErrors.InvalidOperation);

            var purchaseTaskPacHead =
                purchaseTask.PacHeads
                    .FirstOrDefault(e => e.PurchaseTaskHeadId == purchaseTaskId && e.PacHeadId == pacHeadEditModel.Id);
            
            if (purchaseTaskPacHead != null)
            {
                _wmsContext.PurchaseTaskPacHead.Remove(purchaseTaskPacHead);
                
                foreach (var pacLineEditModel in pacHeadEditModel.Lines)
                {
                    var purchaseTaskLineRow =
                        purchaseTask.Lines.FirstOrDefault(e => e.ProductExtId == pacLineEditModel.ProductId);
                    
                    // ReSharper disable once PossibleNullReferenceException
                    purchaseTaskLineRow.Quantity -= pacLineEditModel.QtyExpected;
                    purchaseTaskLineRow.ChangedAt = changeInstant;
                    if (purchaseTaskLineRow.Quantity != 0)
                    {
                        _wmsContext.PurchaseTaskLine.Update(purchaseTaskLineRow);                        
                    }
                    else
                    {
                        _wmsContext.PurchaseTaskLine.Remove(purchaseTaskLineRow);
                    }
                }
                
                purchaseTask.ChangedAt = changeInstant;
                _wmsContext.PurchaseTaskHead.Update(purchaseTask);
            }
            
            return RpcResponse<bool>.WithSuccess(true);
        }
        
        public async Task<RpcResponse<bool>> PurchaseTaskUserInclude(Guid purchaseTaskId, Guid userId)
        {
            var changeInstant = _clock.GetCurrentInstant();
            var purchaseTask = await _wmsContext.PurchaseTaskHead
                .Include(e => e.Users)
                .FirstOrDefaultAsync(e => e.Id == purchaseTaskId);

            if (purchaseTask == null)
                return RpcResponse<bool>.WithError(false, CommonErrors.NotFound("задание"));
            
            if (purchaseTask.PurchaseTaskState != PurchaseTaskState.Created && purchaseTask.PurchaseTaskState != PurchaseTaskState.InProgress)
                return RpcResponse<bool>.WithError(false, CommonErrors.InvalidOperation);
            
            var isPurchaseTaskUserExists = purchaseTask.Users
                .Any(e => e.PurchaseTaskHeadId == purchaseTaskId && e.UserId == userId);

            if (!isPurchaseTaskUserExists)
            {
                var purchaseTaskUserId = NewId.NextGuid();
                var purchaseTaskUserRow = new PurchaseTaskUserRow
                {
                    Id = purchaseTaskUserId, 
                    PurchaseTaskHeadId = purchaseTaskId, 
                    UserId = userId
                };
                await _wmsContext.PurchaseTaskUser.AddAsync(purchaseTaskUserRow);
                await _wmsContext.SaveChangesAsync();

                purchaseTask.ChangedAt = changeInstant;
                _wmsContext.PurchaseTaskHead.Update(purchaseTask);
            }

            return RpcResponse<bool>.WithSuccess(true);
        }

        public async Task<RpcResponse<bool>> PurchaseTaskUserExclude(Guid purchaseTaskId, Guid userId)
        {
            var changeInstant = _clock.GetCurrentInstant();
            var purchaseTask = await _wmsContext.PurchaseTaskHead
                .Include(e => e.Users)
                .FirstOrDefaultAsync(e => e.Id == purchaseTaskId);

            if (purchaseTask == null)
                return RpcResponse<bool>.WithError(false, CommonErrors.NotFound("задание"));
            
            if  (purchaseTask.PurchaseTaskState != PurchaseTaskState.Created && purchaseTask.PurchaseTaskState != PurchaseTaskState.InProgress)
                return RpcResponse<bool>.WithError(false, CommonErrors.InvalidOperation);
            
            var purchaseTaskUserRow =
                purchaseTask.Users
                    .FirstOrDefault(e => e.PurchaseTaskHeadId == purchaseTaskId && e.UserId == userId);
            
            if (purchaseTaskUserRow != null)
            {
                _wmsContext.PurchaseTaskUser.Remove(purchaseTaskUserRow);

                purchaseTask.ChangedAt = changeInstant;
                _wmsContext.PurchaseTaskHead.Update(purchaseTask);
            }
            
            return RpcResponse<bool>.WithSuccess(true);
        }

        public async Task<RpcResponse<bool>> PurchaseTaskStart(Guid purchaseTaskId)
        {
            var changeInstant = _clock.GetCurrentInstant();
            var purchaseTaskHeadRow = await _wmsContext.PurchaseTaskHead.FirstOrDefaultAsync(e => e.Id == purchaseTaskId);
            if (purchaseTaskHeadRow == null)
                return RpcResponse<bool>.WithError(false, CommonErrors.NotFound("задание"));
            
            if (purchaseTaskHeadRow.PurchaseTaskState != PurchaseTaskState.Created)
                return RpcResponse<bool>.WithError(false, CommonErrors.InvalidOperation);

            purchaseTaskHeadRow.PurchaseTaskState = PurchaseTaskState.InProgress;
            purchaseTaskHeadRow.ChangedAt = changeInstant;
            _wmsContext.PurchaseTaskHead.Update(purchaseTaskHeadRow);

            return RpcResponse<bool>.WithSuccess(true);
        }

        public async Task<RpcResponse<bool>> PurchaseTaskCancel(Guid purchaseTaskId)
        {
            var changeInstant = _clock.GetCurrentInstant();
            var purchaseTaskHeadRow = await _wmsContext.PurchaseTaskHead
                .Include(e => e.PacHeads)
                .ThenInclude(e => e.PacHead)
                .ThenInclude(e => e.PacState)
                .FirstOrDefaultAsync(e => e.Id == purchaseTaskId);
            
            if (purchaseTaskHeadRow == null)
                return RpcResponse<bool>.WithError(false, CommonErrors.NotFound("задание"));
            
            if (purchaseTaskHeadRow.PurchaseTaskState != PurchaseTaskState.Created && purchaseTaskHeadRow.PurchaseTaskState != PurchaseTaskState.InProgress)
                return RpcResponse<bool>.WithError(false, CommonErrors.InvalidOperation);

            foreach (var purchaseTaskPacHeadRow in purchaseTaskHeadRow.PacHeads)
            {
                purchaseTaskPacHeadRow.PacHead.ResponsibleUserId = null;
                purchaseTaskPacHeadRow.PacHead.PacState.IsBusy = false;
                purchaseTaskPacHeadRow.PacHead.PacState.IsProcessed = false;
                _wmsContext.PacState.Update(purchaseTaskPacHeadRow.PacHead.PacState);
            }
            
            purchaseTaskHeadRow.PurchaseTaskState = PurchaseTaskState.Cancelled;
            purchaseTaskHeadRow.ChangedAt = changeInstant;
            _wmsContext.PurchaseTaskHead.Update(purchaseTaskHeadRow);

            return RpcResponse<bool>.WithSuccess(true);
        }

        public async Task<RpcResponse<Guid>> PurchaseTaskGetResponsibleUserId(Guid purchaseTaskId)
        {
            var responsibleUserId = await _wmsContext.PurchaseTaskHead
                .Where(e => e.Id == purchaseTaskId)
                .Select(e => e.CreatedByUserId)
                .FirstOrDefaultAsync();

            return RpcResponse<Guid>.WithSuccess(responsibleUserId);
        }

        public async Task<RpcResponse<bool>> PurchaseTaskStop(Guid purchaseTaskId)
        {
            var changeInstant = _clock.GetCurrentInstant();
            var purchaseTaskHeadRow = await PurchaseTaskRowReadById(purchaseTaskId);
            if (!purchaseTaskHeadRow.IsSuccess)
                return RpcResponse<bool>.WithErrors(false, purchaseTaskHeadRow.Errors);

            if (purchaseTaskHeadRow.Data.PurchaseTaskState != PurchaseTaskState.InProgress)
                return RpcResponse<bool>.WithError(false, CommonErrors.InvalidOperation);

            return await PurchaseTaskStop(purchaseTaskHeadRow.Data, changeInstant);
        }
        
        private async Task<RpcResponse<bool>> PurchaseTaskStop(PurchaseTaskHeadRow purchaseTaskHeadRow, Instant changeInstant)
        {
            await PurchaseTaskSplitByPacs(changeInstant, purchaseTaskHeadRow);
            foreach (var purchaseTaskPacHeadRow in purchaseTaskHeadRow.PacHeads)
            {
                purchaseTaskPacHeadRow.PacHead.PacState.IsProcessed = true;
                _wmsContext.PacState.Update(purchaseTaskPacHeadRow.PacHead.PacState);
            }
            
            purchaseTaskHeadRow.PurchaseTaskState = PurchaseTaskState.Processed;
            _wmsContext.PurchaseTaskHead.Update(purchaseTaskHeadRow);

            return RpcResponse<bool>.WithSuccess(true);
        }

        public async Task<RpcResponse<bool>> PurchaseTaskUpload(Guid purchaseTaskId, IPacUploader pacUploader)
        {
            var purchaseTaskHeadRow = await PurchaseTaskRowReadById(purchaseTaskId);
            if (!purchaseTaskHeadRow.IsSuccess)
                return RpcResponse<bool>.WithErrors(false, purchaseTaskHeadRow.Errors);
            
            if (purchaseTaskHeadRow.Data.PurchaseTaskState != PurchaseTaskState.Processed)
                return RpcResponse<bool>.WithError(false, CommonErrors.InvalidOperation);
            
            return await PurchaseTaskUpload(purchaseTaskHeadRow.Data, pacUploader);
        }
        
        private async Task<RpcResponse<bool>> PurchaseTaskUpload(PurchaseTaskHeadRow purchaseTaskHeadRow, IPacUploader pacUploader)
        {
            var pacHeadRowList = purchaseTaskHeadRow.PacHeads.Select(e => e.PacHead).ToArray(); 
            var uploadPacResult = await pacUploader.Upload(pacHeadRowList, purchaseTaskHeadRow.Code);
            if (!uploadPacResult.IsSuccess)
                return RpcResponse<bool>.WithErrors(false, uploadPacResult.Errors);
            
            foreach (var pacHeadRow in pacHeadRowList)
            {
                pacHeadRow.PacState.IsExported = true;
                _wmsContext.PacState.Update(pacHeadRow.PacState);
            }
            purchaseTaskHeadRow.IsExported = true;
            _wmsContext.PurchaseTaskHead.Update(purchaseTaskHeadRow);
            
            await _wmsContext.SaveChangesAsync();
            
            return RpcResponse<bool>.WithSuccess(true);
        }

        public async Task<RpcResponse<bool>> PurchaseTaskStopAndUpload(Guid purchaseTaskId, IPacUploader pacUploader)
        {
            var changeInstant = _clock.GetCurrentInstant();
            var purchaseTaskHeadRow = await PurchaseTaskRowReadById(purchaseTaskId);
            if (!purchaseTaskHeadRow.IsSuccess)
                return RpcResponse<bool>.WithErrors(false, purchaseTaskHeadRow.Errors);
            
            if (purchaseTaskHeadRow.Data.PurchaseTaskState != PurchaseTaskState.InProgress)
                return RpcResponse<bool>.WithError(false, CommonErrors.InvalidOperation);

            var purchaseTaskStopResult = await PurchaseTaskStop(purchaseTaskHeadRow.Data, changeInstant);
            if (!purchaseTaskStopResult.IsSuccess)
                return RpcResponse<bool>.WithErrors(false, purchaseTaskStopResult.Errors);
            
            return await PurchaseTaskUpload(purchaseTaskHeadRow.Data, pacUploader);
        }
        
        private async Task<RpcResponse<PurchaseTaskHeadRow>> PurchaseTaskRowReadById(Guid purchaseTaskId)
        {
            var purchaseTaskHeadRow = await _wmsContext.PurchaseTaskHead
                .Include(e => e.Lines)
                .ThenInclude(e => e.PurchaseTaskLinePalletedStates)
                .Include(e => e.PacHeads)
                .ThenInclude(e => e.PacHead)
                .ThenInclude(e => e.ResponsibleUser)
                .Include(e => e.PacHeads)
                .ThenInclude(e => e.PacHead)
                .ThenInclude(e => e.PacState)
                .Include(e => e.PacHeads)
                .ThenInclude(e => e.PacHead)
                .ThenInclude(e => e.Lines)
                .ThenInclude(e => e.PacLineStates)
                .FirstOrDefaultAsync(e => e.Id == purchaseTaskId);
            
            if (purchaseTaskHeadRow == null)
                return RpcResponse<PurchaseTaskHeadRow>.WithError(null, CommonErrors.NotFound("задание"));
            
            return RpcResponse<PurchaseTaskHeadRow>.WithSuccess(purchaseTaskHeadRow);
        }
        
        private async Task PurchaseTaskSplitByPacs(Instant changeInstant, PurchaseTaskHeadRow purchaseTaskHeadRow)
        {
            // читаем данные приходов и данные о размещении на паллетах
            var taskLinesData = 
                purchaseTaskHeadRow.Lines
                    .SelectMany(
                        e => e.PurchaseTaskLinePalletedStates,
                        (row, stateRow) => new { row, stateRow }
                    )
                    .ToList()
                    .OrderBy(e => e.row.ProductExtId)
                    .ThenBy(e => e.stateRow.PalletCode)
                    .ToList();
            var pacLinesData = 
                purchaseTaskHeadRow.PacHeads
                    .Select(e => e.PacHead)
                    .SelectMany(e => e.Lines)
                    .ToList()
                    .OrderBy(e => e.ProductId)
                    .ThenBy(e => e.ExtId)
                    .ToList();
            
            // указатель текущей позиции курсора (sqflite не умеет курсоры)
            var taskLineNumber = 0;
            var pacLineNumber = 0;

            // состояние текущей позиции разноски товаров (для работы алгоритма)
            string pacProductId = string.Empty;
            decimal pacQty = 0;
            string palletProductId = string.Empty;
            decimal palletQty = 0;
            
            // состояние текущей позиции разноски товаров (для данных, которые будут скопированы)
            PacLineRow currentPacLine = null;
            decimal quantityNormalConfirmed = 0;
            decimal quantityBrokenConfirmed = 0;
            LocalDate? expirationDate = null;
            int expirationDaysPlus = 0;
            string palletCode = string.Empty;
            
            do
            {
                var compareResult = 0;
                if (pacProductId != palletProductId) {
                    compareResult = string.Compare(pacProductId, palletProductId, StringComparison.InvariantCultureIgnoreCase);
                }
                var biZero = (pacQty == 0) && (palletQty == 0);
                var biNonZero = (pacQty != 0) && (palletQty != 0);
                
                // курсор закупки надо прочитать и сдвинуть вперед если:
                // оба количества равны нулю и равны между собой
                // оба количества не равны нулю и не равны между собой, и код товара закупок меньше кода товара паллет
                // кол-во в закупке равно 0
                if ((biZero && compareResult == 0) || (biNonZero && compareResult == -1) || (pacQty == 0))
                {
                    if (pacLineNumber < pacLinesData.Count) {
                        currentPacLine = pacLinesData[pacLineNumber];
                        pacProductId = currentPacLine.ProductId;
                        pacQty = currentPacLine.QtyExpected;

                        pacLineNumber++;
                    }
                    else {
                        break;
                    }
                }
                
                // курсор паллеты надо прочитать и сдвинуть вперед если:
                // оба количества равны нулю и равны между собой
                // оба количества не равны нулю и не равны между собой, и код товара закупок больше кода товара паллет
                // кол-во в закупке равно 0
                if ((biZero && compareResult == 0) || (biNonZero && compareResult == 1) || (palletQty == 0))
                {
                    if (taskLineNumber < taskLinesData.Count) {
                        var currentTaskLine = taskLinesData[taskLineNumber];
                        palletProductId = currentTaskLine.row.ProductExtId;
                        palletQty = currentTaskLine.stateRow.QtyNormal + currentTaskLine.stateRow.QtyBroken;
                        quantityNormalConfirmed = currentTaskLine.stateRow.QtyNormal;
                        quantityBrokenConfirmed = currentTaskLine.stateRow.QtyBroken;
                        expirationDate = currentTaskLine.stateRow.ExpirationDate;
                        expirationDaysPlus = currentTaskLine.stateRow.ExpirationDaysPlus;
                        palletCode = currentTaskLine.stateRow.PalletCode;

                        taskLineNumber++;
                    }
                    else {
                        break;
                    }
                }
                
                // если в обоих курсорах один и тот же товар, и ненулевые количества
                // то минимальное из количеств мы можем добавить в разноску
                if ((pacProductId == palletProductId) && (pacQty != 0 && palletQty != 0))
                {
                    var qtyMin = Math.Min(pacQty, palletQty);
                    var qtyNormalMin = Math.Min(qtyMin, quantityNormalConfirmed);
                    var qtyBrokenMin = Math.Min(qtyMin - qtyNormalMin, quantityBrokenConfirmed);

                    var pacLineStateRow = new PacLineStateRow
                    {
                        Id = NewId.NextGuid(),
                        ChangedAt = changeInstant,
                        QtyNormal = qtyNormalMin,
                        QtyBroken = qtyBrokenMin,
                        ExpirationDate = expirationDate,
                        ExpirationDaysPlus = expirationDaysPlus,
                        PalletCode = palletCode,
                        PacLine = currentPacLine
                    };
                    currentPacLine.PacLineStates.Add(pacLineStateRow);
                    await _wmsContext.PacLineState.AddAsync(pacLineStateRow);
                    _wmsContext.PacLine.Update(currentPacLine);
                    await _wmsContext.SaveChangesAsync();
                    
                    pacQty -= qtyMin;
                    quantityNormalConfirmed -= qtyNormalMin;
                    quantityBrokenConfirmed -= qtyBrokenMin;
                    palletQty = quantityNormalConfirmed + quantityBrokenConfirmed;
                }
            } while (true);

            foreach (var pacLineRow in pacLinesData.Where(pacLineRow => pacLineRow.PacLineStates.Count > 1))
            {
                LocalDate? worstDate = null;
                LocalDate? stateExpirationDate = null;
                int stateExpirationDaysPlus = 0;
                
                foreach (var pacLineStateRow in pacLineRow.PacLineStates)
                {
                    var currentPalletDate = pacLineStateRow.ExpirationDate?.PlusDays(pacLineStateRow.ExpirationDaysPlus);
                    var newWorstDate =
                        currentPalletDate != null
                            ? worstDate == null
                                ? currentPalletDate
                                : LocalDate.Min(currentPalletDate.Value, worstDate.Value)
                            : worstDate;
                    if (newWorstDate != worstDate)
                    {
                        worstDate = newWorstDate;
                        stateExpirationDate = pacLineStateRow.ExpirationDate;
                        stateExpirationDaysPlus = pacLineStateRow.ExpirationDaysPlus;
                    }
                }

                foreach (var pacLineStateRow in pacLineRow.PacLineStates)
                {
                    pacLineStateRow.ExpirationDate = stateExpirationDate;
                    pacLineStateRow.ExpirationDaysPlus = stateExpirationDaysPlus;
                    _wmsContext.PacLineState.Update(pacLineStateRow);
                    await _wmsContext.SaveChangesAsync();
                }
            }

            foreach (var pacLineRow in pacLinesData.Where(pacLineRow => pacLineRow.PacLineStates.Count == 0))
            {
                var pacLineStateRow = new PacLineStateRow { Id = NewId.NextGuid(), ChangedAt = changeInstant, PacLine = pacLineRow };
                pacLineRow.PacLineStates.Add(pacLineStateRow);
                await _wmsContext.PacLineState.AddAsync(pacLineStateRow);
                _wmsContext.PacLine.Update(pacLineRow);
                await _wmsContext.SaveChangesAsync();
            }
        }
    }
}