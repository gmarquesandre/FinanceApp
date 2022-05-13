
class Spending {
  int? id;
  String name;
  double spendingValue;
  DateTime initialDate;
  DateTime endDate;
  int? recurrenceId;
  int categoryId;
  int timesRecurrence;
  int isEndless;
  int isRequiredSpending;


  Spending({
    this.id,
    required this.name,
    required this.spendingValue,
    required this.initialDate,
    required this.endDate,
    required this.recurrenceId,
    required this.categoryId,
    required this.timesRecurrence,
    required this.isEndless,
    required this.isRequiredSpending,
  });

  Map<String, dynamic> toMap() {
    final spendingMap = Map<String, dynamic>();
    spendingMap['name'] = name;
    spendingMap['spendingValue'] = spendingValue;
    spendingMap['initialDate'] = initialDate.millisecondsSinceEpoch;
    spendingMap['endDate'] = endDate.millisecondsSinceEpoch;
    spendingMap['recurrenceId'] = recurrenceId;
    spendingMap['categoryId'] = categoryId;
    spendingMap['timesRecurrence'] = timesRecurrence;
    spendingMap['isEndless']= isEndless;
    spendingMap['isRequiredSpending']= isRequiredSpending;
    return spendingMap;
  }

  factory Spending.fromMap(Map<String, dynamic> row) {
    return Spending(
      id: row['id'],
      name: row['name'],
      spendingValue: row['spendingValue'],
      initialDate: DateTime.fromMillisecondsSinceEpoch(row['initialDate']),
      endDate: DateTime.fromMillisecondsSinceEpoch(row['endDate']),
      recurrenceId: row['recurrenceId'],
      categoryId: row['categoryId'],
      timesRecurrence: row['timesRecurrence'],
      isEndless: row['isEndless'],
      isRequiredSpending: row['isRequiredSpending'],
    );
  }
}
