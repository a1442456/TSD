class PurchaseTaskUpdateReadReq {
  String purchaseTaskId;
  int purchaseTaskVersion;

  PurchaseTaskUpdateReadReq(this.purchaseTaskId, this.purchaseTaskVersion);

  toJson() {
    return {
      'purchaseTaskId': purchaseTaskId,
      'purchaseTaskVersion': purchaseTaskVersion,
    };
  }
}