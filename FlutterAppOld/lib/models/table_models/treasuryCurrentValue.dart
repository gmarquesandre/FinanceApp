class TreasuryCurrentValue {
  String codeISIN;
  String treasuryBondName;
  DateTime dateLastUpdate;
  double unitPriceBuy;
  double unitPriceSell;
  DateTime expirationDate;
  String indexName;
  double fixedInterestValueBuy;
  double fixedInterestValueSell;
  DateTime lastAvailableDate;

  TreasuryCurrentValue({
    required this.codeISIN,
    required this.treasuryBondName,
    required this.dateLastUpdate,
    required this.unitPriceBuy,
    required this.unitPriceSell,
    required this.expirationDate,
    required this.indexName,
    required this.fixedInterestValueBuy,
    required this.fixedInterestValueSell,
    required this.lastAvailableDate
  });

  Map<String, dynamic> toMap() {
    final map = Map<String,dynamic>();

    map['codeISIN'] = codeISIN;
    map['treasuryBondName'] = treasuryBondName;
    map['dateLastUpdate'] = dateLastUpdate.millisecondsSinceEpoch;
    map['unitPriceBuy'] = unitPriceBuy;
    map['unitPriceSell'] = unitPriceSell;
    map['expirationDate'] = expirationDate.millisecondsSinceEpoch;
    map['indexName'] = indexName;
    map['fixedInterestValueBuy'] = fixedInterestValueBuy;
    map['fixedInterestValueSell'] = fixedInterestValueSell;
    map['lastAvailableDate'] = lastAvailableDate.millisecondsSinceEpoch;
    return map;
  }

  factory TreasuryCurrentValue.fromMap(Map<String, dynamic> row) {
    return TreasuryCurrentValue(
      codeISIN: row['codeISIN'],
      treasuryBondName: row['treasuryBondName'],
      dateLastUpdate: DateTime.fromMillisecondsSinceEpoch(row['dateLastUpdate']),
      unitPriceBuy: row['unitPriceBuy'],
      unitPriceSell: row['unitPriceSell'],
      expirationDate: DateTime.fromMillisecondsSinceEpoch(row['expirationDate']),
      indexName: row['indexName'],
      fixedInterestValueBuy: row['fixedInterestValueBuy'],
      fixedInterestValueSell: row['fixedInterestValueSell'],
      lastAvailableDate: DateTime.fromMillisecondsSinceEpoch(row['lastAvailableDate']),

    );
  }

  TreasuryCurrentValue.fromJson(Map<String, dynamic> json) :
        codeISIN = json['codeISIN'],
        treasuryBondName = json['treasuryBondName'],
        dateLastUpdate = DateTime.tryParse(json['dateLastUpdate'].toString())!,
        unitPriceBuy = double.tryParse(json['unitPriceBuy'].toString())!,
        unitPriceSell = double.tryParse(json['unitPriceSell'].toString())!,
        expirationDate = DateTime.tryParse(json['expirationDate'].toString())!,
        indexName = json['indexName'].toString(),
        fixedInterestValueBuy = double.tryParse(json['fixedInterestValueBuy']
            .toString())!,
        fixedInterestValueSell = double.tryParse
        (json['fixedInterestValueSell'].toString())!,
        lastAvailableDate = DateTime.tryParse(json['lastAvailableDate']
            .toString())!;


  Map<String, dynamic> toJson() =>
    {
      'treasuryBondName' : treasuryBondName,
      'dateLastUpdate': dateLastUpdate,
      'unitPriceBuy': unitPriceBuy,
      'unitPriceSell': unitPriceSell,
      'expirationDate': expirationDate,
      'indexName': indexName,
      'fixedInterestValueBuy': fixedInterestValueBuy,
      'fixedInterestValueSell': fixedInterestValueSell,
      'lastAvailableDate': lastAvailableDate,
    };


}
