class Income {
  String name;
  int recurrence;
  String recurrenceDisplayValue;
  double amount;
  DateTime initialDate;
  DateTime? endDate;
  bool isEndless;
  int timesRecurrence;

  Income(
      {required this.name,
      required this.recurrence,
      required this.recurrenceDisplayValue,
      required this.amount,
      required this.initialDate,
      required this.endDate,
      required this.isEndless,
      required this.timesRecurrence});

  factory Income.copyWith(Income element) {
    return Income(
        name: element.name,
        recurrence: element.recurrence,
        recurrenceDisplayValue: element.recurrenceDisplayValue,
        amount: element.amount,
        initialDate: element.initialDate,
        endDate: element.endDate,
        isEndless: element.isEndless,
        timesRecurrence: element.timesRecurrence);
  }

  Income.fromJson(Map<String, dynamic> json)
      : name = json['name'].toString(),
        recurrence = json['recurrence'],
        recurrenceDisplayValue = json['recurrenceDisplayValue'].toString(),
        amount = json['amount'].toDouble(),
        initialDate = DateTime.tryParse(json['initialDate'].toString())!,
        endDate = DateTime.tryParse(json['endDate'].toString()),
        isEndless = json['isEndless'] == 1,
        timesRecurrence = json['timesRecurrence'];
}
