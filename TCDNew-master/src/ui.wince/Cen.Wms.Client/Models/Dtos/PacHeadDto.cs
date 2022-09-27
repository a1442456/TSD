using System;

namespace Cen.Wms.Client.Models.Dtos
{
    public class PacHeadDto : PacKeyDto
    {
        public string PacCode { get; set; }

        public DateTime DeliveryDate { get; set; }
        public string SupplierName { get; set; }
        public string Gate { get; set; }

        public decimal TotalLinesSum { get; set; }
        public long TotalLinesCount { get; set; }
    }
}
