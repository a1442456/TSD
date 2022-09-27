using System;
using System.Collections.Generic;
using Cen.Common.Domain.Interfaces;
using Cen.Common.Domain.Models;
using NodaTime;

namespace Cen.Wms.Data.Models.Purchase
{
    public class PacLineRow: DataModel, ISyncable
    {
        public Guid PacHeadId { get; set; }
        public int LineNum { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductAbc { get; set; }
        public string ProductBarcodeMain { get; set; }
        public string ProductUnitOfMeasure { get; set; }
        public decimal QtyExpected { get; set; }
        public List<string> ProductBarcodes { get; set; }
        
        public string ExtId { get; set; }
        public Instant ChangedAt { get; set; }
        
        public PacHeadRow PacHead { get; set; }
        public List<PacLineStateRow> PacLineStates { get; set; }
    }
}