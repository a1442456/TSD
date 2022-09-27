using System;

namespace Cen.Wms.Domain.Purchase.Api.Dtos
{
    public class PurchaseTaskLineUpdatePostReq
    {
        public Guid PurchaseTaskId { get; set; }
        public string ProductBarcode { get; set; }
        public string CurrentPalletCode { get; set; }
        public PurchaseTaskLineUpdateDto PurchaseTaskLineUpdate { get; set; }
    }
}