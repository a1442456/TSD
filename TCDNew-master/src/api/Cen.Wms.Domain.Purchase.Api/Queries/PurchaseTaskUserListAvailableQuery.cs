using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Data.DataSource.AgGrid;
using Cen.Common.Data.DataSource.Dtos;
using Cen.Common.Data.DataSource.Extensions;
using Cen.Wms.Data.Context;
using Cen.Wms.Domain.Purchase.Api.Dtos;
using Cen.Wms.Domain.User.Manage.Models;

namespace Cen.Wms.Domain.Purchase.Api.Queries
{
    public class PurchaseTaskUserListAvailableQuery : IQueryProcessor<TableRowsWithParamReq<PurchaseTaskUserListAvailableReq>, RpcResponse<DataSourceResult<UserListModel>>>
    {
        private readonly IMapper _mapper;
        private readonly WmsContext _wmsContext;

        public PurchaseTaskUserListAvailableQuery(IMapper mapper, WmsContext wmsContext)
        {
            _mapper = mapper;
            _wmsContext = wmsContext;
        }
        
        public async Task<RpcResponse<DataSourceResult<UserListModel>>> Run(IUserIdProvider userIdProvider,
            TableRowsWithParamReq<PurchaseTaskUserListAvailableReq> request)
        {
            var pacHeadsListQuery =
                _wmsContext.User
                    .Join(
                        _wmsContext.FacilityAccess.Where(fa => fa.FacilityId == request.Data.FacilityId),
                        userRow => userRow.Id,
                        facilityAccessRow => facilityAccessRow.UserId,
                        (u, fa) => u
                    )
                    .GroupJoin(
                        _wmsContext.PurchaseTaskUser.Where(e => e.PurchaseTaskHeadId == request.Data.PurchaseTaskHeadId),
                        userRow => userRow.Id,
                        purchaseTaskPacHeadRow => purchaseTaskPacHeadRow.UserId,
                        (u, ptu) => new {u, ptu }
                    )
                    .SelectMany(
                        uptu =>  uptu.ptu.DefaultIfEmpty(),
                        (uptu, ptu) => new { uptu.u, ptu })
                    .Where(uptu => uptu.ptu == null)
                    .Select(uptu => uptu.u)
                    .ProjectTo<UserListModel>(_mapper.ConfigurationProvider);
            
            var dataSourceResult = await pacHeadsListQuery.ToDataSourceResultAsync(request.GetDataSourceRequest());
            return RpcResponse<DataSourceResult<UserListModel>>.WithSuccess(
                dataSourceResult.AsTyped<UserListModel>()
            );
        }
    }
}