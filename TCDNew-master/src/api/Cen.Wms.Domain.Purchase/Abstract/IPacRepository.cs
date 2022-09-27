using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.Data.DataSource.AgGrid;
using Cen.Common.Data.DataSource.Dtos;
using Cen.Common.Domain.Models;
using Cen.Wms.Domain.Purchase.Models;

namespace Cen.Wms.Domain.Purchase.Abstract
{
    public interface IPacRepository
    {
        public Task<RpcResponse<DataSourceResult<PacHeadListModel>>> PacHeadListByFacilityExtId(TableRowsReq request, string facilityExtId);
        public Task<RpcResponse<DataSourceResult<PacLineListModel>>> PacLineList(TableRowsReq request, ByIdReq pacId);
        public Task<RpcResponse<Guid>> PacStore(PacHeadEditModel pacHeadEditModel);
        public Task<RpcResponse<bool>> PacExists(ByIdReq request);
        public Task<RpcResponse<PacHeadEditModel>> PacRead(ByIdReq request);
        public Task<RpcResponse<List<PacHeadEditModel>>> PacReadMany(IEnumerable<ByIdReq> request);
        public Task<RpcResponse<Guid>> PacIdByExtId(string extId);
        public Task<RpcResponse<bool>> PacGetBusy(ByIdReq pacId);
        public Task<RpcResponse<bool>> PacSetBusy(ByIdReq pacId, bool isBusy);
        Task<RpcResponse<bool>> PacSetResponsibleUserId(ByIdReq pacId, ByIdReq userId);
        public Task<RpcResponse<bool>> PacSetProcessed(ByIdReq pacId, bool isProcessed);
    }
}