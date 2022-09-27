namespace Cen.Wms.Host.Sync.Jobs
{
    public class SyncPacsUploadInvokeJobOptions
    {
        public static string SectionName => "Sync:Host:Jobs:SyncPacsUploadInvokeJob";

        public string Url { get; set; }
        public int TimeoutMs { get; set; }
    }
}