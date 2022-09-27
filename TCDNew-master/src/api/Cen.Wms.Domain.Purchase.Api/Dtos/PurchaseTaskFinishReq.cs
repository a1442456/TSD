using System;

namespace Cen.Wms.Domain.Purchase.Api.Dtos
{
    public class PurchaseTaskFinishReq
    {
        public Guid PurchaseTaskId { get; set; }
        public bool IsDecline { get; set; }
        public bool DoUpload { get; set; }
    }
}