using System.Threading.Tasks;
using AutoMapper;
using Cen.Common.CQRS;
using Cen.Wms.Data.Context;
using Cen.Wms.Data.Models.Device;
using Cen.Wms.Domain.Device.Manage.Abstract;
using Cen.Wms.Domain.Device.Manage.Models;
using Cen.Wms.Domain.Device.Manage.Enums;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Cen.Wms.Domain.Device.Manage.Store.EntityFramework
{
    public class EntityFrameworkDeviceRepository: IDeviceRepository
    {
        private readonly WmsContext _wmsContext;
        private readonly IMapper _mapper;

        public EntityFrameworkDeviceRepository(WmsContext wmsContext, IMapper mapper)
        {
            _wmsContext = wmsContext;
            _mapper = mapper;
        }

        public async Task<RpcResponse<bool>> DeviceStore(DeviceIdentifierDto request, DeviceStatus defaultDeviceStatus)
        {
            var deviceRow = await _wmsContext.Device.FirstOrDefaultAsync(e => e.DevicePublicKey == request.DevicePublicKey);
            if (deviceRow == null)
            {
                var newId = NewId.NextGuid();
                deviceRow = _mapper.Map<DeviceRow>(request);
                deviceRow.Id = newId;
                await _wmsContext.Device.AddAsync(deviceRow);
                await _wmsContext.SaveChangesAsync();
                
                var deviceStateRow = new DeviceStateRow {Id = newId, DevicePublicKey = request.DevicePublicKey, DeviceStatus = defaultDeviceStatus};
                await _wmsContext.DeviceState.AddAsync(deviceStateRow);
                await _wmsContext.SaveChangesAsync();
            }

            return RpcResponse<bool>.WithSuccess(true);
        }

        public async Task<RpcResponse<DeviceStatus>> DeviceStateGet(DeviceIdentifierDto request)
        {
            var deviceStateRow = await _wmsContext.DeviceState.FirstOrDefaultAsync(e => e.DevicePublicKey == request.DevicePublicKey);
            return
                deviceStateRow != null
                    ? RpcResponse<DeviceStatus>.WithSuccess(deviceStateRow.DeviceStatus)
                    : RpcResponse<DeviceStatus>.WithSuccess(DeviceStatus.Missing);
        }

        public async Task<RpcResponse<bool>> DeviceStatusSet(DeviceIdentifierDto request, DeviceStatus deviceStatus)
        {
            var deviceStateRow = await _wmsContext.DeviceState.FirstOrDefaultAsync(e => e.DevicePublicKey == request.DevicePublicKey);
            if (deviceStateRow == null)
            {
                deviceStateRow = new DeviceStateRow {Id = NewId.NextGuid(), DevicePublicKey = request.DevicePublicKey, DeviceStatus = deviceStatus};
                await _wmsContext.DeviceState.AddAsync(deviceStateRow);
                await _wmsContext.SaveChangesAsync();
            }
            else
            {
                deviceStateRow.DeviceStatus = deviceStatus;
                _wmsContext.DeviceState.Update(deviceStateRow);
            }
            
            return RpcResponse<bool>.WithSuccess(true);
        }

        public async Task<RpcResponse<bool>> DeviceDelete(DeviceIdentifierDto request)
        {
            var deviceRow = await _wmsContext.Device.FirstOrDefaultAsync(e => e.DevicePublicKey == request.DevicePublicKey);
            var deviceStateRow = await _wmsContext.DeviceState.FirstOrDefaultAsync(e => e.DevicePublicKey == request.DevicePublicKey);
            
            if (deviceStateRow != null)
            {
                _wmsContext.DeviceState.Remove(deviceStateRow);
            }
            if (deviceRow != null)
            {
                _wmsContext.Device.Remove(deviceRow);
            }

            return RpcResponse<bool>.WithSuccess(true);
        }
    }
}