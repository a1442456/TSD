import 'ProductDto.dart';
import 'PurchaseTaskLineStateDto.dart';

class PurchaseTaskLineDto {
  String id;
  ProductDto product;
  num quantity;
  PurchaseTaskLineStateDto state;

  PurchaseTaskLineDto(this.id, this.product, this.quantity, this.state);

  factory PurchaseTaskLineDto.fromJson(Map<String, dynamic> json) {
    PurchaseTaskLineDto data;

    if (json != null) {
      data = PurchaseTaskLineDto(
        json['id'].toString(),
        ProductDto.fromJson(json['product']),
        json['quantity'] ?? 0.00,
        PurchaseTaskLineStateDto.fromJson(json['state'])
      );
    }

    return data;
  }
}
