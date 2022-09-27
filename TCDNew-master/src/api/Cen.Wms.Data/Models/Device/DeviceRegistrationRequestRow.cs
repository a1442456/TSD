using Cen.Common.Domain.Models;
using NodaTime;

namespace Cen.Wms.Data.Models.Device
{
    public class DeviceRegistrationRequestRow: DataModel
    {
        public string DevicePublicKey { get; set; }
        public Instant CreatedAt { get; set; }
    }
}