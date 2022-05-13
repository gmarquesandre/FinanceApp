import 'package:financial_app/models/table_models/holiday.dart';
import 'package:flutter/material.dart';

class HolidaysList extends ChangeNotifier{

  final List<Holiday> holidaysList = [];

  getList() {
    return holidaysList;
  }

  refreshList(List<Holiday> holidays) {

    holidaysList.clear();
    holidays.forEach((element) { holidaysList.add(element);});

    notifyListeners();

  }


}