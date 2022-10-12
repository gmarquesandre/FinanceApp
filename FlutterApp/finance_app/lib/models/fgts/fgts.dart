class FGTS {
  double monthlyGrossIncome;
  double currentBalance;
  bool anniversaryWithdraw;
  int monthAniversaryWithdraw;
  DateTime updateDateTime;

  FGTS({
    required this.currentBalance,
    required this.updateDateTime,
    required this.anniversaryWithdraw,
    required this.monthAniversaryWithdraw,
    required this.monthlyGrossIncome,
  });

  factory FGTS.copyWith(FGTS element) {
    return FGTS(
      currentBalance: element.currentBalance,
      anniversaryWithdraw: element.anniversaryWithdraw,
      updateDateTime: element.updateDateTime,
      monthlyGrossIncome: element.monthlyGrossIncome,
      monthAniversaryWithdraw: element.monthAniversaryWithdraw,
    );
  }

  FGTS.fromJson(Map<String, dynamic> json)
      : updateDateTime = DateTime.tryParse(json['updateDateTime'].toString())!,
        currentBalance = json['currentBalance'].toDouble(),
        monthlyGrossIncome = json['monthlyGrossIncome'].toDouble(),
        monthAniversaryWithdraw = json['monthAniversaryWithdraw'],
        anniversaryWithdraw = json['anniversaryWithdraw'];
}
