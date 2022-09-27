import 'package:flutter/material.dart';
import 'package:tiq/models/dtos/ViewModelSimple.dart';

class ViewModelSimpleCard extends StatelessWidget {
  final String _selectedId;
  final ViewModelSimple _viewModel;
  final Future<void> Function(ViewModelSimple purchaseTask) onItemTap;

  ViewModelSimpleCard(this._viewModel, this._selectedId, this.onItemTap);

  @override
  Widget build(BuildContext context) {
    return Card(
      child: ListTile(
        title: Text(this._viewModel.name),
        onTap: this.onTap,
        selected: this._viewModel.id == _selectedId,
      ),
    );
  }

  Future<void> onTap() async {
    return this.onItemTap(this._viewModel);
  }
}