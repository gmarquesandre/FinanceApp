import 'package:financial_app/models/dto_models/assetToBalance.dart';
import 'package:financial_app/models/dto_models/balanceMonth.dart';
import 'package:financial_app/models/dto_models/fundToBalance.dart';
import 'package:financial_app/models/dto_models/spendingAndIncome.dart';
import 'package:financial_app/models/models/fgtsBalance.dart';
import 'package:financial_app/models/models/fixedInterestMonthly.dart';
import 'package:financial_app/models/models/treasuryBondMonthly.dart';

class BalanceDto {
  List<BalanceMonth> balanceMonth;
  List<AssetToBalance> assetList;
  List<TreasuryBondMonthly> treasuryList;
  List<FixedInterestMonthly> fixedInvestmentList;
  List<FgtsBalance> fgtsList;
  List<SpendingAndIncome> spendingIncomeList;
  List<FundToBalance> investmentFund;


  BalanceDto({
    required this.balanceMonth,
    required this.assetList,
    required this.treasuryList,
    required this.fixedInvestmentList,
    required this.fgtsList,
    required this.spendingIncomeList,
    required this.investmentFund,

  });

}