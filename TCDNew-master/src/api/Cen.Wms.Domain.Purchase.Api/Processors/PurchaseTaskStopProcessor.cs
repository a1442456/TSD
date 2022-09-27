using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Data.EntityFramework;
using Cen.Common.Domain.Models;
using Cen.Wms.Data.Context;
using Cen.Wms.Domain.Purchase.Abstract;
using Serilog;

namespace Cen.Wms.Domain.Purchase.Api.Processors
{
    public class PurchaseTaskStopProcessor : IQueryProcessor<ByIdReq, RpcResponse<bool>>
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork<WmsContext> _unitOfWork;
        private readonly IPurchaseTaskRepository _purchaseTaskRepository;

        public PurchaseTaskStopProcessor(ILogger logger, UnitOfWork<WmsContext> unitOfWork, IPurchaseTaskRepository purchaseTaskRepository)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _purchaseTaskRepository = purchaseTaskRepository;
        }

        public async Task<RpcResponse<bool>> Run(IUserIdProvider userIdProvider, ByIdReq request)
        {
            var result = await _purchaseTaskRepository.PurchaseTaskStop(request.Id);
            
            await _unitOfWork.Context.SaveChangesAsync();
            _unitOfWork.Commit();

            return result;
        }
    }
}