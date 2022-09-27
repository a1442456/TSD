namespace Cen.Wms.Domain.Auth.Providers.Ldap
{
    public class LdapUserAuthenticatorOptions
    {
        public static string SectionName => "Auth:Providers:Ldap";
        public string LdapHost { get; set; }
        public int LdapPort { get; set; }
        public string LdapUserRdnTemplate { get; set; }
    }
}