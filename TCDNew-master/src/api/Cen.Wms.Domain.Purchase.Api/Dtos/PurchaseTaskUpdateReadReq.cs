using System;

namespace Cen.Wms.Domain.Purchase.Api.Dtos
{
    public class PurchaseTaskUpdateReadReq
    {
        public Guid PurchaseTaskId { get; set; }
        public long PurchaseTaskVersion { get; set; }
    }
}