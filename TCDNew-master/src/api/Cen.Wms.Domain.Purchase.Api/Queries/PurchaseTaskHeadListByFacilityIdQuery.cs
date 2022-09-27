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
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Cen.Wms.Domain.Purchase.Api.Queries
{
    public class PurchaseTaskHeadListByFacilityIdQuery : IQueryProcessor<TableRowsWithParamReq<ByIdReq>, RpcResponse<DataSourceResult<PurchaseTaskHeadListModel>>>
    {
        private readonly IMapper _mapper;
        private readonly WmsContext _wmsContext;
        private readonly IFacilityRepository _facilityRepository;

        public PurchaseTaskHeadListByFacilityIdQuery(IMapper mapper, IFacilityRepository facilityRepository, WmsContext wmsContext)
        {
            _mapper = mapper;
            _facilityRepository = facilityRepository;
            _wmsContext = wmsContext;
        }
        
        public async Task<RpcResponse<DataSourceResult<PurchaseTaskHeadListModel>>> Run(IUserIdProvider userIdProvider,
            TableRowsWithParamReq<ByIdReq> request)
        {
            var facilityReadResult = await _facilityRepository.FacilityRead(request.Data);
            if (!facilityReadResult.IsSuccess)
                return RpcResponse<DataSourceResult<PurchaseTaskHeadListModel>>.WithSuccess(DataSourceResult<PurchaseTaskHeadListModel>.Empty());
            if (facilityReadResult.Data == null)
                return RpcResponse<DataSourceResult<PurchaseTaskHeadListModel>>.WithSuccess(DataSourceResult<PurchaseTaskHeadListModel>.Empty());

            var dataSourceRequest = request.GetDataSourceRequest();
            var dataSourceResult = await _wmsContext.PurchaseTaskHead
                .Include(e => e.CreatedByUser)
                .Where(e => e.FacilityId == request.Data.Id)
                .ProjectTo<PurchaseTaskHeadListModel>(_mapper.ConfigurationProvider)
                .ToDataSourceResultAsync(dataSourceRequest);

            return RpcResponse<DataSourceResult<PurchaseTaskHeadListModel>>.WithSuccess(
                dataSourceResult.AsTyped<PurchaseTaskHeadListModel>()
            );
        }
    }
}