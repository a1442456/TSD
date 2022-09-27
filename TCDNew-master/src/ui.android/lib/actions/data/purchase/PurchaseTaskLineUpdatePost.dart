import 'dart:async';

import 'package:flutter/widgets.dart';
import 'package:tiq/actions/ui/utility/ShowModalMessage.dart';
import 'package:tiq/common/Messages.dart';
import 'package:tiq/models/dtos/PurchaseTaskLineUpdateDto.dart';
import 'package:tiq/models/dtos/PurchaseTaskLineUpdatePostReq.dart';
import 'package:tiq/models/exceptions/RpcException.dart';
import 'package:tiq/models/exceptions/UnauthorizedAccessException.dart';
import 'package:tiq/services/GStateProvider.dart';
import 'package:tiq/services/RpcService.dart';

class PurchaseTaskLineUpdatePost {
  Future<bool> run(BuildContext context, String purchaseTaskId, String productBarcode, String currentPalletCode, PurchaseTaskLineUpdateDto purchaseTaskLineUpdate) async {
    bool result;

    try {
      if (!GStateProvider.instance.stateAuth.isAuthorised) throw UnauthorizedAccessException();

      result = await RpcService.instance.purchaseTaskLineUpdatePost(PurchaseTaskLineUpdatePostReq(
          purchaseTaskId, productBarcode, currentPalletCode, purchaseTaskLineUpdate), GStateProvider.instance.stateAuth.authToken);
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
      await ShowModalMessage().run(context, Messages.ErrorUnknown, Messages.ErrorPurchaseTaskLineUpdatePost);
    }

    return result;
  }
}