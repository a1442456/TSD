import 'package:tiq/models/dtos/UserCredentials.dart';
import 'package:tiq/router.dart';
import 'package:flutter/widgets.dart';

class AskForUserCredentials {
  Future<UserCredentials> run(BuildContext context) async {
    final formData = await Navigator.pushNamed(context, AuthRoute);
    return (formData as UserCredentials);
  }
}
