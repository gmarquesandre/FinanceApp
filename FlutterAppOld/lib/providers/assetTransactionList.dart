
import 'package:financial_app/models/models/assetTransaction.dart';
import 'package:flutter/material.dart';

class AssetTransactionList extends ChangeNotifier {
  final List<AssetTransaction> assetTransactionList = [];


  init(){

  }

  getList(String? assetCode ) {
    return assetTransactionList.where((element) => element.asset.assetCode ==
        assetCode || assetCode == null).toList();
  }

  clearList(){
    assetTransactionList.clear();

    notifyListeners();
  }

  refresh(List<AssetTransaction> list) {
    assetTransactionList.clear();

    list.forEach((element) {
      assetTransactionList.add(element);
    });

    notifyListeners();
  }
}
