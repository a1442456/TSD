using System;
using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Data.EntityFramework;
using Cen.Wms.Data.Context;
using Cen.Wms.Domain.Device.Manage.Abstract;
using Cen.Wms.Domain.Device.Manage.Models;
using Cen.Wms.Domain.Device.Manage.Enums;
using NodaTime;
using Serilog;

namespace Cen.Wms.Domain.Device.Manage.Api.Processors
{
    public class RegistrationRequestStoreProcessor: IQueryProcessor<DeviceIdentifierDto, RpcResponse<Guid>>
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork<WmsContext> _unitOfWork;
        private readonly IDeviceRepository _deviceRepository;
        private readonly IRegistrationRequestRepository _registrationRequestRepository;
        private readonly IClock _clock;

        public RegistrationRequestStoreProcessor(
            ILogger logger, UnitOfWork<WmsContext> unitOfWork,
            IDeviceRepository deviceRepository, IRegistrationRequestRepository registrationRequestRepository, IClock clock)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _deviceRepository = deviceRepository;
            _registrationRequestRepository = registrationRequestRepository;
            _clock = clock;
        }

        public async Task<RpcResponse<Guid>> Run(IUserIdProvider userIdProvider, DeviceIdentifierDto request)
        {
            var deviceStateResult = await _deviceRepository.DeviceStateGet(request);
            if (!deviceStateResult.IsSuccess)
                return RpcResponse<Guid>.WithErrors(Guid.Empty, deviceStateResult.Errors);
            if (deviceStateResult.Data != DeviceStatus.Missing)
                return RpcResponse<Guid>.WithError(Guid.Empty, DeviceManageErrors.DeviceIsAlreadyRegistered);
            
            var storeDeviceResult = await _deviceRepository.DeviceStore(request, DeviceStatus.Awaiting);
            if (!storeDeviceResult.IsSuccess)
                return RpcResponse<Guid>.WithErrors(Guid.Empty, storeDeviceResult.Errors);
            
            var storeRegistrationRequest = 
                await _registrationRequestRepository.RegistrationRequestStore(
                    new DeviceRegistrationRequest(request.DevicePublicKey, _clock.GetCurrentInstant())
                );
            
            await _unitOfWork.Context.SaveChangesAsync();
            _unitOfWork.Commit();
            
            return storeRegistrationRequest;
        }
    }
}