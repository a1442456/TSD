using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Data.EntityFramework;
using Cen.Common.Domain.Models;
using Cen.Common.Errors;
using Cen.Wms.Data.Context;
using Cen.Wms.Domain.Purchase.Abstract;
using Cen.Wms.Domain.Purchase.Api.Dtos;
using Cen.Wms.Domain.User.Manage.Abstract;
using Serilog;

namespace Cen.Wms.Domain.Purchase.Api.Processors
{
    public class PurchaseTaskUserExcludeProcessor : IQueryProcessor<PurchaseTaskUserEditReq, RpcResponse<bool>>
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork<WmsContext> _unitOfWork;
        private readonly IPurchaseTaskRepository _purchaseTaskRepository;
        private readonly IUserRepository _userRepository;

        public PurchaseTaskUserExcludeProcessor(ILogger logger, UnitOfWork<WmsContext> unitOfWork,
            IPurchaseTaskRepository purchaseTaskRepository, IUserRepository userRepository)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _purchaseTaskRepository = purchaseTaskRepository;
            _userRepository = userRepository;
        }
        
        public async Task<RpcResponse<bool>> Run(IUserIdProvider userIdProvider, PurchaseTaskUserEditReq request)
        {
            var userByIdReq = new ByIdReq {Id = request.UserId};
            var userExistsResult = await _userRepository.UserExists(userByIdReq);
            if (!userExistsResult.IsSuccess)
                return RpcResponse<bool>.WithErrors(false, userExistsResult.Errors);
            if (!userExistsResult.Data)
                return RpcResponse<bool>.WithError(false, CommonErrors.NotFound("сотрудник"));
            
            var purchaseTaskPacExcludeResult = await _purchaseTaskRepository.PurchaseTaskUserExclude(request.PurchaseTaskId, request.UserId);
            if (!purchaseTaskPacExcludeResult.IsSuccess)
                return RpcResponse<bool>.WithErrors(false, purchaseTaskPacExcludeResult.Errors);

            await _unitOfWork.Context.SaveChangesAsync();
            _unitOfWork.Commit();
            
            return purchaseTaskPacExcludeResult;
        }
    }
}