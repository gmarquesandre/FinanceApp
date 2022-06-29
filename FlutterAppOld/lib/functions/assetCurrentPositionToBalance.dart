import 'package:financial_app/database/dao/assetCurrentValue_dao.dart';
import 'package:financial_app/functions/assetCurrentPosition.dart';
import 'package:financial_app/models/dto_models/assetToBalance.dart';
import 'package:financial_app/models/models/assetCurrentPosition.dart';
import 'package:financial_app/models/table_models/assetCurrentValue.dart';
import 'package:flutter/cupertino.dart';

Future<List<AssetToBalance>> getAssetCurrentPositionToBalance(BuildContext
context) async {
  final AssetCurrentValueDao _daoAssetValue = AssetCurrentValueDao();

  List<AssetCurrentPosition> listAssetPosition = await assetCurrentPosition
    (context);

  if (listAssetPosition.length == 0)
    return [];
  List<AssetToBalance> listAssetToBalance = [];
  List<AssetCurrentValue> listCurrentValue = await _daoAssetValue.findAll();

  listAssetPosition.where(( element) => element.quantity != 0).forEach((element) {

    var currentValue = listCurrentValue.firstWhere((item) => item.assetCode ==
        element.assetCode);

    listAssetToBalance.add(AssetToBalance(assetCode: element.assetCode,
        avgBuyPrice: element.avgUnitPrice,
        unitPrice: currentValue.unitPrice,
        quantity: element.quantity,
        dateLastUpdate: currentValue.dateLastUpdate));
  });

  return listAssetToBalance;
}