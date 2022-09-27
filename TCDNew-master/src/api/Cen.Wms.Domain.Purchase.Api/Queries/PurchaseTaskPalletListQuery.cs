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
    public class PurchaseTaskPalletListQuery : IQueryProcessor<TableRowsWithParamReq<ByIdReq>, RpcResponse<DataSourceResult<PurchaseTaskPalletListModel>>>
    {
        private readonly IMapper _mapper;
        private readonly WmsContext _wmsContext;

        public PurchaseTaskPalletListQuery(IMapper mapper, WmsContext wmsContext)
        {
            _mapper = mapper;
            _wmsContext = wmsContext;
        }
        
        public async Task<RpcResponse<DataSourceResult<PurchaseTaskPalletListModel>>> Run(
            IUserIdProvider userIdProvider, TableRowsWithParamReq<ByIdReq> request)
        {
            var pacHeadsListQuery =
                _wmsContext.PurchaseTaskPallet.Where(e => e.PurchaseTaskHeadId == request.Data.Id)
                    .ProjectTo<PurchaseTaskPalletListModel>(_mapper.ConfigurationProvider);

            var dataSourceResult = await pacHeadsListQuery.ToDataSourceResultAsync(request.GetDataSourceRequest());
            return RpcResponse<DataSourceResult<PurchaseTaskPalletListModel>>.WithSuccess(
                dataSourceResult.AsTyped<PurchaseTaskPalletListModel>()
            );
        }
    }
}