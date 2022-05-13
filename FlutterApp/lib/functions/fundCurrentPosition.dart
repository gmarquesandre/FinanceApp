import 'package:financial_app/database/dao/investmentFund_dao.dart';
import 'package:financial_app/functions/updateData.dart';
import 'package:financial_app/models/models/fundCurrentPosition.dart';
import 'package:financial_app/models/models/fundTransaction.dart';
import 'package:financial_app/models/table_models/investmentFund.dart';
import 'package:financial_app/providers/investmentFundCurrentPositionList.dart';
import 'package:financial_app/providers/investmentFundTransactionList.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

Future<List<FundCurrentPosition>> fundCurrentPosition(
    BuildContext context) async{

  try{
    await updateFunds();
  }
  catch(e){

  }


  final InvestmentFundDao _dao = InvestmentFundDao();



  List<InvestmentFund> list = await _dao.findAll();

  if (list.length == 0) {
    Provider.of<FundTransactionList>(context, listen: false).refresh([]);

    Provider.of<FundCurrentPositionList>(context, listen: false).refresh([]);
    return [];
  }
  List<String> names = list.map((e) => e.cnpj).toSet().toList();

  //Ordena pelo id e data para manter o resultado igual
  list.sort((a, b) => a.id!.compareTo(b.id!));
  list.sort((a, b) =>
      a.date.millisecondsSinceEpoch.compareTo(b.date.millisecondsSinceEpoch));

  double quantity = 0;
  double avgPrice = 0;

  List<FundCurrentPosition> currentPositionList = [];
  List<FundTransaction> transactionalList = [];
  names.forEach(
    (element) {
      quantity = 0;
      avgPrice = 0;

      list.where((item) => item.cnpj == element).forEach(
        (element) {
          //Cria lista transacional




          //Compra
          if (element.operation == 1) {
            avgPrice =
                (element.unitPrice * element.quantity! + quantity * avgPrice) /
                    (element.quantity! + quantity);

            quantity += element.quantity!;


          } else {

            quantity -= element.quantity!;
            avgPrice = quantity <= 0 ? 0 : avgPrice;

          }

          transactionalList.add(
            FundTransaction(
                quantityCumulated: double.tryParse(quantity.toStringAsFixed
                  (2))!,
                avgUnitPrice: avgPrice,
                operation: element.operation == 1? 'C' : 'V',
                remainingQuantity: element.operation == 1 ? element.quantity!
                    : 0,
                fund: element),
          );
        },
      );
    },
  );



  transactionalList.where((a) => a.fund.operation == 1).forEach((element) {
    //   //Todas compras anteriores  e totas vendas futuras
    double totalBuyBefore = 0;
    double totalSellAfter = 0;

    var listBuyBefore = transactionalList
        .where((item) =>
            item.fund.date.compareTo(element.fund.date) <= 0 &&
            item.fund.id! < element.fund.id! &&
            item.fund.operation == 1)
        .map((e) => e.fund.quantity)
        .toList();

    var listSellAfter = transactionalList
        .where((item) =>
    ((item.fund.date.compareTo(element.fund.date) == 0 &&
            item.fund.id! > element.fund.id!)
        || item.fund.date.compareTo(element.fund.date) > 0) &&
            item.fund.operation == 2)
        .map((e) => e.fund.quantity)
        .toList();

    totalSellAfter =
        (listSellAfter.length > 0 ? listSellAfter.reduce((a, b) => a! + b!) :
        0)!;

    totalBuyBefore =
        (listBuyBefore.length > 0 ? listBuyBefore.reduce((a, b) => a! + b!) : 0)!;

    element.remainingQuantity = totalBuyBefore - totalSellAfter < 0?
    totalBuyBefore - totalSellAfter + element.fund.quantity! < 0? 0 :
    totalBuyBefore - totalSellAfter + element.fund.quantity!:
    element
        .fund.quantity!;

    element.remainingQuantity = double.tryParse(element.remainingQuantity.toStringAsFixed(2))!;

  });

  //Ordena do maior pro menor
  transactionalList.sort((a, b) => b.fund.id!.compareTo(a.fund.id!));

  transactionalList.sort((a, b) => b.fund.date.millisecondsSinceEpoch
      .compareTo(a.fund.date.millisecondsSinceEpoch));


  names.forEach((element) {
    var thisItem = transactionalList
        .firstWhere((item) => item.fund.cnpj == element);

    currentPositionList.add(FundCurrentPosition(
        quantity: thisItem.quantityCumulated,
        avgUnitPrice: thisItem.avgUnitPrice,
        name: thisItem.fund.name,
        cnpj:thisItem.fund.cnpj));
  });


  Provider.of<FundTransactionList>(context, listen: false)
      .refresh(transactionalList);

  Provider.of<FundCurrentPositionList>(context, listen: false)
      .refresh(currentPositionList);

  return currentPositionList;
}
