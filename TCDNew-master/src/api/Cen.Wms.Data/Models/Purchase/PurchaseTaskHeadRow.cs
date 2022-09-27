using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Cen.Common.Domain.Interfaces;
using Cen.Common.Domain.Models;
using Cen.Wms.Data.Models.Facility;
using Cen.Wms.Data.Models.Purchase.Enums;
using Cen.Wms.Data.Models.User;
using NodaTime;

namespace Cen.Wms.Data.Models.Purchase
{
    public class PurchaseTaskHeadRow: DataModel, ISyncable
    {
        public const string ClassSequenceName = "purchase_task_code_seq";
        public override string SequenceName => ClassSequenceName;

        [MaxLength(36)]
        public string Code { get; set; }
        public string ExtId { get; set; }
        public Instant CreatedAt { get; set; }
        public Instant ChangedAt { get; set; }
        public Instant? StartedAt { get; set; }
        public bool IsPubliclyAvailable { get; set; }
        public bool IsExported { get; set; }
        
        public Guid FacilityId { get; set; }
        public FacilityRow Facility { get; set; }
        public PurchaseTaskState PurchaseTaskState { get; set; }
        public Guid CreatedByUserId { get; set; }
        public UserRow CreatedByUser { get; set; }
        
        public List<PurchaseTaskLineRow> Lines { get; set; }
        public List<PurchaseTaskLineUpdateRow> LineUpdates { get; set; }
        public List<PurchaseTaskPalletRow> Pallets { get; set; }
        public List<PurchaseTaskPacHeadRow> PacHeads { get; set; }
        public List<PurchaseTaskUserRow> Users { get; set; }
    }
}