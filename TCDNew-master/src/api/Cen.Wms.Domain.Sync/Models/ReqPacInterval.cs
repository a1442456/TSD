using NodaTime;

namespace Cen.Wms.Domain.Sync.Models
{
    public class ReqPacInterval
    {
        public Instant PacDateTimeFrom { get; set; }

        public Instant PacDateTimeTo { get; set; }
    }
}