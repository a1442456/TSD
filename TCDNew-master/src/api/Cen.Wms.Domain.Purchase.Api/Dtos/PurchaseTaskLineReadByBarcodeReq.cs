using System;

namespace Cen.Wms.Domain.Purchase.Api.Dtos
{
    public class PurchaseTaskLineReadByBarcodeReq
    {
        public Guid PurchaseTaskId { get; set; }
        public string Barcode { get; set; }
        public string CurrentPalletCode { get; set; }
    }
}