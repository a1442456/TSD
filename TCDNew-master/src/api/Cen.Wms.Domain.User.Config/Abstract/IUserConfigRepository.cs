using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.Domain.Models;
using Cen.Wms.Domain.User.Config.Models;

namespace Cen.Wms.Domain.User.Config.Abstract
{
    public interface IUserConfigRepository
    {
        public Task<RpcResponse<UserConfigEditModel>> UserConfigGet(ByIdReq userId);
        public Task<RpcResponse<bool>> UserConfigSet(ByIdReq userId, UserConfigEditModel userConfigEditModel);
    }
}