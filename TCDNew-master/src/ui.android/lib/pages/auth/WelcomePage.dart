import 'dart:io';

import 'package:tiq/actions/operation/ApplicationLogin.dart';
import 'package:tiq/actions/operation/ExecuteTask.dart';
import 'package:tiq/actions/operation/FacilitySelectAndSet.dart';
import 'package:tiq/actions/ui/settings/SettingsEdit.dart';
import 'package:tiq/common/Consts.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:tiq/services/UpdateService.dart';

import '../../version.dart';

class WelcomePage extends StatefulWidget {
  WelcomePage({Key key}) : super(key: key);

  @override
  _WelcomePageState createState() => _WelcomePageState();
}

class _WelcomePageState extends State<WelcomePage> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Inventory Central'),
      ),
      body: Container(
        margin: const EdgeInsets.all(Consts.marginDefault),
        child: Column(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            crossAxisAlignment: CrossAxisAlignment.stretch,
            children: <Widget>[
              Text(
                'Для начала работы необходимо нажать кнопку "ВХОД"',
                textAlign: TextAlign.center,
                style: TextStyle(fontSize: 20.0),
              ),
              Column(
                crossAxisAlignment: CrossAxisAlignment.stretch,
                children: <Widget>[
                  Row(
                    mainAxisAlignment: MainAxisAlignment.spaceBetween,
                    children: <Widget>[
                      Text('V' + Version.current.toString()),
//                  Image.asset(
//                    'assets/images/CloudsEnergyTextLogo_sm.jpg',
//                    width: 154.0,
//                    height: 24,
//                  ),
                    ],
                  ),
                  RaisedButton(
                    child: Text('ВХОД'),
                    color: Colors.blue,
                    textColor: Colors.white,
                    onPressed: this.onLogin
                  ),
                  Divider(),
                  RaisedButton(
                      child: Text('Настройки'),
                      onPressed: this.onSettings
                  ),
                  RaisedButton(
                      child: Text('Обновление'),
                      onPressed: this.onUpdate
                  ),
                ],
              ),
            ]
        ),
      ),
    );
  }

  Future<void> onLogin() async {
    final wasLoginSuccessful = await ApplicationLogin().run(context);
    if (wasLoginSuccessful) {
        final wasFacilitySelected = await FacilitySelectAndSet().run(context);
        if (wasFacilitySelected) {
          await ExecuteTask().run(context);
        }
    }

    await SystemChannels.platform.invokeMethod('SystemNavigator.pop');

    exit(0);
  }

  Future<void> onSettings() async {
    await SettingsEdit().run(context);
  }

  Future<void> onUpdate() async {
    await UpdateService.updateApplication();
  }
}
