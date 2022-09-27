import 'dart:async';

import 'package:flutter/material.dart';
import 'package:tiq/actions/data/purchase/PurchaseTaskFinish.dart';
import 'package:tiq/actions/data/purchase/PurchaseTaskLineReadByBarcode.dart';
import 'package:tiq/actions/data/purchase/PurchaseTaskLineUpdatePost.dart';
import 'package:tiq/actions/data/purchase/PurchaseTaskUpdateRead.dart';

import 'package:tiq/actions/ui/purchase/BarcodeInput.dart';
import 'package:tiq/actions/ui/purchase/BarcodePalletScan.dart';
import 'package:tiq/actions/ui/purchase/PurchaseTaskLineEdit.dart';
import 'package:tiq/actions/ui/purchase/PurchaseTaskPause.dart';
import 'package:tiq/actions/ui/utility/ShowModalDialog.dart';
import 'package:tiq/actions/ui/utility/ShowModalMessage.dart';
import 'package:tiq/models/dtos/PurchaseTaskLineDto.dart';
import 'package:tiq/models/dtos/ScanResult.dart';
import 'package:tiq/models/enums/AcceptanceProcessType.dart';
import 'package:tiq/models/enums/PurchaseTaskPauseAction.dart';
import 'package:tiq/services/PurchaseTaskStateView.dart';
import 'package:tiq/services/BarcodeScannerService.dart';
import 'package:tiq/services/GStateProvider.dart';
import 'package:tiq/common/Consts.dart';
import 'package:tiq/common/Messages.dart';
import 'package:tiq/widgets/purchase/PurchaseTaskLineCard.dart';

class PurchaseTaskContentScanPage extends StatefulWidget {
  final String purchaseTaskId;

  PurchaseTaskContentScanPage(this.purchaseTaskId, {Key key})
    : super(key: key);

  @override
  _PurchaseTaskContentScanPageState createState() => _PurchaseTaskContentScanPageState(
    purchaseTaskId
  );
}

class _PurchaseTaskContentScanPageState extends State<PurchaseTaskContentScanPage> {
  BarcodeScannerService _barcodeScannerService;
  StreamSubscription<ScanResult> _onBarcodeScannedSubscription;

  PurchaseTaskStateView _taskStateView;
  List<PurchaseTaskLineDto> purchaseTaskLines = List<PurchaseTaskLineDto>();
  String _statsPalletPanelText = ':';
  String _statsDiffPanelText = '0/0/0';
  bool _uiLocked = false;
  bool _isPalletCodeInitialised = false;

  _PurchaseTaskContentScanPageState(String purchaseTaskId) {
    this._barcodeScannerService = BarcodeScannerService();
    this._onBarcodeScannedSubscription = this._barcodeScannerService.onBarcodeScanned.listen(onScan);

    _taskStateView = PurchaseTaskStateView(purchaseTaskId);
  }

  @override
  void initState() {
    super.initState();

    _initPage();
  }

  Future<void> _initPage() async {
    var initialized = await _fillSessionLines();
    if (!initialized) {
      Navigator.of(context).pop();
      return;
    }
    initialized &= await _initCurrentPalletCode();
    if (!initialized) {
      Navigator.of(context).pop();
    }
  }

  Future<bool> _fillSessionLines() async {
    var result = false;
    Exception fillSessionLinesException;

    try
    {
      final purchaseTaskUpdate = await PurchaseTaskUpdateRead().run(context, _taskStateView.purchaseTaskId, 0);
      _taskStateView.purchaseTaskUpdateApply(purchaseTaskUpdate);

      final purchaseTaskLines = await _taskStateView.getLinesDataSource();
      setState(() {
        this.purchaseTaskLines = purchaseTaskLines;
      });

      result = true;
    }
    catch (exception)
    {
      fillSessionLinesException = exception;
    }

    if (fillSessionLinesException != null)
      await ShowModalMessage().run(context, Messages.TitleError, "Проблема при загрузке информации о закупке!");

    return result;
  }

  Future<bool> _initCurrentPalletCode() async {
    var isPalletCodeInitialised = !isAcceptanceProcessTypeIsPalletized();
    if (!isPalletCodeInitialised) {
      isPalletCodeInitialised = await onBarcodePalletEnter();
    }
    if (isPalletCodeInitialised) {
      setState(() { this._isPalletCodeInitialised = isPalletCodeInitialised; } );
    }

    return isPalletCodeInitialised;
  }

  bool isAcceptanceProcessTypeIsPalletized() {
    return GStateProvider.instance.settingsFacility.acceptanceProcessType == AcceptanceProcessType.palletized;
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Приемка товара'),
      ),
      body: !this._isPalletCodeInitialised
          ? buildWaiting(context)
          : buildWorking(context)
    );
  }

  Widget buildWaiting(BuildContext context) {
    return
      Container(
        margin: const EdgeInsets.all(Consts.marginDefault),
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            LinearProgressIndicator(value: null)
          ],
        ),
      );
  }

  Widget buildWorking(BuildContext context) {
    return
      Container(
          margin: const EdgeInsets.all(Consts.marginDefault),
          child: Column(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: <Widget>[
              this._uiLocked ? LinearProgressIndicator(value: null) : null,
              Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: <Widget>[
                  Expanded(
                      flex: 2,
                      child: Container(
                        child: RaisedButton(
                          color: Colors.blue,
                          textColor: Colors.white,
                          child: Text('Ш'),
                          onPressed: _uiLocked ? null : onBarcodeItemEnter,
                        ),
                      )
                  ),
                  this.isAcceptanceProcessTypeIsPalletized()
                      ? Expanded(
                      flex: 3,
                      child: Container(
                        padding: EdgeInsets.only(left: Consts.marginDefault),
                        child: RaisedButton(
                          color: Colors.purple,
                          textColor: Colors.white,
                          child: Text('П'),
                          onPressed: _uiLocked ? null : onBarcodePalletEnter,
                        ),
                      )
                  )
                      : null,
                  Expanded(
                      flex: 6,
                      child: Container(
                        padding: EdgeInsets.only(left: Consts.marginDefault),
                        child: RaisedButton(
                          color: Colors.orange,
                          textColor: Colors.white,
                          child: Text('Пауза'),
                          onPressed: _uiLocked ? null : onPause,
                        ),
                      )
                  ),
                  Expanded(
                      flex: 3,
                      child: Container(
                        padding: EdgeInsets.only(left: Consts.marginDefault),
                        child: RaisedButton(
                          color: Colors.green,
                          textColor: Colors.white,
                          child: Text('R'),
                          onPressed: _uiLocked ? null : onRefresh,
                        ),
                      )
                  ),
                ].where((e) => e != null).toList(),
              ),
              Expanded(
                child: Container(
                    margin: EdgeInsets.symmetric(vertical: Consts.marginDefault),
                    child: Scrollbar(
                      child: ListView.separated(
                        itemCount: purchaseTaskLines.length,
                        itemBuilder: (BuildContext context, int index) =>
                            PurchaseTaskLineCard(purchaseTaskLines[index]),
                        separatorBuilder: (BuildContext context, int index) => const Divider(),
                      ),
                    )
                ),
              ),
              Row(
                mainAxisAlignment: MainAxisAlignment.center,
                children: <Widget>[
                  this.isAcceptanceProcessTypeIsPalletized()
                      ? Expanded(
                    flex: 1,
                    child: Text(_statsPalletPanelText),
                  )
                      : null,
                  Expanded(
                    flex: 1,
                    child: Text(_statsDiffPanelText),
                  ),
                ].where((e) => e != null).toList(),
              ),
            ].where((e) => e != null).toList(),
          )
      );
  }

  Future<void> onScan(ScanResult data) async {
    bool wasExceptionRaised = false;

    try {
      await this._onBarcodeScannedSubscription.cancel();
      this._onBarcodeScannedSubscription = null;
      this.setState(() => this._uiLocked = true);

      if (data.isSuccessful) {
        if (data.barcode1 != null) {
          final barcode = data.barcode1.trim();
          if (barcode != '') {
            await _processProductBarcode(barcode);
            await this._updateStatsPanel();
          } else {
            await ShowModalMessage().run(context, Messages.TitleError, "Штрих-код пуст!");
          }
        } else {
          await ShowModalMessage().run(context, Messages.TitleError, "Штрих-код пуст!");
        }
      }
    }
    catch(ex) {
      wasExceptionRaised = true;
    }
    finally {
      this.setState(() => this._uiLocked = false);
      if (this._onBarcodeScannedSubscription == null) {
        this._onBarcodeScannedSubscription = this._barcodeScannerService.onBarcodeScanned.listen(onScan);
      }
    }
    if (wasExceptionRaised) {
      await ShowModalMessage().run(context, Messages.TitleError, "Произошла ошибка выполнении операции!");
      await this._updateStatsPanel();
    }
  }

  Future<void> onBarcodeItemEnter() async {
    bool wasExceptionRaised = false;

    try {
      await this._onBarcodeScannedSubscription.cancel();
      this._onBarcodeScannedSubscription = null;
      this.setState(() => this._uiLocked = true);

      var barcode = await BarcodeInput().run(context);
      if (barcode != null ) {
        barcode = barcode.trim();
        if (barcode != '') {
          await _processProductBarcode(barcode);
          await this._updateStatsPanel();
        } else {
          await ShowModalMessage().run(context, Messages.TitleError, "Штрих-код товара пуст!");
        }
      } else {
        await ShowModalMessage().run(context, Messages.TitleError, "Штрих-код товара пуст!");
      }
    }
    catch(ex) {
      wasExceptionRaised = true;
    }
    finally {
      this.setState(() => this._uiLocked = false);
      if (this._onBarcodeScannedSubscription == null) {
        this._onBarcodeScannedSubscription = this._barcodeScannerService.onBarcodeScanned.listen(onScan);
      }
    }
    if (wasExceptionRaised) {
      await ShowModalMessage().run(context, Messages.TitleError, "Произошла ошибка выполнении операции!");
      await this._updateStatsPanel();
    }
  }

  Future<bool> onBarcodePalletEnter() async {
    var result = false;
    bool wasExceptionRaised = false;

    try {
      await this._onBarcodeScannedSubscription.cancel();
      this._onBarcodeScannedSubscription = null;
      this.setState(() => this._uiLocked = true);

      var barcode = await BarcodePalletScan().run(context);
      if (barcode != null ) {
        barcode = barcode.trim();
        if (barcode != '') {
          _taskStateView.setCurrentPalletCode(barcode);
          await this._updateStatsPanel();
          result = true;
        } else {
          await ShowModalMessage().run(context, Messages.TitleError, "Штрих-код паллеты пуст!");
        }
      } else {
        await ShowModalMessage().run(context, Messages.TitleError, "Штрих-код паллеты пуст!");
      }
    }
    catch(ex) {
      wasExceptionRaised = true;
    }
    finally {
      this.setState(() => this._uiLocked = false);
      if (this._onBarcodeScannedSubscription == null) {
        this._onBarcodeScannedSubscription = this._barcodeScannerService.onBarcodeScanned.listen(onScan);
      }
    }
    if (wasExceptionRaised) {
      await ShowModalMessage().run(context, Messages.TitleError, "Произошла ошибка выполнении операции!");
      await this._updateStatsPanel();
    }

    return result;
  }

  Future<void> onPause() async {
    bool wasExceptionRaised = false;

    try {
      await this._onBarcodeScannedSubscription.cancel();
      this._onBarcodeScannedSubscription = null;
      this.setState(() => this._uiLocked = true);

      final purchasePauseAction = await PurchaseTaskPause().run(context);

      switch (purchasePauseAction) {
        case PurchaseTaskPauseAction.back:
          // продолжаем
          break;

        case PurchaseTaskPauseAction.leave:
          // выходим
          Navigator.of(context).pop();
          break;

        case PurchaseTaskPauseAction.cancel:
          // останавливаем приемку и возвращаем
          final declineResult = await PurchaseTaskFinish().run(context, _taskStateView.purchaseTaskId, true, false);
          if (declineResult)
          {
            await ShowModalMessage().run(context, Messages.TitleMessage, "Закупка успешно отменена!");
            Navigator.of(context).pop();
          }
          else {
            await ShowModalMessage().run(context, Messages.TitleError, "Не удалось отменить закупку! Обязательно повторите попытку еще раз!!!");
            await this._updateStatsPanel();
          }
          break;

        // завершаем задание
        case PurchaseTaskPauseAction.finish:
          final diffCount = _taskStateView.getDifferencesCount();
          final confirmed = await ShowModalDialog().run(context, 'Расхождения', 'Количество расхождений: $diffCount');
          if (confirmed)
          {

            final acceptResult = await PurchaseTaskFinish().run(context, _taskStateView.purchaseTaskId, false, false);
            if (acceptResult)
            {
              // закончили приемку, все ок
              await ShowModalMessage().run(context, Messages.TitleMessage, "Задание успешно завершено!");
              Navigator.of(context).pop();
            }
            else {
              await ShowModalMessage().run(context, Messages.TitleError, "Не удалось завершить задание! Обязательно повторите попытку еще раз!!!");
              await this._updateStatsPanel();
            }
          }
          break;

        // завершаем задание и отправляем во внешнюю систему
        case PurchaseTaskPauseAction.finishAndUpload:
          final diffCount = _taskStateView.getDifferencesCount();
          final confirmed = await ShowModalDialog().run(context, 'Расхождения', 'Количество расхождений: $diffCount');
          if (confirmed)
          {

            final acceptResult = await PurchaseTaskFinish().run(context, _taskStateView.purchaseTaskId, false, true);
            if (acceptResult)
            {
              // закончили приемку, все ок
              await ShowModalMessage().run(context, Messages.TitleMessage, "Задание успешно завершено и отправлено!");
              Navigator.of(context).pop();
            }
            else {
              await ShowModalMessage().run(context, Messages.TitleError, "Не удалось завершить и отправить задание! Обязательно повторите попытку еще раз!!!");
              await this._updateStatsPanel();
            }
          }
          break;
      }
    }
    catch(ex) {
      wasExceptionRaised = true;
    }
    finally {
      this.setState(() => this._uiLocked = false);
      if (this._onBarcodeScannedSubscription == null) {
        this._onBarcodeScannedSubscription = this._barcodeScannerService.onBarcodeScanned.listen(onScan);
      }
    }
    if (wasExceptionRaised) {
      await ShowModalMessage().run(context, Messages.TitleError, "Произошла ошибка выполнении операции!");
      await this._updateStatsPanel();
    }
  }

  Future<void> onRefresh() async {
    var purchaseTaskUpdate = await PurchaseTaskUpdateRead().run(context, _taskStateView.purchaseTaskId, _taskStateView.purchaseTaskVersion);
    _taskStateView.purchaseTaskUpdateApply(purchaseTaskUpdate);

    await this._updateStatsPanel();
  }

  Future<void> _processProductBarcode(String barcode) async {
    var purchaseTaskUpdate = await PurchaseTaskUpdateRead().run(context, _taskStateView.purchaseTaskId, _taskStateView.purchaseTaskVersion);
    _taskStateView.purchaseTaskUpdateApply(purchaseTaskUpdate);

    final currentPalletCode = _taskStateView.getCurrentPalletCode();
    final currentPalletAbc = _taskStateView.getCurrentPalletABC();
    final purchaseTaskLine = await PurchaseTaskLineReadByBarcode().run(
        context, _taskStateView.purchaseTaskId, barcode, currentPalletCode);
    if (purchaseTaskLine == null) {
      await ShowModalMessage().run(
        context,
        Messages.TitleError,
        "Неверный штрих-код товара!"
      );
      return;
    }
    if (isAcceptanceProcessTypeIsPalletized()) {
      if (!_taskStateView.canPalletAcceptProductAbc(currentPalletAbc, purchaseTaskLine.product.abc))
      {
        await ShowModalMessage().run(
            context,
            Messages.TitleError,
            "Нельзя положить на паллету с товаром категории ${currentPalletAbc.toUpperCase()} товар категории ${purchaseTaskLine.product.abc.toUpperCase()}!"
        );
        return;
      }
      if (_taskStateView.isPalletABCEmpty(currentPalletAbc) && _taskStateView.isPalletABCUsed(purchaseTaskLine.product.abc)) {
        final useNewPalletConfirmed = await ShowModalDialog().run(
            context,
            "Категория товара паллеты",
            "Паллета под товары категории ${purchaseTaskLine.product.abc.toUpperCase()} уже используется. Уверены, что хотите использовать еще одну паллету?"
        );
        if (!useNewPalletConfirmed) {
          return;
        }
      }
    }

    final purchaseTaskLineUpdate = await PurchaseTaskLineEdit().run(context, purchaseTaskLine);
    final wasUpdateSuccessful =
      await PurchaseTaskLineUpdatePost().run(context, _taskStateView.purchaseTaskId, barcode, currentPalletCode, purchaseTaskLineUpdate);
    if (!wasUpdateSuccessful) {
      await ShowModalMessage().run(
          context,
          Messages.TitleError,
          "Данные не сохранены на сервере! Попробуйте еще раз!"
      );
    }

    purchaseTaskUpdate = await PurchaseTaskUpdateRead().run(context, _taskStateView.purchaseTaskId, _taskStateView.purchaseTaskVersion);
    _taskStateView.purchaseTaskUpdateApply(purchaseTaskUpdate);

    await this._updateStatsPanel();
  }

  Future<void> _updateStatsPanel() async {
    final currentPalletABC = _taskStateView.getCurrentPalletABC();
    final currentPalletCode = _taskStateView.getCurrentPalletCode();

    final acceptedCount = _taskStateView.getAcceptedCount();
    final differencesCount = _taskStateView.getDifferencesCount();
    final totalCount = _taskStateView.getTotalCount();

    final statsPalletPanelText = '${currentPalletABC.toUpperCase()}:$currentPalletCode';
    final statsDiffPanelText = '$acceptedCount/$differencesCount/$totalCount';
    setState(() {
      _statsPalletPanelText = statsPalletPanelText;
      _statsDiffPanelText = statsDiffPanelText;
    });
  }

  @override
  void dispose() {
    this._onBarcodeScannedSubscription.cancel();

    super.dispose();
  }
}