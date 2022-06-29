import 'package:financial_app/database/dao/treasuryCurrentValue_dao.dart';
import 'package:financial_app/functions/treasuryCurrentPosition.dart';
import 'package:financial_app/models/dto_models/treasuryToBalance.dart';
import 'package:financial_app/models/models/treasuryTransaction.dart';
import 'package:financial_app/models/table_models/treasuryCurrentValue.dart';
import 'package:flutter/material.dart';

Future<List<TreasuryToBalance>> getTreasuryCurrentPositionToBalance
    (BuildContext context) async {
  final TreasuryCurrentValueDao _dao = TreasuryCurrentValueDao();

  List<TreasuryTransaction> listTreasuryTransaction = await
  treasuryCurrentPosition
    (context);



  if (listTreasuryTransaction.length == 0)
    return [];
  List<TreasuryToBalance> listTreasuryToBalance = [];
  List<TreasuryCurrentValue> listCurrentValue = await _dao.findAll();

  listTreasuryTransaction.where((element) => element.remainingQuantity > 0 && element.operation == 'C').forEach((
      element) {
    var currentValue = listCurrentValue.firstWhere((item) =>
    item.treasuryBondName ==
        element.treasury.treasuryBondName);

    listTreasuryToBalance.add(TreasuryToBalance(id: element.treasury.id!,
        treasuryBondName: element.treasury.treasuryBondName,
        quantity: element.remainingQuantity,
        dateLastUpdate: currentValue.dateLastUpdate,
        unitPriceBuy: currentValue.unitPriceBuy,
        unitPricePurchase: element.treasury.unitPrice,
        unitPriceSell: currentValue.unitPriceSell,
        expirationDate: currentValue.expirationDate,
        indexName: currentValue.indexName,
        investmentDate: element.treasury.date,
        fixedInterestValueBuy: currentValue.fixedInterestValueBuy,
        fixedInterestValueSell: currentValue.fixedInterestValueSell));
  });

  return listTreasuryToBalance;
}