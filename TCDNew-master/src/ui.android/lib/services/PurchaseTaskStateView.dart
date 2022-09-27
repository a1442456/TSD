import 'dart:async';

import 'package:tiq/models/dtos/PurchaseTaskLineDto.dart';
import 'package:tiq/models/dtos/PurchaseTaskPalletDto.dart';
import 'package:tiq/models/dtos/PurchaseTaskUpdateDto.dart';


class PurchaseTaskStateView {
  String purchaseTaskId;
  int purchaseTaskVersion = 0;
  List<PurchaseTaskLineDto> _purchaseTaskLines = List<PurchaseTaskLineDto>();
  List<PurchaseTaskPalletDto> _purchaseTaskPallets = List<PurchaseTaskPalletDto>();
  String _currentPalletCode = '';

  PurchaseTaskStateView(this.purchaseTaskId);

  int getDifferencesCount() {
    final count = _purchaseTaskLines.where((e) => e.quantity != e.state.qtyFull).length;

    return count;
  }

  int getTotalCount() {
    return _purchaseTaskLines.length;
  }

  int getAcceptedCount() {
    final count = _purchaseTaskLines.where((e) => e.quantity == e.state.qtyFull).length;

    return count;
  }

  void setCurrentPalletCode(String palletCode) {
    this._currentPalletCode = palletCode;
  }

  String getCurrentPalletCode() {
    return this._currentPalletCode ?? '';
  }

  String getCurrentPalletABC() {
    final purchaseTaskPallet = _purchaseTaskPallets.firstWhere(
        (e) => e.code == _currentPalletCode,
        orElse: () => null);
    return purchaseTaskPallet != null ? purchaseTaskPallet.abc : "";
  }

  bool isPalletABCUsed(String abc) {
    final purchaseTaskPallet = _purchaseTaskPallets.firstWhere(
        (e) => e.abc.toLowerCase().contains(abc.trim().toLowerCase()),
        orElse: () => null);
    return purchaseTaskPallet != null;
  }

  bool isPalletABCEmpty(String abc) {
    return abc.trim() == "";
  }

  bool canPalletAcceptProductAbc(String palletABC, String goodABC) {
    return ((palletABC.toLowerCase() == "") || (palletABC.toLowerCase().contains(goodABC.trim().toLowerCase())));
  }

  void purchaseTaskUpdateApply(PurchaseTaskUpdateDto purchaseTaskUpdate) {
    if (purchaseTaskUpdate.purchaseTaskId != this.purchaseTaskId) return;
    // if (purchaseTaskUpdate.purchaseTaskVersion <= this.purchaseTaskVersion) return;

    for(final lineUpdated in purchaseTaskUpdate.linesUpdated) {
      final lineLocal = this._purchaseTaskLines.firstWhere(
          (e) => e.id == lineUpdated.id,
          orElse: () => null);
      if (lineLocal != null) {
        lineLocal.quantity = lineUpdated.quantity;
        lineLocal.state.expirationDate = lineUpdated.state.expirationDate;
        lineLocal.state.expirationDaysPlus = lineUpdated.state.expirationDaysPlus;
        lineLocal.state.qtyNormal = lineUpdated.state.qtyNormal;
        lineLocal.state.qtyBroken = lineUpdated.state.qtyBroken;
        lineLocal.product.id = lineUpdated.product.id;
        lineLocal.product.name = lineUpdated.product.name;
        lineLocal.product.abc = lineUpdated.product.abc;
        lineLocal.product.barcodes = lineUpdated.product.barcodes;
      } else {
        this._purchaseTaskLines.add(lineUpdated);
      }
    }
    for(final palleteUpdated in purchaseTaskUpdate.palletsUpdated) {
      final palleteLocal = this._purchaseTaskPallets.firstWhere(
              (e) => e.code == palleteUpdated.code,
              orElse: () => null);
      if (palleteLocal != null) {
        palleteLocal.abc = palleteUpdated.abc;
      } else {
        this._purchaseTaskPallets.add(palleteUpdated);
      }
    }

    this.purchaseTaskVersion = purchaseTaskUpdate.purchaseTaskVersion;
  }

  Future<List<PurchaseTaskLineDto>> getLinesDataSource() async {
    return this._purchaseTaskLines;
  }
}
