import 'package:financial_app/models/table_models/investmentFund.dart';

class FundTransaction {
  double quantityCumulated;
  double avgUnitPrice;
  double remainingQuantity;
  InvestmentFund fund;
  String operation;

  FundTransaction({
    required this.quantityCumulated,
    required this.avgUnitPrice,
    required this.remainingQuantity,
    required this.fund,
    required this.operation
  });


}
