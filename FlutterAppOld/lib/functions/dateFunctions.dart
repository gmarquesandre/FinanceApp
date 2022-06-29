import 'package:financial_app/database/dao/holidays_dao.dart';

final HolidaysDao _daoHolidays = HolidaysDao();

bool predicate(DateTime day) {

  if([6,7].contains(day.weekday)){
    return false;
  }
  return true;
}


Future<DateTime> getLastWorkingDay() async{

  DateTime date = DateTime(DateTime.now().year,DateTime.now().month, DateTime.now().day) ;

  while(await isHoliday(date) || [6,7].contains(date.weekday)){
      date = date.subtract(const Duration(days: 1));
  }

  return date;
}


Future<bool> isHoliday(DateTime date)
 async {

  bool isHoliday = await _daoHolidays.findDate(DateTime(date.year, date.month, date.day));

  return isHoliday;
}

int returnWorkingDays(DateTime initialDate, DateTime finalDate,
    List<DateTime> holidays) {

  // weekday
  // 1 - Monday
  // 2 - Tuesday
  // 3 - Wednesday
  // 4 - Thursday
  // 5 - Friday
  // 6 - Saturday
  // 7 - Sunday


  int workingDays = 0;
  for(DateTime i = initialDate; i.compareTo(DateTime(finalDate.year, finalDate.month, finalDate.day)) < 0; i = DateTime(i
      .year, i.month, i.day +1))
  {
      if(![6,7].contains(i.weekday) && !holidays.contains(i))
      {
        workingDays++;
      }
  }

  if(holidays.contains(DateTime(finalDate.year, finalDate.month, finalDate.day)))
    workingDays--;

  return workingDays;
}

DateTime getInitialDatePicker(){
  return [6,7].contains(DateTime.now().weekday) ?
  DateTime.now().subtract(Duration(days: DateTime.now().weekday-5)) :
  DateTime
      .now();

}

