using SimpleInjector;
using AutoMapper;
using Cen.Common.Sync.Interfaces;
using Cen.Wms.Domain.Sync.Models;
using Cen.Wms.Domain.Sync.Providers.Internal.Destinations;
using Cen.Wms.Domain.Sync.Providers.Internal.Profiles;
using Microsoft.Extensions.Configuration;

namespace Cen.Wms.Domain.Sync.Providers.Internal
{
    public class SyncProvidersInternalModule
    {
        public static void RegisterTypes(IConfiguration configuration, Container container)
        {
            container.Register<ISyncDestination<object, FacilityExt>, InternalFacilityDestination>(Lifestyle.Scoped);
            container.Register<ISyncDestination<object, UserExt>, InternalUserDestination>(Lifestyle.Scoped);
            container.Register<ISyncDestination<ReqPacInterval, PacExt>, InternalPacDestination>(Lifestyle.Scoped);
        }
        
        public static void RegisterProfiles(IMapperConfigurationExpression mapperConfigurationExpression)
        {
            mapperConfigurationExpression.AddProfile<FacilityProfile>();
            mapperConfigurationExpression.AddProfile<UserProfile>();
            mapperConfigurationExpression.AddProfile<PacProfile>();
        }
    }
}