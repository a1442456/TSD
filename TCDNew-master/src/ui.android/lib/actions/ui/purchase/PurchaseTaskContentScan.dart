import 'package:tiq/router.dart';
import 'package:flutter/material.dart';

class PurchaseTaskContentScan {
  Future<void> runWithPurchaseTaskId(BuildContext context, String purchaseTaskId) async {
    await Navigator.pushNamed(
      context,
      PurchaseTaskContentScanRoute,
      arguments: purchaseTaskId
    );
  }
}