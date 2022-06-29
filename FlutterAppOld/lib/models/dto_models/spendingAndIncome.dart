class SpendingAndIncome {

  DateTime? date;
  DateTime initialDate;
  DateTime endDate;
  String name;
  int isSpending;
  double amount;
  int recurrenceId;
  int isEndless;
  int timesRecurrence;
  int isRequiredSpending;


  SpendingAndIncome({
    this.date,
    required this.initialDate,
    required this.endDate,
    required this.name,
    required this.isSpending,
    required this.amount,
    required this.recurrenceId,
    required this.isEndless,
    required this.timesRecurrence,
    required this.isRequiredSpending,
  });

  factory SpendingAndIncome.fromMap(Map<String, dynamic> map) {
    return SpendingAndIncome(
      endDate: DateTime.fromMillisecondsSinceEpoch(map['endDate']),
      initialDate: DateTime.fromMillisecondsSinceEpoch(map['initialDate']),
      name: map['name'],
      isSpending: map['isSpending'],
      amount: map['amount'],
      recurrenceId: map['recurrenceId'],
      isEndless: map['isEndless'],
      timesRecurrence: map['timesRecurrence'],
      isRequiredSpending: map['isRequiredSpending'],
    );
  }

  factory SpendingAndIncome.copyWith(SpendingAndIncome element) {
    return SpendingAndIncome(
      date: element.date,
      endDate: element.endDate,
      initialDate: element.initialDate,
      name: element.name,
      isSpending: element.isSpending,
      amount: element.amount,
      recurrenceId: element.recurrenceId,
      timesRecurrence: element.timesRecurrence,
      isEndless: element.isEndless,
      isRequiredSpending: element.isRequiredSpending
    );
  }

}