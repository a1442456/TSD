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
using Cen.Wms.Data.Context;
using Cen.Wms.Domain.Facility.Manage.Models;
using Serilog;

namespace Cen.Wms.Domain.Facility.Access.Api.Processors
{
    public class FacilityListDataSourceReadByPersonQuery : IQueryProcessor<TableRowsReq, RpcResponse<DataSourceResult<FacilityListModel>>>
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork<WmsContext> _unitOfWork;
        private readonly WmsContext _wmsContext;
        private readonly IMapper _mapper;

        public FacilityListDataSourceReadByPersonQuery(
            ILogger logger, UnitOfWork<WmsContext> unitOfWork, 
            WmsContext wmsContext, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _wmsContext = wmsContext;
            _mapper = mapper;
        }
        
        public async Task<RpcResponse<DataSourceResult<FacilityListModel>>> Run(IUserIdProvider userIdProvider,
            TableRowsReq request)
        {
            var dataSourceRequest = request.GetDataSourceRequest();
            var dataSourceResult = await _wmsContext.Facility
                .Join(
                    _wmsContext.FacilityAccess.Where(fa => fa.UserId == userIdProvider.UserGuid),
                    f => f.Id,
                    fa => fa.FacilityId,
                    (f, fa) => f
                )
                .ProjectTo<FacilityListModel>(_mapper.ConfigurationProvider)
                .ToDataSourceResultAsync(dataSourceRequest);
            
            return RpcResponse<DataSourceResult<FacilityListModel>>.WithSuccess(
                dataSourceResult.AsTyped<FacilityListModel>()
            );
        }
    }
}