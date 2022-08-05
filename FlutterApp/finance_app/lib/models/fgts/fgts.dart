class FGTS {
  int id;
  double monthlyGrossIncome;
  double currentValue;
  bool anniversaryWithdraw;
  int monthAniversaryWithdraw;
  DateTime updateDateTime;

  FGTS({
    required this.id,
    required this.currentValue,
    required this.updateDateTime,
    required this.anniversaryWithdraw,
    required this.monthAniversaryWithdraw,
    required this.monthlyGrossIncome,
  });

  factory FGTS.copyWith(FGTS element) {
    return FGTS(
      id: element.id,
      currentValue: element.currentValue,
      anniversaryWithdraw: element.anniversaryWithdraw,
      updateDateTime: element.updateDateTime,
      monthlyGrossIncome: element.monthlyGrossIncome,
      monthAniversaryWithdraw: element.monthAniversaryWithdraw,
    );
  }

  FGTS.fromJson(Map<String, dynamic> json)
      : id = json['id'],
        updateDateTime = DateTime.tryParse(json['updateDateTime'].toString())!,
        currentValue = json['currentValue'].toDouble(),
        monthlyGrossIncome = json['monthlyGrossIncome'].toDouble(),
        monthAniversaryWithdraw = json['monthAniversaryWithdraw'],
        anniversaryWithdraw = json['anniversaryWithdraw'];
}
