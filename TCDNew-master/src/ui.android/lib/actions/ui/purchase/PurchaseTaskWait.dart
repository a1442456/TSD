import 'package:tiq/router.dart';
import 'package:flutter/material.dart';

class PurchaseTaskWait {
  Future<String> run(BuildContext context) async {
    final formData = await Navigator.pushNamed(context, PurchaseTaskWaitRoute);
    return (formData as String);
  }
}
