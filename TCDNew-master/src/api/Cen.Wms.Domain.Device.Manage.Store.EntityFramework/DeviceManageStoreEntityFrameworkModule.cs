using SimpleInjector;
using AutoMapper;
using Cen.Wms.Domain.Device.Manage.Abstract;
using Cen.Wms.Domain.Device.Manage.Store.EntityFramework.Profiles;
using Microsoft.Extensions.Configuration;

namespace Cen.Wms.Domain.Device.Manage.Store.EntityFramework
{
    public class DeviceManageStoreEntityFrameworkModule
    {
        public static void RegisterTypes(IConfiguration configuration, Container container)
        {
            container.Register<IRegistrationRequestRepository, EntityFrameworkRegistrationRequestRepository>(Lifestyle.Scoped);
            container.Register<IDeviceRepository, EntityFrameworkDeviceRepository>(Lifestyle.Scoped);
        }
        
        public static void RegisterProfiles(IMapperConfigurationExpression mapperConfigurationExpression)
        {
            mapperConfigurationExpression.AddProfile<DeviceProfile>();
            mapperConfigurationExpression.AddProfile<DeviceRegistrationRequestProfile>();
        }
    }
}