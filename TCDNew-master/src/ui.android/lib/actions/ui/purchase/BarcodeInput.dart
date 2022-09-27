import 'package:tiq/router.dart';
import 'package:flutter/material.dart';

class BarcodeInput {
  Future<String> run(BuildContext context) async {
    final formData = await Navigator.pushNamed(context, BarcodeInputRoute);
    return (formData as String);
  }
}