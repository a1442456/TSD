using System;
using Cen.Wms.Data.Models.Purchase.Enums;
using NodaTime;

namespace Cen.Wms.Domain.Purchase.Models
{
    public class PurchaseTaskHeadListModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public PurchaseTaskState PurchaseTaskState { get; set; }
        public string CreatedByUserId { get; set; }
        public string CreatedByUserName { get; set; }
        public Instant CreatedAt { get; set; }
        public Instant ChangedAt { get; set; }
        public Instant? StartedAt { get; set; }
        public bool IsPubliclyAvailable { get; set; }
        public bool IsExported { get; set; }
    }
}