class ForecastParameters {
  String id;
  double percentageCdiLoan;
  double percentageCdiFixedInteresIncometSavings;
  double percentageCdiVariableIncome;
  double savingsLiquidPercentage;
  int monthsSavingWarning;

  ForecastParameters({
    required this.id,
    required this.percentageCdiLoan,
    required this.percentageCdiFixedInteresIncometSavings,
    required this.percentageCdiVariableIncome,
    required this.savingsLiquidPercentage,
    required this.monthsSavingWarning,
  });

  ForecastParameters.fromJson(Map<String, dynamic> json)
      : id = json['id'],
        percentageCdiLoan = json['percentageCdiLoan'],
        percentageCdiVariableIncome = json['percentageCdiVariableIncome'],
        savingsLiquidPercentage = json['savingsLiquidPercentage'],
        percentageCdiFixedInteresIncometSavings =
            json['percentageCdiFixedInteresIncometSavings'],
        monthsSavingWarning = json['monthsSavingWarning'];
}
