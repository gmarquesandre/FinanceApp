import 'package:financial_app/functions/lists/common_lists.dart';
import 'package:intl/intl.dart';

class FixedInterest {
  int? id;
  String name;
  int typeFixedInterestId;
  double amount;
  String indexName;
  int preFixedInvestment;
  double indexPercentage;
  double additionalFixedInterest;
  DateTime investmentDate;
  DateTime expirationDate;
  int liquidityOnExpiration;

  FixedInterest({
    this.id,
    required this.name,
    required this.typeFixedInterestId,
    required this.amount,
    required this.preFixedInvestment,
    required this.indexName,
    required this.indexPercentage,
    required this.additionalFixedInterest,
    required this.investmentDate,
    required this.expirationDate,
    required this.liquidityOnExpiration});


  String period(){
    return DateFormat('dd/MM/yy')
        .format(investmentDate)
        .toString() +
        " - " +
        DateFormat('dd/MM/yy')
            .format(expirationDate)
            .toString();
  }

  String investmentInterest(){

    if(indexName == ""){
      return  CommonLists.fixedInterestTypeList
          .firstWhere((a) =>
      a.id ==
          typeFixedInterestId)
        .name
          .toString() + " ( "+ (additionalFixedInterest *
          100)
        .toStringAsFixed(2)+"% )";
    }
    else if (additionalFixedInterest == 0){
      return CommonLists.fixedInterestTypeList
          .firstWhere((a) =>
      a.id ==
          typeFixedInterestId)
          .name
          .toString() +
          (indexName != "" ? " ( " +
              ((indexPercentage * 100)
                  .toStringAsFixed(2)) +
              "% " +
              indexName +" )": "");
    }
    return CommonLists.fixedInterestTypeList
            .firstWhere((a) =>
        a.id ==
            typeFixedInterestId)
            .name
            .toString() +
        " ( " +
        ((indexPercentage * 100)
            .toStringAsFixed(2)) +
        "% " +
      indexName
            +
        " +" +
        (additionalFixedInterest *
            100)
            .toStringAsFixed(2)+"% )";

  }

  Map<String, dynamic> toMap() {
    final fixedInterestMap = Map<String, dynamic>();
    if (id != null) {
      fixedInterestMap['id'] = id;
    }
    fixedInterestMap['name'] = name;
    fixedInterestMap['preFixedInvestment'] = preFixedInvestment;
    fixedInterestMap['typeFixedInterestId'] = typeFixedInterestId;
    fixedInterestMap['amount'] = amount;
    fixedInterestMap['indexName'] = indexName;
    fixedInterestMap['indexPercentage'] = indexPercentage;
    fixedInterestMap['additionalFixedInterest'] = additionalFixedInterest;
    fixedInterestMap['investmentDate'] = investmentDate.millisecondsSinceEpoch;
    fixedInterestMap['expirationDate'] = expirationDate.millisecondsSinceEpoch;
    fixedInterestMap['liquidityOnExpiration'] = liquidityOnExpiration;
    return fixedInterestMap;
  }

  factory FixedInterest.fromMap(Map<String, dynamic> row){
    return FixedInterest(
      id: row['id'],
      name: row['name'],
      preFixedInvestment: row['preFixedInvestment'],
      typeFixedInterestId: row['typeFixedInterestId'],
      amount: row['amount'],
      indexName: row['indexName'],
      indexPercentage: row['indexPercentage'],
      additionalFixedInterest: row['additionalFixedInterest'],
      investmentDate: DateTime.fromMillisecondsSinceEpoch(row['investmentDate']),
      expirationDate: DateTime.fromMillisecondsSinceEpoch(row['expirationDate']),
      liquidityOnExpiration: row['liquidityOnExpiration'],
    );
  }






}