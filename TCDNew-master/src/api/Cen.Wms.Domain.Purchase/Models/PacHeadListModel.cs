using System;
using NodaTime;

namespace Cen.Wms.Domain.Purchase.Models
{
    public class PacHeadListModel
    {
        public Guid Id { get; set; }
        public string ExtId { get; set; }
        public Instant PacDateTime { get; set; }
        public string FacilityId { get; set; }
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string PurchaseBookingId { get; set; }
        public LocalDate PurchaseBookingDate { get; set; }
        public string PurchaseId { get; set; }
        public LocalDate PurchaseDate { get; set; }
        public Instant ChangedAt { get; set; }
        public Instant? StartedAt { get; set; }
        public Guid? ResponsibleUserId { get; set; }
        public string ResponsibleUserName { get; set; }
        public bool IsBusy { get; set; }
        public bool IsProcessed { get; set; }
        public bool IsExported { get; set; }
    }
}