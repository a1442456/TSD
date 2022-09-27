using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Data.EntityFramework;
using Cen.Common.Errors;
using Cen.Wms.Data.Context;
using Cen.Wms.Domain.Device.Manage.Abstract;
using Cen.Wms.Domain.Device.Manage.Models;
using Cen.Wms.Domain.Device.Manage.Enums;
using Serilog;

namespace Cen.Wms.Domain.Device.Manage.Api.Processors
{
    public class DeviceLockProcessor: IQueryProcessor<DeviceIdentifierDto, RpcResponse<bool>>
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork<WmsContext> _unitOfWork;
        private readonly IDeviceRepository _deviceRepository;

        public DeviceLockProcessor(
            ILogger logger, UnitOfWork<WmsContext> unitOfWork,
            IDeviceRepository deviceRepository)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _deviceRepository = deviceRepository;
        }

        public async Task<RpcResponse<bool>> Run(IUserIdProvider userIdProvider, DeviceIdentifierDto request)
        {
            var deviceStateResult = await _deviceRepository.DeviceStateGet(request);
            if (!deviceStateResult.IsSuccess)
                return RpcResponse<bool>.WithErrors(false, deviceStateResult.Errors);
            if (deviceStateResult.Data == DeviceStatus.Missing)
                return RpcResponse<bool>.WithError(false, CommonErrors.NotFound("устройство"));
            if (deviceStateResult.Data != DeviceStatus.Active)
                return RpcResponse<bool>.WithError(false, CommonErrors.InvalidOperation);
            
            var deviceStateSetResult = await _deviceRepository.DeviceStatusSet(request, DeviceStatus.Locked);
            
            await _unitOfWork.Context.SaveChangesAsync();
            _unitOfWork.Commit();
            
            return deviceStateSetResult;
        }
    }
}