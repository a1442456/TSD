using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Data.EntityFramework;
using Cen.Common.Domain.Models;
using Cen.Common.Errors;
using Cen.Wms.Data.Context;
using Cen.Wms.Domain.User.Manage.Abstract;
using Cen.Wms.Domain.User.Manage.Models;
using Serilog;

namespace Cen.Wms.Domain.User.Manage.Api.Processors
{
    public class UserLockProcessor: IQueryProcessor<ByIdReq, RpcResponse<bool>>
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork<WmsContext> _unitOfWork;
        private readonly IUserRepository _userRepository;

        public UserLockProcessor(ILogger logger, UnitOfWork<WmsContext> unitOfWork, IUserRepository userRepository)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
        }

        public async Task<RpcResponse<bool>> Run(IUserIdProvider userIdProvider, ByIdReq request)
        {
            var userExistsResult = await _userRepository.UserExists(request);
            if (!userExistsResult.IsSuccess)
                return RpcResponse<bool>.WithErrors(false, userExistsResult.Errors);
            if (!userExistsResult.Data)
                return RpcResponse<bool>.WithError(false, CommonErrors.NotFound("пользователь"));
            
            var userStateResult = await _userRepository.UserStateGet(request);
            if (!userStateResult.IsSuccess)
                return RpcResponse<bool>.WithErrors(false, userStateResult.Errors);
            if (userStateResult.Data.IsLocked)
                return RpcResponse<bool>.WithError(false, CommonErrors.InvalidOperation);
            
            var userStateSetResult = await _userRepository.UserStateSet(request, new UserStateEditModel { IsLocked = true });
            
            await _unitOfWork.Context.SaveChangesAsync();
            _unitOfWork.Commit();
            
            return userStateSetResult;
        }
    }
}