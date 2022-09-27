import 'dart:io';
import 'dart:math';

import 'package:crypto/crypto.dart';
import 'package:path/path.dart';
import 'package:path_provider/path_provider.dart';
import 'package:tweetnacl/tweetnacl.dart';

Future<void> initSecurityKeys(String privateKeyFileName, String publicKeyFileName) async {
  final privateKeyFilePath = await _getPrivateKeyFilePath(privateKeyFileName);
  if (FileSystemEntity.typeSync(privateKeyFilePath) == FileSystemEntityType.notFound) {
    final random = Random.secure();
    final seed = List<int>.generate(32, (i) => random.nextInt(256));
    final Digest digest = sha256.convert(seed);

    final keypair = Signature.keyPair_fromSeed(digest.bytes);

    final publicKeyFilePath = await _getPublicKeyFilePath(publicKeyFileName);

    await File(privateKeyFilePath).writeAsString(TweetNaclFast.hexEncodeToString(keypair.secretKey), mode: FileMode.writeOnly);
    await File(publicKeyFilePath).writeAsString(TweetNaclFast.hexEncodeToString(keypair.publicKey), mode: FileMode.writeOnly);
  }
}

Future<String> getPublicKey(String publicKeyFileName) async {
  final publicKeyPath = await _getPublicKeyFilePath(publicKeyFileName);

  return await File(publicKeyPath).readAsString();
}

Future<String> getPrivateKey(String privateKeyFileName) async {
  final privateKeyPath = await _getPrivateKeyFilePath(privateKeyFileName);

  return await File(privateKeyPath).readAsString();
}

Future<String> _getPrivateKeyFilePath(String privateKeyFileName) async {
  final documentsDirectory = await getApplicationDocumentsDirectory();
  final fileName = join(documentsDirectory.path, privateKeyFileName);

  return fileName;
}

Future<String> _getPublicKeyFilePath(String publicKeyFileName) async {
  final documentsDirectory = await getApplicationDocumentsDirectory();
  final fileName = join(documentsDirectory.path, publicKeyFileName);

  return fileName;
}