class PurchaseTaskLineReadByBarcodeReq {
  String purchaseTaskId;
  String barcode;
  String currentPalletCode;

  PurchaseTaskLineReadByBarcodeReq(this.purchaseTaskId, this.barcode, this.currentPalletCode);

  toJson() {
    return {
      'purchaseTaskId': purchaseTaskId,
      'barcode': barcode,
      'currentPalletCode': currentPalletCode,
    };
  }
}