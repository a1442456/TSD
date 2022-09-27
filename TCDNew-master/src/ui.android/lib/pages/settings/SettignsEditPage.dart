import 'package:flutter/material.dart';
import 'package:tiq/common/Consts.dart';
import 'package:tiq/widgets/settings/SettingsForm.dart';

class SettingsEditPage extends StatefulWidget {
  SettingsEditPage({Key key}) : super(key: key);

  @override
  _SettingsEditPageState createState() => _SettingsEditPageState();
}

class _SettingsEditPageState extends State<SettingsEditPage> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: Text('Настройки'),
        ),
        body: Container(
            margin: const EdgeInsets.all(Consts.marginDefault),
            child: SettingsForm()
        )
    );
  }
}