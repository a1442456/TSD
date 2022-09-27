using Cen.Common.Domain.Interfaces;
using Cen.Common.Domain.Models;
using NodaTime;

namespace Cen.Wms.Data.Models.Facility
{
    public class FacilityRow: DataModelWithName, ISyncable
    {
        public string ExtId { get; set; }
        public Instant ChangedAt { get; set; }
    }
}