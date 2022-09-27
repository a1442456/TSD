using NodaTime;

namespace Cen.Wms.Domain.Sync.Providers.LsFusion.Dtos
{
    public class LsFusionPacExtId
    {
        public string PacId { get; set; }
        public Instant ChangedAt { get; set; }
    }
}