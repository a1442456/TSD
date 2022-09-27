using SimpleInjector;
using AutoMapper;
using Cen.Wms.Domain.User.Manage.Abstract;
using Cen.Wms.Domain.User.Manage.Store.EntityFramework.Profiles;
using Microsoft.Extensions.Configuration;

namespace Cen.Wms.Domain.User.Manage.Store.EntityFramework
{
    public class UserManageStoreEntityFrameworkModule
    {
        public static void RegisterTypes(IConfiguration configuration, Container container)
        {
            container.Register<IUserRepository, EntityFrameworkUserRepository>(Lifestyle.Scoped);
        }
        
        public static void RegisterProfiles(IMapperConfigurationExpression mapperConfigurationExpression)
        {
            mapperConfigurationExpression.AddProfile<UserProfile>();
            mapperConfigurationExpression.AddProfile<UserStateProfile>();
        }
    }
}