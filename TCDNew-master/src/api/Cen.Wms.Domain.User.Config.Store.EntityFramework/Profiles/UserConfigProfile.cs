using AutoMapper;
using Cen.Wms.Data.Models.User;
using Cen.Wms.Domain.User.Config.Models;

namespace Cen.Wms.Domain.User.Config.Store.EntityFramework.Profiles
{
    public class UserConfigProfile: Profile
    {
        public UserConfigProfile()
        {
            CreateMap<UserConfigEditModel, UserConfigRow>()
                .ForMember(e => e.Id, m => m.Ignore());
            CreateMap<UserConfigRow, UserConfigEditModel>();
        }
    }
}