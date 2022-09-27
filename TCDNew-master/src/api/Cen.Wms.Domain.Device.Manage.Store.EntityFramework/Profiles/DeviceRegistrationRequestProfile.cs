using AutoMapper;
using Cen.Wms.Data.Models.Device;

namespace Cen.Wms.Domain.Device.Manage.Store.EntityFramework.Profiles
{
    public class DeviceRegistrationRequestProfile: Profile
    {
        public DeviceRegistrationRequestProfile()
        {
            CreateMap<Models.DeviceRegistrationRequest, DeviceRegistrationRequestRow>()
                .ForMember(e => e.Id, m => m.Ignore());
            CreateMap<DeviceRegistrationRequestRow, Models.DeviceRegistrationRequest>();
        }
    }
}