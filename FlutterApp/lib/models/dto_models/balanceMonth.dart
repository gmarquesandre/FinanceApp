class BalanceMonth {

  DateTime date;
  double incomeCumulated;
  double spendingCumulated;
  double spending;
  double loan;
  double income;
  double currentBalance;
  double fundsValue;
  double stocksValue;
  double fixedIncomeValueLiquidityOnExpire;
  double fixedIncomeValueFreeLiquidity;
  double treasuryBondValue;
  double incomeLessSpending;
  double totalPatrimony;
  double totalPatrimonyLiquid;
  double fgtsValue;
  double treasuryBondValueExpired;
  double totalAvailable;
  double fgtsWithdraw;
  double cupomPayments;
  double spendingRequired;

  BalanceMonth({
    required this.date,
    required this.spending,
    required this.income,
    required this.loan,
    required this.incomeCumulated,
    required this.spendingCumulated,
    required this.incomeLessSpending,
    required this.stocksValue,
    required this.fundsValue,
    required this.currentBalance,
    required this.fixedIncomeValueFreeLiquidity,
    required this.fixedIncomeValueLiquidityOnExpire,
    required this.treasuryBondValue,
    required this.totalPatrimony,
    required this.totalPatrimonyLiquid,
    required this.fgtsValue,
    required this.treasuryBondValueExpired,
    required this.totalAvailable,
    required this.fgtsWithdraw,
    required this.cupomPayments,
    required this.spendingRequired
  });
}
