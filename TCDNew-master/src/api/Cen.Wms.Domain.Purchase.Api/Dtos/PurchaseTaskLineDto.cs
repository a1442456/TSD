using System;

namespace Cen.Wms.Domain.Purchase.Api.Dtos
{
    public class PurchaseTaskLineDto
    {
        public Guid Id { get; set; }

        public ProductDto Product { get; set; }
        public decimal Quantity { get; set; }

        public PurchaseTaskLineStateDto State { get; set; }
    }
}