using NodaTime;

namespace Cen.Wms.Domain.Sync.Models
{
    public class PacResultLineExt
    {
        public string PacLineId { get; set; }
        public string ProductId { get; set; }
        public decimal QtyExpected { get; set; }
        public decimal QtyNormal { get; set; }
        public decimal QtyBroken { get; set; }
        public LocalDate? ProdDate { get; set; }
        public long ExpDays { get; set; }
        public LocalDate? ExpDate { get; set; }
        public string PalletCode { get; set; }
    }
}