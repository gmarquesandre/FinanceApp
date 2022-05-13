class FundToBalance {

  String cnpj;
  String nameShort;
  double unitPrice;
  double quantity;
  DateTime dateLastUpdate;

  FundToBalance({
    required this.nameShort,
    required this.cnpj,
    required this.unitPrice,
    required this.quantity,
    required this.dateLastUpdate,
  });

  factory FundToBalance.fromMap(Map<String, dynamic> map) {
    return FundToBalance(
      nameShort: map['nameShort'] ,
      cnpj: map['cnpj'] ,
      unitPrice: map['unitPrice'],
      quantity: map['quantity'],
      dateLastUpdate: DateTime.fromMillisecondsSinceEpoch(map['dateLastUpdate']),
    );
  }

}