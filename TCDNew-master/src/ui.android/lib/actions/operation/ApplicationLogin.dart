import 'package:tiq/actions/data/auth/UserLogin.dart';
import 'package:tiq/actions/ui/auth/AskForUserCredentials.dart';
import 'package:flutter/material.dart';
import 'package:tiq/models/state/StateAuth.dart';
import 'package:tiq/services/GStateProvider.dart';

class ApplicationLogin {
  Future<bool> run(BuildContext context) async {
    final credentials = await AskForUserCredentials().run(context);
    if (credentials != null) {
      final userToken = await UserLogin().run(context, credentials);
      GStateProvider.instance.setStateAuth(
        StateAuth(userToken)
      );
    }

    return GStateProvider.instance.stateAuth.isAuthorised;
  }
}
