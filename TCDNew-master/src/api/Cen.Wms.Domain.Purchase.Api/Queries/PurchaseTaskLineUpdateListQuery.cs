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
using Microsoft.EntityFrameworkCore;

namespace Cen.Wms.Domain.Purchase.Api.Queries
{
    public class PurchaseTaskLineUpdateListQuery : IQueryProcessor<TableRowsWithParamReq<ByIdReq>, RpcResponse<DataSourceResult<PurchaseTaskLineUpdateListModel>>>
    {
        private readonly IMapper _mapper;
        private readonly WmsContext _wmsContext;

        public PurchaseTaskLineUpdateListQuery(IMapper mapper, WmsContext wmsContext)
        {
            _mapper = mapper;
            _wmsContext = wmsContext;
        }

        public async Task<RpcResponse<DataSourceResult<PurchaseTaskLineUpdateListModel>>> Run(
            IUserIdProvider userIdProvider, TableRowsWithParamReq<ByIdReq> request)
        {
            var dataSourceRequest = request.GetDataSourceRequest();
            var dataSourceResult = await _wmsContext.PurchaseTaskLineUpdate
                .Include(e => e.PurchaseTaskLine)
                .Where(e => e.PurchaseTaskHeadId == request.Data.Id)
                .ProjectTo<PurchaseTaskLineUpdateListModel>(_mapper.ConfigurationProvider)
                .ToDataSourceResultAsync(dataSourceRequest);

            return RpcResponse<DataSourceResult<PurchaseTaskLineUpdateListModel>>.WithSuccess(
                dataSourceResult.AsTyped<PurchaseTaskLineUpdateListModel>()
            );
        }
    }
}