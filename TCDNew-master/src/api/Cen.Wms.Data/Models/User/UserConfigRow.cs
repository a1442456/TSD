using System;
using Cen.Common.Domain.Models;

namespace Cen.Wms.Data.Models.User
{
    public class UserConfigRow: DataModel
    {
        public Guid DefaultFacilityId { get; set; }
    }
}