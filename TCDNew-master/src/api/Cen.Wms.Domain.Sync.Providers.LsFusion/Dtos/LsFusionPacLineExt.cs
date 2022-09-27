using System.Collections.Generic;

namespace Cen.Wms.Domain.Sync.Providers.LsFusion.Dtos
{
    public class LsFusionPacLineExt
    {
        public short LineNum { get; set; }
        public string PacLineId { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductAbc { get; set; }
        public string ProductBarcodeMain { get; set; }
        public string ProductUnitOfMeasure { get; set; }
        public decimal QtyExpected { get; set; }
        public List<string> ProductBarcodes { get; set; }
    }
}