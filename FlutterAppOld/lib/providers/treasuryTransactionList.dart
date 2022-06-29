
import 'package:financial_app/models/models/treasuryTransaction.dart';
import 'package:flutter/material.dart';

class TreasuryTransactionList extends ChangeNotifier {
  final List<TreasuryTransaction> treasuryTransactionList = [];

  getList(String? assetCode ) {
    return treasuryTransactionList.where((element) => element.treasury.treasuryBondName ==
        assetCode || assetCode == null).toList();
  }

  clearList(){
    treasuryTransactionList.clear();

    notifyListeners();
  }

  refresh(List<TreasuryTransaction> list) {
    treasuryTransactionList.clear();

    list.forEach((element) {
      treasuryTransactionList.add(element);
    });

    notifyListeners();
  }
}
