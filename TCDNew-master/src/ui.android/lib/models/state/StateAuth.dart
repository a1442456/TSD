import 'package:tiq/models/dtos/UserTokenResp.dart';

class StateAuth {
  UserTokenResp _userToken;

  StateAuth(UserTokenResp userToken) {
    if (userToken != null) {
      this._userToken = UserTokenResp(userToken.authToken, userToken.displayName);
    }
  }

  bool get isAuthorised => this._userToken != null;
  String get authToken => this._userToken.authToken;
  String get displayName => this._userToken.authToken;
}