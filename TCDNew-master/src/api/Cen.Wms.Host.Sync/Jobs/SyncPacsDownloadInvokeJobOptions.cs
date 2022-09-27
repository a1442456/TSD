namespace Cen.Wms.Host.Sync.Jobs
{
    public class SyncPacsDownloadInvokeJobOptions
    {
        public static string SectionName => "Sync:Host:Jobs:SyncPacsDownloadInvokeJob";

        public string Url { get; set; }
        public int TimeoutMs { get; set; }
        public int DaysBack { get; set; }
        public int DaysForward { get; set; }
    }
}