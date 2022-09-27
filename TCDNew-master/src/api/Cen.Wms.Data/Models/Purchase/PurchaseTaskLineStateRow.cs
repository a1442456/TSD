using System;
using Cen.Common.Domain.Models;
using NodaTime;

namespace Cen.Wms.Data.Models.Purchase
{
    public class PurchaseTaskLineStateRow: DataModel
    {
        public LocalDate? ExpirationDate { get; set; }
        public int ExpirationDaysPlus { get; set; }
        public decimal QtyNormal { get; set; }
        public decimal QtyBroken { get; set; }
        public Instant ChangedAt { get; set; }
        
        public Guid PurchaseTaskLineId { get; set; }
        public PurchaseTaskLineRow PurchaseTaskLine { get; set; }
    }
}