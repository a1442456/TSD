using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Common.Domain.Models;

namespace Cen.Wms.Domain.Facility.Access.Abstract
{
    public interface IFacilityAccessRepository
    {
        public Task<RpcResponse<bool>> FacilityAccessGrant(ByIdReq facilityId, ByIdReq userId);
        /// <summary>
        /// Переключаем доступ со склада, куда он установлен автоматически на заданный склад
        /// Необходимо для того, чтобы иметь возможность в одну команду из синхронизации изменить склад юзера
        /// </summary>
        /// <param name="facilityId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<RpcResponse<bool>> DefaultFacilityAccessSwitchTo(ByIdReq facilityId, ByIdReq userId);
        public Task<RpcResponse<bool>> FacilityAccessWithdraw(ByIdReq facilityId, ByIdReq userId);
        public Task<RpcResponse<bool>> FacilityAccessIsGranted(ByIdReq facilityId, ByIdReq userId);
    }
}