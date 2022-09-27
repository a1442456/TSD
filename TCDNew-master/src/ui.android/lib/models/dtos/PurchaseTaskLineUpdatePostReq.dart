import 'package:tiq/models/dtos/PurchaseTaskLineUpdateDto.dart';

class PurchaseTaskLineUpdatePostReq {
  String purchaseTaskId;
  String productBarcode;
  String currentPalletCode;
  PurchaseTaskLineUpdateDto purchaseTaskLineUpdate;

  PurchaseTaskLineUpdatePostReq(this.purchaseTaskId, this.productBarcode, this.currentPalletCode, this.purchaseTaskLineUpdate);

  toJson() {
    return {
      'purchaseTaskId': purchaseTaskId,
      'productBarcode': productBarcode,
      'currentPalletCode': currentPalletCode,
      'purchaseTaskLineUpdate': purchaseTaskLineUpdate.toJson(),
    };
  }
}