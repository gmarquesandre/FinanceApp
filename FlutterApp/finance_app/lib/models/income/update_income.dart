class UpdateIncome {
  String id;
  String name;
  int recurrence;
  double amount;
  DateTime initialDate;
  DateTime? endDate;
  bool isEndless;
  int timesRecurrence;

  UpdateIncome(
      {required this.id,
      required this.name,
      required this.recurrence,
      required this.amount,
      required this.initialDate,
      required this.endDate,
      required this.isEndless,
      required this.timesRecurrence});

  Map<String, dynamic> toJson() => {
        'id': id,
        'name': name,
        'recurrence': recurrence,
        'amount': amount,
        'initialDate': initialDate.toString(),
        'endDate': endDate?.toString(),
        'isEndless': isEndless,
        'timesRecurrence': timesRecurrence,
      };
}
