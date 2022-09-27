using SimpleInjector;
using Cen.Common.Sync.Interfaces;
using Cen.Wms.Domain.Sync.Abstract;
using Cen.Wms.Domain.Sync.Models;
using Cen.Wms.Domain.Sync.Providers.LsFusion.Domain;
using Cen.Wms.Domain.Sync.Providers.LsFusion.Sources;
using Microsoft.Extensions.Configuration;

namespace Cen.Wms.Domain.Sync.Providers.LsFusion
{
    public class SyncProvidersLsFusionModule
    {
        public static void RegisterTypes(IConfiguration configuration, Container container)
        {
            container.Register(
                () => configuration.GetSection(SyncProvidersLsFusionOptions.SectionName).Get<SyncProvidersLsFusionOptions>(),
                Lifestyle.Singleton
            );
            container.Register<ISyncSource<object, FacilityExt>, LsFusionFacilityExtSource>(Lifestyle.Scoped);
            container.Register<ISyncSource<object, UserExt>, LsFusionUserExtSource>(Lifestyle.Scoped);
            container.Register<ISyncSource<ReqPacInterval, PacExt>, LsFusionPacExtSource>(Lifestyle.Scoped);
            container.Register<IPacUploader, LsFusionPacUploader>(Lifestyle.Scoped);
        }
    }
}