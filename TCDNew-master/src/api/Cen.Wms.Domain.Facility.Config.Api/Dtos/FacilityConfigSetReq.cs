using Cen.Common.Domain.Models;
using Cen.Wms.Domain.Facility.Config.Models;

namespace Cen.Wms.Domain.Facility.Config.Api.Dtos
{
    public class FacilityConfigSetReq
    {
        public ByIdReq FacilityId { get; set; }
        public FacilityConfigEditModel FacilityConfigEditModel { get; set; }
    }
}