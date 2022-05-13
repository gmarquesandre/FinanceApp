
class InvestmentFund {
  int? id;
  DateTime date;
  String cnpj;
  String name;
  double totalInvestment;
  int operation;
  double unitPrice;
  double? quantity;
  //Pesquisar como transformar essa variable só em um "gettable"

  InvestmentFund({
    this.id,
    required this.date,
    required this.cnpj,
    required this.operation,
    required this.name,
    required this.totalInvestment,
    required this.unitPrice,
    this.quantity

  });


  Map<String, dynamic> toMap() {
    final map = Map<String, dynamic>();
    if(id != null ){
      map['id'] = id;
    }
    map['date'] = date.millisecondsSinceEpoch;
    map['cnpj'] = cnpj;
    map['name'] = name;
    map['operation'] = operation;
    map['unitPrice'] = unitPrice;
    map['totalInvestment'] = totalInvestment;
    map['quantity'] = totalInvestment/unitPrice;
    return map;
  }

  factory InvestmentFund.fromMap(Map<String, dynamic> map) {
    return InvestmentFund(
      id: map['id'],
      date: DateTime.fromMillisecondsSinceEpoch(map['date']),
      cnpj: map['cnpj'],
      name: map['name'],
        operation: map['operation'],
      totalInvestment: map['totalInvestment'],
      unitPrice: map['unitPrice'],
      quantity: map['quantity']!
    );
  }
}
