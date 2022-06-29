class Income {
  int? id;
  String name;
  double incomeValue;
  DateTime initialDate;
  DateTime endDate;
  int timesRecurrence;
  int? recurrenceId;
  int isEndless;

  Income({
    this.id,
    required this.name,
    required this.incomeValue,
    required this.initialDate,
    required this.endDate,
    required this.recurrenceId,
    required this.timesRecurrence,
    required this.isEndless,
  });

  Map<String, dynamic> toMap() {
    final incomeMap = Map<String,dynamic>();
    if(id != null) {
      incomeMap['id'] = id;
    }
    incomeMap['name'] = name;
    incomeMap['incomeValue'] = incomeValue;
    incomeMap['initialDate'] = initialDate.millisecondsSinceEpoch;
    incomeMap['endDate'] = endDate.millisecondsSinceEpoch;
    incomeMap['recurrenceId'] = recurrenceId;
    incomeMap['timesRecurrence'] = timesRecurrence;
    incomeMap['isEndless'] = isEndless;

    return incomeMap;
  }
  factory Income.fromMap(Map<String, dynamic> row){
    return Income(
      id: row['id'],
      name: row['name'],
      incomeValue: row['incomeValue'],
      initialDate: DateTime.fromMillisecondsSinceEpoch(row['initialDate']),
      endDate: DateTime.fromMillisecondsSinceEpoch(row['endDate']),
      recurrenceId: row['recurrenceId'],
      timesRecurrence: row['timesRecurrence'],
      isEndless: row['isEndless']
      );
  }
}
