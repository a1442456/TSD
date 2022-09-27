enum PurchaseTaskLineUpdateType {
  none,
  update,
  reset,
}

int purchaseTaskLineUpdateTypeToInt(PurchaseTaskLineUpdateType purchaseTaskLineUpdateType) {
  return
    purchaseTaskLineUpdateType == PurchaseTaskLineUpdateType.none ? 2
        : purchaseTaskLineUpdateType == PurchaseTaskLineUpdateType.update ? 4
        : purchaseTaskLineUpdateType == PurchaseTaskLineUpdateType.reset ? 8
        : throw FormatException("PurchaseTaskLineUpdateType: incorrect enum value");
}