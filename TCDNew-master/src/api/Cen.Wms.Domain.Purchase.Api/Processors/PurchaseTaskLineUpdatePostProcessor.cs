using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Data.EntityFramework;
using Cen.Common.Errors;
using Cen.Wms.Data.Context;
using Cen.Wms.Data.Models.Purchase;
using Cen.Wms.Data.Models.Purchase.Enums;
using Cen.Wms.Domain.Facility.Config.Enums;
using Cen.Wms.Domain.Purchase.Api.Dtos;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Serilog;

namespace Cen.Wms.Domain.Purchase.Api.Processors
{
    public class PurchaseTaskLineUpdatePostProcessor : IQueryProcessor<PurchaseTaskLineUpdatePostReq, RpcResponse<bool>>
    {
        private readonly ILogger _logger;
        private readonly IClock _clock;
        private readonly IMapper _mapper;
        private readonly UnitOfWork<WmsContext> _unitOfWork;
        private readonly WmsContext _wmsContext;

        public PurchaseTaskLineUpdatePostProcessor(IClock clock, IMapper mapper, UnitOfWork<WmsContext> unitOfWork, WmsContext wmsContext, ILogger logger)
        {
            _clock = clock;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _wmsContext = wmsContext;
            _logger = logger;
        }
        public async Task<RpcResponse<bool>> Run(IUserIdProvider userIdProvider, PurchaseTaskLineUpdatePostReq request)
        {
            var changedInstant = _clock.GetCurrentInstant();
            var purchaseTask = await _wmsContext.PurchaseTaskHead
                .Include(e => e.Users)
                .Include(e => e.Pallets)
                .Include(e => e.Lines)
                .ThenInclude(e => e.PurchaseTaskLineState)
                .Include(e => e.Lines)
                .ThenInclude(e => e.PurchaseTaskLinePalletedStates)
                .Include(e => e.PacHeads)
                .ThenInclude(e => e.PacHead)
                .AsSplitQuery()
                .FirstOrDefaultAsync(e => e.Id == request.PurchaseTaskId);
            if (purchaseTask == null)
                return RpcResponse<bool>.WithError(false, CommonErrors.NotFound("задание"));
            if (purchaseTask.PurchaseTaskState != PurchaseTaskState.InProgress)
                return RpcResponse<bool>.WithError(false, CommonErrors.InvalidOperation);

            var userIsIncludedInTask = purchaseTask.Users.Any(e => e.UserId == userIdProvider.UserGuid);
            if (!(purchaseTask.IsPubliclyAvailable || userIsIncludedInTask))
                return RpcResponse<bool>.WithError(false, CommonErrors.AccessDenied);
            
            var purchaseTaskLine = purchaseTask.Lines.FirstOrDefault(e => e.Id == request.PurchaseTaskLineUpdate.PurchaseTaskLineId);
            if (purchaseTaskLine == null)
                return RpcResponse<bool>.WithError(false, CommonErrors.NotFound("строка задания"));

            var quantityIsExceededResult = PurchaseTaskLineQuantityIsExceeded(purchaseTaskLine, request.PurchaseTaskLineUpdate);
            if (quantityIsExceededResult)
                return RpcResponse<bool>.WithError(false, PurchaseErrors.PurchaseTaskLineQuantityIsExceeded);
            
            PurchaseTaskUpdateStartInstant(purchaseTask, changedInstant);
            PurchaseTaskUpdateChangedAtInstant(purchaseTask, changedInstant);
            await PurchaseTaskLineUpdateStore(userIdProvider, request, purchaseTask);

            var facilityConfig = await _wmsContext.FacilityConfig.FirstAsync(e => e.Id == purchaseTask.FacilityId);
            var isAcceptanceProcessTypeIsPalletized = facilityConfig.AcceptanceProcessType == AcceptanceProcessType.Palletized;
            if (isAcceptanceProcessTypeIsPalletized)
            {
                var purchaseTaskPallet = purchaseTask.Pallets.FirstOrDefault(e => e.Code == request.CurrentPalletCode);
                if (purchaseTaskPallet == null)
                {
                    purchaseTaskPallet = await PurchaseTaskPalletCreate(request, purchaseTask, purchaseTaskLine, changedInstant);
                }
                
                if (!CanPalletAbcAcceptItemAbc(purchaseTaskPallet.Abc, purchaseTaskLine.ProductAbc))
                    return RpcResponse<bool>.WithError(false, CantPlaceProductOnPalletError(purchaseTaskPallet.Abc, purchaseTaskLine.ProductAbc));
            }

            if (request.PurchaseTaskLineUpdate.PurchaseTaskLineUpdateType == PurchaseTaskLineUpdateType.Reset)
            {
                PurchaseTaskLineReset(changedInstant, purchaseTaskLine, isAcceptanceProcessTypeIsPalletized);
            }

            if (request.PurchaseTaskLineUpdate.PurchaseTaskLineUpdateType == PurchaseTaskLineUpdateType.Update)
            {
                await PurchaseTaskLineUpdate(changedInstant, request, purchaseTaskLine, isAcceptanceProcessTypeIsPalletized, false);
            }
            
            if (request.PurchaseTaskLineUpdate.PurchaseTaskLineUpdateType == PurchaseTaskLineUpdateType.Set)
            {
                await PurchaseTaskLineUpdate(changedInstant, request, purchaseTaskLine, isAcceptanceProcessTypeIsPalletized, true);
            }

            await _unitOfWork.Context.SaveChangesAsync();
            _unitOfWork.Commit();
            
            return RpcResponse<bool>.WithSuccess(true);
        }

        private bool PurchaseTaskLineQuantityIsExceeded(PurchaseTaskLineRow purchaseTaskLine, PurchaseTaskLineUpdateDto purchaseTaskLineUpdate)
        {
            var currentQty = purchaseTaskLine.PurchaseTaskLineState != null
                ? purchaseTaskLine.PurchaseTaskLineState.QtyNormal + purchaseTaskLine.PurchaseTaskLineState.QtyBroken
                : 0;

            var updateQty = purchaseTaskLineUpdate.PurchaseTaskLineState != null
                ? purchaseTaskLineUpdate.PurchaseTaskLineState.QtyNormal +
                  purchaseTaskLineUpdate.PurchaseTaskLineState.QtyBroken
                : 0;
            
            var qtyIsExceededResult = 
                purchaseTaskLineUpdate.PurchaseTaskLineUpdateType == PurchaseTaskLineUpdateType.Reset ? false
                : purchaseTaskLineUpdate.PurchaseTaskLineUpdateType == PurchaseTaskLineUpdateType.Set ? updateQty < 0 && updateQty > purchaseTaskLine.Quantity
                : purchaseTaskLineUpdate.PurchaseTaskLineUpdateType == PurchaseTaskLineUpdateType.Update ? updateQty + currentQty > purchaseTaskLine.Quantity
                : true;

            return qtyIsExceededResult;
        }
        
        private void PurchaseTaskUpdateStartInstant(PurchaseTaskHeadRow purchaseTask, Instant changedInstant)
        {
            if (!purchaseTask.StartedAt.HasValue)
            {
                purchaseTask.StartedAt = changedInstant;
                _wmsContext.PurchaseTaskHead.Update(purchaseTask);
                
                foreach (var purchaseTaskPacHeadRow in purchaseTask.PacHeads)
                {
                    purchaseTaskPacHeadRow.PacHead.StartedAt = changedInstant;
                    _wmsContext.PacHead.Update(purchaseTaskPacHeadRow.PacHead);
                }
            }
        }
        
        private void PurchaseTaskUpdateChangedAtInstant(PurchaseTaskHeadRow purchaseTask, Instant changedInstant)
        {
            purchaseTask.ChangedAt = changedInstant;
            _wmsContext.PurchaseTaskHead.Update(purchaseTask);
        }

        private async Task PurchaseTaskLineUpdateStore(IUserIdProvider userIdProvider, PurchaseTaskLineUpdatePostReq request, PurchaseTaskHeadRow purchaseTask)
        {
            var purchaseTaskLineUpdateRow = _mapper.Map<PurchaseTaskLineUpdateRow>(request);
            purchaseTaskLineUpdateRow.Id = NewId.NextGuid();
            purchaseTaskLineUpdateRow.UserId = userIdProvider.UserGuid;
            purchaseTaskLineUpdateRow.PurchaseTaskHeadId = request.PurchaseTaskId;
            await _wmsContext.PurchaseTaskLineUpdate.AddAsync(purchaseTaskLineUpdateRow);
        }

        private async Task<PurchaseTaskPalletRow> PurchaseTaskPalletCreate(PurchaseTaskLineUpdatePostReq request, PurchaseTaskHeadRow purchaseTask, PurchaseTaskLineRow purchaseTaskLine, Instant changedInstant)
        {
            var purchaseTaskPallet = new PurchaseTaskPalletRow
            {
                Id = NewId.NextGuid(),
                PurchaseTaskHead = purchaseTask,
                Code = request.CurrentPalletCode,
                Abc = ProductAbcToPalletAbc(purchaseTaskLine.ProductAbc),
                ChangedAt = changedInstant
            };
            await _wmsContext.PurchaseTaskPallet.AddAsync(purchaseTaskPallet);
            
            return purchaseTaskPallet;
        }

        private async Task PurchaseTaskLineUpdate(Instant changedInstant, PurchaseTaskLineUpdatePostReq request, PurchaseTaskLineRow purchaseTaskLine, bool isAcceptanceProcessTypeIsPalletized, bool replaceValue)
        {
            LocalDate? expirationDateOld =
                purchaseTaskLine.PurchaseTaskLineState.ExpirationDate.HasValue
                    ? purchaseTaskLine.PurchaseTaskLineState.ExpirationDate.Value.Plus(Period.FromDays(purchaseTaskLine.PurchaseTaskLineState.ExpirationDaysPlus))
                    : null;
            LocalDate? expirationDateNew =
                request.PurchaseTaskLineUpdate.PurchaseTaskLineState.ExpirationDate.HasValue
                    ? request.PurchaseTaskLineUpdate.PurchaseTaskLineState.ExpirationDate.Value.InUtc().Date.Plus(Period.FromDays(request.PurchaseTaskLineUpdate.PurchaseTaskLineState.ExpirationDaysPlus))
                    : null;
            var dateUpdateNeeded = expirationDateNew.HasValue && (!expirationDateOld.HasValue || (expirationDateOld.Value > expirationDateNew));
                
            if (dateUpdateNeeded)
                purchaseTaskLine.PurchaseTaskLineState.ExpirationDate = request.PurchaseTaskLineUpdate.PurchaseTaskLineState.ExpirationDate?.InUtc().Date;
            if (dateUpdateNeeded)
                purchaseTaskLine.PurchaseTaskLineState.ExpirationDaysPlus = request.PurchaseTaskLineUpdate.PurchaseTaskLineState.ExpirationDaysPlus;
            purchaseTaskLine.PurchaseTaskLineState.QtyNormal = 
                replaceValue 
                    ? request.PurchaseTaskLineUpdate.PurchaseTaskLineState.QtyNormal 
                    : request.PurchaseTaskLineUpdate.PurchaseTaskLineState.QtyNormal + purchaseTaskLine.PurchaseTaskLineState.QtyNormal;
            purchaseTaskLine.PurchaseTaskLineState.QtyBroken = 
                replaceValue
                    ? request.PurchaseTaskLineUpdate.PurchaseTaskLineState.QtyBroken
                    : request.PurchaseTaskLineUpdate.PurchaseTaskLineState.QtyBroken + purchaseTaskLine.PurchaseTaskLineState.QtyBroken;
            purchaseTaskLine.PurchaseTaskLineState.ChangedAt = changedInstant;

            var purchaseTaskLinePalletedState = 
                isAcceptanceProcessTypeIsPalletized
                    ? purchaseTaskLine.PurchaseTaskLinePalletedStates.FirstOrDefault(e => e.PalletCode == request.CurrentPalletCode)
                    : purchaseTaskLine.PurchaseTaskLinePalletedStates.FirstOrDefault(e => e.PalletCode == string.Empty);
            if (purchaseTaskLinePalletedState == null)
            {
                await PurchaseTaskLinePalletedStateCreate(changedInstant, request, purchaseTaskLine, isAcceptanceProcessTypeIsPalletized);
            }
            else
            {
                PurchaseTaskLinePalletedStateUpdate(request, changedInstant, purchaseTaskLinePalletedState);
            }

            _wmsContext.PurchaseTaskLineState.Update(purchaseTaskLine.PurchaseTaskLineState);
        }
        
        private void PurchaseTaskLineReset(Instant changedInstant, PurchaseTaskLineRow purchaseTaskLine, bool isAcceptanceProcessTypeIsPalletized)
        {
            purchaseTaskLine.PurchaseTaskLineState.ExpirationDate = null;
            purchaseTaskLine.PurchaseTaskLineState.ExpirationDaysPlus = 0;
            purchaseTaskLine.PurchaseTaskLineState.QtyNormal = 0;
            purchaseTaskLine.PurchaseTaskLineState.QtyBroken = 0;
            purchaseTaskLine.PurchaseTaskLineState.ChangedAt = changedInstant;

            PurchaseTaskLinePalletedStateClear(purchaseTaskLine);

            _wmsContext.PurchaseTaskLineState.Update(purchaseTaskLine.PurchaseTaskLineState);
        }

        private async Task PurchaseTaskLinePalletedStateCreate(Instant changedInstant, PurchaseTaskLineUpdatePostReq request, PurchaseTaskLineRow purchaseTaskLine, bool isAcceptanceProcessTypeIsPalletized)
        {
            var purchaseTaskLinePalletedState = new PurchaseTaskLinePalletedStateRow
            {
                Id = NewId.NextGuid(),
                PurchaseTaskLine = purchaseTaskLine,
                PalletCode = 
                    isAcceptanceProcessTypeIsPalletized
                        ? request.CurrentPalletCode
                        : string.Empty,
                PalletAbc = 
                    isAcceptanceProcessTypeIsPalletized
                        ? ProductAbcToPalletAbc(purchaseTaskLine.ProductAbc)
                        : string.Empty,
                ExpirationDate = request.PurchaseTaskLineUpdate.PurchaseTaskLineState.ExpirationDate?.InUtc().Date,
                ExpirationDaysPlus = request.PurchaseTaskLineUpdate.PurchaseTaskLineState.ExpirationDaysPlus,
                QtyNormal = request.PurchaseTaskLineUpdate.PurchaseTaskLineState.QtyNormal,
                QtyBroken = request.PurchaseTaskLineUpdate.PurchaseTaskLineState.QtyBroken,
                ChangedAt = changedInstant
            };

            await _wmsContext.PurchaseTaskLinePalletedState.AddAsync(purchaseTaskLinePalletedState);
        }

        private void PurchaseTaskLinePalletedStateUpdate(PurchaseTaskLineUpdatePostReq request, Instant changedInstant, PurchaseTaskLinePalletedStateRow purchaseTaskLinePalletedState)
        {
            LocalDate? expirationDateOld =
                purchaseTaskLinePalletedState.ExpirationDate.HasValue
                    ? purchaseTaskLinePalletedState.ExpirationDate.Value.Plus(Period.FromDays(purchaseTaskLinePalletedState.ExpirationDaysPlus))
                    : null;
            LocalDate? expirationDateNew =
                request.PurchaseTaskLineUpdate.PurchaseTaskLineState.ExpirationDate.HasValue
                    ? request.PurchaseTaskLineUpdate.PurchaseTaskLineState.ExpirationDate.Value.InUtc().Date.Plus(Period.FromDays(request.PurchaseTaskLineUpdate.PurchaseTaskLineState.ExpirationDaysPlus))
                    : null;
            var dateUpdateNeeded = expirationDateNew.HasValue && (!expirationDateOld.HasValue || (expirationDateOld.Value > expirationDateNew));
            
            if (dateUpdateNeeded)
                purchaseTaskLinePalletedState.ExpirationDate = request.PurchaseTaskLineUpdate.PurchaseTaskLineState.ExpirationDate?.InUtc().Date;
            if (dateUpdateNeeded)
                purchaseTaskLinePalletedState.ExpirationDaysPlus = request.PurchaseTaskLineUpdate.PurchaseTaskLineState.ExpirationDaysPlus;
            purchaseTaskLinePalletedState.QtyNormal += request.PurchaseTaskLineUpdate.PurchaseTaskLineState.QtyNormal;
            purchaseTaskLinePalletedState.QtyBroken += request.PurchaseTaskLineUpdate.PurchaseTaskLineState.QtyBroken;
            purchaseTaskLinePalletedState.ChangedAt = changedInstant;

            _wmsContext.PurchaseTaskLinePalletedState.Update(purchaseTaskLinePalletedState);
        }
        
        private void PurchaseTaskLinePalletedStateClear(PurchaseTaskLineRow purchaseTaskLine)
        {
            _wmsContext.PurchaseTaskLinePalletedState.RemoveRange(purchaseTaskLine.PurchaseTaskLinePalletedStates);
        }

        private string ProductAbcToPalletAbc(string productAbc)
        {
            return (productAbc.Trim().ToLowerInvariant() == "a") ? "a" : "bc";
        }

        private bool CanPalletAbcAcceptItemAbc(string palletAbc, string productAbc)
        {
            return palletAbc.Trim().ToLowerInvariant().Contains(productAbc.Trim().ToLowerInvariant());
        }

        private RpcError CantPlaceProductOnPalletError(string palletAbc, string productAbc)
        {
            return new RpcError
            {
                ErrorCode = "PURCH001",
                ErrorText = $"Нельзя положить на паллету с товаром категории {palletAbc.ToUpperInvariant()} товар категории {productAbc.ToUpperInvariant()}!"
            };
        }
    }
}