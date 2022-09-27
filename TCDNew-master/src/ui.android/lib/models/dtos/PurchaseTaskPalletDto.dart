class PurchaseTaskPalletDto {
  String code;
  String abc;

  PurchaseTaskPalletDto(this.code, this.abc);

  factory PurchaseTaskPalletDto.fromJson(Map<String, dynamic> json) {
    PurchaseTaskPalletDto data;

    if (json != null) {
      data = PurchaseTaskPalletDto(
          json['code'],
          json['abc']
      );
    }

    return data;
  }
}