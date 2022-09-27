import 'package:intl/intl.dart';
import 'package:tiq/services/BarcodeScannerService.dart';
import 'package:flutter/material.dart';
import 'package:tiq/services/GStateProvider.dart';
import 'package:tiq/utils/EdDsaHelpers.dart';
import 'package:tiq/utils/JsonHelpers.dart';
import 'package:tiq/widgets/WmsApp.dart';

import 'common/Consts.dart';
import 'models/state/SettingsApp.dart';

Future<void> main() async {
  WidgetsFlutterBinding.ensureInitialized();
  await BarcodeScannerService.setupScanner();
  Intl.defaultLocale = 'en_US';
//  initializeDateFormatting('ru_RU', null);

  await initSecurityKeys(Consts.privateKeyFileName, Consts.publicKeyFileName);
  await initSettingsApp();

  runApp(WmsApp());
}

Future<void> initSettingsApp() async {
  var settingsFilePath = await SettingsApp.getSettingsFilePath();
  var settingsFileJsonMap = await jsonMapFromFile(settingsFilePath);
  GStateProvider.instance.setSettingsApp(
    settingsFileJsonMap != null
      ? SettingsApp.fromJson(settingsFileJsonMap)
      : SettingsApp.fromDefaults()
  );
}
