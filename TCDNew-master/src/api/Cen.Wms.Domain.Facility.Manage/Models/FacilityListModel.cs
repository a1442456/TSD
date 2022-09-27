using System;
using NodaTime;

namespace Cen.Wms.Domain.Facility.Manage.Models
{
    public class FacilityListModel
    {
        public Guid Id { get; set; }
        public string ExtId { get; set; }
        public string Name { get; set; }
        public Instant ChangedAt { get; set; }
    }
}