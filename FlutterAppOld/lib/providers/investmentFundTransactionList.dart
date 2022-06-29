
import 'package:financial_app/models/models/fundTransaction.dart';
import 'package:flutter/material.dart';

class FundTransactionList extends ChangeNotifier {
  final List<FundTransaction> list = [];

  getList(String? cnpj) {
    return list.where((element) => element.fund.cnpj ==
        cnpj || cnpj == null).toList();

  }

  clearList(){
    list.clear();

    notifyListeners();
  }

  refresh(List<FundTransaction> listInsert) {
    list.clear();

    listInsert.forEach((element) {
      list.add(element);
    });

    notifyListeners();
  }
}
