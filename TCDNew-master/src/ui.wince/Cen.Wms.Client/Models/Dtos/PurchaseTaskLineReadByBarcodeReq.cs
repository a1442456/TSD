namespace Cen.Wms.Client.Models.Dtos
{
    public class PurchaseTaskLineReadByBarcodeReq
    {
        public string PurchaseTaskId { get; set; }
        public string Barcode { get; set; }
        public string CurrentPalletCode { get; set; }
    }
}
