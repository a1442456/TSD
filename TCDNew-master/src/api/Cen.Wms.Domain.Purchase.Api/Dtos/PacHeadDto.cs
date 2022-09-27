using NodaTime;

namespace Cen.Wms.Domain.Purchase.Api.Dtos
{
    public class PacHeadDto
    {
        public string PacId { get; set; }
        public string PacCode { get; set; }

        public Instant DeliveryDate { get; set; }
        public string SupplierName { get; set; }
        public string Gate { get; set; }

        public decimal TotalLinesSum { get; set; }
        public int TotalLinesCount { get; set; }
    }
}
