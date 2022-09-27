class PacHeadReadByBarcodeReq {
  String barcode;
  String facilityId;

  PacHeadReadByBarcodeReq({this.barcode, this.facilityId});

  toJson() {
    return {
      'barcode': barcode,
      'facilityId': facilityId,
    };
  }
}