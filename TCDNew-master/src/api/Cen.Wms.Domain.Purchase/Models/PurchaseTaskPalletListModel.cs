using System;
using NodaTime;

namespace Cen.Wms.Domain.Purchase.Models
{
    public class PurchaseTaskPalletListModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Abc { get; set; }
        public Instant ChangedAt { get; set; }
    }
}