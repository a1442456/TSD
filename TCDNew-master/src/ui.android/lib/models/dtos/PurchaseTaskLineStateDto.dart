class PurchaseTaskLineStateDto {
  DateTime expirationDate;
  int expirationDaysPlus;
  num qtyNormal;
  num qtyBroken;
  num get qtyFull => qtyNormal + qtyBroken;

  PurchaseTaskLineStateDto(this.expirationDate, this.expirationDaysPlus, this.qtyNormal, this.qtyBroken);

  factory PurchaseTaskLineStateDto.fromJson(Map<String, dynamic> json) {
    PurchaseTaskLineStateDto data;

    if (json != null) {
      data = PurchaseTaskLineStateDto(
          json['expirationDate'] != null
            ? DateTime.fromMillisecondsSinceEpoch(json['expirationDate'], isUtc: true)
            : null,
          json['expirationDaysPlus'] ?? 0,
          json['qtyNormal'] ?? 0.00,
          json['qtyBroken'] ?? 0.00
      );
    }

    return data;
  }

  toJson() {
    return {
      'expirationDate': expirationDate != null
        ? expirationDate.millisecondsSinceEpoch
        : null,
      'expirationDaysPlus': expirationDaysPlus,
      'qtyNormal': qtyNormal,
      'qtyBroken': qtyBroken,
    };
  }
}