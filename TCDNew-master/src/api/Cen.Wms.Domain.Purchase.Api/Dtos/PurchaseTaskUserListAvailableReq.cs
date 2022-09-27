using System;

namespace Cen.Wms.Domain.Purchase.Api.Dtos
{
    public class PurchaseTaskUserListAvailableReq
    {
        public Guid FacilityId { get; set; }
        public Guid PurchaseTaskHeadId { get; set; }
    }
}