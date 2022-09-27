using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Data.EntityFramework;
using Cen.Common.Domain.Models;
using Cen.Wms.Data.Context;
using Cen.Wms.Domain.Purchase.Abstract;
using Cen.Wms.Domain.Sync.Abstract;
using Serilog;

namespace Cen.Wms.Domain.Purchase.Api.Processors
{
    public class PurchaseTaskStopAndUploadProcessor : IQueryProcessor<ByIdReq, RpcResponse<bool>>
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork<WmsContext> _unitOfWork;
        private readonly IPurchaseTaskRepository _purchaseTaskRepository;
        private readonly IPacUploader _pacUploader;

        public PurchaseTaskStopAndUploadProcessor(ILogger logger, UnitOfWork<WmsContext> unitOfWork, IPurchaseTaskRepository purchaseTaskRepository, IPacUploader pacUploader)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _purchaseTaskRepository = purchaseTaskRepository;
            _pacUploader = pacUploader;
        }

        public async Task<RpcResponse<bool>> Run(IUserIdProvider userIdProvider, ByIdReq request)
        {
            var result = await _purchaseTaskRepository.PurchaseTaskStopAndUpload(request.Id, _pacUploader);
            
            await _unitOfWork.Context.SaveChangesAsync();
            _unitOfWork.Commit();

            return result;
        }
    }
}