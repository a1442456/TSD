using System;
using Cen.Wms.Data.Models.Purchase.Enums;

namespace Cen.Wms.Domain.Purchase.Api.Dtos
{
    public class PurchaseTaskLineUpdateDto
    {
        public Guid PurchaseTaskLineId { get; set; }
        public PurchaseTaskLineUpdateType PurchaseTaskLineUpdateType { get; set; }
        public PurchaseTaskLineStateDto PurchaseTaskLineState { get; set; }
    }
}