class PurchaseTaskListReadByPersonReq {
  String facilityId;

  PurchaseTaskListReadByPersonReq(this.facilityId);

  toJson() {
    return {
      'facilityId': facilityId,
    };
  }
}