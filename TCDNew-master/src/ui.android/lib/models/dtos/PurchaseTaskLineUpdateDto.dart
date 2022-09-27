import 'package:tiq/models/enums/PurchaseTaskLineUpdateType.dart';

import 'PurchaseTaskLineStateDto.dart';

class PurchaseTaskLineUpdateDto {
  String _purchaseTaskLineId;

  String purchaseTaskLineId() => _purchaseTaskLineId;
  PurchaseTaskLineUpdateType purchaseTaskLineUpdateType;
  PurchaseTaskLineStateDto purchaseTaskLineState;

  PurchaseTaskLineUpdateDto.empty(String purchaseTaskLineId) {
    this._purchaseTaskLineId = purchaseTaskLineId;
    this.purchaseTaskLineUpdateType = PurchaseTaskLineUpdateType.none;
    this.purchaseTaskLineState = null;
  }

  PurchaseTaskLineUpdateDto.withPurchaseTaskLineState(String purchaseTaskLineId, PurchaseTaskLineStateDto purchaseTaskLineState) {
    this._purchaseTaskLineId = purchaseTaskLineId;
    this.purchaseTaskLineUpdateType = PurchaseTaskLineUpdateType.none;
    this.purchaseTaskLineState = PurchaseTaskLineStateDto(
      purchaseTaskLineState.expirationDate,
      purchaseTaskLineState.expirationDaysPlus,
      0,
      0
    );
  }

  toJson() {
    return {
      'purchaseTaskLineId': _purchaseTaskLineId,
      'purchaseTaskLineUpdateType': purchaseTaskLineUpdateTypeToInt(purchaseTaskLineUpdateType),
      'purchaseTaskLineState': purchaseTaskLineState.toJson(),
    };
  }
}