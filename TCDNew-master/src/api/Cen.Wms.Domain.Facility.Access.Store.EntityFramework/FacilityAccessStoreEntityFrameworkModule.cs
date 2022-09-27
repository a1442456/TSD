using SimpleInjector;
using Cen.Wms.Domain.Facility.Access.Abstract;
using Microsoft.Extensions.Configuration;

namespace Cen.Wms.Domain.Facility.Access.Store.EntityFramework
{
    public class FacilityAccessStoreEntityFrameworkModule
    {
        public static void RegisterTypes(IConfiguration configuration, Container container)
        {
            container.Register<IFacilityAccessRepository, EntityFrameworkFacilityAccessRepository>(Lifestyle.Scoped);
        }
    }
}