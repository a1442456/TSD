class UserTokenResp {
  String authToken;
  String displayName;

  UserTokenResp(this.authToken, this.displayName);

  factory UserTokenResp.fromJson(Map<String, dynamic> json) {
    UserTokenResp data;

    if (json != null) {
      data = UserTokenResp(
        json['authToken'],
        json['displayName']
      );
    }

    return data;
  }
}