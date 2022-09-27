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
    public class DeviceDeleteProcessor: IQueryProcessor<DeviceIdentifierDto, RpcResponse<bool>>
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork<WmsContext> _unitOfWork;
        private readonly IDeviceRepository _deviceRepository;
        private readonly IRegistrationRequestRepository _registrationRequestRepository;

        public DeviceDeleteProcessor(
            ILogger logger, UnitOfWork<WmsContext> unitOfWork,
            IDeviceRepository deviceRepository, IRegistrationRequestRepository registrationRequestRepository)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _deviceRepository = deviceRepository;
            _registrationRequestRepository = registrationRequestRepository;
        }

        public async Task<RpcResponse<bool>> Run(IUserIdProvider userIdProvider, DeviceIdentifierDto request)
        {
            var deviceStateResult = await _deviceRepository.DeviceStateGet(request);
            if (!deviceStateResult.IsSuccess)
                return RpcResponse<bool>.WithErrors(false, deviceStateResult.Errors);
            if (deviceStateResult.Data == DeviceStatus.Missing)
                return RpcResponse<bool>.WithError(false, CommonErrors.NotFound("устройство"));
            
            var cleanRequestsResult = await _registrationRequestRepository.RegistrationRequestsCleanByDevice(request);
            if (!cleanRequestsResult.IsSuccess)
                return cleanRequestsResult;
            
            var setDeviceStateResult = await _deviceRepository.DeviceDelete(request);
            
            await _unitOfWork.Context.SaveChangesAsync();
            _unitOfWork.Commit();
            
            return setDeviceStateResult;
        }
    }
}