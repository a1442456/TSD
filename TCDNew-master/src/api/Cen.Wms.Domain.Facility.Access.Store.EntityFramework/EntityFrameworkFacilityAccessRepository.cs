using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cen.Common.CQRS;
using Cen.Common.Domain.Models;
using Cen.Wms.Data.Context;
using Cen.Wms.Data.Models.Facility;
using Cen.Wms.Domain.Facility.Access.Abstract;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Cen.Wms.Domain.Facility.Access.Store.EntityFramework
{
    public class EntityFrameworkFacilityAccessRepository: IFacilityAccessRepository
    {
        private readonly WmsContext _wmsContext;
        private readonly IMapper _mapper;

        public EntityFrameworkFacilityAccessRepository(WmsContext wmsContext, IMapper mapper)
        {
            _wmsContext = wmsContext;
            _mapper = mapper;
        }

        public async Task<RpcResponse<bool>> FacilityAccessGrant(ByIdReq facilityId, ByIdReq userId)
        {
            var facilityAccessRow = 
                await _wmsContext.FacilityAccess
                    .FirstOrDefaultAsync(e => e.FacilityId == facilityId.Id && e.UserId == userId.Id);
            if (facilityAccessRow == null)
            {
                // TODO: user automapper here
                facilityAccessRow = new FacilityAccessRow { Id = NewId.NextGuid(), FacilityId = facilityId.Id, UserId = userId.Id, IsManual = true };
                await _wmsContext.FacilityAccess.AddAsync(facilityAccessRow);
            }
            
            return RpcResponse<bool>.WithSuccess(true);
        }

        public async Task<RpcResponse<bool>> DefaultFacilityAccessSwitchTo(ByIdReq facilityId, ByIdReq userId)
        {
            var facilityAccessRowList = 
                await _wmsContext.FacilityAccess
                    .Where(e => e.UserId == userId.Id && !e.IsManual)
                    .ToListAsync();

            var isDefaultAccessAlreadyExists = false;
            foreach (var facilityAccessRow in facilityAccessRowList)
            {
                if (!(facilityAccessRow.FacilityId == facilityId.Id && facilityAccessRow.UserId == userId.Id))
                {
                    _wmsContext.FacilityAccess.Remove(facilityAccessRow);
                    continue;
                }

                isDefaultAccessAlreadyExists = true;
            }

            if (!isDefaultAccessAlreadyExists)
            {
                // TODO: user automapper here
                var facilityAccessRow = new FacilityAccessRow { Id = NewId.NextGuid(), FacilityId = facilityId.Id, UserId = userId.Id, IsManual = false };
                await _wmsContext.FacilityAccess.AddAsync(facilityAccessRow);
            }
            
            return RpcResponse<bool>.WithSuccess(true);
        }

        public async Task<RpcResponse<bool>> FacilityAccessWithdraw(ByIdReq facilityId, ByIdReq userId)
        {
            var facilityAccessRow = await _wmsContext.FacilityAccess
                .FirstOrDefaultAsync(e => e.FacilityId == facilityId.Id && e.UserId == userId.Id);
            if (facilityAccessRow != null)
            {
                _wmsContext.FacilityAccess.Remove(facilityAccessRow);
            }
            
            return RpcResponse<bool>.WithSuccess(true);
        }

        public async Task<RpcResponse<bool>> FacilityAccessIsGranted(ByIdReq facilityId, ByIdReq userId)
        {
            var facilityAccessRowExists = await _wmsContext.FacilityAccess
                .AnyAsync(e => e.FacilityId == facilityId.Id && e.UserId == userId.Id);
            
            return RpcResponse<bool>.WithSuccess(facilityAccessRowExists);
        }
    }
}