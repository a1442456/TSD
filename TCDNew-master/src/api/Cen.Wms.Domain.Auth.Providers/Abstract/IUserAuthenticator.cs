using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Wms.Domain.Auth.Providers.Models;

namespace Cen.Wms.Domain.Auth.Providers.Abstract
{
    public interface IUserAuthenticator<in TAuthenticateRequest>
    {
        Task<RpcResponse<UserIdentity>> Authenticate(TAuthenticateRequest request);
    }
}