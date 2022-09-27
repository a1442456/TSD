import 'package:flutter/material.dart';
import 'package:tiq/models/dtos/PurchaseTaskLineDto.dart';
import 'package:tiq/models/dtos/PurchaseTaskLineUpdateDto.dart';
import 'package:tiq/router.dart';

class PurchaseTaskLineEdit {
  Future<PurchaseTaskLineUpdateDto> run(BuildContext context, PurchaseTaskLineDto purchaseTaskLine) async {
    final formData = await Navigator.pushNamed(context, PurchaseTaskLineEditRoute, arguments: purchaseTaskLine);
    return (formData as PurchaseTaskLineUpdateDto);
  }
}