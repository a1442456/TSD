using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Data.EntityFramework;
using Cen.Common.Errors;
using Cen.Wms.Data.Context;
using Cen.Wms.Domain.User.Config.Abstract;
using Cen.Wms.Domain.User.Config.Api.Dtos;
using Cen.Wms.Domain.User.Manage.Abstract;
using Serilog;

namespace Cen.Wms.Domain.User.Config.Api.Processors
{
    public class UserConfigSetProcessor: IQueryProcessor<UserConfigSetReq, RpcResponse<bool>>
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork<WmsContext> _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IUserConfigRepository _userConfigRepository;

        public UserConfigSetProcessor(
            ILogger logger, UnitOfWork<WmsContext> unitOfWork,
            IUserRepository userRepository, IUserConfigRepository userConfigRepository)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _userConfigRepository = userConfigRepository;
        }

        public async Task<RpcResponse<bool>> Run(IUserIdProvider userIdProvider, UserConfigSetReq request)
        {
            var userExistsResult = await _userRepository.UserExists(request.UserId);
            if (!userExistsResult.IsSuccess)
                return RpcResponse<bool>.WithErrors(false, userExistsResult.Errors);
            if (!userExistsResult.Data)
                return RpcResponse<bool>.WithError(false, CommonErrors.NotFound("пользователь"));

            var userConfigSetResult = await _userConfigRepository.UserConfigSet(request.UserId, request.UserConfigEditModel);
            
            await _unitOfWork.Context.SaveChangesAsync();
            _unitOfWork.Commit();
            
            return userConfigSetResult;
        }
    }
}