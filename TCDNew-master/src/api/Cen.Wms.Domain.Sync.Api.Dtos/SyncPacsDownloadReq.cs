using NodaTime;

namespace Cen.Wms.Domain.Sync.Api.Dtos
{
    public class SyncPacsDownloadReq
    {
        public Instant PacDateTimeFrom { get; set; }

        public Instant PacDateTimeTo { get; set; }
    }
}