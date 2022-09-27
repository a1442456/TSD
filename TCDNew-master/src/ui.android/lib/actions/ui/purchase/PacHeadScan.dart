import 'package:tiq/models/dtos/PacHeadDto.dart';
import 'package:tiq/router.dart';
import 'package:flutter/material.dart';

class PacHeadScan {
  Future<List<PacHeadDto>> run(BuildContext context) async {
    final formData = await Navigator.pushNamed(context, PacScanRoute);
    return (formData as List<PacHeadDto>);
  }
}
