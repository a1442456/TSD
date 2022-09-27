using System;
using Cen.Common.Domain.Models;
using NodaTime;

namespace Cen.Wms.Data.Models.Purchase
{
    public class PurchaseTaskPalletRow: DataModel
    {
        public string Code { get; set; }
        public string Abc { get; set; }
        public Instant ChangedAt { get; set; }
        
        public Guid PurchaseTaskHeadId { get; set; }
        public PurchaseTaskHeadRow PurchaseTaskHead { get; set; }
    }
}