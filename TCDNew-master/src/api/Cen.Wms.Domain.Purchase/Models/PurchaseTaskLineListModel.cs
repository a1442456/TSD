using System;
using NodaTime;

namespace Cen.Wms.Domain.Purchase.Models
{
    public class PurchaseTaskLineListModel
    {
        public Guid Id { get; set; }
        public string ProductExtId { get; set; }
        public string ProductName { get; set; }
        public string ProductAbc { get; set; }
        public string[] ProductBarcodes { get; set; }
        public decimal Quantity { get; set; }
        
        public LocalDate? ExpirationDate { get; set; }
        public int ExpirationDaysPlus { get; set; }
        public decimal QtyNormal { get; set; }
        public decimal QtyBroken { get; set; }
    }
}