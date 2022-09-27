using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.Domain.Models;
using Cen.Wms.Domain.Facility.Config.Models;

namespace Cen.Wms.Domain.Facility.Config.Abstract
{
    public interface IFacilityConfigRepository
    {
        public Task<RpcResponse<FacilityConfigEditModel>> FacilityConfigGet(ByIdReq facilityId);
        public Task<RpcResponse<bool>> FacilityConfigSet(ByIdReq facilityId, FacilityConfigEditModel facilityConfigEditModel);
        public Task<RpcResponse<bool>> FacilityConfigSetIfNotExists(ByIdReq facilityId, FacilityConfigEditModel facilityConfigEditModel);
    }
}