import 'package:tiq/models/enums/TaskType.dart';
import 'package:tiq/common/Consts.dart';
import 'package:flutter/material.dart';
import 'package:tiq/services/GStateProvider.dart';

class TaskSelectPage extends StatefulWidget {
  @override
  _TaskSelectPageState createState() => _TaskSelectPageState();
}

class _TaskSelectPageState extends State<TaskSelectPage> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: Text('Вид работ'),
        ),
        body: Container(
          margin: const EdgeInsets.all(Consts.marginDefault),
          child: Column(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            crossAxisAlignment: CrossAxisAlignment.stretch,
            children: <Widget>[
              Column(
                children: <Widget>[
                  Text(
                    'Выберите вид работ',
                    textAlign: TextAlign.center,
                    style: TextStyle(fontSize: 20.0),
                  )
                ],
              ),
              Column(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: <Widget>[
                  RaisedButton(
                    color: Colors.blue,
                    textColor: Colors.white,
                    onPressed: this.onSelectPurchaseByTask,
                    child: Text('Приемка по заданию'),
                  ),
                  GStateProvider.instance.settingsFacility.isAcceptanceByPapersEnabled
                    ? RaisedButton(
                        onPressed: this.onSelectPurchaseByPapers,
                        child: Text('Приемка по КЛП'),
                    )
                    : null,
                  Divider(),
                  RaisedButton(
                    onPressed: this.onCancel,
                    child: Text('Отмена'),
                  ),
                ].where((element) => element != null).toList(),
              )
            ],
          ),
        )
    );
  }

  void onSelectPurchaseByTask() {
    Navigator.of(context).pop(TaskType.purchaseByTask);
  }

  void onSelectPurchaseByPapers() {
    Navigator.of(context).pop(TaskType.purchaseByPapers);
  }

  void onCancel() {
    Navigator.of(context).pop(TaskType.exit);
  }
}
