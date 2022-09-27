import 'package:tiq/common/Consts.dart';
import 'package:flutter/material.dart';
import 'package:tiq/models/dtos/PurchaseTaskLineDto.dart';
import 'package:tiq/widgets/purchase/PurchaseTaskLineEditForm.dart';

class PurchaseTaskLineEditPage extends StatefulWidget {
  final PurchaseTaskLineDto _purchaseTaskLine;
  PurchaseTaskLineEditPage(this._purchaseTaskLine, {Key key}) : super(key: key);

  @override
  _PurchaseTaskLineEditPageState createState() => _PurchaseTaskLineEditPageState();
}

class _PurchaseTaskLineEditPageState extends State<PurchaseTaskLineEditPage> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: Text('Товарная позиция'),
        ),
        body: Container(
            margin: const EdgeInsets.all(Consts.marginDefault),
            child: PurchaseTaskLineEditForm(widget._purchaseTaskLine)
        )
    );
  }
}