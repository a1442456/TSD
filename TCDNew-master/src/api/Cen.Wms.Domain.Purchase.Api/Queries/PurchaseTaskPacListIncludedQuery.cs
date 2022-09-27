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
using Cen.Wms.Domain.Purchase.Models;

namespace Cen.Wms.Domain.Purchase.Api.Queries
{
    public class PurchaseTaskPacListIncludedQuery : IQueryProcessor<TableRowsWithParamReq<ByIdReq>, RpcResponse<DataSourceResult<PacHeadListModel>>>
    {
        private readonly IMapper _mapper;
        private readonly WmsContext _wmsContext;

        public PurchaseTaskPacListIncludedQuery(IMapper mapper, WmsContext wmsContext)
        {
            _mapper = mapper;
            _wmsContext = wmsContext;
        }
        
        public async Task<RpcResponse<DataSourceResult<PacHeadListModel>>> Run(IUserIdProvider userIdProvider,
            TableRowsWithParamReq<ByIdReq> request)
        {
            var pacHeadsListQuery =
                _wmsContext.PacHead
                    .Join(_wmsContext.PurchaseTaskPacHead
                            .Where(purchaseTaskPacHeadRow => purchaseTaskPacHeadRow.PurchaseTaskHeadId == request.Data.Id),
                        pacHeadRow => pacHeadRow.Id,
                        purchaseTaskPacHeadRow => purchaseTaskPacHeadRow.PacHeadId,
                        (u, fa) => u
                    )
                    .ProjectTo<PacHeadListModel>(_mapper.ConfigurationProvider);

            var dataSourceResult = await pacHeadsListQuery.ToDataSourceResultAsync(request.GetDataSourceRequest());
            return RpcResponse<DataSourceResult<PacHeadListModel>>.WithSuccess(
                dataSourceResult.AsTyped<PacHeadListModel>()
            );
        }
    }
}