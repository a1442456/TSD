class PurchaseTaskDto {
  String id;
  String code;
  DateTime createdAt;
  DateTime changedAt;

  PurchaseTaskDto(this.id, this.code, this.createdAt, this.changedAt);
  factory PurchaseTaskDto.fromJson(Map<String, dynamic> json) {
    PurchaseTaskDto data;

    if (json != null) {
      data = PurchaseTaskDto(
          json['id'],
          json['code'],
          DateTime.fromMillisecondsSinceEpoch(json['createdAt'], isUtc: true),
          DateTime.fromMillisecondsSinceEpoch(json['changedAt'], isUtc: true)
      );
    }

    return data;
  }
}