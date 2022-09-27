using System.Collections.Generic;
using NodaTime;

namespace Cen.Wms.Domain.Sync.Models
{
    public class PacResultExt
    {
        public string PacId { get; set; }
        public string PacStatus { get; set; }
        public string ResponsibleUserId { get; set; }
        public Instant ChangedAt { get; set; }
        public Instant StartedAt { get; set; }
        public List<PacResultLineExt> Lines { get; set; }
    }
}