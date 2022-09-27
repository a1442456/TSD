namespace Cen.Wms.Domain.Sync.Providers.LsFusion
{
    public class SyncProvidersLsFusionOptions
    {
        public static string SectionName => "Sync:Providers:LsFusion";
        public string WMSServiceBaseAddress { get; set; }
        public string WMSServiceBaseLogin { get; set; }
        public string WMSServiceBasePassword { get; set; }
        public int BatchSize = 25;
        public int TimeoutMs { get; set; }
        public string SyncLogsFolderPath { get; set; }
    }
}