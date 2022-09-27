import 'package:flutter/material.dart';
import 'package:flutter/services.dart';

class BarcodeInputForm extends StatefulWidget {
  @override
  _BarcodeInputFormState createState() => _BarcodeInputFormState();
}

class _BarcodeInputFormState extends State<BarcodeInputForm> {
  final _formKey = GlobalKey<FormState>();
  final _barcodeEditingController = TextEditingController();

  @override
  Widget build(BuildContext context) {
    return Form(
      key: _formKey,
      child: Column(
        mainAxisAlignment: MainAxisAlignment.spaceBetween,
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: <Widget>[
          Column(
            children: <Widget>[
              TextFormField(
                autofocus: true,
                decoration: const InputDecoration(
                  labelText: 'Штрих-код',
                ),
                controller: _barcodeEditingController,
                validator: validateBarcode,
                keyboardType: TextInputType.number,
                inputFormatters: <TextInputFormatter>[
                  FilteringTextInputFormatter.digitsOnly
                ],
              ),
            ],
          ),
          Column(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            crossAxisAlignment: CrossAxisAlignment.stretch,
            children: <Widget>[
              RaisedButton(
                color: Colors.blue,
                textColor: Colors.white,
                onPressed: this.onFind,
                child: Text('Найти'),
              ),
              RaisedButton(
                onPressed: this.onCancel,
                child: Text('Отмена'),
              ),
            ],
          )
        ],
      ),
    );
  }

  String validateBarcode(String value) {
    if (value.isEmpty) {
      return 'Веедите штрих-код';
    }
    return null;
  }

  void onFind() {
    Navigator.of(context).pop(_barcodeEditingController.text);
  }

  void onCancel() {
    Navigator.of(context).pop(null);
  }

  @override
  void dispose() {
    _barcodeEditingController.dispose();

    super.dispose();
  }
}
