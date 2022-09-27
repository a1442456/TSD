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
using Cen.Wms.Domain.User.Manage.Models;

namespace Cen.Wms.Domain.Purchase.Api.Queries
{
    public class PurchaseTaskUserListIncludedQuery : IQueryProcessor<TableRowsWithParamReq<ByIdReq>, RpcResponse<DataSourceResult<UserListModel>>>
    {
        private readonly IMapper _mapper;
        private readonly WmsContext _wmsContext;

        public PurchaseTaskUserListIncludedQuery(IMapper mapper, WmsContext wmsContext)
        {
            _mapper = mapper;
            _wmsContext = wmsContext;
        }
        
        public async Task<RpcResponse<DataSourceResult<UserListModel>>> Run(IUserIdProvider userIdProvider,
            TableRowsWithParamReq<ByIdReq> request)
        {
            var pacHeadsListQuery =
                _wmsContext.User
                    .Join(_wmsContext.PurchaseTaskUser
                            .Where(purchaseTaskUserRow => purchaseTaskUserRow.PurchaseTaskHeadId == request.Data.Id),
                        userRow => userRow.Id,
                        purchaseTaskPacHeadRow => purchaseTaskPacHeadRow.UserId,
                        (u, fa) => u
                    )
                    .ProjectTo<UserListModel>(_mapper.ConfigurationProvider);

            var dataSourceResult = await pacHeadsListQuery.ToDataSourceResultAsync(request.GetDataSourceRequest());
            return RpcResponse<DataSourceResult<UserListModel>>.WithSuccess(
                dataSourceResult.AsTyped<UserListModel>()
            );
        }
    }
}