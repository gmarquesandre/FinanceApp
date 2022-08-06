class CreateOrUpdateFGTS {
  int id;
  double monthlyGrossIncome;
  double currentBalance;
  bool anniversaryWithdraw;
  int monthAniversaryWithdraw;

  CreateOrUpdateFGTS({
    required this.id,
    required this.currentBalance,
    required this.anniversaryWithdraw,
    required this.monthAniversaryWithdraw,
    required this.monthlyGrossIncome,
  });

  Map<String, dynamic> toJson() => {
        'id': id,
        'currentBalance': currentBalance,
        'anniversaryWithdraw': anniversaryWithdraw,
        'monthAniversaryWithdraw': monthAniversaryWithdraw,
        'monthlyGrossIncome': monthlyGrossIncome,
      };
}
