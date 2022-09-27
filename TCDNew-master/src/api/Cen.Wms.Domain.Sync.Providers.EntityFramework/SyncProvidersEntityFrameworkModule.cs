using SimpleInjector;
using Cen.Common.Sync.Interfaces;
using Cen.Wms.Domain.Sync.Providers.EntityFramework.Infrastructures;

namespace Cen.Wms.Domain.Sync.Providers.EntityFramework
{
    public class SyncProvidersEntityFrameworkModule
    {
        public static void RegisterTypes(Container container)
        {
            container.Register<ISyncSession, EntityFrameworkSyncSession>(Lifestyle.Scoped);
            container.Register<ISyncPositionsStore, EntityFrameworkSyncPositionStore>(Lifestyle.Scoped);
        }
    }
}