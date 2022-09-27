using NodaTime;

namespace Cen.Wms.Domain.Purchase.Api.Dtos
{
    public class PurchaseTaskLineStateDto
    {
        public Instant? ExpirationDate { get; set; }
        public int ExpirationDaysPlus { get; set; }
        public decimal QtyNormal { get; set; }
        public decimal QtyBroken { get; set; }
    }
}