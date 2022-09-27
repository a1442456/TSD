using System.Threading.Tasks;
using AutoMapper;
using Cen.Common.CQRS;
using Cen.Common.Domain.Models;
using Cen.Common.Errors;
using Cen.Wms.Data.Context;
using Cen.Wms.Data.Models.Facility;
using Cen.Wms.Domain.Facility.Config.Abstract;
using Cen.Wms.Domain.Facility.Config.Models;
using Microsoft.EntityFrameworkCore;

namespace Cen.Wms.Domain.Facility.Config.Store.EntityFramework
{
    public class EntityFrameworkFacilityConfigRepository: IFacilityConfigRepository
    {
        private readonly WmsContext _wmsContext;
        private readonly IMapper _mapper;

        public EntityFrameworkFacilityConfigRepository(WmsContext wmsContext, IMapper mapper)
        {
            _wmsContext = wmsContext;
            _mapper = mapper;
        }

        public async Task<RpcResponse<FacilityConfigEditModel>> FacilityConfigGet(ByIdReq facilityId)
        {
            var facilityConfigRow = await _wmsContext.FacilityConfig.FirstOrDefaultAsync(e => e.Id == facilityId.Id);
            if (facilityConfigRow == null)
                return RpcResponse<FacilityConfigEditModel>.WithError(null, CommonErrors.NotFound("настроки торгового объекта"));

            var facilityConfig = _mapper.Map<FacilityConfigEditModel>(facilityConfigRow);
            return RpcResponse<FacilityConfigEditModel>.WithSuccess(facilityConfig);
        }

        public async Task<RpcResponse<bool>> FacilityConfigSet(ByIdReq facilityId, FacilityConfigEditModel facilityConfigEditModel)
        {
            var facilityConfigRow = await _wmsContext.FacilityConfig.FirstOrDefaultAsync(e => e.Id == facilityId.Id);
            if (facilityConfigRow == null)
            {
                facilityConfigRow = new FacilityConfigRow();
                _mapper.Map(facilityConfigEditModel, facilityConfigRow);
                facilityConfigRow.Id = facilityId.Id;
                await _wmsContext.FacilityConfig.AddAsync(facilityConfigRow);
            }
            else
            {
                _mapper.Map(facilityConfigEditModel, facilityConfigRow);
                _wmsContext.FacilityConfig.Update(facilityConfigRow);
            }
            
            return RpcResponse<bool>.WithSuccess(true);
        }

        public async Task<RpcResponse<bool>> FacilityConfigSetIfNotExists(ByIdReq facilityId, FacilityConfigEditModel facilityConfigEditModel)
        {
            var facilityConfigRow = await _wmsContext.FacilityConfig.FirstOrDefaultAsync(e => e.Id == facilityId.Id);
            if (facilityConfigRow == null)
            {
                facilityConfigRow = new FacilityConfigRow();
                _mapper.Map(facilityConfigEditModel, facilityConfigRow);
                facilityConfigRow.Id = facilityId.Id;
                await _wmsContext.FacilityConfig.AddAsync(facilityConfigRow);
            }
            
            return RpcResponse<bool>.WithSuccess(true);
        }
    }
}