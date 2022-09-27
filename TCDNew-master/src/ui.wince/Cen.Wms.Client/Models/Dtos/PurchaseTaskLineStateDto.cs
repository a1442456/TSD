using System;

namespace Cen.Wms.Client.Models.Dtos
{
    public class PurchaseTaskLineStateDto
    {
        public DateTime? ExpirationDate { get; set; }
        public long ExpirationDaysPlus { get; set; }
        public decimal QtyNormal { get; set; }
        public decimal QtyBroken { get; set; }

        public decimal QtyFull { get { return QtyNormal + QtyBroken; } }
    }
}
