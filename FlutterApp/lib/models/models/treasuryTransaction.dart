import 'package:financial_app/models/table_models/treasury.dart';

class TreasuryTransaction {
  double quantityCumulated;
  double avgUnitPrice;
  double remainingQuantity;
  String operation;
  Treasury treasury;


  TreasuryTransaction({
    required this.quantityCumulated,
    required this.avgUnitPrice,
    required this.operation,
    required this.remainingQuantity,
    required this.treasury
  });

}


