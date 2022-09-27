using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Data.EntityFramework;
using Cen.Common.Domain.Models;
using Cen.Wms.Data.Context;
using Cen.Wms.Domain.Purchase.Abstract;
using Cen.Wms.Domain.Purchase.Api.Dtos;
using Serilog;

namespace Cen.Wms.Domain.Purchase.Api.Processors
{
    public class PurchaseTaskPacIncludeProcessor : IQueryProcessor<PurchaseTaskPacHeadEditReq, RpcResponse<bool>>
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork<WmsContext> _unitOfWork;
        private readonly IPacRepository _pacRepository;
        private readonly IPurchaseTaskRepository _purchaseTaskRepository;

        public PurchaseTaskPacIncludeProcessor(ILogger logger, UnitOfWork<WmsContext> unitOfWork, 
            IPacRepository pacRepository, IPurchaseTaskRepository purchaseTaskRepository)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _pacRepository = pacRepository;
            _purchaseTaskRepository = purchaseTaskRepository;
        }
        
        public async Task<RpcResponse<bool>> Run(IUserIdProvider userIdProvider, PurchaseTaskPacHeadEditReq request)
        {
            var pacByIdReq = new ByIdReq {Id = request.PacId};
            var pacReadResult = await _pacRepository.PacRead(pacByIdReq);
            if (!pacReadResult.IsSuccess)
                return RpcResponse<bool>.WithErrors(false, pacReadResult.Errors);
            
            var purchaseTaskPacIncludeResult = await _purchaseTaskRepository.PurchaseTaskPacInclude(request.PurchaseTaskId, pacReadResult.Data);
            if (!purchaseTaskPacIncludeResult.IsSuccess)
                return RpcResponse<bool>.WithErrors(false, purchaseTaskPacIncludeResult.Errors);

            var responsibleUserIdResult = await _purchaseTaskRepository.PurchaseTaskGetResponsibleUserId(request.PurchaseTaskId);
            if (!responsibleUserIdResult.IsSuccess)
                return RpcResponse<bool>.WithErrors(false, responsibleUserIdResult.Errors);
            
            var pacSetBusyResult = await _pacRepository.PacSetBusy(pacByIdReq, true);
            if (!pacSetBusyResult.IsSuccess)
                return RpcResponse<bool>.WithErrors(false, pacSetBusyResult.Errors);
            
            var pacSetResponsibleUserIdResult = await _pacRepository.PacSetResponsibleUserId(
                pacByIdReq,
                new ByIdReq { Id = responsibleUserIdResult.Data }
            );
            if (!pacSetResponsibleUserIdResult.IsSuccess)
                return RpcResponse<bool>.WithErrors(false, pacSetResponsibleUserIdResult.Errors);
            
            await _unitOfWork.Context.SaveChangesAsync();
            _unitOfWork.Commit();
            
            return pacSetResponsibleUserIdResult;
        }
    }
}