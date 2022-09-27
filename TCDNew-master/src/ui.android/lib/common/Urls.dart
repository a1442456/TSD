import 'package:tiq/services/GStateProvider.dart';

class Urls {
  Urls._internal();
  static final Urls _singleton = new Urls._internal();
  static Urls get instance => _singleton;

  final String _timeRead =                        'time/read';
  final String _userLogin =                       'auth/login';
  final String _facilityListSimpleReadByPerson =  'facility/list/simple/by_person';
  final String _facilityConfigGet =               'facility/config/get';
  final String _pacHeadReadByBarcode =            'pac/head/read';
  final String _purchaseTaskListByPerson =        'purchase/task/list/by_person';
  final String _purchaseTaskCreateFromPacs =      'purchase/task/create/from_pacs';
  final String _purchaseTaskUpdateRead =          'purchase/task/update/read';
  final String _purchaseTaskLineReadByBarcode =   'purchase/task/line/read/by_barcode';
  final String _purchaseTaskLineUpdatePost =      'purchase/task/line/update/post';
  final String _purchaseTaskFinish =              'purchase/task/finish';

  String get timeReadUrl => GStateProvider.instance.settingsApp.wmsServiceBaseAddress + _timeRead;
  String get userLoginUrl => GStateProvider.instance.settingsApp.wmsServiceBaseAddress + _userLogin;
  String get facilityListSimpleReadByPersonUrl => GStateProvider.instance.settingsApp.wmsServiceBaseAddress + _facilityListSimpleReadByPerson;
  String get facilityConfigGetUrl => GStateProvider.instance.settingsApp.wmsServiceBaseAddress + _facilityConfigGet;
  String get pacHeadReadByBarcodeUrl => GStateProvider.instance.settingsApp.wmsServiceBaseAddress + _pacHeadReadByBarcode;
  String get purchaseTaskListForPersonUrl => GStateProvider.instance.settingsApp.wmsServiceBaseAddress + _purchaseTaskListByPerson;
  String get purchaseTaskCreateFromPacsUrl => GStateProvider.instance.settingsApp.wmsServiceBaseAddress + _purchaseTaskCreateFromPacs;
  String get purchaseTaskUpdateReadUrl => GStateProvider.instance.settingsApp.wmsServiceBaseAddress + _purchaseTaskUpdateRead;
  String get purchaseTaskLineReadByBarcodeUrl => GStateProvider.instance.settingsApp.wmsServiceBaseAddress + _purchaseTaskLineReadByBarcode;
  String get purchaseTaskLineUpdatePostUrl => GStateProvider.instance.settingsApp.wmsServiceBaseAddress + _purchaseTaskLineUpdatePost;
  String get purchaseTaskFinishUrl => GStateProvider.instance.settingsApp.wmsServiceBaseAddress + _purchaseTaskFinish;
}