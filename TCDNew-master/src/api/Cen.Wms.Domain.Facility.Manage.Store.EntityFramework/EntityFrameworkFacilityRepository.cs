using System;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cen.Common.CQRS;
using Cen.Common.Data.DataSource.AgGrid;
using Cen.Common.Data.DataSource.Dtos;
using Cen.Common.Data.DataSource.Extensions;
using Cen.Common.Domain.Models;
using Cen.Wms.Data.Context;
using Cen.Wms.Data.Models.Facility;
using Cen.Wms.Domain.Facility.Manage.Abstract;
using Cen.Wms.Domain.Facility.Manage.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Cen.Wms.Domain.Facility.Manage.Store.EntityFramework
{
    public class EntityFrameworkFacilityRepository: IFacilityRepository
    {
        private readonly WmsContext _wmsContext;
        private readonly IMapper _mapper;

        public EntityFrameworkFacilityRepository(WmsContext wmsContext, IMapper mapper)
        {
            _wmsContext = wmsContext;
            _mapper = mapper;
        }

        public async Task<RpcResponse<FacilityEditModel>> FacilityRead(ByIdReq request)
        {
            var facilityRow = await _wmsContext.Facility.FirstOrDefaultAsync(e => e.Id == request.Id);
            var facilityEditModel = _mapper.Map<FacilityEditModel>(facilityRow);
            
            return RpcResponse<FacilityEditModel>.WithSuccess(facilityEditModel);
        }

        public async Task<RpcResponse<Guid>> FacilityStore(FacilityEditModel facilityEditModel)
        {
            Guid result;
            
            if (facilityEditModel.Id == Guid.Empty)
            {
                var facilityRow = new FacilityRow();
                _mapper.Map(facilityEditModel, facilityRow);
                facilityRow.Id = NewId.NextGuid();
                await _wmsContext.Facility.AddAsync(facilityRow);
                
                result = facilityRow.Id.Value;
            }
            else
            {
                var facilityRow = await _wmsContext.Facility.FirstOrDefaultAsync(e => e.Id == facilityEditModel.Id);
                _mapper.Map(facilityEditModel, facilityRow);
                _wmsContext.Facility.Update(facilityRow);
                
                // ReSharper disable once PossibleInvalidOperationException
                result = facilityRow.Id.Value;
            }

            return RpcResponse<Guid>.WithSuccess(result);
        }

        public async Task<RpcResponse<bool>> FacilityExists(ByIdReq request)
        {
            var facilityExists = await _wmsContext.Facility.AnyAsync(e => e.Id == request.Id);
            
            return RpcResponse<bool>.WithSuccess(facilityExists);
        }

        public async Task<RpcResponse<Guid>> FacilityIdByExtId(string extId)
        {
            var facilityRow = await _wmsContext.Facility.FirstOrDefaultAsync(e => e.ExtId == extId);
            
            return RpcResponse<Guid>.WithSuccess(facilityRow?.Id ?? Guid.Empty);
        }

        public async Task<RpcResponse<DataSourceResult<FacilityListModel>>> FacilityList(TableRowsReq request)
        {
            var dataSourceRequest = request.GetDataSourceRequest();
            var dataSourceResult = await _wmsContext.Facility
                .ProjectTo<FacilityListModel>(_mapper.ConfigurationProvider)
                .ToDataSourceResultAsync(dataSourceRequest);
            
            return RpcResponse<DataSourceResult<FacilityListModel>>.WithSuccess(
                dataSourceResult.AsTyped<FacilityListModel>()
            );
        }
    }
}