using AutoMapper;
using Cen.Wms.Domain.Sync.Models;
using Cen.Wms.Domain.User.Manage.Models;

namespace Cen.Wms.Domain.Sync.Providers.Internal.Profiles
{
    public class UserProfile: Profile
    {
        public UserProfile()
        {
            CreateMap<UserExt, UserEditModel>()
                .ForMember(d => d.PositionExtId, memberConfigurationExpression => memberConfigurationExpression.MapFrom(s => s.PositionId))
                .ForMember(d => d.DepartmentExtId, memberConfigurationExpression => memberConfigurationExpression.MapFrom(s => s.DepartmentId))
                .ForMember(d => d.FacilityExtId, memberConfigurationExpression => memberConfigurationExpression.MapFrom(s => s.FacilityId))
                .ForMember(d => d.ExtId, memberConfigurationExpression => memberConfigurationExpression.MapFrom(s => s.UserId))
                .ForMember(d => d.Login, memberConfigurationExpression => memberConfigurationExpression.MapFrom(s => s.UserLogin))
                .ForMember(d => d.Name, memberConfigurationExpression => memberConfigurationExpression.MapFrom(s => s.UserName))
                .ForMember(d => d.Id, m => m.Ignore());
        }
    }
}