using AutoMapper;
using Cen.Wms.Data.Models.Device;
using Cen.Wms.Domain.Device.Manage.Models;

namespace Cen.Wms.Domain.Device.Manage.Store.EntityFramework.Profiles
{
    public class DeviceProfile: Profile
    {
        public DeviceProfile()
        {
            CreateMap<DeviceIdentifierDto, DeviceRow>()
                .ForMember(e => e.Id, m => m.Ignore());
        }
    }
}