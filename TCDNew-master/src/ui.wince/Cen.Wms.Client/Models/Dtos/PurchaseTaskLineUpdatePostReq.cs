namespace Cen.Wms.Client.Models.Dtos
{
    public class PurchaseTaskLineUpdatePostReq
    {
        public string PurchaseTaskId { get; set; }
        public string ProductBarcode { get; set; }
        public string CurrentPalletCode { get; set; }
        public PurchaseTaskLineUpdateDto PurchaseTaskLineUpdate { get; set; }
    }
}
