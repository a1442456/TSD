using AutoMapper;
using Cen.Wms.Domain.Purchase.Models;
using Cen.Wms.Domain.Sync.Models;

namespace Cen.Wms.Domain.Sync.Providers.Internal.Profiles
{
    public class PacProfile: Profile
    {
        public PacProfile()
        {
            CreateMap<PacExt, PacHeadEditModel>()
                .ForMember(e => e.Id, m => m.Ignore())
                .ForMember(e => e.ExtId, m => m.MapFrom(s => s.PacId));
            CreateMap<PacLineExt, PacLineEditModel>()
                .ForMember(e => e.ExtId, m => m.MapFrom(s => s.PacLineId));
        }
    }
}