using System;
using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.Data.DataSource.AgGrid;
using Cen.Common.Data.DataSource.Dtos;
using Cen.Common.Domain.Models;
using Cen.Wms.Domain.Facility.Manage.Models;

namespace Cen.Wms.Domain.Facility.Manage.Abstract
{
    public interface IFacilityRepository
    {
        
        public Task<RpcResponse<FacilityEditModel>> FacilityRead(ByIdReq request);
        public Task<RpcResponse<Guid>> FacilityStore(FacilityEditModel facilityEditModel);
        public Task<RpcResponse<bool>> FacilityExists(ByIdReq request);
        public Task<RpcResponse<Guid>> FacilityIdByExtId(string extId);
        public Task<RpcResponse<DataSourceResult<FacilityListModel>>> FacilityList(TableRowsReq request);
    }
}