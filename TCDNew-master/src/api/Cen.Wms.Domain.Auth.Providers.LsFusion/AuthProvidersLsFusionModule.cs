using SimpleInjector;
using Cen.Wms.Domain.Auth.Providers.Abstract;
using Cen.Wms.Domain.Auth.Providers.Models;
using Microsoft.Extensions.Configuration;

namespace Cen.Wms.Domain.Auth.Providers.LsFusion
{
    public class AuthProvidersLsFusionModule
    {
        public static void RegisterTypes(IConfiguration configuration, Container container)
        {
            container.Register(
                () => configuration.GetSection(LsFusionUserAuthenticatorOptions.SectionName).Get<LsFusionUserAuthenticatorOptions>(),
                Lifestyle.Singleton
            );
            container.Register<IUserAuthenticator<UserCredentials>, LsFusionUserAuthenticator>(Lifestyle.Scoped);
        }
    }
}