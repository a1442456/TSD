import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:tiq/models/dtos/PurchaseTaskDto.dart';

class PurchaseTaskCard extends StatelessWidget {
  final _dateFormat = DateFormat('dd.MM.yyyy HH:mm');
  final String _selectedPurchaseTaskId;
  final PurchaseTaskDto _purchaseTask;
  final Future<void> Function(PurchaseTaskDto purchaseTask) onItemTap;

  PurchaseTaskCard(this._purchaseTask, this._selectedPurchaseTaskId, this.onItemTap);

  @override
  Widget build(BuildContext context) {
    return Card(
      child: ListTile(
        title: Text(_dateFormat.format(_purchaseTask.createdAt.toLocal()) + ', ' + _purchaseTask.code),
        onTap: this.onTap,
        selected: this._purchaseTask.id == _selectedPurchaseTaskId,
      ),
    );
  }

  Future<void> onTap() async {
    return this.onItemTap(this._purchaseTask);
  }
}