import 'package:tiq/models/dtos/UserCredentials.dart';
import 'package:flutter/material.dart';

class AuthForm extends StatefulWidget {
  @override
  _AuthFormState createState() => _AuthFormState();
}

class _AuthFormState extends State<AuthForm> {
  final _formKey = GlobalKey<FormState>();
  final _userNameEditingController = TextEditingController();
  final _passwordEditingController = TextEditingController();

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
                  labelText: 'Пользователь',
                ),
                controller: _userNameEditingController,
                validator: validateUserName,
              ),
              TextFormField(
                decoration: const InputDecoration(
                  labelText: 'Пароль',
                ),
                controller: _passwordEditingController,
                validator: validatePassword,
                obscureText: true,
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
                onPressed: this.onLogin,
                child: Text('Войти'),
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

  void onLogin() {
    Navigator.of(context).pop(UserCredentials(
      userName: _userNameEditingController.text,
      userPassword: _passwordEditingController.text
    ));
  }

  void onCancel() {
    Navigator.of(context).pop(null);
  }

  String validateUserName(String value) {
    if (value.isEmpty) {
      return 'Введите пользователя';
    }
    return null;
  }

  String validatePassword(String value) {
    if (value.isEmpty) {
      return 'Введите пароль';
    }
    return null;
  }

  @override
  void dispose() {
    _userNameEditingController.dispose();
    _passwordEditingController.dispose();

    super.dispose();
  }
}
