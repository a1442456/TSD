import 'PacKeyDto.dart';

class PurchaseTaskCreateFromPacsReq {
  String facilityId;
  List<PacKeyDto> pacs;

  PurchaseTaskCreateFromPacsReq({this.facilityId, this.pacs});

  toJson() {
    return {
      'facilityId': facilityId,
      'pacs': pacs,
    };
  }
}