class PacKeyDto
{
  String pacId;

  PacKeyDto({this.pacId});

  toJson() {
    return {
      'pacId': pacId,
    };
  }
}