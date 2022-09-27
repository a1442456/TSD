using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Data.DataSource.AgGrid;
using Cen.Common.Data.DataSource.Dtos;
using Cen.Common.Domain.Models;
using Cen.Wms.Domain.Facility.Manage.Abstract;
using Cen.Wms.Domain.Purchase.Abstract;
using Cen.Wms.Domain.Purchase.Models;

namespace Cen.Wms.Domain.Purchase.Api.Queries
{
    public class PacHeadListByFacilityIdQuery : IQueryProcessor<TableRowsWithParamReq<ByIdReq>, RpcResponse<DataSourceResult<PacHeadListModel>>>
    {
        private readonly IFacilityRepository _facilityRepository;
        private readonly IPacRepository _pacRepository;

        public PacHeadListByFacilityIdQuery(IFacilityRepository facilityRepository, IPacRepository pacRepository)
        {
            _facilityRepository = facilityRepository;
            _pacRepository = pacRepository;
        }

        public async Task<RpcResponse<DataSourceResult<PacHeadListModel>>> Run(IUserIdProvider userIdProvider,
            TableRowsWithParamReq<ByIdReq> request)
        {
            var facilityReadResult = await _facilityRepository.FacilityRead(request.Data);
            if (!facilityReadResult.IsSuccess)
                return RpcResponse<DataSourceResult<PacHeadListModel>>.WithSuccess(DataSourceResult<PacHeadListModel>.Empty());
            if (facilityReadResult.Data == null)
                return RpcResponse<DataSourceResult<PacHeadListModel>>.WithSuccess(DataSourceResult<PacHeadListModel>.Empty());

            return await _pacRepository.PacHeadListByFacilityExtId(request, facilityReadResult.Data.ExtId);
        }
    }
}