import 'package:tiq/actions/ui/task/AskForTaskType.dart';
import 'package:tiq/models/enums/TaskType.dart';
import 'package:flutter/material.dart';

import 'PurchaseTaskAcceptByPapers.dart';
import 'PurchaseTaskAcceptByTask.dart';

class ExecuteTask {
  Future<void> run(BuildContext context) async {
    var taskType = TaskType.none;
    while (taskType != TaskType.exit)
    {
      taskType = await AskForTaskType().run(context);

      if (taskType == TaskType.purchaseByPapers) {
        await PurchaseTaskAcceptByPapers().run(context);
      }

      if (taskType == TaskType.purchaseByTask) {
        await PurchaseTaskAcceptByTask().run(context);
      }
    }
  }
}