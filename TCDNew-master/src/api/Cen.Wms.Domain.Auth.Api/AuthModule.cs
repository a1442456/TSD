using SimpleInjector;
using Cen.Common.CQRS;
using Cen.Common.Http.Server.CQRS;
using Cen.Common.Http.Server.CQRS.OpenApi;
using Cen.Wms.Domain.Auth.Api.Dtos;
using Cen.Wms.Domain.Auth.Api.Processors;
using Cen.Wms.Domain.Auth.Providers.Models;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi;

namespace Cen.Wms.Domain.Auth.Api
{
    public class AuthModule
    {
        public static void RegisterTypes(IConfiguration configuration, Container container)
        {
            container.Register(
                () => configuration.GetSection(AuthOptions.SectionName).Get<AuthOptions>(),
                Lifestyle.Singleton
            );
            container.Register<GetServerPublicKeyProcessor>(Lifestyle.Scoped);
            container.Register<LoginQueryProcessor>(Lifestyle.Scoped);
        }

        public static void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder, HttpQueryRunner httpQueryRunner)
        {
            var r = new HttpRoutesRegistrar<AuthModule>(httpQueryRunner);
            
            r.RegisterProcessor<object, RpcResponse<ServerPublicKeyResp>, GetServerPublicKeyProcessor>("/auth/pk/get");
            r.RegisterProcessor<UserCredentials, RpcResponse<UserTokenResp>, LoginQueryProcessor>("/auth/login");
            
            r.Commit("/auth/openapi.json", OpenApiSpecVersion.OpenApi2_0, endpointRouteBuilder);
        }
    }
}