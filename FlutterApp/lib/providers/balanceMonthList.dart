import 'package:financial_app/models/dto_models/balanceMonth.dart';
import 'package:flutter/material.dart';

class BalanceMonthList extends ChangeNotifier {
  final List<BalanceMonth> balanceMonthList = [];

  getList() {
    return balanceMonthList;
  }

  clearBalance(){
    balanceMonthList.clear();

    notifyListeners();
  }

  refreshBalance(List<BalanceMonth> list) {
    balanceMonthList.clear();
    list.forEach((element) {
      balanceMonthList.add(element);
    });

    notifyListeners();
  }
}
