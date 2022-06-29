import 'package:financial_app/models/models/fundCurrentPosition.dart';
import 'package:flutter/material.dart';

class FundCurrentPositionList extends ChangeNotifier {
  final List<FundCurrentPosition> list = [];

  getList() {
    return list;
  }

  clearList(){
    list.clear();

    notifyListeners();
  }

  sortByQtd(bool ascending){
    if(ascending)
      list.sort((a,b) => a.quantity.compareTo(b.quantity));
    else
      list.sort((a,b) => b.quantity.compareTo(a.quantity));
    notifyListeners();
  }

  refresh(List<FundCurrentPosition> listInsert) {
    list.clear();

    listInsert.forEach((element) {
      list.add(element);
    });

    notifyListeners();
  }
}
