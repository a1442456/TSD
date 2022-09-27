import 'PacKeyDto.dart';

class PacHeadDto extends PacKeyDto {
  String pacCode;
  DateTime deliveryDate;
  String supplierName;
  String gate;

  num totalLinesSum;
  int totalLinesCount;

  PacHeadDto(purchaseId, this.pacCode, this.deliveryDate, this.supplierName, this.gate, this.totalLinesSum, this.totalLinesCount)
      : super(pacId: purchaseId);

  factory PacHeadDto.fromJson(Map<String, dynamic> json) {
    PacHeadDto data;

    if (json != null) {
      data = PacHeadDto(
        json['pacId'],
        json['pacCode'],
        DateTime.fromMillisecondsSinceEpoch(json['deliveryDate'], isUtc: true),
        json['supplierName'],
        json['gate'],
        json['totalLinesSum'],
        json['totalLinesCount'],
      );
    }
    return data;
  }
}
