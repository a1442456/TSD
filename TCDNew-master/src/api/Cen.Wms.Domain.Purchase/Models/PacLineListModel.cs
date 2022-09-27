using System;
using System.Collections.Generic;
using NodaTime;

namespace Cen.Wms.Domain.Purchase.Models
{
    public class PacLineListModel
    {
        public Guid Id { get; set; }
        public string ExtId { get; set; }
        public int LineNum { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductBarcodeMain { get; set; }
        public string ProductUnitOfMeasure { get; set; }
        public decimal QtyExpected { get; set; }
        public List<string> ProductBarcodes { get; set; }
        
        public LocalDate? ExpirationDate { get; set; }
        public int ExpirationDaysPlus { get; set; }
        public decimal QtyNormal { get; set; }
        public decimal QtyBroken { get; set; }
    }
}