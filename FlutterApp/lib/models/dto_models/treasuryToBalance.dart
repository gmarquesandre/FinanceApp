class TreasuryToBalance {
  int id;
  String treasuryBondName;
  double quantity;
  DateTime dateLastUpdate;
  double unitPriceBuy;
  double unitPricePurchase;
  double unitPriceSell;
  DateTime expirationDate;
  String indexName;
  DateTime investmentDate;
  double fixedInterestValueBuy;
  double fixedInterestValueSell;

  TreasuryToBalance({
    required this.id,
    required this.treasuryBondName,
    required this.quantity,
    required this.dateLastUpdate,
    required this.unitPriceBuy,
    required this.unitPricePurchase,
    required this.unitPriceSell,
    required this.expirationDate,
    required this.indexName,
    required this.investmentDate,
    required this.fixedInterestValueBuy,
    required this.fixedInterestValueSell
  });

  factory TreasuryToBalance.fromMap(Map<String, dynamic> map) {
    return TreasuryToBalance(
      id: map['id'],
      treasuryBondName: map['treasuryBondName'] ,
      quantity: map['quantity'],
      dateLastUpdate: DateTime.fromMillisecondsSinceEpoch(map['dateLastUpdate']),
      unitPricePurchase: map['unitPricePurchase'],
      unitPriceBuy: map['unitPriceBuy'],
      unitPriceSell: map['unitPriceSell'],
      expirationDate: DateTime.fromMillisecondsSinceEpoch(map['expirationDate']),
      indexName: map['indexName'],
      investmentDate: DateTime.fromMillisecondsSinceEpoch(map['investmentDate']),
      fixedInterestValueBuy: map['fixedInterestValueBuy'],
      fixedInterestValueSell: map['fixedInterestValueSell'],

    );
  }






}