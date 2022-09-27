using AutoMapper;
using Cen.Wms.Data.Models.Facility;
using Cen.Wms.Domain.Facility.Manage.Models;

namespace Cen.Wms.Domain.Facility.Manage.Store.EntityFramework.Profiles
{
    public class FacilityProfile: Profile
    {
        public FacilityProfile()
        {
            CreateMap<FacilityEditModel, FacilityRow>()
                .ForMember(e => e.Id, m => m.Ignore());
            CreateMap<FacilityRow, FacilityEditModel>();
            CreateMap<FacilityRow, FacilityListModel>();
        }
    }
}