import 'package:intl/intl.dart';
import 'package:tiq/models/dtos/PacHeadDto.dart';
import 'package:tiq/common/Consts.dart';
import 'package:flutter/material.dart';

class PacConfirmSelectionPage extends StatefulWidget {
  PacConfirmSelectionPage({Key key, this.pacHead}) : super(key: key);

  final PacHeadDto pacHead;

  @override
  _PacConfirmSelectionPageState createState() =>
      _PacConfirmSelectionPageState();
}

class _PacConfirmSelectionPageState extends State<PacConfirmSelectionPage> {
  final _dateFormat = DateFormat('dd.MM.yyyy');

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: Text('Закупка'),
        ),
        body: Container(
          margin: const EdgeInsets.all(Consts.marginDefault),
          child: Column(
            mainAxisAlignment: MainAxisAlignment.start,
            crossAxisAlignment: CrossAxisAlignment.stretch,
            children: <Widget>[
              Expanded(
                  child: Column(
                    children: <Widget>[
                      Row(
                        mainAxisAlignment: MainAxisAlignment.start,
                        children: <Widget>[
                          Text(
                            'Код:',
                            textAlign: TextAlign.left,
                            style: TextStyle(fontSize: 20.0, fontWeight: FontWeight.bold),
                          ),
                        ],
                      ),
                      Row(
                        mainAxisAlignment: MainAxisAlignment.start,
                        children: <Widget>[
                          Text(
                            this.widget.pacHead.pacCode,
                            textAlign: TextAlign.left,
                            style: TextStyle(fontSize: 20.0),
                          ),
                        ],
                      ),
                      Row(
                        mainAxisAlignment: MainAxisAlignment.start,
                        children: <Widget>[
                          Text(
                            'Дата поставки:',
                            textAlign: TextAlign.left,
                            style: TextStyle(fontSize: 20.0, fontWeight: FontWeight.bold),
                          ),
                        ],
                      ),
                      Row(
                        mainAxisAlignment: MainAxisAlignment.start,
                        children: <Widget>[
                          Text(
                            this.widget.pacHead.deliveryDate != null
                              ? _dateFormat.format(this.widget.pacHead.deliveryDate)
                              : null,
                            textAlign: TextAlign.left,
                            style: TextStyle(fontSize: 20.0),
                          ),
                        ],
                      ),
                      Row(
                        mainAxisAlignment: MainAxisAlignment.start,
                        children: <Widget>[
                          Text(
                            'Поставщик:',
                            textAlign: TextAlign.left,
                            style: TextStyle(fontSize: 20.0, fontWeight: FontWeight.bold),
                          ),
                        ],
                      ),
                      Row(
                        mainAxisAlignment: MainAxisAlignment.start,
                        children: <Widget>[
                          Expanded(child: Text(
                            this.widget.pacHead.supplierName,
                            textAlign: TextAlign.left,
                            style: TextStyle(fontSize: 20.0),
                          )),
                        ],
                      ),
                      Row(
                        mainAxisAlignment: MainAxisAlignment.start,
                        children: <Widget>[
                          Text(
                            'Рампа:',
                            textAlign: TextAlign.left,
                            style: TextStyle(fontSize: 20.0, fontWeight: FontWeight.bold),
                          ),
                        ],
                      ),
                      Row(
                        mainAxisAlignment: MainAxisAlignment.start,
                        children: <Widget>[
                          Text(
                            this.widget.pacHead.gate,
                            textAlign: TextAlign.left,
                            style: TextStyle(fontSize: 20.0),
                          ),
                        ],
                      ),
                    ],
                  )
              ),
              Column(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: <Widget>[
                  RaisedButton(
                    color: Colors.blue,
                    textColor: Colors.white,
                    onPressed: this.onConfirm,
                    child: Text('Добавить'),
                  ),
                  RaisedButton(
                    onPressed: this.onCancel,
                    child: Text('Отмена'),
                  ),
                ],
              )
            ],
          ),
        ));
  }

  void onConfirm() {
    Navigator.of(context).pop(true);
  }

  void onCancel() {
    Navigator.of(context).pop(false);
  }
}
