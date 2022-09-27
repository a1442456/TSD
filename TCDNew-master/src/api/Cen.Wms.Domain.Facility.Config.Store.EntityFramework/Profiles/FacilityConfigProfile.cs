using AutoMapper;
using Cen.Wms.Data.Models.Facility;
using Cen.Wms.Domain.Facility.Config.Models;

namespace Cen.Wms.Domain.Facility.Config.Store.EntityFramework.Profiles
{
    public class FacilityConfigProfile: Profile
    {
        public FacilityConfigProfile()
        {
            CreateMap<FacilityConfigEditModel, FacilityConfigRow>();
            CreateMap<FacilityConfigRow, FacilityConfigEditModel>();
        }
    }
}