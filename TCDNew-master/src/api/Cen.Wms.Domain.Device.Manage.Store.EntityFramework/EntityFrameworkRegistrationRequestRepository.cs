using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cen.Common.CQRS;
using Cen.Common.Domain.Models;
using Cen.Wms.Data.Context;
using Cen.Wms.Data.Models.Device;
using Cen.Wms.Domain.Device.Manage.Abstract;
using Cen.Wms.Domain.Device.Manage.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using DeviceRegistrationRequest = Cen.Wms.Domain.Device.Manage.Models.DeviceRegistrationRequest;

namespace Cen.Wms.Domain.Device.Manage.Store.EntityFramework
{
    public class EntityFrameworkRegistrationRequestRepository: IRegistrationRequestRepository
    {
        private readonly WmsContext _wmsContext;
        private readonly IMapper _mapper;

        public EntityFrameworkRegistrationRequestRepository(WmsContext wmsContext, IMapper mapper)
        {
            _wmsContext = wmsContext;
            _mapper = mapper;
        }
        
        public async Task<RpcResponse<Guid>> RegistrationRequestStore(DeviceRegistrationRequest registrationRequest)
        {
            var registrationRequestRow = 
                await _wmsContext.DeviceRegistrationRequest
                    .FirstOrDefaultAsync(e => 
                        e.DevicePublicKey == registrationRequest.DevicePublicKey
                        && e.CreatedAt == registrationRequest.CreatedAt
                    );
            if (registrationRequestRow == null)
            {
                registrationRequestRow = _mapper.Map<DeviceRegistrationRequestRow>(registrationRequest);
                registrationRequestRow.Id = NewId.NextGuid();

                await _wmsContext.DeviceRegistrationRequest.AddAsync(registrationRequestRow);
                await _wmsContext.SaveChangesAsync();
            }

            return RpcResponse<Guid>.WithSuccess(registrationRequestRow.Id ?? Guid.Empty);
        }

        public async Task<RpcResponse<DeviceRegistrationRequest>> RegistrationRequestGetById(ByIdReq request)
        {
            var registrationRequestRow =
                await _wmsContext.DeviceRegistrationRequest.FirstOrDefaultAsync(e => e.Id == request.Id);
            var registrationRequest = _mapper.Map<DeviceRegistrationRequest>(registrationRequestRow);
            
            return RpcResponse<DeviceRegistrationRequest>.WithSuccess(registrationRequest);
        }

        public async Task<RpcResponse<bool>> RegistrationRequestAccept(ByIdReq registrationRequestId)
        {
            var registrationRequestState = 
                await _wmsContext.DeviceRegistrationRequestState.FirstOrDefaultAsync(e => e.Id == registrationRequestId.Id);
            if (registrationRequestState == null)
            {
                registrationRequestState = new DeviceRegistrationRequestStateRow { Id = registrationRequestId.Id, IsAccepted = true };
                await _wmsContext.DeviceRegistrationRequestState.AddAsync(registrationRequestState);
                await _wmsContext.SaveChangesAsync();
            }
            else
            {
                registrationRequestState.IsAccepted = true;
                _wmsContext.DeviceRegistrationRequestState.Update(registrationRequestState);
            }
            
            return RpcResponse<bool>.WithSuccess(true);
        }

        public async Task<RpcResponse<bool>> RegistrationRequestDecline(ByIdReq registrationRequestId)
        {
            var registrationRequestState = 
                await _wmsContext.DeviceRegistrationRequestState.FirstOrDefaultAsync(e => e.Id == registrationRequestId.Id);
            if (registrationRequestState == null)
            {
                registrationRequestState = new DeviceRegistrationRequestStateRow { Id = registrationRequestId.Id, IsAccepted = false };
                await _wmsContext.DeviceRegistrationRequestState.AddAsync(registrationRequestState);
                await _wmsContext.SaveChangesAsync();
            }
            else
            {
                registrationRequestState.IsAccepted = false;
                _wmsContext.DeviceRegistrationRequestState.Update(registrationRequestState);
            }
            
            return RpcResponse<bool>.WithSuccess(true);
        }

        public Task<RpcResponse<bool>> RegistrationRequestsCleanByDevice(DeviceIdentifierDto request)
        {
            var registrationRequestRows =
                _wmsContext.DeviceRegistrationRequest
                    .Where(e => e.DevicePublicKey == request.DevicePublicKey)
                    .ToArray();
            var registrationRequestIds = registrationRequestRows.Select(e => e.Id).ToList();
            var registrationRequestStateRows =
                _wmsContext.DeviceRegistrationRequestState
                    .Where(e => registrationRequestIds.Contains(e.Id))
                    .ToArray();
            
            _wmsContext.DeviceRegistrationRequestState.RemoveRange(registrationRequestStateRows);
            _wmsContext.DeviceRegistrationRequest.RemoveRange(registrationRequestRows);

            return Task.FromResult(RpcResponse<bool>.WithSuccess(true));
        }
    }
}