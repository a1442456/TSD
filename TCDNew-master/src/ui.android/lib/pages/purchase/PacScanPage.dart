import 'dart:async';

import 'package:tiq/actions/ui/purchase/PacHeadConfirmSelection.dart';
import 'package:tiq/actions/data/purchase/PacHeadReadByBarcode.dart';
import 'package:tiq/actions/ui/utility/ShowModalMessage.dart';
import 'package:tiq/models/dtos/PacHeadDto.dart';
import 'package:tiq/models/dtos/ScanResult.dart';
import 'package:tiq/services/BarcodeScannerService.dart';
import 'package:tiq/common/Consts.dart';
import 'package:flutter/material.dart';
import 'package:tiq/common/Messages.dart';
import 'package:tiq/services/GStateProvider.dart';
import 'package:tiq/widgets/purchase/PacHeadCard.dart';

class PacScanPage extends StatefulWidget {
  @override
  _PacScanPageState createState() => _PacScanPageState();
}

class _PacScanPageState extends State<PacScanPage> {
  BarcodeScannerService _barcodeScannerService;
  StreamSubscription<ScanResult> _onBarcodeScannedSubscription;
  final List<PacHeadDto> _entries = <PacHeadDto>[];

  _PacScanPageState() {
    this._barcodeScannerService = BarcodeScannerService();
    this._onBarcodeScannedSubscription = this._barcodeScannerService.onBarcodeScanned.listen(onScan);
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(
            title: Text('Закупки'),
        ),
        body: Container(
            margin: const EdgeInsets.all(Consts.marginDefault),
            child: Column(
              children: <Widget>[
                Text(
                  'Сосканируйте закупки',
                  textAlign: TextAlign.left,
                  style: TextStyle(fontSize: 20.0),
                ),
                Expanded(
                  child: Scrollbar(
                    child: ListView.separated(
                      padding: const EdgeInsets.all(8),
                      itemCount: _entries.length,
                      itemBuilder: (BuildContext context, int index) => PacHeadCard(_entries[index]),
                      separatorBuilder: (BuildContext context, int index) => const Divider(),
                    ),
                  )
                ),
                Column(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  crossAxisAlignment: CrossAxisAlignment.stretch,
                  children: <Widget>[
                    RaisedButton(
                      color: Colors.blue,
                      textColor: Colors.white,
                      onPressed: this.onPurchaseProcessingStart,
                      child: Text('Начать приемку'),
                    ),
                    RaisedButton(
                      onPressed: this.onCancel,
                      child: Text('Отмена'),
                    ),
                  ],
                )
              ],
            )
        )
    );
  }

  Future<void> onScan(ScanResult data) async {
    if (data.isSuccessful) {
      final barcode = data.barcode1.trim();
      if (barcode != '') {
        final pacHead = await PacHeadReadByBarcode().run(context, barcode, GStateProvider.instance.settingsFacility.facilityId);
        if (pacHead != null) {
          final isAlreadyInList =
            this._entries.any(
              (entry) => entry.pacId == pacHead.pacId
            );
          if (!isAlreadyInList) {
            final wasConfirmed = await PacHeadConfirmSelection().run(context, pacHead);
            if (wasConfirmed) {
              this.setState(() {
                this._entries.add(pacHead);
              });
            }
          } else {
            await ShowModalMessage().run(context, Messages.TitleError, "Закупка уже добавлена в список!");
          }
        }
      } else {
        await ShowModalMessage().run(context, Messages.TitleError, "Штрих-код закупки пуст!");
      }
    }
  }

  void onPurchaseProcessingStart() {
    Navigator.of(context).pop(_entries);
  }

  void onCancel() {
    Navigator.of(context).pop(List<PacHeadDto>());
  }

  @override
  void dispose() {
    this._onBarcodeScannedSubscription.cancel();

    super.dispose();
  }
}
