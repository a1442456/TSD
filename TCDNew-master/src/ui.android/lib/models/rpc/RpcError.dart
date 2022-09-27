class RpcError {
  String errorCode;
  String errorText;

  RpcError(this.errorCode, this.errorText);

  factory RpcError.fromJson(Map<String, dynamic> json) {
    RpcError data;

    if (json != null) {
      data = RpcError(
        json['errorCode'],
        json['errorText'],
      );
    }
    return data;
  }
}