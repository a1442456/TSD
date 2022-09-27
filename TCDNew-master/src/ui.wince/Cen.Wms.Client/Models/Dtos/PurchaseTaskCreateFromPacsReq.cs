using System.Collections.Generic;

namespace Cen.Wms.Client.Models.Dtos
{
    public class PurchaseTaskCreateFromPacsReq
    {
        public string FacilityId { get; set; }
        public IEnumerable<PacKeyDto> Pacs { get; set; }
    }
}
