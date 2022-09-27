using System;
using System.Collections.Generic;
using Cen.Common.Domain.Models;
using NodaTime;

namespace Cen.Wms.Data.Models.Purchase
{
    public class PurchaseTaskLineRow: DataModel
    {
        public string ProductExtId { get; set; }
        public string ProductName { get; set; }
        public string ProductAbc { get; set; }
        public string[] ProductBarcodes { get; set; }
        public decimal Quantity { get; set; }
        public Instant ChangedAt { get; set; }

        public Guid PurchaseTaskHeadId { get; set; }
        public PurchaseTaskHeadRow PurchaseTaskHead { get; set; }
        public PurchaseTaskLineStateRow PurchaseTaskLineState { get; set; }
        public List<PurchaseTaskLinePalletedStateRow> PurchaseTaskLinePalletedStates { get; set; }
    }
}