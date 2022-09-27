class ScanResult {
  final bool isSuccessful;
  final String barcode1;
  final String barcode2;

  const ScanResult({
    this.isSuccessful,
    this.barcode1,
    this.barcode2,
  });

  factory ScanResult.fromJson(Map<String, dynamic> json) {
    if (json == null) {
      throw FormatException('Incorrect JSON');
    }

    return ScanResult(
      isSuccessful: json['isSuccessful'],
      barcode1: json['barcode1'],
      barcode2: json['barcode2']
    );
  }
}