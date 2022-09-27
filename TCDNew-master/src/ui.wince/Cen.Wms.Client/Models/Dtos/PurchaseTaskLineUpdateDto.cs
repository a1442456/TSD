using Cen.Wms.Client.Models.Enums;

namespace Cen.Wms.Client.Models.Dtos
{
    public class PurchaseTaskLineUpdateDto
    {
        private readonly string _purchaseTaskLineId;

        public string PurchaseTaskLineId { get { return _purchaseTaskLineId; } }
        public PurchaseTaskLineUpdateType PurchaseTaskLineUpdateType { get; set; }
        public PurchaseTaskLineStateDto PurchaseTaskLineState { get; set; }

        public PurchaseTaskLineUpdateDto(string purchaseTaskLineId)
        {
            _purchaseTaskLineId = purchaseTaskLineId;
            PurchaseTaskLineUpdateType = PurchaseTaskLineUpdateType.None;
            PurchaseTaskLineState = null;
        }

        public PurchaseTaskLineUpdateDto(string purchaseTaskLineId, PurchaseTaskLineStateDto purchaseTaskLineState)
        {
            _purchaseTaskLineId = purchaseTaskLineId;
            PurchaseTaskLineUpdateType = PurchaseTaskLineUpdateType.None;
            PurchaseTaskLineState = new PurchaseTaskLineStateDto
            {
                ExpirationDate = purchaseTaskLineState.ExpirationDate,
                ExpirationDaysPlus = purchaseTaskLineState.ExpirationDaysPlus,
                QtyNormal = 0,
                QtyBroken = 0
            };
        }
    }
}
