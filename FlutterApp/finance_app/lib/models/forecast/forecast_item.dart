class ForecastItem {
  DateTime dateReference;
  double nominalAmount;
  double realAmount;
  double cumulatedAmount;

  ForecastItem(
      {required this.dateReference,
      required this.realAmount,
      required this.nominalAmount,
      required this.cumulatedAmount});

  ForecastItem.fromJson(Map<String, dynamic> json)
      : dateReference = DateTime.parse(json['dateReference']),
        nominalAmount = json['nominalAmount'].toDouble(),
        realAmount = json['realAmount'].toDouble(),
        cumulatedAmount = json['cumulatedAmount'].toDouble();
}
