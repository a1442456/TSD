using System;
using System.Collections.Generic;

namespace Cen.Wms.Domain.Purchase.Api.Dtos
{
    public class PurchaseTaskUpdateDto
    {
        public Guid PurchaseTaskId { get; set; }
        public long PurchaseTaskVersion { get; set; }
        public List<PurchaseTaskLineDto> LinesUpdated { get; set; }
        public List<PurchaseTaskPalletDto> PalletsUpdated { get; set; }
    }
}