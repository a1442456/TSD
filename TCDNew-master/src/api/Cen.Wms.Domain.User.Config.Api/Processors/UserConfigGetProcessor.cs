using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Data.EntityFramework;
using Cen.Common.Domain.Models;
using Cen.Common.Errors;
using Cen.Wms.Data.Context;
using Cen.Wms.Domain.User.Config.Abstract;
using Cen.Wms.Domain.User.Config.Models;
using Cen.Wms.Domain.User.Manage.Abstract;
using Serilog;

namespace Cen.Wms.Domain.User.Config.Api.Processors
{
    public class UserConfigGetProcessor: IQueryProcessor<ByIdReq, RpcResponse<UserConfigEditModel>>
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork<WmsContext> _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IUserConfigRepository _userConfigRepository;

        public UserConfigGetProcessor(
            ILogger logger, UnitOfWork<WmsContext> unitOfWork,
            IUserRepository userRepository, IUserConfigRepository userConfigRepository)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _userConfigRepository = userConfigRepository;
        }

        public async Task<RpcResponse<UserConfigEditModel>> Run(IUserIdProvider userIdProvider, ByIdReq request)
        {
            var userExistsResult = await _userRepository.UserExists(request);
            if (!userExistsResult.IsSuccess)
                return RpcResponse<UserConfigEditModel>.WithErrors(null, userExistsResult.Errors);
            if (!userExistsResult.Data)
                return RpcResponse<UserConfigEditModel>.WithError(null, CommonErrors.NotFound("пользователь"));

            var userConfigGetResult = await _userConfigRepository.UserConfigGet(request);
            
            _unitOfWork.Rollback();
            
            return userConfigGetResult;
        }
    }
}