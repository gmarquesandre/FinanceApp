class ForecastItem {
  DateTime dateReference;
  double amount;
  double cumulatedAmount;

  ForecastItem(
      {required this.dateReference,
      required this.amount,
      required this.cumulatedAmount});

  ForecastItem.fromJson(Map<String, dynamic> json)
      : dateReference = DateTime.parse(json['dateReference']),
        amount = json['amount'].toDouble(),
        cumulatedAmount = json['cumulatedAmount'].toDouble();
}
