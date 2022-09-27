using NodaTime;

namespace Cen.Wms.Domain.Sync.Providers.LsFusion.Dtos
{
    public class LsFusionFacilityExt
    {
        public string FacilityId { get; set; }
        public string FacilityName { get; set; }
        public Instant ChangedAt { get; set; }
    }
}