import 'dart:convert';
import 'dart:io';

Future<Map<String, dynamic>> jsonMapFromFile(String fileName) async {
  final file = File(fileName);
  Map<String, dynamic> jsonMap;
  if (file.existsSync()) {
    final String fileContents = await file.readAsString();
    jsonMap = json.decode(fileContents);
  }

  return jsonMap;
}

Future<void> objectToJsonFile<T>(T data, String fileName) async {
  final jsonResponse = json.encode(data);
  await File(fileName).writeAsString(jsonResponse, mode: FileMode.writeOnly);
}
