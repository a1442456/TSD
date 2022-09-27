using SimpleInjector;
using Cen.Common.CQRS;
using Cen.Common.Data.DataSource.AgGrid;
using Cen.Common.Data.DataSource.Dtos;
using Cen.Common.Domain.Models;
using Cen.Common.Http.Server.CQRS;
using Cen.Common.Http.Server.CQRS.OpenApi;
using Cen.Wms.Domain.Facility.Manage.Api.Processors;
using Cen.Wms.Domain.Facility.Manage.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi;

namespace Cen.Wms.Domain.Facility.Manage.Api
{
    public class FacilityManageModule
    {
        public static void RegisterTypes(Container container)
        {
            container.Register<FacilityReadQuery>(Lifestyle.Scoped);
            container.Register<FacilityListQuery>(Lifestyle.Scoped);
        }
        
        public static void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder, HttpQueryRunner httpQueryRunner)
        {
            var r = new HttpRoutesRegistrar<FacilityManageModule>(httpQueryRunner);
            
            r.RegisterProcessor<ByIdReq, RpcResponse<FacilityListModel>, FacilityReadQuery>("/facility/read");
            r.RegisterProcessor<TableRowsReq, RpcResponse<DataSourceResult<FacilityListModel>>, FacilityListQuery>("/facility/list");
            
            r.Commit("/facility/manage/openapi.json", OpenApiSpecVersion.OpenApi2_0, endpointRouteBuilder);
        }
    }
}