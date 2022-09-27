using NodaTime;

namespace Cen.Wms.Domain.Sync.Models
{
    public class UserExt
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserLogin { get; set; }
        public bool IsLocked { get; set; }
        public string FacilityId { get; set; }
        public string FacilityName { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string PositionId { get; set; }
        public string PositionName { get; set; }
        public Instant ChangedAt { get; set; }
    }
}