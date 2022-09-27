using AutoMapper;
using Cen.Wms.Data.Models.User;
using Cen.Wms.Domain.User.Manage.Models;

namespace Cen.Wms.Domain.User.Manage.Store.EntityFramework.Profiles
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<UserEditModel, UserRow>()
                .ForMember(e => e.IsLockedExt, m => m.MapFrom(e => e.IsLocked))
                .ForMember(e => e.Id, m => m.Ignore());
            CreateMap<UserRow, UserListModel>();
            CreateMap<UserRow, UserEditModel>()
                .ForMember(e => e.IsLocked, m => m.MapFrom(e => e.IsLockedExt));
        }
    }
}