using System;

namespace Cen.Wms.Domain.Purchase.Api.Dtos
{
    public class PurchaseTaskPacHeadEditReq
    {
        public Guid PurchaseTaskId { get; set; }
        public Guid PacId { get; set; }
    }
}