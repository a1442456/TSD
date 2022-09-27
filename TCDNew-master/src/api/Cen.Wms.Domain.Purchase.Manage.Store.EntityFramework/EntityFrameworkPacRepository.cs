using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cen.Common.CQRS;
using Cen.Common.Data.DataSource.AgGrid;
using Cen.Common.Data.DataSource.Dtos;
using Cen.Common.Data.DataSource.Extensions;
using Cen.Common.Domain.Models;
using Cen.Wms.Data.Context;
using Cen.Wms.Data.Models.Purchase;
using Cen.Wms.Domain.Purchase.Abstract;
using Cen.Wms.Domain.Purchase.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Cen.Wms.Domain.Purchase.Manage.Store.EntityFramework
{
    public class EntityFrameworkPacRepository: IPacRepository
    {
        private readonly WmsContext _wmsContext;
        private readonly IMapper _mapper;

        public EntityFrameworkPacRepository(WmsContext wmsContext, IMapper mapper)
        {
            _wmsContext = wmsContext;
            _mapper = mapper;
        }

        public async Task<RpcResponse<DataSourceResult<PacHeadListModel>>> PacHeadListByFacilityExtId(TableRowsReq request, string facilityExtId)
        {
            var dataSourceRequest = request.GetDataSourceRequest();
            var dataSourceResult = await _wmsContext.PacHead
                .Where(e => e.FacilityId == facilityExtId)
                .ProjectTo<PacHeadListModel>(_mapper.ConfigurationProvider)
                .ToDataSourceResultAsync(dataSourceRequest);

            return RpcResponse<DataSourceResult<PacHeadListModel>>.WithSuccess(
                dataSourceResult.AsTyped<PacHeadListModel>()
            );
        }

        public async Task<RpcResponse<DataSourceResult<PacLineListModel>>> PacLineList(TableRowsReq request, ByIdReq pacId)
        {
            var dataSourceRequest = request.GetDataSourceRequest();
            var pacLineRows = await _wmsContext.PacLine
                .Include(e => e.PacLineStates)
                .Where(e => e.PacHeadId == pacId.Id)
                .ToListAsync();
            var dataSourceResult = await _mapper.Map<IEnumerable<PacLineListModel>>(pacLineRows)
                .ToDataSourceResultAsync(dataSourceRequest);

            return RpcResponse<DataSourceResult<PacLineListModel>>.WithSuccess(
                dataSourceResult.AsTyped<PacLineListModel>()
            );
        }

        public async Task<RpcResponse<Guid>> PacStore(PacHeadEditModel pacHeadEditModel)
        {
            Guid result;
            
            if (pacHeadEditModel.Id == Guid.Empty)
            {
                var pacHeadRow = new PacHeadRow();
                pacHeadRow.Lines = new List<PacLineRow>();
                _mapper.Map(pacHeadEditModel, pacHeadRow);
                pacHeadRow.PacState = new PacStateRow { Id = NewId.NextGuid() };
                pacHeadRow.Id = NewId.NextGuid();
                await _wmsContext.PacHead.AddAsync(pacHeadRow);
                await _wmsContext.SaveChangesAsync();

                result = pacHeadRow.Id.Value;
            }
            else
            {
                var pacRow = await _wmsContext.PacHead.Include(e => e.Lines).FirstOrDefaultAsync(e => e.Id == pacHeadEditModel.Id);
                _mapper.Map(pacHeadEditModel, pacRow);
                _wmsContext.PacHead.Update(pacRow);
                
                // ReSharper disable once PossibleInvalidOperationException
                result = pacRow.Id.Value;
            }

            return RpcResponse<Guid>.WithSuccess(result);
        }

        public async Task<RpcResponse<bool>> PacExists(ByIdReq request)
        {
            var pacExists = await _wmsContext.PacHead.AnyAsync(e => e.Id == request.Id);
            
            return RpcResponse<bool>.WithSuccess(pacExists);
        }
        
        public async Task<RpcResponse<PacHeadEditModel>> PacRead(ByIdReq request)
        {
            var pacEditModel = await _wmsContext.PacHead
                .Include(e => e.Lines)
                .Include(e => e.ResponsibleUser)
                .ProjectTo<PacHeadEditModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(e => e.Id == request.Id);

            return RpcResponse<PacHeadEditModel>.WithSuccess(pacEditModel);
        }
        
        public async Task<RpcResponse<List<PacHeadEditModel>>> PacReadMany(IEnumerable<ByIdReq> request)
        {
            var pacIds = request.Select(e => e.Id).ToList();
            var pacList = await _wmsContext.PacHead
                .Include(e => e.Lines)
                .Include(e => e.ResponsibleUser)
                .Where(e => pacIds.Contains(e.Id ?? Guid.Empty))
                .ProjectTo<PacHeadEditModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return RpcResponse<List<PacHeadEditModel>>.WithSuccess(pacList);
        }

        public async Task<RpcResponse<Guid>> PacIdByExtId(string extId)
        {
            var pacRow = await _wmsContext.PacHead.FirstOrDefaultAsync(e => e.ExtId == extId);
            
            return RpcResponse<Guid>.WithSuccess(pacRow?.Id ?? Guid.Empty);
        }

        public async Task<RpcResponse<bool>> PacGetBusy(ByIdReq pacId)
        {
            var pacRow = await _wmsContext.PacHead.Include(e => e.PacState).FirstOrDefaultAsync(e => e.Id == pacId.Id);
            
            return RpcResponse<bool>.WithSuccess(pacRow.PacState.IsBusy);
        }

        public async Task<RpcResponse<bool>> PacSetBusy(ByIdReq pacId, bool isBusy)
        {
            var pacRow = await _wmsContext.PacHead.Include(e => e.PacState).FirstOrDefaultAsync(e => e.Id == pacId.Id);
            pacRow.PacState.IsBusy = isBusy;
            
            _wmsContext.PacHead.Update(pacRow);
            
            return RpcResponse<bool>.WithSuccess(true);
        }
        
        public async Task<RpcResponse<bool>> PacSetResponsibleUserId(ByIdReq pacId, ByIdReq userId)
        {
            var pacRow = await _wmsContext.PacHead.Include(e => e.PacState).FirstOrDefaultAsync(e => e.Id == pacId.Id);
            pacRow.ResponsibleUserId = userId?.Id;

            _wmsContext.PacHead.Update(pacRow);
            
            return RpcResponse<bool>.WithSuccess(true);
        }
        
        public async Task<RpcResponse<bool>> PacSetProcessed(ByIdReq pacId, bool isProcessed)
        {
            var pacRow = await _wmsContext.PacHead.Include(e => e.PacState).FirstOrDefaultAsync(e => e.Id == pacId.Id);
            pacRow.PacState.IsProcessed = isProcessed;
            
            _wmsContext.PacHead.Update(pacRow);
            
            return RpcResponse<bool>.WithSuccess(true);
        }
    }
}