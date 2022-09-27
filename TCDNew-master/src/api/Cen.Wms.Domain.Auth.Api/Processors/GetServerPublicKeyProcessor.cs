using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Wms.Domain.Auth.Api.Dtos;

namespace Cen.Wms.Domain.Auth.Api.Processors
{
    public class GetServerPublicKeyProcessor: IQueryProcessor<object, RpcResponse<ServerPublicKeyResp>>
    {
        private readonly AuthOptions _authOptions;

        public GetServerPublicKeyProcessor(AuthOptions authOptions)
        {
            _authOptions = authOptions;
        }
        
        public Task<RpcResponse<ServerPublicKeyResp>> Run(IUserIdProvider userIdProvider, object request)
        {
            return Task.FromResult(
                RpcResponse<ServerPublicKeyResp>.WithSuccess(
                    new ServerPublicKeyResp {PublicKeyHexString = _authOptions.GetPublicKeyString()}
                )
            );
        }
    }
}