import 'package:financial_app/database/dao/treasury_dao.dart';
import 'package:financial_app/functions/updateData.dart';
import 'package:financial_app/models/models/treasuryCurrentPosition.dart';
import 'package:financial_app/models/models/treasuryTransaction.dart';
import 'package:financial_app/models/table_models/treasury.dart';
import 'package:financial_app/providers/treasuryCurrentPositionList.dart';
import 'package:financial_app/providers/treasuryTransactionList.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

Future<List<TreasuryTransaction>> treasuryCurrentPosition(
    BuildContext context) async {

  try{
    await updateTreasuryBond();
  }
  catch(e){

  }

  final TreasuryDao _dao = TreasuryDao();

  List<Treasury> list = await _dao.findAll();



  if (list.length == 0) {
    Provider.of<TreasuryTransactionList>(context, listen: false).refresh([]);

    Provider.of<TreasuryCurrentPositionList>(context, listen: false).refresh([]);
    return [];
  }



  List<String> names = list.map((e) => e.treasuryBondName).toSet().toList();

  //Ordena pelo id e data para manter o resultado igual
  list.sort((a, b) => a.id!.compareTo(b.id!));
  list.sort((a, b) =>
      a.date.millisecondsSinceEpoch.compareTo(b.date.millisecondsSinceEpoch));

  double quantity = 0;
  double avgPrice = 0;

  List<TreasuryCurrentPosition> listCurrentPosition = [];
  List<TreasuryTransaction> transactionalList = [];
  names.forEach(
    (element) {
      quantity = 0;
      avgPrice = 0;

      list.where((item) => item.treasuryBondName == element).forEach(
        (element) {
          //Cria lista transacional

          //Compra
          if (element.operation == 1) {
            avgPrice =
                (element.unitPrice * element.quantity + quantity * avgPrice) /
                    (element.quantity + quantity);
            quantity += element.quantity;


          } else {

            quantity -= element.quantity;
            avgPrice = quantity <= 0 ? 0 : avgPrice;

          }
          transactionalList.add(
            TreasuryTransaction(
                quantityCumulated: double.tryParse(quantity.toStringAsFixed
                  (2))!,
                avgUnitPrice: avgPrice,
                operation: element.operation == 1? 'C' : 'V',
                remainingQuantity:
                    element.operation == 1 ? element.quantity : 0,
                treasury: element),
          );
        },
      );
    },
  );



  transactionalList.where((a) => a.treasury.operation == 1).forEach((element) {
    //   //Todas compras anteriores  e totas vendas futuras
    double totalBuyBefore = 0;
    double totalSellAfter = 0;

    var listBuyBefore = transactionalList
        .where((item) =>
            item.treasury.date.compareTo(element.treasury.date) <= 0 &&
            item.treasury.id! < element.treasury.id! &&
            item.treasury.operation == 1)
        .map((e) => e.treasury.quantity)
        .toList();

    var listSellAfter = transactionalList
        .where((item) =>
    ((item.treasury.date.compareTo(element.treasury.date) == 0 &&
            item.treasury.id! > element.treasury.id!)
        || item.treasury.date.compareTo(element.treasury.date) > 0) &&
            item.treasury.operation == 2)
        .map((e) => e.treasury.quantity)
        .toList();

    totalSellAfter =
        listSellAfter.length > 0 ? listSellAfter.reduce((a, b) => a + b) : 0;

    totalBuyBefore =
        listBuyBefore.length > 0 ? listBuyBefore.reduce((a, b) => a + b) : 0;

    element.remainingQuantity = totalBuyBefore - totalSellAfter < 0?
    totalBuyBefore - totalSellAfter + element.treasury.quantity < 0? 0 : totalBuyBefore - totalSellAfter + element.treasury.quantity:
    element
        .treasury.quantity;

    element.remainingQuantity = double.tryParse(element.remainingQuantity.toStringAsFixed(2))!;


  });

  //Ordena do maior pro menor
  transactionalList.sort((a, b) => b.treasury.id!.compareTo(a.treasury.id!));

  transactionalList.sort((a, b) => b.treasury.date.millisecondsSinceEpoch
      .compareTo(a.treasury.date.millisecondsSinceEpoch));


  names.forEach((element) {
    var thisItem = transactionalList
        .firstWhere((item) => item.treasury.treasuryBondName == element);

    listCurrentPosition.add(TreasuryCurrentPosition(
        treasuryName: element,
        quantity: thisItem.quantityCumulated,
        avgUnitPrice: thisItem.avgUnitPrice));
  });


  Provider.of<TreasuryTransactionList>(context, listen: false)
      .refresh(transactionalList);

  Provider.of<TreasuryCurrentPositionList>(context, listen: false)
      .refresh(listCurrentPosition);

  return transactionalList;
}
