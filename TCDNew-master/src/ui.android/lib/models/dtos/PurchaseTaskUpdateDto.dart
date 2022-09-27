import 'package:tiq/models/dtos/PurchaseTaskLineDto.dart';
import 'package:tiq/models/dtos/PurchaseTaskPalletDto.dart';

class PurchaseTaskUpdateDto {
  String purchaseTaskId;
  int purchaseTaskVersion;
  List<PurchaseTaskLineDto> linesUpdated;
  List<PurchaseTaskPalletDto> palletsUpdated;

  PurchaseTaskUpdateDto(this.purchaseTaskId, this.purchaseTaskVersion, this.linesUpdated, this.palletsUpdated);

  factory PurchaseTaskUpdateDto.fromJson(Map<String, dynamic> json) {
    PurchaseTaskUpdateDto data;

    if (json != null) {
      final linesUpdated = List<PurchaseTaskLineDto>();
      json['linesUpdated'].forEach((value) => linesUpdated.add(
          PurchaseTaskLineDto.fromJson(value)
      ));

      final palletsUpdated = List<PurchaseTaskPalletDto>();
      json['palletsUpdated'].forEach((value) => palletsUpdated.add(
          PurchaseTaskPalletDto.fromJson(value)
      ));

      data = PurchaseTaskUpdateDto(
          json['purchaseTaskId'].toString(),
          json['purchaseTaskVersion'],
          linesUpdated,
          palletsUpdated
      );
    }

    return data;
  }
}