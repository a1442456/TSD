using System;
using System.Collections.Generic;
using NodaTime;

namespace Cen.Wms.Domain.Purchase.Models
{
    public class PacHeadEditModel
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
        public List<PacLineEditModel> Lines { get; set; }
        public Instant ChangedAt { get; set; }
    }
}