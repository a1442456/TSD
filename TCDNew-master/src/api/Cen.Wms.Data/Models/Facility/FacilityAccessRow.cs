using System;
using Cen.Common.Domain.Models;

namespace Cen.Wms.Data.Models.Facility
{
    public class FacilityAccessRow: DataModel
    {
        public Guid UserId { get; set; }
        public Guid FacilityId { get; set; }
        public bool IsManual { get; set; }
    }
}