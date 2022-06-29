class TreasuryBondMonthly {
  int id;
  DateTime date;
  double amount;
  String indexName;
  DateTime investmentDate;
  DateTime expirationDate;
  double todayValue;
  double cupomPayment;


  TreasuryBondMonthly({
    required this.id,
    required this.date,
    required this.amount,
    required this.indexName,
    required this.investmentDate,
    required this.expirationDate,
    required this.todayValue,
    required this.cupomPayment,
  });

  Map<String, dynamic> toMap() {
    final fixedInterestMap = Map<String, dynamic>();
    fixedInterestMap['id'] = id;
    fixedInterestMap['date'] = date.millisecondsSinceEpoch;
    fixedInterestMap['amount'] = amount;
    fixedInterestMap['indexName'] = indexName;
    fixedInterestMap['investmentDate'] = investmentDate.millisecondsSinceEpoch;
    fixedInterestMap['expirationDate'] = expirationDate.millisecondsSinceEpoch;
    fixedInterestMap['todayValue'] = todayValue;
    fixedInterestMap['cupomPayment'] = cupomPayment;
    return fixedInterestMap;
  }

  factory TreasuryBondMonthly.fromMap(Map<String, dynamic> row){
    return TreasuryBondMonthly(
      id: row['id'],
      date: DateTime.fromMillisecondsSinceEpoch(row['date']),
      amount: row['amount'],
      indexName: row['indexName'],
      investmentDate: DateTime.fromMillisecondsSinceEpoch(
          row['investmentDate']),
      expirationDate: DateTime.fromMillisecondsSinceEpoch(
          row['expirationDate']),
      todayValue: row['todayValue'],
      cupomPayment: row['cupomPayment'],
    );
  }
}

