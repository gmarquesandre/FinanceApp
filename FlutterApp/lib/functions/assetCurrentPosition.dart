import 'package:financial_app/database/dao/assetCurrentValue_dao.dart';
import 'package:financial_app/database/dao/asset_dao.dart';
import 'package:financial_app/database/dto/asset_dto.dart';
import 'package:financial_app/functions/updateData.dart';
import 'package:financial_app/models/models/assetCurrentPosition.dart';
import 'package:financial_app/models/models/assetTransaction.dart';
import 'package:financial_app/models/table_models/asset.dart';
import 'package:financial_app/models/table_models/assetCurrentValue.dart';
import 'package:financial_app/providers/assetCurrentPositionList.dart';
import 'package:financial_app/providers/assetTransactionList.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';


Future<List<AssetCurrentPosition>> assetCurrentPosition(
    BuildContext context) async {

  try {
    await updateAsset();
  }
  catch(e){

  }

  final AssetDao _dao = AssetDao();
  final AssetCurrentValueDao _daoCurrentValue = AssetCurrentValueDao();

  final AssetDto _dto = AssetDto();

  List<AssetObject> listAssetDto = await _dto.findAll();

  List<AssetCurrentValue> currentValue = await _daoCurrentValue.findAll();
  List<Asset> list = await _dao.findAll();

  if (list.length == 0) {

    Provider.of<AssetTransactionList>(context, listen: false).refresh(
        []);

    Provider.of<AssetCurrentPositionList>(context, listen: false).refresh(
        []);
    return [];
  }
  List<String> assets = list.map((e) => e.assetCode).toSet().toList();

  //Ordena pelo id e data para manter o resultado igual
  list.sort((a, b) => a.id!.compareTo(b.id!));
  list.sort((a, b) =>
      a.date.millisecondsSinceEpoch.compareTo(b.date
          .millisecondsSinceEpoch));

  double quantity = 0;
  double avgPrice = 0;

  List<AssetCurrentPosition> listCurrentPosition = [];
  List<AssetTransaction> transactionalList = [];

  //Ajustar isso aqui, só fiz pra teste

  assets.forEach((element) {
    quantity = 0;
    avgPrice = 0;


    listAssetDto.where((item) => item.assetCode == element).forEach((element) {
      //Cria lista transacional
      double exValue = 0;

      double check = quantity + element.quantity;
      //Se zera a quantidade, será necessario pular
      if(check > 0){

      //Compra
        if (element.operation == 1) {

        avgPrice =
            (element.value * element.quantity + quantity * avgPrice) /
                (element.quantity + quantity);
        quantity += element.quantity;


      }
      //VENDA
      else if(element.operation == 2){
        quantity -= element.quantity;
        avgPrice = quantity <= 0 ? 0 : avgPrice;
      }
      //RENDIMENTO
      else if(element.operation == 3 || element.operation == 4){
        exValue = element.value*0.85;
        avgPrice = avgPrice - element.value*0.85;
      }
      //Div e amortização
      else if(element.operation == 5 || element.operation == 6){
        exValue = element.value;
        avgPrice = avgPrice - element.value;
      }
      //Bonificação
      else if(element.operation == 7){
        avgPrice = avgPrice/(1+element.value);
        quantity = quantity*(1+element.value);
      }
      //Desdobramento e agrupamento
      else if(element.operation == 9 || element.operation == 10 ){
        avgPrice = avgPrice/(1+element.value);
        quantity = quantity*(1+element.value);
      }

      transactionalList.add(AssetTransaction(
          quantityCumulated: quantity.round(),
          operation: element.operationName,
          value: element.value,
          exValue: exValue,
          avgUnitPrice: avgPrice,
          asset: Asset(
              id: element.id,
              assetCode: element.assetCode,
              quantity: element.quantity,
              unitPrice: element.value,
              operation: element.operation,
              date: element.date))


      );
    }
    });

  });

  //Ordena do maior pro menor
  transactionalList.sort((a, b) => b.asset.id!.compareTo(a.asset.id!));

  transactionalList.sort((a, b) =>
      b.asset.date.millisecondsSinceEpoch.compareTo(a.asset.date
          .millisecondsSinceEpoch));


  assets.forEach((element) {
    var thisItem = transactionalList.firstWhere((item) =>
    item.asset.assetCode ==
        element);

    listCurrentPosition.add(AssetCurrentPosition(assetCode: element,
        quantity: thisItem.quantityCumulated,
        avgUnitPrice: thisItem.avgUnitPrice,
        currentPrice: currentValue.firstWhere((item) => item.assetCode == thisItem.asset.assetCode).unitPrice
    ));
  });

  Provider.of<AssetTransactionList>(context, listen: false).refresh(
      transactionalList);

  Provider.of<AssetCurrentPositionList>(context, listen: false).refresh(
      listCurrentPosition);

  return listCurrentPosition;
}


