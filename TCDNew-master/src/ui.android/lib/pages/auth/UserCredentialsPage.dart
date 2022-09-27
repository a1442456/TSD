import 'package:tiq/common/Consts.dart';
import 'package:tiq/widgets/auth/AuthForm.dart';
import 'package:flutter/material.dart';

class UserCredentialsPage extends StatefulWidget {
  @override
  _UserCredentialsPageState createState() => _UserCredentialsPageState();
}

class _UserCredentialsPageState extends State<UserCredentialsPage> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Авторизация'),
      ),
      body: Container(
        margin: const EdgeInsets.all(Consts.marginDefault),
        child: AuthForm()
      )
    );
  }
}