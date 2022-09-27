import 'package:tiq/models/dtos/PacHeadDto.dart';
import 'package:tiq/router.dart';
import 'package:flutter/material.dart';

class PacHeadConfirmSelection {
  Future<bool> run(BuildContext context, PacHeadDto pacHead) async {
    final formData =
      await Navigator.pushNamed(
          context,
          PacConfirmSelectionRoute,
          arguments: pacHead
      );
    return (formData as bool);
  }
}