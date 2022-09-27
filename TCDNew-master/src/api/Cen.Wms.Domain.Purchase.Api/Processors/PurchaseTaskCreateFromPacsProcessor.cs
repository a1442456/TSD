using System;
using System.Linq;
using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Data.EntityFramework;
using Cen.Common.Domain.Models;
using Cen.Common.Errors;
using Cen.Wms.Data.Context;
using Cen.Wms.Domain.Purchase.Abstract;
using Cen.Wms.Domain.Purchase.Api.Dtos;
using Serilog;

namespace Cen.Wms.Domain.Purchase.Api.Processors
{
    public class PurchaseTaskCreateFromPacsProcessor: IQueryProcessor<PurchaseTaskCreateFromPacsReq, RpcResponse<Guid>>
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork<WmsContext> _unitOfWork;
        private readonly IPacRepository _pacRepository;
        private readonly IPurchaseTaskRepository _purchaseTaskRepository;

        public PurchaseTaskCreateFromPacsProcessor(
            ILogger logger, UnitOfWork<WmsContext> unitOfWork,
            IPacRepository pacRepository, IPurchaseTaskRepository purchaseTaskRepository)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _pacRepository = pacRepository;
            _purchaseTaskRepository = purchaseTaskRepository;
        }
        
        public async Task<RpcResponse<Guid>> Run(IUserIdProvider userIdProvider, PurchaseTaskCreateFromPacsReq request)
        {
            var pacIdEnumerable = request.Pacs.Select(e => new ByIdReq { Id = e.PacId }).AsEnumerable();
            var pacListReadResult = await _pacRepository.PacReadMany(pacIdEnumerable);
            if (!pacListReadResult.IsSuccess)
                return RpcResponse<Guid>.WithErrors(Guid.Empty, pacListReadResult.Errors);

            var purchaseTaskCreateEmptyResult =
                await _purchaseTaskRepository.PurchaseTaskCreateEmpty(request.FacilityId, userIdProvider.UserGuid, true);
            if (!purchaseTaskCreateEmptyResult.IsSuccess)
                return RpcResponse<Guid>.WithErrors(Guid.Empty, purchaseTaskCreateEmptyResult.Errors);

            await _unitOfWork.Context.SaveChangesAsync();

            foreach (var pacHeadEditModel in pacListReadResult.Data)
            {
                var getBusyResult = await _pacRepository.PacGetBusy(new ByIdReq {Id = pacHeadEditModel.Id});
                if (!getBusyResult.IsSuccess)
                    return RpcResponse<Guid>.WithErrors(Guid.Empty, getBusyResult.Errors);
                if (getBusyResult.Data)
                    return RpcResponse<Guid>.WithError(Guid.Empty, CommonErrors.InvalidOperation);
                
                await _purchaseTaskRepository.PurchaseTaskPacInclude(
                    purchaseTaskCreateEmptyResult.Data,
                    pacHeadEditModel
                );
                await _pacRepository.PacSetBusy(
                    new ByIdReq { Id = pacHeadEditModel.Id },
                    true
                );
                await _pacRepository.PacSetResponsibleUserId(
                    new ByIdReq { Id = pacHeadEditModel.Id },
                    new ByIdReq { Id = userIdProvider.UserGuid }
                );
                await _unitOfWork.Context.SaveChangesAsync();
            }

            await _purchaseTaskRepository.PurchaseTaskUserInclude(
                purchaseTaskCreateEmptyResult.Data,
                userIdProvider.UserGuid
            );
            await _unitOfWork.Context.SaveChangesAsync();

            await _purchaseTaskRepository.PurchaseTaskStart(purchaseTaskCreateEmptyResult.Data);
            
            await _unitOfWork.Context.SaveChangesAsync();
            _unitOfWork.Commit();
            
            return RpcResponse<Guid>.WithSuccess(purchaseTaskCreateEmptyResult.Data);
        }
    }
}