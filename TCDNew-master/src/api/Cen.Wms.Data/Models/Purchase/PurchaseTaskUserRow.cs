using System;
using Cen.Common.Domain.Models;
using Cen.Wms.Data.Models.User;

namespace Cen.Wms.Data.Models.Purchase
{
    public class PurchaseTaskUserRow: DataModel
    {
        public Guid UserId { get; set; }
        public UserRow User { get; set; }
        
        public Guid PurchaseTaskHeadId { get; set; }
        public PurchaseTaskHeadRow PurchaseTaskHead { get; set; }
    }
}