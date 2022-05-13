class AssetCurrentValue {
  String assetCode;
  double unitPrice;
  String companyName;
  DateTime dateLastUpdate;


  AssetCurrentValue({
    required this.assetCode,
    required this.unitPrice,
    required this.companyName,
    required this.dateLastUpdate
  });


  AssetCurrentValue.fromJson(Map<String, dynamic> json) :
        assetCode = json['assetCode'],
        unitPrice = json['unitPrice'].toDouble(),
        companyName = json['companyName'],
        dateLastUpdate = DateTime.tryParse(json['dateLastUpdate'].toString())!;

  Map<String, dynamic> toJson() =>
      {
        'assetCode' : assetCode,
        'unitPrice': unitPrice,
        'companyName': companyName,
        'dateLastUpdate': dateLastUpdate,
      };

  Map<String, dynamic> toMap() {
    final map = Map<String, dynamic>();
    map['assetCode'] = assetCode;
    map['dateLastUpdate'] = dateLastUpdate.millisecondsSinceEpoch;
    map['unitPrice'] = unitPrice;
    map['companyName'] = companyName;
    return map;
  }

  factory AssetCurrentValue.fromMap(Map<String, dynamic> map) {
    return AssetCurrentValue(
      assetCode: map['assetCode'],
      companyName: map['companyName'],
      dateLastUpdate: DateTime.fromMillisecondsSinceEpoch(map['dateLastUpdate']),
      unitPrice: map['unitPrice'],
    );
  }
}
