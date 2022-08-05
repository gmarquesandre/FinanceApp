class CreateOrUpdateFGTS {
  int id;
  double monthlyGrossIncome;
  double currentValue;
  bool anniversaryWithdraw;
  int monthAniversaryWithdraw;

  CreateOrUpdateFGTS({
    required this.id,
    required this.currentValue,
    required this.anniversaryWithdraw,
    required this.monthAniversaryWithdraw,
    required this.monthlyGrossIncome,
  });

  Map<String, dynamic> toJson() => {
        'id': id,
        'currentValue': currentValue,
        'anniversaryWithdraw': anniversaryWithdraw,
        'monthAniversaryWithdraw': monthAniversaryWithdraw,
        'monthlyGrossIncome': monthlyGrossIncome,
      };
}
