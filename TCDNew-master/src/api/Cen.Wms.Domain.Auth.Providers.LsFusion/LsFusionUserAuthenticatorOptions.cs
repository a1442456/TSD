namespace Cen.Wms.Domain.Auth.Providers.LsFusion
{
    public class LsFusionUserAuthenticatorOptions
    {
        public static string SectionName => "Auth:Providers:LsFusion";
        public string WMSServiceBaseAddress { get; set; }
        public int TimeoutMs { get; set; }
    }
}