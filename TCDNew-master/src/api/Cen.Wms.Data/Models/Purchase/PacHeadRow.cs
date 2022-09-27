using System;
using System.Collections.Generic;
using Cen.Common.Domain.Interfaces;
using Cen.Common.Domain.Models;
using Cen.Wms.Data.Models.User;
using NodaTime;

namespace Cen.Wms.Data.Models.Purchase
{
    public class PacHeadRow: DataModel, ISyncable
    {
        public Instant PacDateTime { get; set; }
        public string FacilityId { get; set; }
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string PurchaseBookingId { get; set; }
        public LocalDate PurchaseBookingDate { get; set; }
        public string PurchaseId { get; set; }
        public LocalDate PurchaseDate { get; set; }
        public List<PacLineRow> Lines { get; set; }
        public PacStateRow PacState { get; set; }
        public Guid? ResponsibleUserId { get; set; }
        public UserRow ResponsibleUser { get; set; }
        
        public string ExtId { get; set; }
        public Instant ChangedAt { get; set; }
        public Instant? StartedAt { get; set; }
    }
}