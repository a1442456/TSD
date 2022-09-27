namespace Cen.Wms.Host.Sync.Jobs
{
    public class SyncCatalogsDownloadInvokeJobOptions
    {
        public static string SectionName => "Sync:Host:Jobs:SyncCatalogsDownloadInvokeJob";

        public string Url { get; set; }
        public int TimeoutMs { get; set; }
    }
}