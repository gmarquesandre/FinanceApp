
class FixedInterestComplete{

  int fixedInvestmentId;
  String fixedInvestmentName;
  double amount;
  double indexPercentage;
  double additionalFixedInterest;
  double todayValue;
  DateTime investmentDate;
  DateTime expirationDate;
  String investmentTypeName;
  int incomeTax;
  int indexId;
  String indexName;

  FixedInterestComplete({
    required this.fixedInvestmentId,
    required this.fixedInvestmentName,
    required this.amount,
    required this.indexPercentage,
    required this.additionalFixedInterest,
    required this.investmentDate,
    required this.expirationDate,
    required this.investmentTypeName,
    required this.incomeTax,
    required this.indexId,
    required this.indexName,
    required this.todayValue,
  });

  factory FixedInterestComplete.fromMap(Map<String, dynamic> map) {
    return
     FixedInterestComplete(
      fixedInvestmentId: map['fixedInvestmentId'],
      fixedInvestmentName: map['fixedInvestmentName'],
      amount: map['amount'],
      indexPercentage: map['indexPercentage'],
      additionalFixedInterest: map['additionalFixedInterest'],
      investmentDate: DateTime.fromMillisecondsSinceEpoch(map['investmentDate']),
      expirationDate: DateTime.fromMillisecondsSinceEpoch(map['expirationDate']),
      incomeTax: map['incomeTax'],
      investmentTypeName: map['investmentTypeName'],
      indexId: map['indexId'],
      indexName: map['indexName'],
       todayValue: map['todayValue'],
     );
  }


  factory FixedInterestComplete.copyWith(FixedInterestComplete element) {
    return FixedInterestComplete(
      fixedInvestmentId: element.fixedInvestmentId,
      fixedInvestmentName: element.fixedInvestmentName,
      amount: element.amount,
      indexPercentage: element.indexPercentage,
      additionalFixedInterest: element.additionalFixedInterest,
      investmentDate: element.investmentDate,
      expirationDate: element.expirationDate,
      investmentTypeName: element.investmentTypeName,
      incomeTax: element.incomeTax,
      indexId: element.indexId,
      indexName: element.indexName,
      todayValue: element.todayValue,
    );
  }


}