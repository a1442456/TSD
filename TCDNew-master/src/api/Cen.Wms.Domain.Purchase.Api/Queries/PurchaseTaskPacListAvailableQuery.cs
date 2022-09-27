using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Data.DataSource.AgGrid;
using Cen.Common.Data.DataSource.Dtos;
using Cen.Common.Data.DataSource.Extensions;
using Cen.Common.Domain.Models;
using Cen.Wms.Data.Context;
using Cen.Wms.Domain.Facility.Manage.Abstract;
using Cen.Wms.Domain.Purchase.Models;

namespace Cen.Wms.Domain.Purchase.Api.Queries
{
    public class PurchaseTaskPacListAvailableQuery : IQueryProcessor<TableRowsWithParamReq<ByIdReq>, RpcResponse<DataSourceResult<PacHeadListModel>>>
    {
        private readonly IMapper _mapper;
        private readonly WmsContext _wmsContext;
        private readonly IFacilityRepository _facilityRepository;

        public PurchaseTaskPacListAvailableQuery(IMapper mapper, WmsContext wmsContext, IFacilityRepository facilityRepository)
        {
            _mapper = mapper;
            _wmsContext = wmsContext;
            _facilityRepository = facilityRepository;
        }
        
        public async Task<RpcResponse<DataSourceResult<PacHeadListModel>>> Run(IUserIdProvider userIdProvider,
            TableRowsWithParamReq<ByIdReq> request)
        {
            var facilityReadResult = await _facilityRepository.FacilityRead(request.Data);
            if (!facilityReadResult.IsSuccess)
                return RpcResponse<DataSourceResult<PacHeadListModel>>.WithSuccess(DataSourceResult<PacHeadListModel>.Empty());
            if (facilityReadResult.Data == null)
                return RpcResponse<DataSourceResult<PacHeadListModel>>.WithSuccess(DataSourceResult<PacHeadListModel>.Empty());
            
            var pacHeadsListQuery = _wmsContext.PacHead
                .Where(pacHeadRow => 
                    pacHeadRow.FacilityId == facilityReadResult.Data.ExtId
                    && !pacHeadRow.PacState.IsBusy && !pacHeadRow.PacState.IsProcessed
                )
                .ProjectTo<PacHeadListModel>(_mapper.ConfigurationProvider);
            
            var dataSourceResult = await pacHeadsListQuery.ToDataSourceResultAsync(request.GetDataSourceRequest());
            return RpcResponse<DataSourceResult<PacHeadListModel>>.WithSuccess(
                dataSourceResult.AsTyped<PacHeadListModel>()
            );
        }
    }
}