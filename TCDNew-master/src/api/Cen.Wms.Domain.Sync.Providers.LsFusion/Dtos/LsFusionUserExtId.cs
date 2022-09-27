using NodaTime;

namespace Cen.Wms.Domain.Sync.Providers.LsFusion.Dtos
{
    public class LsFusionUserExtId
    {
        public string UserId { get; set; }
        public Instant ChangedAt { get; set; }
    }
}