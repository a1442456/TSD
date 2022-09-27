import 'dart:async';

import 'package:flutter/material.dart';
import 'package:tiq/actions/ui/utility/ShowModalMessage.dart';
import 'package:tiq/models/dtos/PacKeyDto.dart';
import 'package:tiq/models/dtos/PurchaseTaskCreateFromPacsReq.dart';
import 'package:tiq/models/exceptions/RpcException.dart';
import 'package:tiq/common/Messages.dart';
import 'package:tiq/models/exceptions/UnauthorizedAccessException.dart';
import 'package:tiq/services/GStateProvider.dart';
import 'package:tiq/services/RpcService.dart';

class PurchaseTaskCreateFromPacs {
  Future<String> run(BuildContext context, String facilityId, List<PacKeyDto> pacKeys) async {
    String result;

    try {
      if (!GStateProvider.instance.stateAuth.isAuthorised) throw UnauthorizedAccessException();

      result = await RpcService.instance.purchaseTaskCreateFromPacs(PurchaseTaskCreateFromPacsReq(
        facilityId: facilityId, pacs: pacKeys), GStateProvider.instance.stateAuth.authToken);
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
      await ShowModalMessage().run(context, Messages.ErrorUnknown, Messages.ErrorPurchaseTaskCreate);
    }

    return result;
  }
}