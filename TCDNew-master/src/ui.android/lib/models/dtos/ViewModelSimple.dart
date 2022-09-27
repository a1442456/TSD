class ViewModelSimple {
  String id;
  String name;

  ViewModelSimple(this.id, this.name);

  factory ViewModelSimple.fromJson(Map<String, dynamic> json) {
    ViewModelSimple data;

    if (json != null) {
      data = ViewModelSimple(
          json['id'],
          json['name']
      );
    }

    return data;
  }
}