using System;
using System.Threading.Tasks;
using Cen.Common.CQRS;
using Cen.Wms.Domain.Purchase.Models;
using Cen.Wms.Domain.Sync.Abstract;

namespace Cen.Wms.Domain.Purchase.Abstract
{
    public interface IPurchaseTaskRepository
    {
        public Task<RpcResponse<Guid>> PurchaseTaskCreateEmpty(Guid facilityId, Guid userId, bool isPubliclyAvailable);
        public Task<RpcResponse<bool>> PurchaseTaskPacInclude(Guid purchaseTaskId, PacHeadEditModel pacHeadEditModel);
        public Task<RpcResponse<bool>> PurchaseTaskPacExclude(Guid purchaseTaskId, PacHeadEditModel pacHeadEditModel);
        public Task<RpcResponse<bool>> PurchaseTaskUserInclude(Guid purchaseTaskId, Guid userId);
        public Task<RpcResponse<bool>> PurchaseTaskUserExclude(Guid purchaseTaskId, Guid userId);
        public Task<RpcResponse<bool>> PurchaseTaskStart(Guid purchaseTaskId);
        public Task<RpcResponse<bool>> PurchaseTaskStop(Guid purchaseTaskId);
        public Task<RpcResponse<bool>> PurchaseTaskCancel(Guid purchaseTaskId);
        public Task<RpcResponse<bool>> PurchaseTaskUpload(Guid purchaseTaskId, IPacUploader pacUploader);
        public Task<RpcResponse<bool>> PurchaseTaskStopAndUpload(Guid purchaseTaskId, IPacUploader pacUploader);
        public Task<RpcResponse<Guid>> PurchaseTaskGetResponsibleUserId(Guid purchaseTaskId);
    }
}