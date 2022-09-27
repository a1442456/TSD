import 'package:tiq/actions/ui/utility/ShowModalMessage.dart';
import 'package:flutter/material.dart';
import 'package:tiq/common/Consts.dart';
import 'package:tiq/common/Messages.dart';
import 'package:tiq/models/state/SettingsApp.dart';
import 'package:tiq/services/GStateProvider.dart';
import 'package:tiq/utils/EdDsaHelpers.dart';
import 'package:tiq/utils/JsonHelpers.dart';

class SettingsForm extends StatefulWidget {
  @override
  _SettingsFormState createState() => _SettingsFormState();
}

class _SettingsFormState extends State<SettingsForm> {
  final _formKey = GlobalKey<FormState>();
  final _wmsServiceBaseAddressEditingController = TextEditingController();
  final _publicKeyEditingController = TextEditingController();
  final _passwordEditingController = TextEditingController();

  _SettingsFormState() {
    _wmsServiceBaseAddressEditingController.text = GStateProvider.instance.settingsApp.wmsServiceBaseAddress;
    getPublicKey(Consts.publicKeyFileName).then((value) => _publicKeyEditingController.text = value );
  }

  @override
  Widget build(BuildContext context) {
    return Form(
      key: _formKey,
      child: Column(
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: <Widget>[
          Column(
            children: <Widget>[
              TextFormField(
                autofocus: true,
                decoration: const InputDecoration(
                  labelText: 'Адрес сервиса',
                ),
                controller: _wmsServiceBaseAddressEditingController,
                validator: validateWMSServiceBaseAddress,
              ),
              TextField(
                decoration: const InputDecoration(
                  labelText: 'Публичный ключ устройства',
                ),
                controller: _publicKeyEditingController,
                readOnly: true,
                keyboardType: TextInputType.multiline,
                maxLines: null
              ),
              TextFormField(
                decoration: const InputDecoration(
                  labelText: 'Пароль',
                ),
                controller: _passwordEditingController,
              ),
            ],
          ),
          Column(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            crossAxisAlignment: CrossAxisAlignment.stretch,
            children: <Widget>[
              RaisedButton(
                color: Colors.blue,
                textColor: Colors.white,
                onPressed: this.onSaveSettings,
                child: Text('Сохранить'),
              ),
              RaisedButton(
                onPressed: this.onCancel,
                child: Text('Отмена'),
              ),
            ],
          )
        ],
      ),
    );
  }

  Future<void> onSaveSettings() async {
    if (_passwordEditingController.text != '1204') {
      await ShowModalMessage().run(
        context,
        Messages.ErrorAuthorisation,
        Messages.ErrorPasswordIncorrect
      );

      return;
    }

    final newSettings = SettingsApp(wmsServiceBaseAddress: _wmsServiceBaseAddressEditingController.text);
    objectToJsonFile(
      newSettings,
      await SettingsApp.getSettingsFilePath()
    );
    GStateProvider.instance.setSettingsApp(newSettings);

    Navigator.of(context).pop(true);
  }

  Future<void> onCancel() async {
    Navigator.of(context).pop(false);
  }

  String validateWMSServiceBaseAddress(String value) {
    if (value.isEmpty) {
      return 'Введите адрес';
    }
    return null;
  }

  @override
  void dispose() {
    _wmsServiceBaseAddressEditingController.dispose();
    _publicKeyEditingController.dispose();
    _passwordEditingController.dispose();

    super.dispose();
  }
}
