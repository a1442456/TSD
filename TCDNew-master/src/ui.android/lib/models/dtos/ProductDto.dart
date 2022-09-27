class ProductDto {
  String id;
  String name;
  String abc;
  List<String> barcodes;

  ProductDto(this.id, this.name, this.abc, this.barcodes);

  factory ProductDto.fromJson(Map<String, dynamic> json) {
    ProductDto data;

    if (json != null) {
      final barcodes = List<String>();
      json['barcodes'].forEach((value) => barcodes.add(value));

      data = new ProductDto(
        json['id'],
        json['name'],
        json['abc'],
        barcodes
      );
    }

    return data;
  }
}