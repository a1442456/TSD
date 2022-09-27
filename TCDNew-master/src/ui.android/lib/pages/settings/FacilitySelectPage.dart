import 'dart:async';

import 'package:flutter/material.dart';
import 'package:flutter/widgets.dart';
import 'package:tiq/actions/data/settings/FacilityListSimpleReadByPerson.dart';
import 'package:tiq/common/Consts.dart';
import 'package:tiq/models/dtos/ViewModelSimple.dart';
import 'package:tiq/widgets/common/ViewModelSimpleCard.dart';

class FacilitySelectPage extends StatefulWidget {
  @override
  _FacilitySelectPageState createState() => _FacilitySelectPageState();
}

class _FacilitySelectPageState extends State<FacilitySelectPage> {
  final List<ViewModelSimple> _entries = <ViewModelSimple>[];
  Timer _timer;
  String result;

  @override
  void initState() {
    super.initState();

    updateFacilityList().then((value) => {
      _timer = Timer.periodic(Duration(seconds: 5), this.onTimer)
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: Text('Торговые объекты'),
        ),
        body: Container(
            margin: const EdgeInsets.all(Consts.marginDefault),
            child: Column(
              children: <Widget>[
                Text(
                  'Выберите торговый объект',
                  textAlign: TextAlign.left,
                  style: TextStyle(fontSize: 20.0),
                ),
                Expanded(
                    child: Scrollbar(
                      child: ListView.builder(
                        padding: const EdgeInsets.symmetric(vertical: 8),
                        itemCount: _entries.length,
                        itemBuilder: (BuildContext context, int index) => ViewModelSimpleCard(_entries[index], this.result, this.onItemTap),
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
                      onPressed: this.result != null ? this.onOK : null,
                      child: Text('Выбрать'),
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
    await this.updateFacilityList();
  }

  Future<void> updateFacilityList() async {
    final facilities = await FacilityListSimpleReadByPerson().run(context);
    facilities.sort((a,b) => a.name.compareTo(b.name));

    setState(() {
      if (!facilities.any((element) => element.id == result)) {
        result = null;
      }
      _entries.clear();
      for(var facility in facilities) {
        _entries.add(facility);
      }
    });
  }

  Future<void> onItemTap(ViewModelSimple facility) async {
    setState(() {
      this.result = facility != null ? facility.id : null;
    });
  }

  void onOK() {
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