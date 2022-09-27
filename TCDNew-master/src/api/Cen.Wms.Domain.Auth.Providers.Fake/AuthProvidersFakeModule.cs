using SimpleInjector;
using Cen.Wms.Domain.Auth.Providers.Abstract;
using Cen.Wms.Domain.Auth.Providers.Models;

namespace Cen.Wms.Domain.Auth.Providers.Fake
{
    public class AuthProvidersFakeModule
    {
        public static void RegisterTypes(Container container)
        {
            container.Register<IUserAuthenticator<UserCredentials>, FakeUserAuthenticator>(Lifestyle.Singleton);
        }
    }
}