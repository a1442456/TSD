import 'package:tiq/models/enums/PurchaseTaskPauseAction.dart';
import 'package:tiq/common/Consts.dart';
import 'package:flutter/material.dart';

class PurchaseTaskPausePage extends StatefulWidget {
  PurchaseTaskPausePage({Key key}) : super(key: key);

  @override
  _PurchaseTaskPausePageState createState() => _PurchaseTaskPausePageState();
}

class _PurchaseTaskPausePageState extends State<PurchaseTaskPausePage> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Выберите действие'),
      ),
      body: Container(
        margin: const EdgeInsets.all(Consts.marginDefault),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.end,
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: <Widget>[
            Column(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              crossAxisAlignment: CrossAxisAlignment.stretch,
              children: <Widget>[
                RaisedButton(
                  color: Colors.green,
                  textColor: Colors.white,
                  onPressed: this.onPurchaseFinishAndUpload,
                  child: Text('Завершить и отправить задание'),
                ),
                SizedBox(height: 32.0),
                RaisedButton(
                  color: Colors.purple,
                  textColor: Colors.white,
                  onPressed: this.onPurchaseFinish,
                  child: Text('Завершить задание'),
                ),
                Divider(height: 32),
                RaisedButton(
                  color: Colors.red,
                  textColor: Colors.white,
                  onPressed: this.onPurchaseCancel,
                  child: Text('Отменить задание'),
                ),
                Divider(height: 32),
                RaisedButton(
                  color: Colors.blue,
                  textColor: Colors.white,
                  onPressed: this.onPurchaseContinue,
                  child: Text('Продолжить задание'),
                ),
                RaisedButton(
                  onPressed: this.onPurchaseLeave,
                  child: Text('Выйти из задания'),
                ),
              ],
            )
          ],
        ),
      )
    );
  }

  onPurchaseFinishAndUpload() {
    Navigator.of(context).pop(PurchaseTaskPauseAction.finishAndUpload);
  }

  onPurchaseFinish() {
    Navigator.of(context).pop(PurchaseTaskPauseAction.finish);
  }

  onPurchaseCancel() {
    Navigator.of(context).pop(PurchaseTaskPauseAction.cancel);
  }

  onPurchaseContinue() {
    Navigator.of(context).pop(PurchaseTaskPauseAction.back);
  }

  onPurchaseLeave() {
    Navigator.of(context).pop(PurchaseTaskPauseAction.leave);
  }
}