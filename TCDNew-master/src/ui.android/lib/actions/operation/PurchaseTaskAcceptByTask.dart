import 'package:flutter/material.dart';
import 'package:tiq/actions/ui/purchase/PurchaseTaskContentScan.dart';
import 'package:tiq/actions/ui/purchase/PurchaseTaskWait.dart';
import 'package:tiq/actions/ui/utility/ShowModalMessage.dart';
import 'package:tiq/common/Messages.dart';

class PurchaseTaskAcceptByTask {
  Future<void> run(BuildContext context) async {
    var purchaseTaskId = await PurchaseTaskWait().run(context);
    if (purchaseTaskId != null) {
      await PurchaseTaskContentScan().runWithPurchaseTaskId(context, purchaseTaskId);
    }
    else {
      await ShowModalMessage().run(context, Messages.TitleError, 'Задание не выбрано!');
    }
  }
}
