class WorkingDaysByYear{

  int year;
  int workingDays;
  DateTime? dateLastUpdate;

  WorkingDaysByYear({
    required this.workingDays,
    required this.year,
    this.dateLastUpdate
  });


  Map<String, dynamic> toMap() {
    final map = Map<String,dynamic>();
    map['year'] = year;
    map['workingDays'] = workingDays;
    map['dateLastUpdate'] = dateLastUpdate!.millisecondsSinceEpoch;
    return map;
  }

  factory WorkingDaysByYear.fromMap(Map<String, dynamic> row) {
    return WorkingDaysByYear(
        year: row['year'],
        workingDays: row['workingDays'],
        dateLastUpdate: DateTime.fromMillisecondsSinceEpoch
          (row['dateLastUpdate']!),
    );
  }

  WorkingDaysByYear.fromJson(Map<String, dynamic> json) :
        year = json['year'],
        dateLastUpdate = DateTime.tryParse(json['dateLastUpdate'].toString()),
        workingDays = int.tryParse(json['workingDays'].toString())!;


  Map<String, dynamic> toJson() =>
      {
        'year' : year,
        'dateLastUpdate': dateLastUpdate,
        'workingDays': workingDays
      };





}