using System;
using SimpleInjector;
using Cen.Common.CQRS;
using Cen.Common.Domain.Models;
using Cen.Common.Http.Server.CQRS;
using Cen.Common.Http.Server.CQRS.OpenApi;
using Cen.Wms.Domain.Device.Manage.Api.Processors;
using Cen.Wms.Domain.Device.Manage.Models;
using Cen.Wms.Domain.Device.Manage.Enums;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi;

namespace Cen.Wms.Domain.Device.Manage.Api
{
    public class DeviceManageModule
    {
        public static void RegisterTypes(Container container)
        {
            container.Register<RegistrationRequestStoreProcessor>(Lifestyle.Scoped);
            container.Register<RegistrationRequestAcceptProcessor>(Lifestyle.Scoped);
            container.Register<RegistrationRequestDeclineProcessor>(Lifestyle.Scoped);
            container.Register<DeviceStateGetProcessor>(Lifestyle.Scoped);
            container.Register<DeviceLockProcessor>(Lifestyle.Scoped);
            container.Register<DeviceUnlockProcessor>(Lifestyle.Scoped);
            container.Register<DeviceDeleteProcessor>(Lifestyle.Scoped);
        }
        
        public static void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder, HttpQueryRunner httpQueryRunner)
        {
            var r = new HttpRoutesRegistrar<DeviceManageModule>(httpQueryRunner);
            
            r.RegisterProcessor<DeviceIdentifierDto, RpcResponse<Guid>, RegistrationRequestStoreProcessor>("/device/reg/req/store");
            r.RegisterProcessor<ByIdReq, RpcResponse<bool>, RegistrationRequestAcceptProcessor>("/device/reg/req/accept");
            r.RegisterProcessor<ByIdReq, RpcResponse<bool>, RegistrationRequestDeclineProcessor>("/device/reg/req/decline");
            r.RegisterProcessor<DeviceIdentifierDto, RpcResponse<DeviceStatus>, DeviceStateGetProcessor>("/device/state/get");
            r.RegisterProcessor<DeviceIdentifierDto, RpcResponse<bool>, DeviceLockProcessor>("/device/state/set/lock");
            r.RegisterProcessor<DeviceIdentifierDto, RpcResponse<bool>, DeviceUnlockProcessor>("/device/state/set/unlock");
            r.RegisterProcessor<DeviceIdentifierDto, RpcResponse<bool>, DeviceDeleteProcessor>("/device/delete");
            
            r.Commit("/device/openapi.json", OpenApiSpecVersion.OpenApi2_0, endpointRouteBuilder);
        }
    }
}