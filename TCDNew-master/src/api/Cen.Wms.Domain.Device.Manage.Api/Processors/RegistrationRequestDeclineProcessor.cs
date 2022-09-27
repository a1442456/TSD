using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Data.EntityFramework;
using Cen.Common.Domain.Models;
using Cen.Common.Errors;
using Cen.Wms.Data.Context;
using Cen.Wms.Domain.Device.Manage.Abstract;
using Cen.Wms.Domain.Device.Manage.Models;
using Cen.Wms.Domain.Device.Manage.Enums;
using Serilog;

namespace Cen.Wms.Domain.Device.Manage.Api.Processors
{
    public class RegistrationRequestDeclineProcessor: IQueryProcessor<ByIdReq, RpcResponse<bool>>
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork<WmsContext> _unitOfWork;
        private readonly IDeviceRepository _deviceRepository;
        private readonly IRegistrationRequestRepository _registrationRequestRepository;

        public RegistrationRequestDeclineProcessor(
            ILogger logger, UnitOfWork<WmsContext> unitOfWork,
            IDeviceRepository deviceRepository, IRegistrationRequestRepository registrationRequestRepository)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _deviceRepository = deviceRepository;
            _registrationRequestRepository = registrationRequestRepository;
        }

        public async Task<RpcResponse<bool>> Run(IUserIdProvider userIdProvider, ByIdReq request)
        {
            var registrationRequest = await _registrationRequestRepository.RegistrationRequestGetById(request);
            if (!registrationRequest.IsSuccess)
                return RpcResponse<bool>.WithErrors(false, registrationRequest.Errors);
            if (registrationRequest.Data == null)
                return RpcResponse<bool>.WithError(false, CommonErrors.NotFound("запрос на регистрацию"));

            var deviceIdentifier = new DeviceIdentifierDto {DevicePublicKey = registrationRequest.Data.DevicePublicKey};
            var deviceStateResult = await _deviceRepository.DeviceStateGet(deviceIdentifier);
            if (!deviceStateResult.IsSuccess)
                return RpcResponse<bool>.WithErrors(false, deviceStateResult.Errors);
            if (deviceStateResult.Data == DeviceStatus.Missing)
                return RpcResponse<bool>.WithError(false, CommonErrors.NotFound("устройство"));
            if (deviceStateResult.Data != DeviceStatus.Awaiting)
                return RpcResponse<bool>.WithError(false, CommonErrors.InvalidOperation);
            
            var declineRequestResult = await _registrationRequestRepository.RegistrationRequestDecline(request);
            if (!declineRequestResult.IsSuccess)
                return declineRequestResult;
            
            var setDeviceStateResult = await _deviceRepository.DeviceStatusSet(deviceIdentifier, DeviceStatus.Declined);

            await _unitOfWork.Context.SaveChangesAsync();
            _unitOfWork.Commit();
            
            return setDeviceStateResult;
        }
    }
}