namespace Cen.Wms.Host.Sync
{
    public class SyncSchedulerHostedServiceOptions
    {
        public static string SectionName => "Sync:Host";
        public bool SyncCatalogsDownloadInvokeJobEnabled { get; set; }
        public int SyncCatalogsDownloadInvokeJobIntervalInSeconds { get; set; }
        public bool SyncPacsDownloadInvokeJobEnabled { get; set; }
        public int SyncPacsDownloadInvokeJobIntervalInSeconds { get; set; }
        public bool SyncPacsUploadInvokeJobEnabled { get; set; }
        public int SyncPacsUploadInvokeJobIntervalInSeconds { get; set; }

    }
}