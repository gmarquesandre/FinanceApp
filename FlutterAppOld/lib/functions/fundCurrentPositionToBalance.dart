import 'package:financial_app/database/dao/investmentFundCurrentValue_dao.dart';
import 'package:financial_app/functions/fundCurrentPosition.dart';
import 'package:financial_app/models/dto_models/fundToBalance.dart';
import 'package:financial_app/models/models/fundCurrentPosition.dart';
import 'package:financial_app/models/table_models/investmentFundCurrentValue.dart';
import 'package:flutter/cupertino.dart';

Future<List<FundToBalance>> getFundCurrentPositionToBalance(BuildContext
context) async {
  final InvestmentFundCurrentValueDao _daoInvestmentFundValue = InvestmentFundCurrentValueDao();

  List<FundCurrentPosition> listInvestmentFundPosition = await fundCurrentPosition(context);

  if (listInvestmentFundPosition.length == 0)
    return [];
  List<FundToBalance> listInvestmentFundToBalance = [];
  List<InvestmentFundCurrentValue> listCurrentValue = await _daoInvestmentFundValue.findAll();

  listInvestmentFundPosition.where(( element) => element.quantity != 0).forEach((element) {

    var currentValue = listCurrentValue.firstWhere((item) => item.cnpj ==
        element.cnpj);

    listInvestmentFundToBalance.add(FundToBalance(cnpj: element.cnpj,

        nameShort: element.name,
        unitPrice: currentValue.unitPrice,
        quantity: element.quantity,
        dateLastUpdate: DateTime.now() ));
  });

  return listInvestmentFundToBalance;
}