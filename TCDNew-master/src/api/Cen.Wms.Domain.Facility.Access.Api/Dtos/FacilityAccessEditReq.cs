using Cen.Common.Domain.Models;

namespace Cen.Wms.Domain.Facility.Access.Api.Dtos
{
    public class FacilityAccessEditReq
    {
        public ByIdReq FacilityId { get; set; }
        public ByIdReq UserId { get; set; }
    }
}