class UpdateSpending {
  String id;
  String name;
  int recurrence;
  String? creditCardId;
  double amount;
  int payment;
  bool isRequired;
  String? categoryId;
  DateTime initialDate;
  DateTime? endDate;
  bool isEndless;
  int timesRecurrence;

  UpdateSpending(
      {required this.id,
      required this.name,
      required this.creditCardId,
      required this.recurrence,
      required this.payment,
      required this.amount,
      required this.isRequired,
      required this.categoryId,
      required this.initialDate,
      required this.endDate,
      required this.isEndless,
      required this.timesRecurrence});

  Map<String, dynamic> toJson() => {
        'id': id,
        'name': name,
        'recurrence': recurrence,
        'amount': amount,
        'payment': payment,
        'creditCardId': creditCardId,
        'isRequired': isRequired,
        'categoryId': categoryId,
        'initialDate': initialDate.toString(),
        'endDate': endDate?.toString(),
        'isEndless': isEndless,
        'timesRecurrence': timesRecurrence,
      };
}
