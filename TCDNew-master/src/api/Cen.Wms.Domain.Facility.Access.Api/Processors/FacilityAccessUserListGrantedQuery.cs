using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Data.DataSource.AgGrid;
using Cen.Common.Data.DataSource.Dtos;
using Cen.Common.Data.DataSource.Extensions;
using Cen.Common.Data.EntityFramework;
using Cen.Common.Domain.Models;
using Cen.Wms.Data.Context;
using Cen.Wms.Domain.Facility.Access.Models;
using Cen.Wms.Domain.User.Manage.Models;
using Serilog;

namespace Cen.Wms.Domain.Facility.Access.Api.Processors
{
    public class FacilityAccessUserListGrantedQuery : IQueryProcessor<TableRowsWithParamReq<ByIdReq>, RpcResponse<DataSourceResult<UserWithAccessListModel>>>
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork<WmsContext> _unitOfWork;
        private readonly WmsContext _wmsContext;
        private readonly IMapper _mapper;

        public FacilityAccessUserListGrantedQuery(
            ILogger logger, UnitOfWork<WmsContext> unitOfWork, WmsContext wmsContext, IMapper mapper
            )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _wmsContext = wmsContext;
            _mapper = mapper;
        }
        
        public async Task<RpcResponse<DataSourceResult<UserWithAccessListModel>>> Run(IUserIdProvider userIdProvider,
            TableRowsWithParamReq<ByIdReq> request)
        {
            var usersListQuery =
                _wmsContext.User
                    .Join(
                        _wmsContext.FacilityAccess.Where(fa => fa.FacilityId == request.Data.Id),
                        u => u.Id,
                        fa => fa.UserId,
                        (u, fa) => new UserWithAccessListModel
                        {
                            Id = u.Id.Value,
                            ExtId = u.ExtId,
                            Name = u.Name,
                            Login = u.Login,
                            // TODO: IsLocked should be mapped from user config table
                            IsLocked = false,
                            FacilityExtId = u.FacilityExtId,
                            FacilityName = u.FacilityName,
                            DepartmentExtId = u.DepartmentExtId,
                            DepartmentName = u.DepartmentName,
                            PositionExtId = u.PositionExtId,
                            PositionName = u.PositionName,
                            ChangedAt = u.ChangedAt,
                            IsManual = fa.IsManual
                        }
                    );

            var dataSourceResult = await usersListQuery.ToDataSourceResultAsync(request.GetDataSourceRequest());
            return RpcResponse<DataSourceResult<UserWithAccessListModel>>.WithSuccess(
                dataSourceResult.AsTyped<UserWithAccessListModel>()
            );
        }
    }
}