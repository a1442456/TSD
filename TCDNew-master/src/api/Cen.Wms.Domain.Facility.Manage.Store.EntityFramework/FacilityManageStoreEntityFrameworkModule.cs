using SimpleInjector;
using AutoMapper;
using Cen.Wms.Domain.Facility.Manage.Abstract;
using Cen.Wms.Domain.Facility.Manage.Store.EntityFramework.Profiles;
using Microsoft.Extensions.Configuration;

namespace Cen.Wms.Domain.Facility.Manage.Store.EntityFramework
{
    public class FacilityManageStoreEntityFrameworkModule
    {
        public static void RegisterTypes(IConfiguration configuration, Container container)
        {
            container.Register<IFacilityRepository, EntityFrameworkFacilityRepository>(Lifestyle.Scoped);
        }
        
        public static void RegisterProfiles(IMapperConfigurationExpression mapperConfigurationExpression)
        {
            mapperConfigurationExpression.AddProfile<FacilityProfile>();
        }
    }
}