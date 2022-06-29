class Asset {
  int? id;
  String assetCode;
  int quantity;
  double unitPrice;
  int operation;
  DateTime date;


  Asset({
    this.id,
    required this.assetCode,
    required this.quantity,
    required this.unitPrice,
    required this.operation,
    required this.date
  });

  Map<String, dynamic> toMap() {
    final assetMap = Map<String, dynamic>();
    if (id != null) {
      assetMap['id'] = id;
    }
    assetMap['operation'] = operation;
    assetMap['assetCode'] = assetCode;
    assetMap['quantity'] = quantity;
    assetMap['unitPrice'] = unitPrice;
    assetMap['date'] = date.millisecondsSinceEpoch;
    return assetMap;
  }

  factory Asset.fromMap(Map<String, dynamic> map) {
    return Asset(
      operation: map['operation'],
      id: map['id'],
      assetCode: map['assetCode'],
      quantity: map['quantity'],
      unitPrice: map['unitPrice'],
      date: DateTime.fromMillisecondsSinceEpoch(map['date']),
    );
  }
}
