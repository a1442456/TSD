import 'package:tiq/models/rpc/RpcError.dart';

class RpcException implements Exception {
  List<RpcError> errors;

  RpcException(this.errors);
}
