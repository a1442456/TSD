using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Domain.Models;
using Cen.IdentityModel.EdDsa;
using Cen.Wms.Domain.Auth.Api.Dtos;
using Cen.Wms.Domain.Auth.Providers;
using Cen.Wms.Domain.Auth.Providers.Abstract;
using Cen.Wms.Domain.Auth.Providers.Models;
using Cen.Wms.Domain.User.Manage.Abstract;
using Microsoft.IdentityModel.Tokens;
using NodaTime;

namespace Cen.Wms.Domain.Auth.Api.Processors
{
    public class LoginQueryProcessor: IQueryProcessor<UserCredentials, RpcResponse<UserTokenResp>>
    {
        private readonly IUserAuthenticator<UserCredentials> _userAuthenticator;
        private readonly AuthOptions _authOptions;
        private readonly IUserRepository _userRepository;

        public LoginQueryProcessor(IUserAuthenticator<UserCredentials> userAuthenticator, AuthOptions authOptions, IUserRepository userRepository)
        {
            _userAuthenticator = userAuthenticator;
            _authOptions = authOptions;
            _userRepository = userRepository;
        }
        
        public async Task<RpcResponse<UserTokenResp>> Run(IUserIdProvider userIdProvider, UserCredentials request)
        {
            var authenticateResponse = await _userAuthenticator.Authenticate(request);
            if (!authenticateResponse.IsSuccess)
                return RpcResponse<UserTokenResp>.WithErrors(null, authenticateResponse.Errors);

            var userReadResponse = await _userRepository.UserRead(new ByIdReq {Id = authenticateResponse.Data.UserId});
            if (!userReadResponse.IsSuccess)
                return RpcResponse<UserTokenResp>.WithErrors(null, userReadResponse.Errors);
            
            if (userReadResponse.Data == null)
                return RpcResponse<UserTokenResp>.WithError(null, Errors.UserNotFound);
            
            var claimsPrincipal = GetClaimsPrincipal(authenticateResponse.Data.UserId, Array.Empty<string>());
            var expirationDateTime = SystemClock.Instance.GetCurrentInstant() + Duration.FromSeconds(_authOptions.LifetimeSeconds);
            var jwtToken = JwtBearerCreate(claimsPrincipal, expirationDateTime, _authOptions.Issuer);
            
            return RpcResponse<UserTokenResp>.WithSuccess(
                new UserTokenResp { AuthToken = jwtToken, DisplayName = userReadResponse.Data.Name }
            );
        }
        
        private string JwtBearerCreate(ClaimsPrincipal claimsPrincipal, Instant expirationInstant, string issuer)
        {
            var jwt = new JwtSecurityToken(
                issuer: issuer,
                notBefore: SystemClock.Instance.GetCurrentInstant().Minus(Duration.FromSeconds(15)).ToDateTimeUtc(),
                expires: expirationInstant.ToDateTimeUtc(),
                claims: claimsPrincipal.Claims,
                signingCredentials: new SigningCredentials(
                    new Ed25519SecurityKey(_authOptions.GetPrivateKeyParameters()),
                    EdDsaSignatureAlgorithms.EdDsa
                ));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            
            return encodedJwt;
        }
        
        private ClaimsPrincipal GetClaimsPrincipal(Guid personId, IEnumerable<string> roles)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, personId.ToString("D")));
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var claimsIdentity = 
                new ClaimsIdentity(
                    claims,
                    Consts.TokenAuthenticationType,
                    ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType
                );
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return claimsPrincipal;
        }
    }
}