import 'dart:convert';
import 'package:tiq/models/dtos/ScanResult.dart';
import 'package:flutter/services.dart';
import 'package:rxdart/rxdart.dart';


class BarcodeScannerService {
  static const mt65ToNativeMethodChannel = const MethodChannel('wms.bwd.by/mt65/2n');
  static const mt65ToDartMethodChannel = const MethodChannel('wms.bwd.by/mt65/2d');
  static final onBarcodeScannedStatic = PublishSubject<ScanResult>();
  static final BarcodeScannerService _singleton = new BarcodeScannerService._internal();

  final onBarcodeScanned = PublishSubject<ScanResult>();

  factory BarcodeScannerService() {
    return _singleton;
  }

  BarcodeScannerService._internal() {
    BarcodeScannerService.onBarcodeScannedStatic.listen(onData);
  }

  static Future<void> setupScanner() async {
    await mt65ToNativeMethodChannel.invokeMethod('scannerSetup');
    mt65ToDartMethodChannel.setMethodCallHandler(_methodCallHandler);
  }

  static Future<dynamic> _methodCallHandler(MethodCall call) async {
    if (call.method == 'scannerScan') {
      final jsonString = call.arguments.toString();
      final jsonParsed = json.decode(jsonString);
      final scanResult = ScanResult.fromJson(jsonParsed);

      onBarcodeScannedStatic.add(scanResult);
    }
  }

  void onData(ScanResult event) {
    this.onBarcodeScanned.add(event);
  }
}
