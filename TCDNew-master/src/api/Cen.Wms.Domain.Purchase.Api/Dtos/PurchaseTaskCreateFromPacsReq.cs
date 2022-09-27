using System;
using System.Collections.Generic;

namespace Cen.Wms.Domain.Purchase.Api.Dtos
{
    public class PurchaseTaskCreateFromPacsReq
    {
        public Guid FacilityId { get; set; }
        public IEnumerable<PacKeyDto>  Pacs { get; set; }
    }
}