using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Data.EntityFramework;
using Cen.Common.Errors;
using Cen.Wms.Data.Context;
using Cen.Wms.Domain.Purchase.Abstract;
using Cen.Wms.Domain.Purchase.Api.Dtos;
using Cen.Wms.Domain.Sync.Abstract;
using Serilog;

namespace Cen.Wms.Domain.Purchase.Api.Processors
{
    public class PurchaseTaskFinishProcessor: IQueryProcessor<PurchaseTaskFinishReq, RpcResponse<bool>>
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork<WmsContext> _unitOfWork;
        private readonly IPurchaseTaskRepository _purchaseTaskRepository;
        private readonly IPacUploader _pacUploader;

        public PurchaseTaskFinishProcessor(ILogger logger, UnitOfWork<WmsContext> unitOfWork, IPurchaseTaskRepository purchaseTaskRepository, IPacUploader pacUploader)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _purchaseTaskRepository = purchaseTaskRepository;
            _pacUploader = pacUploader;
        }

        public async Task<RpcResponse<bool>> Run(IUserIdProvider userIdProvider, PurchaseTaskFinishReq request)
        {
            var responsibleUserIdResult = await _purchaseTaskRepository.PurchaseTaskGetResponsibleUserId(request.PurchaseTaskId);
            if (!responsibleUserIdResult.IsSuccess)
                return RpcResponse<bool>.WithErrors(false, responsibleUserIdResult.Errors);
            
            if (responsibleUserIdResult.Data != userIdProvider.UserGuid)
                return RpcResponse<bool>.WithError(false, CommonErrors.AccessDenied);
            
            var result =
                request.IsDecline
                    ? await _purchaseTaskRepository.PurchaseTaskCancel(request.PurchaseTaskId)
                    : request.DoUpload
                        ? await _purchaseTaskRepository.PurchaseTaskStopAndUpload(request.PurchaseTaskId, _pacUploader)
                        : await _purchaseTaskRepository.PurchaseTaskStop(request.PurchaseTaskId);

            await _unitOfWork.Context.SaveChangesAsync();
            _unitOfWork.Commit();

            return result;
        }
    }
}