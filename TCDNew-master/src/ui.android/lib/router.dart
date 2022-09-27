import 'package:tiq/pages/auth/UserCredentialsPage.dart';
import 'package:tiq/pages/auth/WelcomePage.dart';
import 'package:tiq/pages/purchase/BarcodeInputPage.dart';
import 'package:tiq/pages/purchase/BarcodePalletScanPage.dart';
import 'package:tiq/pages/purchase/PacConfirmSelectionPage.dart';
import 'package:tiq/pages/purchase/PurchaseTaskContentScanPage.dart';
import 'package:tiq/pages/purchase/PurchaseTaskLineEditPage.dart';
import 'package:tiq/pages/purchase/PurchaseTaskPausePage.dart';
import 'package:tiq/pages/purchase/PacScanPage.dart';
import 'package:tiq/pages/purchase/PurchaseTaskWaitPage.dart';
import 'package:tiq/pages/settings/FacilitySelectPage.dart';
import 'package:tiq/pages/settings/SettignsEditPage.dart';
import 'package:tiq/pages/task/TaskSelectPage.dart';
import 'package:flutter/material.dart';

const String WelcomeRoute = '/';
const String AuthRoute = 'auth/login';
const String FacilitySelectRoute = 'facility/select';
const String TaskSelectRoute = 'task/select';
const String PacScanRoute = 'purchase/scan';
const String PurchaseTaskWaitRoute = 'purchase/task/wait';
const String PacConfirmSelectionRoute = 'purchase/confirm';
const String PurchaseTaskContentScanRoute = 'purchase/content/scan';
const String PurchaseTaskLineEditRoute = 'purchase/line/edit';
const String PurchaseTaskPauseRoute = 'purchase/pause';
const String BarcodeInputRoute = 'barcode/input';
const String BarcodePalletScanRoute = 'barcode/pallet/scan';
const String SettingsEditRoute = 'settings/edit';

Route<dynamic> generateRoute(RouteSettings settings) {
  switch (settings.name) {
    case WelcomeRoute:
      return MaterialPageRoute(builder: (context) => WelcomePage());

    case AuthRoute:
      return MaterialPageRoute(builder: (context) => UserCredentialsPage());

    case FacilitySelectRoute:
      return MaterialPageRoute(builder: (context) => FacilitySelectPage());

    case TaskSelectRoute:
      return MaterialPageRoute(builder: (context) => TaskSelectPage());

    case PacScanRoute:
      return MaterialPageRoute(builder: (context) => PacScanPage());

    case PurchaseTaskWaitRoute:
      return MaterialPageRoute(builder: (context) => PurchaseTaskWaitPage());

    case PacConfirmSelectionRoute:
      return MaterialPageRoute(builder: (context) => PacConfirmSelectionPage(
        pacHead: settings.arguments
      ));

    case PurchaseTaskContentScanRoute:
      return MaterialPageRoute(builder: (context) => PurchaseTaskContentScanPage(
          settings.arguments
      ));

    case PurchaseTaskLineEditRoute:
      return MaterialPageRoute(builder: (context) => PurchaseTaskLineEditPage(
          settings.arguments
      ));

    case PurchaseTaskPauseRoute:
      return MaterialPageRoute(builder: (context) => PurchaseTaskPausePage());

    case BarcodeInputRoute:
      return MaterialPageRoute(builder: (context) => BarcodeInputPage());

    case BarcodePalletScanRoute:
      return MaterialPageRoute(builder: (context) => BarcodePalletScanPage());

    case SettingsEditRoute:
      return MaterialPageRoute(builder: (context) => SettingsEditPage());

    default:
      return MaterialPageRoute(builder: (context) => WelcomePage());
  }
}
