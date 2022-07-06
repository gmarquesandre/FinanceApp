class CreateIncome {
  String name;
  int recurrence;
  double amount;
  DateTime initialDate;
  DateTime? endDate;
  bool isEndless;
  int timesRecurrence;

  CreateIncome(
      {required this.name,
      required this.recurrence,
      required this.amount,
      required this.initialDate,
      required this.endDate,
      required this.isEndless,
      required this.timesRecurrence});

  Map<String, dynamic> toJson() => {
        'name': name,
        'recurrence': recurrence,
        'amount': amount,
        'initialDate': initialDate,
        'endDate': endDate,
        'isEndless': isEndless,
        'timesRecurrence': timesRecurrence,
      };
}
