using Cen.Common.Domain.Models;
using Cen.Wms.Domain.Device.Manage.Enums;

namespace Cen.Wms.Data.Models.Device
{
    public class DeviceStateRow: DataModel
    {
        public string DevicePublicKey { get; set; }
        public DeviceStatus DeviceStatus { get; set; }
    }
}