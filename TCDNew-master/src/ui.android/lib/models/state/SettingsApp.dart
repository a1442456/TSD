import 'package:path/path.dart';
import 'package:path_provider/path_provider.dart';
import 'package:tiq/common/Consts.dart';

class SettingsApp {
  final String wmsServiceBaseAddress;

  SettingsApp({this.wmsServiceBaseAddress});

  factory SettingsApp.fromDefaults() {
    return SettingsApp(
      wmsServiceBaseAddress: Consts.defaultWMSServiceBaseAddress,
    );
  }

  factory SettingsApp.fromJson(Map<String, dynamic> json) {
    if (json == null) {
      throw FormatException('Incorrect JSON');
    }

    return SettingsApp(
      wmsServiceBaseAddress: json['wmsServiceBaseAddress'],
    );
  }

  toJson() {
    return {
      'wmsServiceBaseAddress': wmsServiceBaseAddress,
    };
  }

  static Future<String> getSettingsFilePath() async {
    final documentsDirectory = await getApplicationDocumentsDirectory();
    final fileName = join(documentsDirectory.path, Consts.settingsFileName);

    return fileName;
  }
}