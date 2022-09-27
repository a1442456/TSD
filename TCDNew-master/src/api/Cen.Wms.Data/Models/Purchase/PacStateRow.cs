using System;
using Cen.Common.Domain.Models;

namespace Cen.Wms.Data.Models.Purchase
{
    public class PacStateRow: DataModel
    {
        public bool IsBusy { get; set; }
        public bool IsProcessed { get; set; }
        public bool IsExported { get; set; }
        
        public Guid PacHeadId { get; set; }
        public PacHeadRow PacHead { get; set; }
    }
}