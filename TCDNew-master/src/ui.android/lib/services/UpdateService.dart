import 'dart:async';
import 'dart:io';

import 'package:flutter/services.dart';
import 'package:http/http.dart';
import 'package:path_provider/path_provider.dart';

import '../version.dart';
import 'GStateProvider.dart';

typedef void OnDownloadProgressCallback(int receivedBytes, int totalBytes);

class UpdateService {
  static const platform = const MethodChannel('by.tiq/update');

  static Future updateApplication() async {
    try
    {
      final versionLocal = Version.current;
      final versionRemote = await _getRemoteVersion();
      if (versionRemote > versionLocal) {
        final downloadsDirectory = await getExternalStorageDirectory();
        final apkFileName = "$versionRemote.apk";
        final apkFilePath = downloadsDirectory.path + apkFileName;
        if (File(apkFilePath).existsSync()) {
          await File(apkFilePath).delete();
        }
        await _fileDownload(
          url: '${GStateProvider.instance.settingsApp.wmsServiceBaseAddress}update/android/$apkFileName',
          apkFilePath: apkFilePath,
        );
        if (File(apkFilePath).existsSync()) {
          platform.invokeMethod("updateApk", { 'apkFilePath': apkFilePath });
          await SystemChannels.platform.invokeMethod('SystemNavigator.pop');
        }
      }
    }
    catch (e) {
      //
    }
  }

  static Future<int> _getRemoteVersion({int timeout = 10}) async {
    var result = 0;

    try
    {
      String url = "${GStateProvider.instance.settingsApp.wmsServiceBaseAddress}update/android/version.txt";
      Map<String, String> headers = {
        'Content-type': 'text/plain',
      };
      final responseFuture = await
        get(
          Uri.parse(url),
          headers: headers,
        )
        .timeout(new Duration(seconds: timeout));

      result = int.parse(responseFuture.body.trim());
    }
    catch (e) {
      //
    }

    return result;
  }

  static Future<String> _fileDownload(
      {String url, String apkFilePath, OnDownloadProgressCallback onDownloadProgress}) async {

    final httpClient = new HttpClient();
    final request = await httpClient.getUrl(Uri.parse(url));
    request.headers.add(HttpHeaders.contentTypeHeader, "application/octet-stream");

    final httpResponse = await request.close();

    int byteCount = 0;
    int totalBytes = httpResponse.contentLength;

    final apkFile = new File(apkFilePath);
    final raf = apkFile.openSync(mode: FileMode.write);

    Completer completer = new Completer<String>();
    httpResponse.listen(
      (data) {
        byteCount += data.length;

        raf.writeFromSync(data);

        if (onDownloadProgress != null) {
          onDownloadProgress(byteCount, totalBytes);
        }
      },
      onDone: () {
        raf.closeSync();

        completer.complete(apkFile.path);
      },
      onError: (e) {
        raf.closeSync();
        apkFile.deleteSync();
        completer.completeError(e);
      },
      cancelOnError: true,
    );

    return completer.future;
  }
}