using System;
using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.Errors;
using Cen.Common.Http.Client;
using Cen.Wms.Domain.Auth.Providers.Abstract;
using Cen.Wms.Domain.Auth.Providers.LsFusion.Dtos;
using Cen.Wms.Domain.Auth.Providers.Models;
using Cen.Wms.Domain.User.Manage.Abstract;
using Serilog;

namespace Cen.Wms.Domain.Auth.Providers.LsFusion
{
    public class LsFusionUserAuthenticator: IUserAuthenticator<UserCredentials>
    {
        private readonly ILogger _logger;
        private readonly LsFusionUserAuthenticatorOptions _lsFusionUserAuthenticatorOptions;
        private readonly HttpQueryCall _httpQueryCall;
        private readonly IUserRepository _userRepository;

        public LsFusionUserAuthenticator(HttpQueryCall httpQueryCall, LsFusionUserAuthenticatorOptions lsFusionUserAuthenticatorOptions, IUserRepository userRepository, ILogger logger)
        {
            _httpQueryCall = httpQueryCall;
            _lsFusionUserAuthenticatorOptions = lsFusionUserAuthenticatorOptions;
            _userRepository = userRepository;
            _logger = logger;
        }
        
        public async Task<RpcResponse<UserIdentity>> Authenticate(UserCredentials request)
        {
            LsUserIdentity response = null;
            try
            {
                response = await _httpQueryCall.RunRaw<LsUserCredentials, LsUserIdentity>(
                    new LsUserCredentials { Login = request.UserName, Password = request.UserPassword }, 
                    $"{_lsFusionUserAuthenticatorOptions.WMSServiceBaseAddress}/exec?action=TerminalServer.authenticate",
                    _lsFusionUserAuthenticatorOptions.TimeoutMs
                );
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "LsFusionUserAuthenticator: {UserName}", request);
            }

            if (response == null)
                return RpcResponse<UserIdentity>.WithError(null, CommonErrors.ExternalSystemError(null));
            
            if (response.Error > 0)
                return RpcResponse<UserIdentity>.WithError(null, CommonErrors.ExternalSystemError(response.ErrorMessage));

            if (string.IsNullOrWhiteSpace(response.UserId))
                return RpcResponse<UserIdentity>.WithError(null, Errors.UserNotFound);
            
            var userIdResponse = await _userRepository.UserIdByExtId(response.UserId);
            if (!userIdResponse.IsSuccess)
                return RpcResponse<UserIdentity>.WithErrors(null, userIdResponse.Errors);
            
            if (userIdResponse.Data == Guid.Empty)
                return RpcResponse<UserIdentity>.WithError(null, Errors.UserNotFound);
            
            return RpcResponse<UserIdentity>.WithSuccess(new UserIdentity(userIdResponse.Data));
        }
    }
}