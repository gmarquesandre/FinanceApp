class NationalHoliday {

  DateTime date;

  NationalHoliday({
    required this.date,
  });


  Map<String, dynamic> toMap() {
    final nationalHolidayMap = Map<String, dynamic>();

    nationalHolidayMap['date'] = date.millisecondsSinceEpoch;

    return nationalHolidayMap;
  }

  factory NationalHoliday.fromMap(Map<String, dynamic> row){
    return NationalHoliday(
      date: DateTime.fromMillisecondsSinceEpoch(row['date']),
    );
  }
}