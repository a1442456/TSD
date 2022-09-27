using SimpleInjector;
using Cen.Common.CQRS;
using Cen.Common.Data.DataSource.AgGrid;
using Cen.Common.Data.DataSource.Dtos;
using Cen.Common.Domain.Models;
using Cen.Common.Http.Server.CQRS;
using Cen.Common.Http.Server.CQRS.OpenApi;
using Cen.Wms.Domain.User.Manage.Api.Processors;
using Cen.Wms.Domain.User.Manage.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi;

namespace Cen.Wms.Domain.User.Manage.Api
{
    public class UserManageModule
    {
        public static void RegisterTypes(Container container)
        {
            container.Register<UserLockProcessor>(Lifestyle.Scoped);
            container.Register<UserUnlockProcessor>(Lifestyle.Scoped);
            container.Register<UserListQuery>(Lifestyle.Scoped);
            container.Register<UserWhoAmIQuery>(Lifestyle.Scoped);
        }
        
        public static void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder, HttpQueryRunner httpQueryRunner)
        {
            var r = new HttpRoutesRegistrar<UserManageModule>(httpQueryRunner);
            
            r.RegisterProcessor<ByIdReq, RpcResponse<bool>, UserLockProcessor>("/user/state/lock");
            r.RegisterProcessor<ByIdReq, RpcResponse<bool>, UserUnlockProcessor>("/user/state/unlock");
            r.RegisterProcessor<TableRowsReq, RpcResponse<DataSourceResult<UserListModel>>, UserListQuery>("/user/list");
            r.RegisterProcessor<object, RpcResponse<ViewModelSimple>, UserWhoAmIQuery>("/user/whoami");
            
            r.Commit("/user/state/openapi.json", OpenApiSpecVersion.OpenApi2_0, endpointRouteBuilder);
        }
    }
}