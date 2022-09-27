import 'dart:async';

import 'package:tiq/actions/ui/utility/ShowModalMessage.dart';
import 'package:tiq/models/dtos/ScanResult.dart';
import 'package:tiq/services/BarcodeScannerService.dart';
import 'package:tiq/common/Consts.dart';
import 'package:flutter/material.dart';
import 'package:tiq/common/Messages.dart';
import 'package:tiq/services/GStateProvider.dart';

class BarcodePalletScanPage extends StatefulWidget {
  @override
  _BarcodePalletScanState createState() => _BarcodePalletScanState();
}

class _BarcodePalletScanState extends State<BarcodePalletScanPage> {
  BarcodeScannerService _barcodeScannerService;
  StreamSubscription<ScanResult> _onBarcodeScannedSubscription;

  _BarcodePalletScanState() {
    this._barcodeScannerService = BarcodeScannerService();
    this._onBarcodeScannedSubscription = this._barcodeScannerService.onBarcodeScanned.listen(onScan);
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
          title: Text('Паллета'),
        ),
        body: Container(
          margin: const EdgeInsets.all(Consts.marginDefault),
          child: Column(
            children: <Widget>[
              Text(
                'Сосканируйте штрих-код паллеты...',
                textAlign: TextAlign.center,
                style: TextStyle(fontSize: 20.0),
              ),
            ],
          )
        )
    );
  }

  Future<void> onScan(ScanResult data) async {
    if (data.isSuccessful) {
      if (data.barcode1 != null ) {
        final barcode = data.barcode1.trim();
        if (barcode != '') {
          if (barcode.startsWith(GStateProvider.instance.settingsFacility.palletCodePrefix)) {
            Navigator.of(context).pop(barcode);
          } else {
            await ShowModalMessage().run(context, Messages.TitleError, "Сосканированный штрих-код не паллеты!");
          }
        } else {
          await ShowModalMessage().run(context, Messages.TitleError, "Штрих-код пуст!");
        }
      } else {
        await ShowModalMessage().run(context, Messages.TitleError, "Штрих-код пуст!");
      }
    }
  }

  void onCancel() {
    Navigator.of(context).pop(null);
  }

  @override
  void dispose() {
    this._onBarcodeScannedSubscription.cancel();

    super.dispose();
  }
}
