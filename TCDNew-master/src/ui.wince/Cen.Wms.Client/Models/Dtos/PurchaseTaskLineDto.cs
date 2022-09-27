namespace Cen.Wms.Client.Models.Dtos
{
    public class PurchaseTaskLineDto
    {
        public string Id { get; set; }

        public ProductDto Product { get; set; }
        public decimal Quantity { get; set; }

        public PurchaseTaskLineStateDto State { get; set; }
    }
}