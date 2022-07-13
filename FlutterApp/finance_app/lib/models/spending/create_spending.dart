class CreateSpending {
  String name;
  int recurrence;
  double amount;
  DateTime initialDate;
  DateTime? endDate;
  int? creditCardId;
  int? categoryId;
  bool isRequired;
  bool isEndless;
  int timesRecurrence;
  int payment;

  CreateSpending(
      {required this.name,
      required this.recurrence,
      required this.amount,
      required this.initialDate,
      required this.endDate,
      required this.payment,
      required this.creditCardId,
      required this.categoryId,
      required this.isRequired,
      required this.isEndless,
      required this.timesRecurrence});

  Map<String, dynamic> toJson() => {
        'name': name,
        'payment': payment,
        'recurrence': recurrence,
        'amount': amount,
        'initialDate': initialDate.toString(),
        'categoryId': categoryId,
        'isRequired': isRequired,
        'creditCardId': creditCardId,
        'endDate': endDate?.toString(),
        'isEndless': isEndless,
        'timesRecurrence': timesRecurrence,
      };
}
