using System;
using Cen.Common.Domain.Models;
using Cen.Wms.Data.Models.Purchase.Enums;
using Cen.Wms.Data.Models.User;
using NodaTime;

namespace Cen.Wms.Data.Models.Purchase
{
    public class PurchaseTaskLineUpdateRow: DataModel
    {
        public Guid PurchaseTaskHeadId { get; set; }
        public PurchaseTaskHeadRow PurchaseTaskHead { get; set; }
        public Guid PurchaseTaskLineId { get; set; }
        public PurchaseTaskLineRow PurchaseTaskLine { get; set; }
        
        public Guid UserId { get; set; }
        public UserRow User { get; set; }
        
        public string ProductBarcode { get; set; }
        public string CurrentPalletCode { get; set; }
        
        public PurchaseTaskLineUpdateType PurchaseTaskLineUpdateType { get; set; }
        public LocalDate? ExpirationDate { get; set; }
        public int ExpirationDaysPlus { get; set; }
        public decimal QtyNormal { get; set; }
        public decimal QtyBroken { get; set; }
        public Instant CreatedAt { get; set; }
    }
}