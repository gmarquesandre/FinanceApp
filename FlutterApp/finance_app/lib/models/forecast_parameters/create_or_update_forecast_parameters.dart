class CreateOrUpdateForecastParameters {
  String id;
  double percentageCdiLoan;
  double percentageCdiFixedInteresIncometSavings;
  double percentageCdiVariableIncome;
  double savingsLiquidPercentage;
  int monthsSavingWarning;

  CreateOrUpdateForecastParameters({
    required this.id,
    required this.percentageCdiLoan,
    required this.percentageCdiFixedInteresIncometSavings,
    required this.percentageCdiVariableIncome,
    required this.savingsLiquidPercentage,
    required this.monthsSavingWarning,
  });

  Map<String, dynamic> toJson() => {
        'id': id,
        'percentageCdi': percentageCdiLoan,
        'percentageCdiFixedInteresIncometSavings':
            percentageCdiFixedInteresIncometSavings,
        'percentageCdiVariableIncome': percentageCdiVariableIncome,
        'savingsLiquidPercentage': savingsLiquidPercentage,
        'monthsSavingWarning': monthsSavingWarning
      };
}
