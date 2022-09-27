class UserCredentials {
  String userName;
  String userPassword;

  UserCredentials({this.userName, this.userPassword});

  toJson() {
    return {
      'userName': userName,
      'userPassword': userPassword,
    };
  }
}