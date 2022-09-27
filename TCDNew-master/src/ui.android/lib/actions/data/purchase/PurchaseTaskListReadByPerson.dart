import 'dart:async';

import 'package:flutter/material.dart';
import 'package:tiq/actions/ui/utility/ShowModalMessage.dart';
import 'package:tiq/models/dtos/PurchaseTaskDto.dart';
import 'package:tiq/models/dtos/PurchaseTaskListReadByPersonReq.dart';
import 'package:tiq/models/exceptions/RpcException.dart';
import 'package:tiq/common/Messages.dart';
import 'package:tiq/models/exceptions/UnauthorizedAccessException.dart';
import 'package:tiq/services/GStateProvider.dart';
import 'package:tiq/services/RpcService.dart';

class PurchaseTaskListReadByPerson {
  Future<List<PurchaseTaskDto>> run(BuildContext context) async {
    List<PurchaseTaskDto> result;

    try {
      if (!GStateProvider.instance.stateAuth.isAuthorised) throw UnauthorizedAccessException();

      result = await RpcService.instance.purchaseTaskListReadByPerson(
          PurchaseTaskListReadByPersonReq(GStateProvider.instance.settingsFacility.facilityId), GStateProvider.instance.stateAuth.authToken);
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
      await ShowModalMessage().run(context, Messages.ErrorUnknown, Messages.ErrorPurchaseTaskListReadByPerson);
    }

    return result;
  }
}