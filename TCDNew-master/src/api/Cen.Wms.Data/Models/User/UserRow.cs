using Cen.Common.Domain.Interfaces;
using Cen.Common.Domain.Models;
using NodaTime;

namespace Cen.Wms.Data.Models.User
{
    public class UserRow: DataModelWithName, ISyncable
    {
        public string Login { get; set; }
        public bool IsLockedExt { get; set; }
        public string FacilityExtId { get; set; }
        public string FacilityName { get; set; }
        public string DepartmentExtId { get; set; }
        public string DepartmentName { get; set; }
        public string PositionExtId { get; set; }
        public string PositionName { get; set; }
        public string ExtId { get; set; }
        public Instant ChangedAt { get; set; }
    }
}