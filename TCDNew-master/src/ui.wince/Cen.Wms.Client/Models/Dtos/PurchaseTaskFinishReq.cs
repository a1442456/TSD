namespace Cen.Wms.Client.Models.Dtos
{
    public class PurchaseTaskFinishReq
    {
        public string PurchaseTaskId { get; set; }
        public bool IsDecline { get; set; }
        public bool DoUpload { get; set; }
    }
}
