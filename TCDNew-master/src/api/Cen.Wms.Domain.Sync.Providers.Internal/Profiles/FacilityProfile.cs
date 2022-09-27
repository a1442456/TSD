using AutoMapper;
using Cen.Wms.Domain.Facility.Manage.Models;
using Cen.Wms.Domain.Sync.Models;

namespace Cen.Wms.Domain.Sync.Providers.Internal.Profiles
{
    public class FacilityProfile: Profile
    {
        public FacilityProfile()
        {
            CreateMap<FacilityExt, FacilityEditModel>()
                .ForMember(d => d.ExtId, memberConfigurationExpression => memberConfigurationExpression.MapFrom(s => s.FacilityId))
                .ForMember(d => d.Name, memberConfigurationExpression => memberConfigurationExpression.MapFrom(s => s.FacilityName))
                .ForMember(e => e.Id, m => m.Ignore());
        }
    }
}