using System;
using Cen.Common.Domain.Models;
using NodaTime;

namespace Cen.Wms.Data.Models.Purchase
{
    public class PacLineStateRow: DataModel
    {
        public string PalletCode { get; set; }
        public LocalDate? ExpirationDate { get; set; }
        public int ExpirationDaysPlus { get; set; }
        public decimal QtyNormal { get; set; }
        public decimal QtyBroken { get; set; }
        public Instant ChangedAt { get; set; }
        
        public Guid PacLineId { get; set; }
        public PacLineRow PacLine { get; set; }

    }
}