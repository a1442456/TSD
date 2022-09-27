import 'package:tiq/models/enums/TaskType.dart';
import 'package:tiq/router.dart';
import 'package:flutter/material.dart';

class AskForTaskType {
  Future<TaskType> run(BuildContext context) async {
    final formData = await Navigator.pushNamed(context, TaskSelectRoute);
    return (formData as TaskType);
  }
}