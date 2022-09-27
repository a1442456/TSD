using SimpleInjector;
using Cen.Common.Sync.Interfaces;
using Cen.Wms.Domain.Sync.Models;
using Cen.Wms.Domain.Sync.Providers.Fake.Sources;
using Microsoft.Extensions.Configuration;

namespace Cen.Wms.Domain.Sync.Providers.Fake
{
    public class SyncProvidersFakeModule
    {
        public static void RegisterTypes(IConfiguration configuration, Container container)
        {
            container.Register(
                () => configuration.GetSection(SyncProvidersFakeOptions.SectionName).Get<SyncProvidersFakeOptions>(),
                Lifestyle.Singleton
            );
            container.Register<ISyncSource<object, FacilityExt>, FakeFacilityExtSource>(Lifestyle.Scoped);
            container.Register<ISyncSource<object, UserExt>, FakeUserExtSource>(Lifestyle.Scoped);
            container.Register<ISyncSource<ReqPacInterval, PacExt>, FakePacExtSource>(Lifestyle.Scoped);
        }
    }
}