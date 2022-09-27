import 'package:tiq/router.dart';
import 'package:flutter/material.dart';

class BarcodePalletScan {
  Future<String> run(BuildContext context) async {
    final formData = await Navigator.pushNamed(context, BarcodePalletScanRoute);
    return (formData as String);
  }
}
