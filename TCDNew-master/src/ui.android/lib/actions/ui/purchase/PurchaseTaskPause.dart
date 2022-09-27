import 'package:tiq/models/enums/PurchaseTaskPauseAction.dart';
import 'package:tiq/router.dart';
import 'package:flutter/material.dart';

class PurchaseTaskPause {
  Future<PurchaseTaskPauseAction> run(BuildContext context) async {
    final formData = await Navigator.pushNamed(context, PurchaseTaskPauseRoute);
    return (formData as PurchaseTaskPauseAction);
  }
}
