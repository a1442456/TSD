using NodaTime;

namespace Cen.Wms.Domain.Sync.Models
{
    public class FacilityExt
    {
        public string FacilityId { get; set; }
        public string FacilityName { get; set; }
        public Instant ChangedAt { get; set; }
    }
}