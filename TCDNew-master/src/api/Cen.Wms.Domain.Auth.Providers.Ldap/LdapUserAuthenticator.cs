using System;
using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Wms.Domain.Auth.Providers.Abstract;
using Cen.Wms.Domain.Auth.Providers.Models;
using Cen.Wms.Domain.User.Manage.Abstract;
using Novell.Directory.Ldap;
using Serilog;

namespace Cen.Wms.Domain.Auth.Providers.Ldap
{
    public class LdapUserAuthenticator: IUserAuthenticator<UserCredentials>
    {
        private readonly ILogger _logger;
        private readonly LdapUserAuthenticatorOptions _ldapUserAuthenticatorOptions;
        private readonly IUserRepository _userRepository;

        public LdapUserAuthenticator(ILogger logger, LdapUserAuthenticatorOptions ldapUserAuthenticatorOptions, IUserRepository userRepository)
        {
            _logger = logger;
            _ldapUserAuthenticatorOptions = ldapUserAuthenticatorOptions;
            _userRepository = userRepository;
        }
        
        public async Task<RpcResponse<UserIdentity>> Authenticate(UserCredentials request)
        {
            LdapEntry ldapEntryUser = null;
            var ldapUserId = Guid.Empty;

            using var ldapConnection = new LdapConnection();
            try
            {
                var loginRDN = string.Format(_ldapUserAuthenticatorOptions.LdapUserRdnTemplate, request.UserName);
                ldapConnection.Connect(_ldapUserAuthenticatorOptions.LdapHost, _ldapUserAuthenticatorOptions.LdapPort);
                ldapConnection.Bind(loginRDN, request.UserPassword);
                
                ldapEntryUser = ldapConnection.Read(loginRDN);
                var uidBytes = ldapEntryUser?.GetAttribute("objectGUID")?.ByteValue;
                ldapUserId = new Guid(uidBytes);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "LdapUserAuthenticator: {UserName}", request);
            }
            finally
            {
                ldapConnection.Disconnect();
            }

            if (ldapEntryUser == null)
                return RpcResponse<UserIdentity>.WithError(null, Errors.UserNotFound);
            
            if (ldapUserId == Guid.Empty)
                return RpcResponse<UserIdentity>.WithError(null, Errors.UserNotFound);

            var userIdResponse = await _userRepository.UserIdByExtId(ldapUserId.ToString("N"));
            if (!userIdResponse.IsSuccess)
                return RpcResponse<UserIdentity>.WithErrors(null, userIdResponse.Errors);
            
            if (userIdResponse.Data == Guid.Empty)
                return RpcResponse<UserIdentity>.WithError(null, Errors.UserNotFound);
            
            return RpcResponse<UserIdentity>.WithSuccess(new UserIdentity(userIdResponse.Data));
        }
    }
}