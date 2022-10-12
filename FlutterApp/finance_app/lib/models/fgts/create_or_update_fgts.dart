class CreateOrUpdateFGTS {
  double monthlyGrossIncome;
  double currentBalance;
  bool anniversaryWithdraw;
  int monthAniversaryWithdraw;

  CreateOrUpdateFGTS({
    required this.currentBalance,
    required this.anniversaryWithdraw,
    required this.monthAniversaryWithdraw,
    required this.monthlyGrossIncome,
  });

  Map<String, dynamic> toJson() => {
        'currentBalance': currentBalance,
        'anniversaryWithdraw': anniversaryWithdraw,
        'monthAniversaryWithdraw': monthAniversaryWithdraw,
        'monthlyGrossIncome': monthlyGrossIncome,
      };
}
