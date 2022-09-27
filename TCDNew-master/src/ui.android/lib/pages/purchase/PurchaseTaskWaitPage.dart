import 'dart:async';

import 'package:tiq/actions/data/purchase/PurchaseTaskListReadByPerson.dart';
import 'package:tiq/common/Consts.dart';
import 'package:flutter/material.dart';
import 'package:tiq/models/dtos/PurchaseTaskDto.dart';
import 'package:tiq/widgets/purchase/PurchaseTaskCard.dart';

class PurchaseTaskWaitPage extends StatefulWidget {
  @override
  _PurchaseTaskWaitPageState createState() => _PurchaseTaskWaitPageState();
}

class _PurchaseTaskWaitPageState extends State<PurchaseTaskWaitPage> {
  final List<PurchaseTaskDto> _entries = <PurchaseTaskDto>[];
  Timer _timer;
  String result;

  @override
  void initState() {
    super.initState();

    updatePurchaseTasksList().then((value) => {
      _timer = Timer.periodic(Duration(seconds: 5), this.onTimer)
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
            title: Text('Ожидание задания...'),
        ),
        body: Container(
            margin: const EdgeInsets.all(Consts.marginDefault),
            child: Column(
              children: <Widget>[
                Text(
                  'Выберите задание',
                  textAlign: TextAlign.left,
                  style: TextStyle(fontSize: 20.0),
                ),
                Expanded(
                  child: Scrollbar(
                    child: ListView.builder(
                      padding: const EdgeInsets.symmetric(vertical: 8),
                      itemCount: _entries.length,
                      itemBuilder: (BuildContext context, int index) => PurchaseTaskCard(_entries[index], this.result, this.onItemTap),
                    ),
                  )
                ),
                Column(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  crossAxisAlignment: CrossAxisAlignment.stretch,
                  children: <Widget>[
                    RaisedButton(
                      color: Colors.blue,
                      textColor: Colors.white,
                      onPressed: this.result != null ? this.onPurchaseProcessingStart : null,
                      child: Text('Начать приемку'),
                    ),
                    RaisedButton(
                      onPressed: this.onCancel,
                      child: Text('Отмена'),
                    ),
                  ],
                )
              ],
            )
        )
    );
  }

  Future<void> onTimer(Timer t) async {
    return await this.updatePurchaseTasksList();
  }

  Future<void> updatePurchaseTasksList() async {
    final purchaseTasks = await PurchaseTaskListReadByPerson().run(context);
    purchaseTasks.sort((a,b) => b.createdAt.compareTo(a.createdAt));

    setState(() {
      if (!purchaseTasks.any((element) => element.id == result)) {
        result = null;
      }
      _entries.clear();
      for(var purchaseTask in purchaseTasks) {
        _entries.add(purchaseTask);
      }
    });
  }

  Future<void> onItemTap(PurchaseTaskDto purchaseTask) async {
    setState(() {
      this.result = purchaseTask != null ? purchaseTask.id : null;
    });
  }

  void onPurchaseProcessingStart() {
    _timer.cancel();
    Navigator.of(context).pop(result);
  }

  void onCancel() {
    _timer.cancel();
    Navigator.of(context).pop(null);
  }

  @override
  void dispose() {
    if (_timer != null) {
      _timer.cancel();
    }

    super.dispose();
  }
}
