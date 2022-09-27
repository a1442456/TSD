using System;
using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.Data.DataSource.AgGrid;
using Cen.Common.Data.DataSource.Dtos;
using Cen.Common.Domain.Models;
using Cen.Wms.Domain.User.Manage.Models;

namespace Cen.Wms.Domain.User.Manage.Abstract
{
    public interface IUserRepository
    {
        public Task<RpcResponse<Guid>> UserStore(UserEditModel userEditModel);
        public Task<RpcResponse<ViewModelSimple>> UserReadSimple(ByIdReq request);
        public Task<RpcResponse<bool>> UserExists(ByIdReq request);
        public Task<RpcResponse<Guid>> UserIdByExtId(string extId);
        public Task<RpcResponse<UserEditModel>> UserRead(ByIdReq request);
        public Task<RpcResponse<UserStateEditModel>> UserStateGet(ByIdReq request);
        public Task<RpcResponse<bool>> UserStateSet(ByIdReq request, UserStateEditModel userState);
        public Task<RpcResponse<DataSourceResult<UserListModel>>> UserList(TableRowsReq request);
    }
}