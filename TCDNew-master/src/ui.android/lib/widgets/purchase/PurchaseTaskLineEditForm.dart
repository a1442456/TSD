import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:intl/intl.dart';
import 'package:tiq/actions/ui/utility/ShowModalMessage.dart';
import 'package:tiq/common/Consts.dart';
import 'package:tiq/common/Messages.dart';
import 'package:tiq/models/dtos/PurchaseTaskLineDto.dart';
import 'package:tiq/models/dtos/PurchaseTaskLineUpdateDto.dart';
import 'package:tiq/models/enums/PurchaseTaskLineUpdateType.dart';
import 'package:tiq/widgets/shared/DateInputFormatter.dart';

class PurchaseTaskLineEditForm extends StatefulWidget {
  final PurchaseTaskLineDto _purchaseTaskLine;

  PurchaseTaskLineEditForm(this._purchaseTaskLine);

  @override
  _PurchaseTaskLineEditFormState createState() => _PurchaseTaskLineEditFormState(_purchaseTaskLine);
}

class _PurchaseTaskLineEditFormState extends State<PurchaseTaskLineEditForm> {
  PurchaseTaskLineDto _purchaseTaskLine;
  PurchaseTaskLineUpdateDto _result;

  DateTime _dateDefault;
  DateTime _dateToday;
  final _dateFormat = DateFormat('dd.MM.yyyy');
  final _dateEmptyText = '  .  .    ';

  final _nullFocusNode = FocusNode();
  bool _expirationDateEnabled = true;
  final _expirationDateEditingController = TextEditingController();
  final _expirationDateDaysPlusEditingController = TextEditingController();
  final _expirationDateFocusNode = FocusNode();
  final _expirationDateDaysPlusFocusNode = FocusNode();

  final _qtyNormalBeforeEditingController = TextEditingController();
  final _qtyNormalAddEditingController = TextEditingController();
  final _qtyNormalTotalEditingController = TextEditingController();
  final _qtyNormalAddFocusNode = FocusNode();

  final _qtyBrokenBeforeEditingController = TextEditingController();
  final _qtyBrokenAddEditingController = TextEditingController();
  final _qtyBrokenTotalEditingController = TextEditingController();
  final _qtyBrokenAddFocusNode = FocusNode();

  _PurchaseTaskLineEditFormState(PurchaseTaskLineDto purchaseTaskLine) {
    this._purchaseTaskLine = purchaseTaskLine;
    this._result = PurchaseTaskLineUpdateDto.withPurchaseTaskLineState(
        purchaseTaskLine.id,
        purchaseTaskLine.state
    );

    final _dateNow = DateTime.now();
    _dateToday = DateTime.utc(_dateNow.year, _dateNow.month, _dateNow.day);
    _dateDefault = DateTime.utc(_dateNow.year, _dateNow.month > 1 ? _dateNow.month - 1 : 1, 01);
  }

  @override
  void initState() {
    _expirationDateEditingController.addListener(_updateModelExpirationDate);
    _expirationDateDaysPlusEditingController.addListener(_updateModelExpirationDateDaysPlus);

    _qtyNormalAddEditingController.addListener(_updateModelQtyNormal);
    _qtyBrokenAddEditingController.addListener(_updateModelQtyBroken);

    this._result.purchaseTaskLineState.qtyNormal = 1;
    if (this._result.purchaseTaskLineState.expirationDate == null) {
      if (this._purchaseTaskLine.state.qtyFull == 0) {
        this._result.purchaseTaskLineState.expirationDate = _dateDefault;
      }
    }

    _uiUpdate();

    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    double nameContainerWidth = MediaQuery.of(context).size.width - 12.0;

    final abcCategoryColor =
      _purchaseTaskLine.product.abc == "b"
        ? Colors.cyanAccent
        : _purchaseTaskLine.product.abc == "c"
          ? Colors.purpleAccent
          : Colors.transparent;

    return Column(
      children: <Widget>[
        Expanded(child: SingleChildScrollView (
          child: Column (
            mainAxisAlignment: MainAxisAlignment.start,
            children: <Widget>[
              Container(
                margin: EdgeInsets.only(bottom: 4.0),
                width: nameContainerWidth,
                color: abcCategoryColor,
                child: Text(
                  _purchaseTaskLine.product.name,
                  textAlign: TextAlign.left,
                  style: TextStyle(
                    fontSize: 18.0,
                    fontWeight: FontWeight.bold
                  ),
                ),
              ),
              Row(
                mainAxisAlignment: MainAxisAlignment.start,
                children: <Widget>[
                  Checkbox(
                    value: _expirationDateEnabled,
                    onChanged: _updateModelExpirationDateEnabled
                  ),
                  Text("Срок годности"),
                ],
              ),
              _expirationDateEnabled ?
              Row(
                children: <Widget>[
                  Expanded(
                    child: TextFormField(
                      decoration: const InputDecoration(
                        labelText: 'срок годности',
                      ),
                      controller: _expirationDateEditingController,
                      keyboardType: TextInputType.numberWithOptions(decimal: true, signed: false),
                      inputFormatters: <TextInputFormatter>[
                        DateInputFormatter()
                      ],
                      focusNode: _expirationDateFocusNode,
                      onTap: () => _tapHandler(_expirationDateEditingController, _expirationDateFocusNode, false),
                    ),
                  ),
                  Expanded(
                    child: TextFormField(
                      decoration: const InputDecoration(
                        labelText: '+ дней',
                      ),
                      controller: _expirationDateDaysPlusEditingController,
                      keyboardType: TextInputType.numberWithOptions(),
                      inputFormatters: <TextInputFormatter>[
                        FilteringTextInputFormatter.digitsOnly
                      ],
                      focusNode: _expirationDateDaysPlusFocusNode,
                      onTap: () => _tapHandler(_expirationDateDaysPlusEditingController, _expirationDateDaysPlusFocusNode, true),
                    ),
                  ),
                ],
              ) : null,
              Column(
                children: <Widget>[
                  Row(children: <Widget>[
                    Expanded(
                      flex: 1,
                      child: TextFormField(
                        readOnly: true,
                        decoration: const InputDecoration(
                          labelText: 'ранее',
                        ),
                        controller: _qtyNormalBeforeEditingController,
                        onTap: () => _tapHandler(_qtyNormalAddEditingController, _qtyNormalAddFocusNode, true),
                      ),
                    ),
                    Expanded(
                      flex: 1,
                      child: TextFormField(
                        decoration: const InputDecoration(
                          labelText: 'сейчас',
                          filled: true,
                          fillColor: Colors.greenAccent,
                        ),
                        controller: _qtyNormalAddEditingController,
                        keyboardType: TextInputType.numberWithOptions(decimal: true),
                        inputFormatters: <TextInputFormatter>[
                          FilteringTextInputFormatter.allow(RegExp(r'[0-9]+([.][0-9]?)?'))
                        ],
                        focusNode: _qtyNormalAddFocusNode,
                        onTap: () => _tapHandler(_qtyNormalAddEditingController, _qtyNormalAddFocusNode, true),
                      ),
                    ),
                    Expanded(
                      flex: 1,
                      child: TextFormField(
                        readOnly: true,
                        decoration: const InputDecoration(
                          labelText: 'всего',
                        ),
                        controller: _qtyNormalTotalEditingController,
                        onTap: () => _tapHandler(_qtyNormalAddEditingController, _qtyNormalAddFocusNode, true),
                      ),
                    )
                  ]),
                  GestureDetector(
                    onTap: () => _tapHandler(_qtyNormalAddEditingController, _qtyNormalAddFocusNode, true),
                    child: Row(
                      children: <Widget>[
                        Expanded(
                          child: Container(
                            color: Colors.greenAccent,
                            child: Text('Принято', style: TextStyle(color: Colors.black)),
                            padding: EdgeInsets.all(Consts.paddingDefault),
                            margin: EdgeInsets.only(bottom: Consts.marginPurchaseListItem),
                          )
                        )
                      ],
                    ),
                  ),
                ],
              ),
              Column(
                children: <Widget>[
                  Row(children: <Widget>[
                    Expanded(
                      child: TextFormField(
                        readOnly: true,
                        decoration: const InputDecoration(
                          labelText: 'ранее',
                        ),
                        controller: _qtyBrokenBeforeEditingController,
                        onTap: () => _tapHandler(_qtyBrokenAddEditingController, _qtyBrokenAddFocusNode, true),
                      ),
                    ),
                    Expanded(
                      child: TextFormField(
                        decoration: const InputDecoration(
                            labelText: 'сейчас',
                            filled: true,
                            fillColor: Colors.redAccent
                        ),
                        controller: _qtyBrokenAddEditingController,
                        keyboardType: TextInputType.numberWithOptions(decimal: true),
                        inputFormatters: <TextInputFormatter>[
                          FilteringTextInputFormatter.allow(RegExp(r'[0-9]+([.][0-9]?)?'))
                        ],
                        focusNode: _qtyBrokenAddFocusNode,
                        onTap: () => _tapHandler(_qtyBrokenAddEditingController, _qtyBrokenAddFocusNode, true),
                      ),
                    ),
                    Expanded(
                      child: TextFormField(
                        readOnly: true,
                        decoration: const InputDecoration(
                          labelText: 'всего',
                        ),
                        controller: _qtyBrokenTotalEditingController,
                        onTap: () => _tapHandler(_qtyBrokenAddEditingController, _qtyBrokenAddFocusNode, true),
                      ),
                    )
                  ]),
                  GestureDetector(
                    onTap: () => _tapHandler(_qtyBrokenAddEditingController, _qtyBrokenAddFocusNode, true),
                    child: Row(
                      children: <Widget>[
                        Expanded(
                          child: Container(
                            color: Colors.redAccent,
                            child: Text('Брак', style: TextStyle(color: Colors.white)),
                            padding: EdgeInsets.all(Consts.paddingDefault),
                            margin: EdgeInsets.only(bottom: Consts.marginPurchaseListItem),
                          )
                        )
                      ],
                    )
                  ),
                ],
              ),
            ].where((e) => e != null).toList(),
          )
        )),

        Column(
          children: <Widget>[
            Column(
              children: <Widget>[
                Row(
                  children: <Widget>[
                    Expanded(
                      flex: 2,
                      child: Text(
                          'Итого:',
                          style: TextStyle(fontSize: 20.0)
                      ),
                    ),
                    Expanded(
                      flex: 1,
                      child: Text(
                          _formQtyTotal.toStringAsFixed(3),
                          style: TextStyle(fontSize: 20.0)
                      ),
                    ),
                  ],
                ),
                Row(
                  children: <Widget>[
                    Expanded(
                      flex: 2,
                      child: Text(
                          'Закупка:',
                          style: TextStyle(fontSize: 20.0)
                      ),
                    ),
                    Expanded(
                      flex: 1,
                      child: Text(
                          _purchaseTaskLine.quantity.toStringAsFixed(3),
                          style: TextStyle(fontSize: 20.0)
                      ),
                    ),
                  ],
                )
              ],
            ),
            Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: <Widget>[
                Expanded(
                  flex: 4,
                  child: RaisedButton(
                    color: Colors.red,
                    textColor: Colors.white,
                    child: Text('Пересчет'),
                    onPressed: _onRecalc,
                  ),
                ),
                Expanded(
                    flex: 8,
                    child: Container(
                      padding: EdgeInsets.only(left: Consts.marginDefault),
                      child: RaisedButton(
                        color: Colors.blue,
                        textColor: Colors.white,
                        child: Text('ОК'),
                        onPressed: _onConfirm,
                      ),
                    )
                )
              ],
            ),
          ],
        )
      ],
    );
  }

  DateTime get _formExpirationDate {
    if (_expirationDateEditingController.text == null) {
      return null;
    } else if (_expirationDateEditingController.text == _dateEmptyText) {
      return null;
    } else if (_expirationDateEditingController.text.trim() == '') {
      return null;
    } else {
      return _dateFormat.parse(_expirationDateEditingController.text, true);
    }
  }

  set _formExpirationDate(DateTime value) {
    final selection = _expirationDateEditingController.selection;
    final valueCopy = _expirationDateEditingController.value.copyWith(
        text: value != null ? _dateFormat.format(value) : _dateEmptyText,
        selection: selection
    );
    _expirationDateEditingController.value = valueCopy;
  }

  int get _formExpirationDateDaysPlus {
    var value = 0;
    if (_expirationDateDaysPlusEditingController.text != null && _expirationDateDaysPlusEditingController.text.trim() != '') {
      value = int.tryParse(_expirationDateDaysPlusEditingController.text);
    }

    return value;
  }

  set _formExpirationDateDaysPlus(int value) {
    final selection = _expirationDateDaysPlusEditingController.selection;
    final valueCopy = _expirationDateDaysPlusEditingController.value.copyWith(
        text: value != null ? value != 0 ? value.toString() : '' : '',
        selection: selection
    );
    _expirationDateDaysPlusEditingController.value = valueCopy;
  }

  num get _formQtyNormalBefore {
    num value = 0;
    if (_qtyNormalBeforeEditingController.text != null && _qtyNormalBeforeEditingController.text.trim() != '') {
      value = num.tryParse(_qtyNormalBeforeEditingController.text);
    }

    return value;
  }

  set _formQtyNormalBefore(num value) {
    _qtyNormalBeforeEditingController.text = value != null ? value.toStringAsFixed(3) : '0.000';
  }

  num get _formQtyNormalAdd {
    num value = 0;
    if (_qtyNormalAddEditingController.text != null && _qtyNormalAddEditingController.text.trim() != '') {
      value = num.tryParse(_qtyNormalAddEditingController.text);
    }

    return value;
  }

  set _formQtyNormalAdd(num value) {
    final selection = _qtyNormalAddEditingController.selection;
    final valueCopy = _qtyNormalAddEditingController.value.copyWith(
        text: value != null ? value.toStringAsFixed(3) : '',
        selection: selection
    );
    _qtyNormalAddEditingController.value = valueCopy;
  }

  set _formQtyNormalTotal(num value) {
    _qtyNormalTotalEditingController.text = value != null ? value.toStringAsFixed(3) : '0.000';
  }

  num get _formQtyBrokenBefore {
    num value = 0;
    if (_qtyBrokenBeforeEditingController.text != null && _qtyBrokenBeforeEditingController.text.trim() != '') {
      value = num.tryParse(_qtyBrokenBeforeEditingController.text);
    }

    return value;
  }

  set _formQtyBrokenBefore(num value) {
    _qtyBrokenBeforeEditingController.text = value != null ? value.toStringAsFixed(3) : '0.000';
  }

  num get _formQtyBrokenAdd {
    num value = 0;
    if (_qtyBrokenAddEditingController.text != null && _qtyBrokenAddEditingController.text.trim() != '') {
      value = num.tryParse(_qtyBrokenAddEditingController.text);
    }

    return value;
  }

  set _formQtyBrokenAdd(num value) {
    final selection = _qtyBrokenAddEditingController.selection;
    final valueCopy = _qtyBrokenAddEditingController.value.copyWith(
        text: value != null ? value.toStringAsFixed(3) : '',
        selection: selection
    );
    _qtyBrokenAddEditingController.value = valueCopy;
  }

  set _formQtyBrokenTotal(num value) {
    _qtyBrokenTotalEditingController.text = value != null ? value.toStringAsFixed(3) : '0.000';
  }

  num get _formQtyTotal {
    return
      _purchaseTaskLine.state.qtyNormal + _result.purchaseTaskLineState.qtyNormal +
      _purchaseTaskLine.state.qtyBroken + _result.purchaseTaskLineState.qtyBroken;
  }

  void _unfocusOnSecondTap(FocusNode focusNode) {
    if (focusNode.hasFocus) {
      FocusScope.of(context).requestFocus(_nullFocusNode);
    }
  }

  void _tapHandler(TextEditingController controller, FocusNode focusNode, bool selectContent) {
    _unfocusOnSecondTap(focusNode);

    if (!focusNode.hasFocus) {
      FocusScope.of(context).requestFocus(focusNode);
      controller.selection =
          selectContent
              ? TextSelection(baseOffset: 0, extentOffset: controller.text.length)
              : TextSelection.collapsed(offset: 0);
    }
  }

  void _uiUpdate() {
    setState(() {
      _expirationDateEnabled = _result.purchaseTaskLineState.expirationDate != null;

      _formExpirationDate = _result.purchaseTaskLineState.expirationDate;
      _formExpirationDateDaysPlus = _result.purchaseTaskLineState.expirationDaysPlus;

      _formQtyNormalBefore = _purchaseTaskLine.state.qtyNormal;
      if (_formQtyNormalAdd != _result.purchaseTaskLineState.qtyNormal) {
        _formQtyNormalAdd = _result.purchaseTaskLineState.qtyNormal;
      }

      _formQtyBrokenBefore = _purchaseTaskLine.state.qtyBroken;
      if (_formQtyBrokenAdd != _result.purchaseTaskLineState.qtyBroken) {
        _formQtyBrokenAdd = _result.purchaseTaskLineState.qtyBroken;
      }

      _formQtyNormalTotal = (_purchaseTaskLine.state.qtyNormal + _result.purchaseTaskLineState.qtyNormal);
      _formQtyBrokenTotal = (_purchaseTaskLine.state.qtyBroken + _result.purchaseTaskLineState.qtyBroken);
    });
  }

  void _updateModelExpirationDateEnabled(bool value) {
    if (value) {
      _result.purchaseTaskLineState.expirationDate =  _dateDefault;
      _result.purchaseTaskLineState.expirationDaysPlus = 0;
    } else {
      _result.purchaseTaskLineState.expirationDate = null;
      _result.purchaseTaskLineState.expirationDaysPlus = 0;
    }
    _uiUpdate();
  }

  Future<void> _updateModelExpirationDate() async {
    bool wasExceptionRaised = false;

    try {
      _result.purchaseTaskLineState.expirationDate = _formExpirationDate;
    }
    catch (ex) {
      wasExceptionRaised = true;
    }

    if (wasExceptionRaised)
      await ShowModalMessage().run(context, Messages.TitleError, "Неправильный ввод даты!");

    _uiUpdate();
  }

  Future<void> _updateModelExpirationDateDaysPlus() async {
    bool wasExceptionRaised = false;

    try {
      // TODO: добавить проверку на Settings.MaxExpirationDaysPlus
      _result.purchaseTaskLineState.expirationDaysPlus = _formExpirationDateDaysPlus;
    }
    catch (ex) {
      wasExceptionRaised = true;
    }

    if (wasExceptionRaised)
      await ShowModalMessage().run(context, Messages.TitleError, "Неправильный ввод количества дней!");

    _uiUpdate();
  }

  Future<void> _updateModelQtyNormal() async {
    bool wasExceptionRaised = false;

    try {
      var qtyNormalAdd = _formQtyNormalAdd;

      if (_purchaseTaskLine.state.qtyNormal + _formQtyNormalAdd + _purchaseTaskLine.state.qtyBroken + _formQtyBrokenAdd > _purchaseTaskLine.quantity)
      {
        await ShowModalMessage().run(context, Messages.TitleError, "Вы не можете принять кол-во, большее чем указано в документе!");
        qtyNormalAdd = _purchaseTaskLine.quantity - (_purchaseTaskLine.state.qtyNormal + _purchaseTaskLine.state.qtyBroken + _formQtyBrokenAdd);
      }
      _result.purchaseTaskLineState.qtyNormal = qtyNormalAdd;
    }
    catch (ex) {
      wasExceptionRaised = true;
    }

    if (wasExceptionRaised)
      await ShowModalMessage().run(context, Messages.TitleError, "Неправильный ввод количества!");

    _uiUpdate();
  }

  Future<void> _updateModelQtyBroken() async {
    bool wasExceptionRaised = false;

    try {
      var qtyBrokenAdd = _formQtyBrokenAdd;

      if (_purchaseTaskLine.state.qtyNormal + _formQtyNormalAdd + _purchaseTaskLine.state.qtyBroken + _formQtyBrokenAdd > _purchaseTaskLine.quantity)
      {
        await ShowModalMessage().run(context, Messages.TitleError, "Вы не можете принять кол-во, большее чем указано в документе!");
        qtyBrokenAdd = _purchaseTaskLine.quantity - (_purchaseTaskLine.state.qtyNormal + _formQtyNormalAdd + _purchaseTaskLine.state.qtyBroken);
      }
      _result.purchaseTaskLineState.qtyBroken = qtyBrokenAdd;
    }
    catch (ex) {
      wasExceptionRaised = true;
    }

    if (wasExceptionRaised)
      await ShowModalMessage().run(context, Messages.TitleError, "Неправильный ввод количества!");

    _uiUpdate();
  }

  Future<bool> _validateModel() async {
    var result = true;

    if (_result.purchaseTaskLineState.expirationDate != null) {
      if (_result.purchaseTaskLineState.expirationDate.add(Duration(days: _result.purchaseTaskLineState.expirationDaysPlus)).compareTo(_dateToday) < 0) {
        await ShowModalMessage().run(
          context, Messages.TitleError,
          "Истекший срок годности!"
        );

        result = false;
      }
    }

    return result;
  }

  Future<void> _onConfirm() async {
    final isValid = await _validateModel();

    if (isValid) {
      _result.purchaseTaskLineUpdateType = PurchaseTaskLineUpdateType.update;
      Navigator.of(context).pop(_result);
    }
  }

  Future<void> _onRecalc() async {
    _result.purchaseTaskLineUpdateType = PurchaseTaskLineUpdateType.reset;
    Navigator.of(context).pop(_result);
  }

  @override
  void dispose() {
    _nullFocusNode.dispose();
    _expirationDateFocusNode.dispose();
    _expirationDateDaysPlusFocusNode.dispose();
    _qtyNormalAddFocusNode.dispose();
    _qtyBrokenAddFocusNode.dispose();

    _expirationDateDaysPlusEditingController.dispose();

    _qtyNormalBeforeEditingController.dispose();
    _qtyNormalAddEditingController.dispose();
    _qtyNormalTotalEditingController.dispose();

    _qtyBrokenBeforeEditingController.dispose();
    _qtyBrokenAddEditingController.dispose();
    _qtyBrokenTotalEditingController.dispose();

    super.dispose();
  }
}
