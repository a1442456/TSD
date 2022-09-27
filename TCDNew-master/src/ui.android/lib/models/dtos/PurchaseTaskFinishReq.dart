class PurchaseTaskFinishReq {
  String purchaseTaskId;
  bool isDecline;
  bool doUpload;

  PurchaseTaskFinishReq({this.purchaseTaskId, this.isDecline, this.doUpload});

  toJson() {
    return {
      'purchaseTaskId': purchaseTaskId,
      'isDecline': isDecline,
      'doUpload': doUpload,
    };
  }
}