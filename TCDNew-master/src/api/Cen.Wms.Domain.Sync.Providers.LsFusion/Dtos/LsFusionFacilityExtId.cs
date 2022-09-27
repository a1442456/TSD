using NodaTime;

namespace Cen.Wms.Domain.Sync.Providers.LsFusion.Dtos
{
    public class LsFusionFacilityExtId
    {
        public string FacilityId { get; set; }
        public Instant ChangedAt { get; set; }
    }
}