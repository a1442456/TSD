import 'dart:async';

import 'package:flutter/material.dart';
import 'package:tiq/actions/ui/utility/ShowModalMessage.dart';
import 'package:tiq/models/dtos/PacHeadDto.dart';
import 'package:tiq/models/dtos/PacHeadReadByBarcodeReq.dart';
import 'package:tiq/models/exceptions/RpcException.dart';
import 'package:tiq/common/Messages.dart';
import 'package:tiq/models/exceptions/UnauthorizedAccessException.dart';
import 'package:tiq/services/GStateProvider.dart';
import 'package:tiq/services/RpcService.dart';

class PacHeadReadByBarcode {
  Future<PacHeadDto> run(BuildContext context, String pacBarcode, String facilityId) async {
    PacHeadDto result;

    try {
      if (!GStateProvider.instance.stateAuth.isAuthorised) throw UnauthorizedAccessException();

      result = await RpcService.instance.pacHeadReadByBarcode(
          PacHeadReadByBarcodeReq(barcode: pacBarcode, facilityId: facilityId), GStateProvider.instance.stateAuth.authToken);
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
      await ShowModalMessage().run(context, Messages.ErrorUnknown, Messages.ErrorPacHeadReadByBarcode);
    }

    return result;
  }
}