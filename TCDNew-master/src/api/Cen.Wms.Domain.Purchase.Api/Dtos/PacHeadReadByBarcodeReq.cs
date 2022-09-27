using System;

namespace Cen.Wms.Domain.Purchase.Api.Dtos
{
    public class PacHeadReadByBarcodeReq
    {
        public Guid FacilityId { get; set; }
        public string Barcode { get; set; }
    }
}
