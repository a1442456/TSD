using System.Collections.Generic;

namespace Cen.Wms.Client.Models.Dtos
{
    public class PurchaseTaskUpdateDto
    {
        public string PurchaseTaskId { get; set; }
        public long PurchaseTaskVersion { get; set; }
        public List<PurchaseTaskLineDto> LinesUpdated { get; set; }
        public List<PurchaseTaskPalletDto> PalletsUpdated { get; set; }
    }
}
