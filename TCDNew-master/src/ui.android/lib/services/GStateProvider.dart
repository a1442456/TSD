import 'package:tiq/models/state/SettingsApp.dart';
import 'package:tiq/models/state/SettingsFacility.dart';
import 'package:tiq/models/state/SettingsUser.dart';
import 'package:tiq/models/state/StateAuth.dart';

class GStateProvider {
  GStateProvider._internal();
  static final GStateProvider _singleton = new GStateProvider._internal();
  static GStateProvider get instance => _singleton;

  SettingsApp _settingsApp;
  SettingsUser _settingsUser;
  SettingsFacility _settingsFacility;
  StateAuth _stateAuth;

  SettingsApp get settingsApp => _settingsApp;
  SettingsUser get settingsUser => _settingsUser;
  SettingsFacility get settingsFacility => _settingsFacility;
  StateAuth get stateAuth => _stateAuth;

  void setSettingsUser(SettingsUser settingsUser) {
    this._settingsUser = settingsUser;
  }

  void setSettingsApp(SettingsApp settingsApp) {
    this._settingsApp = settingsApp;
  }

  void setSettingsFacility(SettingsFacility settingsFacility) {
    this._settingsFacility = settingsFacility;
  }

  void setStateAuth(StateAuth stateAuth) {
    this._stateAuth = stateAuth;
  }
}
