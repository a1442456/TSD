using AutoMapper;
using Cen.Wms.Data.Models.User;
using Cen.Wms.Domain.User.Manage.Models;

namespace Cen.Wms.Domain.User.Manage.Store.EntityFramework.Profiles
{
    public class UserStateProfile: Profile
    {
        public UserStateProfile()
        {
            CreateMap<UserStateEditModel, UserStateRow>()
                .ForMember(e => e.Id, m => m.Ignore());
            CreateMap<UserStateRow, UserStateEditModel>();
        }
    }
}