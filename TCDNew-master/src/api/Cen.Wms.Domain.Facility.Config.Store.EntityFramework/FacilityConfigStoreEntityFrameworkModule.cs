using SimpleInjector;
using AutoMapper;
using Cen.Wms.Domain.Facility.Config.Abstract;
using Cen.Wms.Domain.Facility.Config.Store.EntityFramework.Profiles;
using Microsoft.Extensions.Configuration;

namespace Cen.Wms.Domain.Facility.Config.Store.EntityFramework
{
    public class FacilityConfigStoreEntityFrameworkModule
    {
        public static void RegisterTypes(IConfiguration configuration, Container container)
        {
            container.Register<IFacilityConfigRepository, EntityFrameworkFacilityConfigRepository>(Lifestyle.Scoped);
        }
        
        public static void RegisterProfiles(IMapperConfigurationExpression mapperConfigurationExpression)
        {
            mapperConfigurationExpression.AddProfile<FacilityConfigProfile>();
        }
    }
}