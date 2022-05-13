import 'package:financial_app/models/models/fixedInterestMonthly.dart';
import 'package:flutter/material.dart';

class FixedInterestListProvider extends ChangeNotifier {
  final List<FixedInterestMonthly> list = [];

  getList() {
    return list;
  }

  clearList(){
    list.clear();

    notifyListeners();
  }

  refresh(List<FixedInterestMonthly> listAdd) {
    list.clear();

    listAdd.forEach((element) {
      list.add(element);
    });

    notifyListeners();
  }
}
