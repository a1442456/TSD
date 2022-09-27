using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Wms.Domain.Device.Manage.Models;
using Cen.Wms.Domain.Device.Manage.Enums;

namespace Cen.Wms.Domain.Device.Manage.Abstract
{
    public interface IDeviceRepository
    {
        public Task<RpcResponse<bool>> DeviceStore(DeviceIdentifierDto request, DeviceStatus defaultDeviceStatus);
        public Task<RpcResponse<DeviceStatus>> DeviceStateGet(DeviceIdentifierDto request);
        public Task<RpcResponse<bool>> DeviceStatusSet(DeviceIdentifierDto request, DeviceStatus deviceStatus);
        public Task<RpcResponse<bool>> DeviceDelete(DeviceIdentifierDto request);
    }
}