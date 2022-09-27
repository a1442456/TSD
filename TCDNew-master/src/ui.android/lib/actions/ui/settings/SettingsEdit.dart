import 'package:flutter/material.dart';
import 'package:tiq/router.dart';

class SettingsEdit {
  Future<bool> run(BuildContext context) async {
    var result = await Navigator.pushNamed(context, SettingsEditRoute);
    return result;
  }
}