using SimpleInjector;
using AutoMapper;
using Cen.Wms.Domain.User.Config.Abstract;
using Cen.Wms.Domain.User.Config.Store.EntityFramework.Profiles;
using Microsoft.Extensions.Configuration;

namespace Cen.Wms.Domain.User.Config.Store.EntityFramework
{
    public class UserConfigStoreEntityFrameworkModule
    {
        public static void RegisterTypes(IConfiguration configuration, Container container)
        {
            container.Register<IUserConfigRepository, EntityFrameworkUserConfigRepository>(Lifestyle.Scoped);
        }
        
        public static void RegisterProfiles(IMapperConfigurationExpression mapperConfigurationExpression)
        {
            mapperConfigurationExpression.AddProfile<UserConfigProfile>();
        }
    }
}