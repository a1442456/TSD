using System;

namespace Cen.Wms.Domain.Purchase.Api.Dtos
{
    public class PurchaseTaskUserEditReq
    {
        public Guid PurchaseTaskId { get; set; }
        public Guid UserId { get; set; }
    }
}