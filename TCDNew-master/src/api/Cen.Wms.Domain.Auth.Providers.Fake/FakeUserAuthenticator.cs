using System;
using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Wms.Domain.Auth.Providers.Abstract;
using Cen.Wms.Domain.Auth.Providers.Models;
using Cen.Wms.Domain.User.Manage.Abstract;

namespace Cen.Wms.Domain.Auth.Providers.Fake
{
    public class FakeUserAuthenticator: IUserAuthenticator<UserCredentials>
    {
        private readonly IUserRepository _userRepository;

        public FakeUserAuthenticator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<RpcResponse<UserIdentity>> Authenticate(UserCredentials request)
        {
            if (request.UserName != request.UserPassword)
                return RpcResponse<UserIdentity>.WithError(null, Errors.UserNotFound);
            
            var userIdResponse = await _userRepository.UserIdByExtId(request.UserName);
            if (!userIdResponse.IsSuccess)
                return RpcResponse<UserIdentity>.WithErrors(null, userIdResponse.Errors);
            
            if (userIdResponse.Data == Guid.Empty)
                return RpcResponse<UserIdentity>.WithError(null, Errors.UserNotFound);
            
            return RpcResponse<UserIdentity>.WithSuccess(new UserIdentity(userIdResponse.Data));
        }
    }
}