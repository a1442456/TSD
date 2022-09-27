using System;
using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.Domain.Models;
using Cen.Wms.Domain.Device.Manage.Models;

namespace Cen.Wms.Domain.Device.Manage.Abstract
{
    public interface IRegistrationRequestRepository
    {
        public Task<RpcResponse<Guid>> RegistrationRequestStore(DeviceRegistrationRequest registrationRequest);

        public Task<RpcResponse<DeviceRegistrationRequest>> RegistrationRequestGetById(ByIdReq request);
        
        public Task<RpcResponse<bool>> RegistrationRequestAccept(ByIdReq registrationRequestId);
        public Task<RpcResponse<bool>> RegistrationRequestDecline(ByIdReq registrationRequestId);
        public Task<RpcResponse<bool>> RegistrationRequestsCleanByDevice(DeviceIdentifierDto request);
    }
}