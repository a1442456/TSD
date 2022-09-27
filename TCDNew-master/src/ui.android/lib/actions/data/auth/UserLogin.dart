import 'dart:async';

import 'package:flutter/material.dart';
import 'package:tiq/actions/ui/utility/ShowModalMessage.dart';
import 'package:tiq/models/dtos/UserCredentials.dart';
import 'package:tiq/models/dtos/UserTokenResp.dart';
import 'package:tiq/models/exceptions/RpcException.dart';
import 'package:tiq/common/Messages.dart';
import 'package:tiq/services/RpcService.dart';

class UserLogin {
  Future<UserTokenResp> run(BuildContext context, UserCredentials userCredentials) async {
    UserTokenResp result;

    try {
      result = await RpcService.instance.userLogin(userCredentials);
    }
    on RpcException catch(exception) {
      await ShowModalMessage().run(context, Messages.ErrorServer, exception.errors.map((e) => e.errorText).join('\r\n'));
    }
    on TimeoutException {
      await ShowModalMessage().run(context, Messages.ErrorServer, Messages.ErrorTimeout);
    }
    on NoSuchMethodError {
      await ShowModalMessage().run(context, Messages.ErrorUnknown, Messages.ErrorDataStructure);
    }
    catch (exception) {
      await ShowModalMessage().run(context, Messages.ErrorUnknown, Messages.ErrorUserLogin);
    }

    return result;
  }
}
