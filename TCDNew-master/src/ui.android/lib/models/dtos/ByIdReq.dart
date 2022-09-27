class ByIdReq {
  String id;

  ByIdReq(this.id);

  toJson() {
    return {
      'id': id,
    };
  }
}