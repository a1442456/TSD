using SimpleInjector;
using Cen.Common.CQRS;
using Cen.Common.Domain.Models;
using Cen.Common.Http.Server.CQRS;
using Cen.Common.Http.Server.CQRS.OpenApi;
using Cen.Wms.Domain.User.Config.Api.Dtos;
using Cen.Wms.Domain.User.Config.Api.Processors;
using Cen.Wms.Domain.User.Config.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi;

namespace Cen.Wms.Domain.User.Config.Api
{
    public class UserConfigModule
    {
        public static void RegisterTypes(Container container)
        {
            container.Register<UserConfigGetProcessor>(Lifestyle.Scoped);
            container.Register<UserConfigSetProcessor>(Lifestyle.Scoped);
        }
        
        public static void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder, HttpQueryRunner httpQueryRunner)
        {
            var r = new HttpRoutesRegistrar<UserConfigModule>(httpQueryRunner);
            
            r.RegisterProcessor<ByIdReq, RpcResponse<UserConfigEditModel>, UserConfigGetProcessor>("/user/config/get");
            r.RegisterProcessor<UserConfigSetReq, RpcResponse<bool>, UserConfigSetProcessor>("/user/config/set");
            
            r.Commit("/user/config/openapi.json", OpenApiSpecVersion.OpenApi2_0, endpointRouteBuilder);
        }
    }
}