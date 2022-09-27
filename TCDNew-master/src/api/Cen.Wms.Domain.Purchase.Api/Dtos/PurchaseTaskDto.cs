using System;
using NodaTime;

namespace Cen.Wms.Domain.Purchase.Api.Dtos
{
    public class PurchaseTaskDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string CreatedByUserId { get; set; }
        public string CreatedByUserName { get; set; }
        public Instant CreatedAt { get; set; }
        public Instant ChangedAt { get; set; }
        public Instant? StartedAt { get; set; }
        public bool IsPubliclyAvailable { get; set; }
    }
}