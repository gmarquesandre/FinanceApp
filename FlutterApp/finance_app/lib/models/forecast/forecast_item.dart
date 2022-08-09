class ForecastItem {
  DateTime dateReference;
  double nominalLiquidValue;
  double realLiquidValue;
  double nominalCumulatedAmount;
  double realCumulatedAmount;

  ForecastItem(
      {required this.dateReference,
      required this.nominalCumulatedAmount,
      required this.nominalLiquidValue,
      required this.realCumulatedAmount,
      required this.realLiquidValue});

  ForecastItem.fromJson(Map<String, dynamic> json)
      : dateReference = DateTime.parse(json['dateReference']),
        nominalCumulatedAmount = json['nominalCumulatedAmount'].toDouble(),
        nominalLiquidValue = json['nominalLiquidValue'].toDouble(),
        realLiquidValue = json['realLiquidValue'].toDouble(),
        realCumulatedAmount = json['realCumulatedAmount'].toDouble();
}
