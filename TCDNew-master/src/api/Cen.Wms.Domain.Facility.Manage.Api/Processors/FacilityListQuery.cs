using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Data.DataSource.AgGrid;
using Cen.Common.Data.DataSource.Dtos;
using Cen.Common.Data.EntityFramework;
using Cen.Wms.Data.Context;
using Cen.Wms.Domain.Facility.Manage.Abstract;
using Cen.Wms.Domain.Facility.Manage.Models;
using Serilog;

namespace Cen.Wms.Domain.Facility.Manage.Api.Processors
{
    public class FacilityListQuery : IQueryProcessor<TableRowsReq, RpcResponse<DataSourceResult<FacilityListModel>>>
    {
        private readonly ILogger _logger;
        private readonly UnitOfWork<WmsContext> _unitOfWork;
        private readonly IFacilityRepository _facilityRepository;

        public FacilityListQuery(
            ILogger logger, UnitOfWork<WmsContext> unitOfWork, 
            IFacilityRepository facilityRepository
        )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _facilityRepository = facilityRepository;
        }
        
        public Task<RpcResponse<DataSourceResult<FacilityListModel>>> Run(IUserIdProvider userIdProvider,
            TableRowsReq request)
        {
            return _facilityRepository.FacilityList(request);
        }
    }
}