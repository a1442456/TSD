import 'package:tiq/models/dtos/PacHeadDto.dart';
import 'package:flutter/material.dart';

class PacHeadCard extends StatelessWidget {
  final PacHeadDto _pacHead;

  PacHeadCard(this._pacHead);

  @override
  Widget build(BuildContext context) {
    return Container(
      child: Expanded(
        child: Text(
          '${_pacHead.pacCode}: ${_pacHead.supplierName}',
          style: TextStyle(fontSize: 16.0),
        ),
      )
    );
  }
}