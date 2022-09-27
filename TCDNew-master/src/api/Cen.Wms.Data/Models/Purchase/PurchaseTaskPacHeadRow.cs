using System;
using Cen.Common.Domain.Models;

namespace Cen.Wms.Data.Models.Purchase
{
    public class PurchaseTaskPacHeadRow: DataModel
    {
        public Guid PacHeadId { get; set; }
        public PacHeadRow PacHead { get; set; }
        
        public Guid PurchaseTaskHeadId { get; set; }
        public PurchaseTaskHeadRow PurchaseTaskHead { get; set; }
    }
}