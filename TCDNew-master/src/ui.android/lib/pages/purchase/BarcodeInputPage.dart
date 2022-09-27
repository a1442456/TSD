import 'package:tiq/common/Consts.dart';
import 'package:tiq/widgets/purchase/BarcodeInputForm.dart';
import 'package:flutter/material.dart';

class BarcodeInputPage extends StatefulWidget {
  BarcodeInputPage({Key key}) : super(key: key);

  @override
  _BarcodeInputPageState createState() => _BarcodeInputPageState();
}

class _BarcodeInputPageState extends State<BarcodeInputPage> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
            title: Text('Ввод штрих-кода'),
        ),
        body: Container(
            margin: const EdgeInsets.all(Consts.marginDefault),
            child: BarcodeInputForm()
        )
    );
  }
}