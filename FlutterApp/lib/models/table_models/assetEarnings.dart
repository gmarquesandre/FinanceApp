class AssetEarnings {
  int id;
  String type;
  DateTime declarationDate;
  DateTime exDate;
  String assetCode;
  String assetCodeISIN;
  String notes;
  String hash;
  double cashAmount;
  String period;
  DateTime paymentDate;


  AssetEarnings.fromJson(Map<String, dynamic> json) :
        id = json['id'],
        assetCode = json['assetCode'],
        type = json['type'],
        notes = json['notes'],
        hash = json['hash'].toString(),
        cashAmount = json['cashAmount'].toDouble(),
        assetCodeISIN = json['assetCodeISIN'],
        exDate = DateTime.tryParse(json['exDate'].toString())!,
        declarationDate = DateTime.tryParse(json['declarationDate'].toString())!,
        paymentDate = DateTime.tryParse(json['paymentDate'].toString())!,
        period = json['period'];




  AssetEarnings({
    required this.id,
    required this.type,
    required this.assetCode,
    required this.assetCodeISIN,
    required this.declarationDate,
    required this.exDate,
    required this.cashAmount,
    required this.period,
    required this.paymentDate,
    required this.notes,
    required this.hash,
  });

  Map<String, dynamic> toJson() =>
      {
        'id' : id,
        'type' : type,
        'cashAmount': cashAmount,
        'notes': notes,
        'hash': hash,
        'assetCode': assetCode,
        'assetCodeISIN': assetCodeISIN,
        'paymentDate': paymentDate,
        'period': period,
        'exDate': exDate,
        'declarationDate': declarationDate,
      };

  Map<String, dynamic> toMap() {
    final map = Map<String, dynamic>();
    map['id'] = id;
    map['type'] = type;
    map['assetCode'] = assetCode;
    map['assetCodeISIN'] = assetCodeISIN;
    map['period'] = period;
    map['notes'] = notes;
    map['hash'] = hash;
    map['cashAmount'] = cashAmount;
    map['declarationDate'] = declarationDate.millisecondsSinceEpoch;
    map['assetCodeISIN'] = assetCodeISIN;
    map['paymentDate'] = paymentDate.millisecondsSinceEpoch;
    map['exDate'] = exDate.millisecondsSinceEpoch;
    return map;
  }

  factory AssetEarnings.fromMap(Map<String, dynamic> map) {
    return AssetEarnings(
      id: map['id'],
      type: map['type'],
      assetCode: map['assetCode'],
      assetCodeISIN: map['assetCodeISIN'],
      exDate: DateTime.fromMillisecondsSinceEpoch(map['exDate']),
      declarationDate: DateTime.fromMillisecondsSinceEpoch(map['declarationDate']),
      paymentDate: DateTime.fromMillisecondsSinceEpoch(map['paymentDate']),
      cashAmount: map['cashAmount'],
      notes: map['notes'].toString(),
      period: map['period'],
      hash: map['hash'].toString()
    );
  }
}
