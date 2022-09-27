using SimpleInjector;
using Cen.Common.CQRS;
using Cen.Common.Data.DataSource.AgGrid;
using Cen.Common.Data.DataSource.Dtos;
using Cen.Common.Domain.Models;
using Cen.Common.Http.Server.CQRS;
using Cen.Common.Http.Server.CQRS.OpenApi;
using Cen.Wms.Domain.Facility.Access.Api.Dtos;
using Cen.Wms.Domain.Facility.Access.Api.Processors;
using Cen.Wms.Domain.Facility.Access.Models;
using Cen.Wms.Domain.Facility.Manage.Models;
using Cen.Wms.Domain.User.Manage.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi;

namespace Cen.Wms.Domain.Facility.Access.Api
{
    public class FacilityAccessModule
    {
        public static void RegisterTypes(Container container)
        {
            container.Register<FacilityAccessGrantProcessor>(Lifestyle.Scoped);
            container.Register<FacilityAccessWithdrawProcessor>(Lifestyle.Scoped);
            container.Register<FacilityListSimpleReadByPersonQuery>(Lifestyle.Scoped);
            container.Register<FacilityListDataSourceReadByPersonQuery>(Lifestyle.Scoped);
            container.Register<FacilityAccessUserListGrantedQuery>(Lifestyle.Scoped);
            container.Register<FacilityAccessUserListAvailableQuery>(Lifestyle.Scoped);
        }
        
        public static void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder, HttpQueryRunner httpQueryRunner)
        {
            var r = new HttpRoutesRegistrar<FacilityAccessModule>(httpQueryRunner);
            
            r.RegisterProcessor<FacilityAccessEditReq, RpcResponse<bool>, FacilityAccessGrantProcessor>("/facility/access/grant");
            r.RegisterProcessor<FacilityAccessEditReq, RpcResponse<bool>, FacilityAccessWithdrawProcessor>("/facility/access/withdraw");
            r.RegisterProcessor<TableRowsWithParamReq<ByIdReq>, RpcResponse<DataSourceResult<UserWithAccessListModel>>, FacilityAccessUserListGrantedQuery>("/facility/access/user/list/granted");
            r.RegisterProcessor<TableRowsWithParamReq<ByIdReq>, RpcResponse<DataSourceResult<UserListModel>>, FacilityAccessUserListAvailableQuery>("/facility/access/user/list/available");
            
            r.RegisterProcessor<object, RpcResponse<ViewModelSimple[]>, FacilityListSimpleReadByPersonQuery>("/facility/list/simple/by_person");
            r.RegisterProcessor<TableRowsReq, RpcResponse<DataSourceResult<FacilityListModel>>, FacilityListDataSourceReadByPersonQuery>("/facility/list/data_source/by_person");
            
            r.Commit("/facility/access/openapi.json", OpenApiSpecVersion.OpenApi2_0, endpointRouteBuilder);
        }
    }
}