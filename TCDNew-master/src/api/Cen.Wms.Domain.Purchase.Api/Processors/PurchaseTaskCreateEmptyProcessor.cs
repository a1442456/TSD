using System;
using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Data.EntityFramework;
using Cen.Wms.Data.Context;
using Cen.Wms.Domain.Purchase.Abstract;
using Cen.Wms.Domain.Purchase.Api.Dtos;
using Serilog;

namespace Cen.Wms.Domain.Purchase.Api.Processors
{
    public class PurchaseTaskCreateEmptyProcessor : IQueryProcessor<PurchaseTaskCreateEmptyReq, RpcResponse<Guid>>
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork<WmsContext> _unitOfWork;
        private readonly IPurchaseTaskRepository _purchaseTaskRepository;

        public PurchaseTaskCreateEmptyProcessor(ILogger logger, UnitOfWork<WmsContext> unitOfWork, IPurchaseTaskRepository purchaseTaskRepository)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _purchaseTaskRepository = purchaseTaskRepository;
        }


        public async Task<RpcResponse<Guid>> Run(IUserIdProvider userIdProvider, PurchaseTaskCreateEmptyReq request)
        {
            // TODO: check facility code
            var purchaseTaskCreateEmptyResult = await _purchaseTaskRepository.PurchaseTaskCreateEmpty(request.FacilityId, userIdProvider.UserGuid, false);

            await _unitOfWork.Context.SaveChangesAsync();
            _unitOfWork.Commit();
                
            return purchaseTaskCreateEmptyResult;
        }
    }
}