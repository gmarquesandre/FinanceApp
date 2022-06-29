import 'package:financial_app/models/models/assetCurrentPosition.dart';
import 'package:flutter/material.dart';

class AssetCurrentPositionList extends ChangeNotifier {
  final List<AssetCurrentPosition> assetCurrentPositionList = [];

  getList() {

    return assetCurrentPositionList;

  }

  clearList(){
    assetCurrentPositionList.clear();

    notifyListeners();
  }

  sortByQtd(bool ascending){
    if(ascending)
      assetCurrentPositionList.sort((a,b) => a.quantity.compareTo(b.quantity));
    else
      assetCurrentPositionList.sort((a,b) => b.quantity.compareTo(a.quantity));
    notifyListeners();
  }

  refresh(List<AssetCurrentPosition> list) {
    assetCurrentPositionList.clear();

    list.forEach((element) {
      assetCurrentPositionList.add(element);
    });

    notifyListeners();
  }
}
