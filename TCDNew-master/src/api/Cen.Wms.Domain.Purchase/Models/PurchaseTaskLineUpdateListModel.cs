using System;
using Cen.Wms.Data.Models.Purchase.Enums;
using NodaTime;

namespace Cen.Wms.Domain.Purchase.Models
{
    public class PurchaseTaskLineUpdateListModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Instant CreatedAt { get; set; }
        
        public string ProductBarcode { get; set; }
        public string CurrentPalletCode { get; set; }
        
        public string ProductExtId { get; set; }
        public string ProductName { get; set; }
        public string ProductAbc { get; set; }
        
        public PurchaseTaskLineUpdateType PurchaseTaskLineUpdateType { get; set; }
        public LocalDate? ExpirationDate { get; set; }
        public int ExpirationDaysPlus { get; set; }
        public decimal QtyNormal { get; set; }
        public decimal QtyBroken { get; set; }
    }
}