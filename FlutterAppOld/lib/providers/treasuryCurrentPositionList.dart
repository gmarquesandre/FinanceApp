
import 'package:financial_app/models/models/treasuryCurrentPosition.dart';
import 'package:flutter/material.dart';

class TreasuryCurrentPositionList extends ChangeNotifier {
  final List<TreasuryCurrentPosition> treasuryCurrentPositionList = [];

  getList() {
    return treasuryCurrentPositionList;
  }

  clearList(){
    treasuryCurrentPositionList.clear();

    notifyListeners();
  }

  sortByQtd(bool ascending){
    if(ascending)
      treasuryCurrentPositionList.sort((a,b) => a.quantity.compareTo(b.quantity));
    else
      treasuryCurrentPositionList.sort((a,b) => b.quantity.compareTo(a.quantity));
    notifyListeners();
  }

  refresh(List<TreasuryCurrentPosition> list) {
    treasuryCurrentPositionList.clear();

    list.forEach((element) {
      treasuryCurrentPositionList.add(element);
    });

    notifyListeners();
  }
}
