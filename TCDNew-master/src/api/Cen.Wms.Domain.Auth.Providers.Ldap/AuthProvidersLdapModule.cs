using SimpleInjector;
using Cen.Wms.Domain.Auth.Providers.Abstract;
using Cen.Wms.Domain.Auth.Providers.Models;
using Microsoft.Extensions.Configuration;

namespace Cen.Wms.Domain.Auth.Providers.Ldap
{
    public class AuthProvidersLdapModule
    {
        public static void RegisterTypes(IConfiguration configuration, Container container)
        {
            container.Register(
                () => configuration.GetSection(LdapUserAuthenticatorOptions.SectionName).Get<LdapUserAuthenticatorOptions>(),
                Lifestyle.Singleton
            );
            container.Register<IUserAuthenticator<UserCredentials>, LdapUserAuthenticator>(Lifestyle.Scoped);
        }
    }
}