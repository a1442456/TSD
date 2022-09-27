using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.CQRS.Abstract;
using Cen.Common.Data.DataSource.AgGrid;
using Cen.Common.Data.DataSource.Dtos;
using Cen.Common.Domain.Models;
using Cen.Wms.Domain.Purchase.Abstract;
using Cen.Wms.Domain.Purchase.Models;

namespace Cen.Wms.Domain.Purchase.Api.Queries
{
    public class PacLineListQuery : IQueryProcessor<TableRowsWithParamReq<ByIdReq>, RpcResponse<DataSourceResult<PacLineListModel>>>
    {
        private readonly IPacRepository _pacRepository;

        public PacLineListQuery(IPacRepository pacRepository)
        {
            _pacRepository = pacRepository;
        }

        public async Task<RpcResponse<DataSourceResult<PacLineListModel>>> Run(IUserIdProvider userIdProvider,
            TableRowsWithParamReq<ByIdReq> request)
        {
            return await _pacRepository.PacLineList(request, request.Data);
        }
    }
}