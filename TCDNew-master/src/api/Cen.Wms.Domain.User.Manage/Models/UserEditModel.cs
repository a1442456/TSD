using System;
using NodaTime;

namespace Cen.Wms.Domain.User.Manage.Models
{
    public class UserEditModel
    {
        public Guid Id { get; set; }
        public string ExtId { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public bool IsLocked { get; set; }
        public string FacilityExtId { get; set; }
        public string FacilityName { get; set; }
        public string DepartmentExtId { get; set; }
        public string DepartmentName { get; set; }
        public string PositionExtId { get; set; }
        public string PositionName { get; set; }
        public Instant ChangedAt { get; set; }
    }
}