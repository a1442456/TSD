using SimpleInjector;
using Cen.Common.CQRS;
using Cen.Common.Domain.Models;
using Cen.Common.Http.Server.CQRS;
using Cen.Common.Http.Server.CQRS.OpenApi;
using Cen.Wms.Domain.Facility.Config.Api.Dtos;
using Cen.Wms.Domain.Facility.Config.Api.Processors;
using Cen.Wms.Domain.Facility.Config.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi;

namespace Cen.Wms.Domain.Facility.Config.Api
{
    public class FacilityConfigModule
    {
        public static void RegisterTypes(Container container)
        {
            container.Register<FacilityConfigGetProcessor>(Lifestyle.Scoped);
            container.Register<FacilityConfigSetProcessor>(Lifestyle.Scoped);
        }
        
        public static void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder, HttpQueryRunner httpQueryRunner)
        {
            var r = new HttpRoutesRegistrar<FacilityConfigModule>(httpQueryRunner);
            
            r.RegisterProcessor<ByIdReq, RpcResponse<FacilityConfigEditModel>, FacilityConfigGetProcessor>("/facility/config/get");
            r.RegisterProcessor<FacilityConfigSetReq, RpcResponse<bool>, FacilityConfigSetProcessor>("/facility/config/set");
            
            r.Commit("/facility/config/openapi.json", OpenApiSpecVersion.OpenApi2_0, endpointRouteBuilder);
        }
    }
}