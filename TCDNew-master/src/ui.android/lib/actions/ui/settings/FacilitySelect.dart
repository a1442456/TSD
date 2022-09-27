import 'package:tiq/router.dart';
import 'package:flutter/material.dart';

class FacilitySelect {
  Future<String> run(BuildContext context) async {
    final formData = await Navigator.pushNamed(context, FacilitySelectRoute);
    return (formData as String);
  }
}
