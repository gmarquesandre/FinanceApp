import 'package:financial_app/models/table_models/fixedInterest.dart';

class FixedInterestMonthly {
  int id;
  String name;
  DateTime date;
  int typeFixedInterestId;
  double amount;
  String indexName;
  int preFixedInvestment;
  double indexPercentage;
  double additionalFixedInterest;
  DateTime investmentDate;
  DateTime expirationDate;
  int liquidityOnExpiration;
  double todayValue;
  FixedInterest fixedInterestElement;

  FixedInterestMonthly({
    required this.id,
    required this.date,
    required this.name,
    required this.typeFixedInterestId,
    required this.amount,
    required this.preFixedInvestment,
    required this.indexName,
    required this.indexPercentage,
    required this.additionalFixedInterest,
    required this.investmentDate,
    required this.expirationDate,
    required this.liquidityOnExpiration,
    required this.todayValue,
    required this.fixedInterestElement,

  });

  Map<String, dynamic> toMap() {
    final fixedInterestMap = Map<String, dynamic>();
      fixedInterestMap['id'] = id;
      fixedInterestMap['name'] = name;
      fixedInterestMap['date'] = date.millisecondsSinceEpoch;
      fixedInterestMap['preFixedInvestment'] = preFixedInvestment;
      fixedInterestMap['typeFixedInterestId'] = typeFixedInterestId;
      fixedInterestMap['amount'] = amount;
      fixedInterestMap['indexName'] = indexName;
      fixedInterestMap['indexPercentage'] = indexPercentage;
      fixedInterestMap['additionalFixedInterest'] = additionalFixedInterest;
      fixedInterestMap['investmentDate'] = investmentDate.millisecondsSinceEpoch;
      fixedInterestMap['expirationDate'] = expirationDate.millisecondsSinceEpoch;
      fixedInterestMap['liquidityOnExpiration'] = liquidityOnExpiration;
      fixedInterestMap['todayValue'] = todayValue;
      fixedInterestMap['fixedInterestElement'] = fixedInterestElement;
    return fixedInterestMap;
  }

  // factory FixedInterestMonthly.fromMap(Map<String, dynamic> row){
  //   return FixedInterestMonthly(
  //     id: row['id'],
  //     name: row['name'],
  //     date:  DateTime.fromMillisecondsSinceEpoch(row['date']),
  //     preFixedInvestment: row['preFixedInvestment'],
  //     typeFixedInterestId: row['typeFixedInterestId'],
  //     amount: row['amount'],
  //     indexName: row['indexName'],
  //     indexPercentage: row['indexPercentage'],
  //     additionalFixedInterest: row['additionalFixedInterest'],
  //     investmentDate: DateTime.fromMillisecondsSinceEpoch(row['investmentDate']),
  //     expirationDate: DateTime.fromMillisecondsSinceEpoch(row['expirationDate']),
  //     liquidityOnExpiration: row['liquidityOnExpiration'],
  //     todayValue: row['todayValue'],
  //     fixedInterestElement: row['fixedInterestElement'],
  //   );
  // }

}