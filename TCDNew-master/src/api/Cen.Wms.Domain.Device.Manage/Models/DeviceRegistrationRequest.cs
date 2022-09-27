using NodaTime;

namespace Cen.Wms.Domain.Device.Manage.Models
{
    public class DeviceRegistrationRequest
    {
        private readonly Instant _createdAt;
        private readonly string _devicePublicKey;

        public DeviceRegistrationRequest(string devicePublicKey, Instant createdAt)
        {
            _devicePublicKey = devicePublicKey;
            _createdAt = createdAt;
        }

        public string DevicePublicKey => _devicePublicKey;

        public Instant CreatedAt => _createdAt;
    }
}