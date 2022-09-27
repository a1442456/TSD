import 'package:flutter_localizations/flutter_localizations.dart';
import 'package:tiq/router.dart';
import 'package:flutter/material.dart';

class WmsApp extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      localizationsDelegates: [
        GlobalMaterialLocalizations.delegate,
        GlobalWidgetsLocalizations.delegate,
      ],
      supportedLocales: [
        const Locale('en'),
        const Locale('ru'),
      ],
      title: 'Приемка товара',
      theme: ThemeData(
        primarySwatch: Colors.blue,
      ),
      onGenerateRoute: generateRoute,
      initialRoute: WelcomeRoute,
    );
  }
}