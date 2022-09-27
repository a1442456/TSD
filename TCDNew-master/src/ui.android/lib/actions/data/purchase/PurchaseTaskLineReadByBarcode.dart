import 'dart:async';

import 'package:flutter/widgets.dart';
import 'package:tiq/actions/ui/utility/ShowModalMessage.dart';
import 'package:tiq/common/Messages.dart';
import 'package:tiq/models/dtos/PurchaseTaskLineDto.dart';
import 'package:tiq/models/dtos/PurchaseTaskLineReadByBarcodeReq.dart';
import 'package:tiq/models/exceptions/RpcException.dart';
import 'package:tiq/models/exceptions/UnauthorizedAccessException.dart';
import 'package:tiq/services/GStateProvider.dart';
import 'package:tiq/services/RpcService.dart';

class PurchaseTaskLineReadByBarcode {
  Future<PurchaseTaskLineDto> run(BuildContext context, String purchaseTaskId, String barcode, String currentPalletCode) async {
    PurchaseTaskLineDto result;

    try {
      if (!GStateProvider.instance.stateAuth.isAuthorised) throw UnauthorizedAccessException();

      result = await RpcService.instance.purchaseTaskLineReadByBarcode(PurchaseTaskLineReadByBarcodeReq(
          purchaseTaskId, barcode, currentPalletCode), GStateProvider.instance.stateAuth.authToken);
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
      await ShowModalMessage().run(context, Messages.ErrorUnknown, Messages.ErrorPurchaseTaskLineReadByBarcode);
    }

    return result;
  }
}