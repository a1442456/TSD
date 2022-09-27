using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Data.DataSource.AgGrid;
using Cen.Common.Data.DataSource.Dtos;
using Cen.Common.Data.DataSource.Extensions;
using Cen.Common.Data.EntityFramework;
using Cen.Common.Domain.Models;
using Cen.Wms.Data.Context;
using Cen.Wms.Domain.User.Manage.Models;
using Serilog;

namespace Cen.Wms.Domain.Facility.Access.Api.Processors
{
    public class FacilityAccessUserListAvailableQuery : IQueryProcessor<TableRowsWithParamReq<ByIdReq>, RpcResponse<DataSourceResult<UserListModel>>>
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork<WmsContext> _unitOfWork;
        private readonly WmsContext _wmsContext;
        private readonly IMapper _mapper;

        public FacilityAccessUserListAvailableQuery(
            ILogger logger, UnitOfWork<WmsContext> unitOfWork, WmsContext wmsContext, IMapper mapper
            )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _wmsContext = wmsContext;
            _mapper = mapper;
        }
        
        public async Task<RpcResponse<DataSourceResult<UserListModel>>> Run(IUserIdProvider userIdProvider,
            TableRowsWithParamReq<ByIdReq> request)
        {
            var usersListQuery =
                _wmsContext.User
                    .GroupJoin(
                        _wmsContext.FacilityAccess.Where(fa => fa.FacilityId == request.Data.Id),
                        u => u.Id,
                        fa => fa.UserId,
                        (u, fa) => new { u, fa }
                    )
                    .SelectMany(
                        ufa =>  ufa.fa.DefaultIfEmpty(),
                        (ufa, fa) => new { ufa.u, fa })
                    .Where(ufa => ufa.fa == null)
                    .Select(ufa => ufa.u)
                    .ProjectTo<UserListModel>(_mapper.ConfigurationProvider);
            
            var dataSourceResult = await usersListQuery.ToDataSourceResultAsync(request.GetDataSourceRequest());
            return RpcResponse<DataSourceResult<UserListModel>>.WithSuccess(
                dataSourceResult.AsTyped<UserListModel>()
            );
        }
    }
}