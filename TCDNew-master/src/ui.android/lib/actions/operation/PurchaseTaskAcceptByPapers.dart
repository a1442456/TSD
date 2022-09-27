import 'package:tiq/actions/data/purchase/PurchaseTaskCreateFromPacs.dart';
import 'package:tiq/actions/ui/purchase/PurchaseTaskContentScan.dart';
import 'package:tiq/actions/ui/purchase/PacHeadScan.dart';
import 'package:tiq/actions/ui/utility/ShowModalMessage.dart';
import 'package:tiq/models/dtos/PacKeyDto.dart';
import 'package:tiq/common/Messages.dart';
import 'package:flutter/material.dart';
import 'package:tiq/services/GStateProvider.dart';

class PurchaseTaskAcceptByPapers {
  Future<void> run(BuildContext context) async {
    var pacHeadList = await PacHeadScan().run(context);
    if (pacHeadList != null) {
      if (pacHeadList.length > 0) {
        final List<PacKeyDto> pacKeys = pacHeadList.cast<PacKeyDto>();
        var purchaseTaskId = await PurchaseTaskCreateFromPacs().run(
            context,
            GStateProvider.instance.settingsFacility.facilityId,
            pacKeys);
        if (purchaseTaskId != null) {
          await PurchaseTaskContentScan().runWithPurchaseTaskId(context, purchaseTaskId);
        }
        else {
          await ShowModalMessage().run(context, Messages.TitleError, 'Не удалось создать новое задание!');
        }
      }
      else {
        await ShowModalMessage().run(context, Messages.TitleError, 'Закупки не выбраны!');
      }
    }
  }
}
