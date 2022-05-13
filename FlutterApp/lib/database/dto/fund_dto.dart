

import 'package:financial_app/database/dao/investmentFundCurrentValue_dao.dart';
import 'package:financial_app/database/dao/investmentFund_dao.dart';
import 'package:financial_app/database/getDatabase.dart';
import 'package:financial_app/models/dto_models/fundToBalance.dart';
import 'package:sqflite/sqflite.dart';

class FundToBalanceDto{

  String query = '''
      SELECT t1.${InvestmentFundDao.cnpj},t1.${InvestmentFundDao.quantity}, t2
      .${InvestmentFundCurrentValueDao.unitPrice}, t2.${InvestmentFundCurrentValueDao.dateLastUpdate}
      FROM 
      ${InvestmentFundDao.tableName} as t1
      left join ${InvestmentFundCurrentValueDao.tableName} as t2
      on t1.${InvestmentFundDao.cnpj} = t2.${InvestmentFundCurrentValueDao.cnpj}
  ''';

  Future<List<FundToBalance>> findAll() async {
    final Database db = await getDatabase();
    final List<Map<String, dynamic>> result = await db.rawQuery(query);
    final List<FundToBalance> assets = [];

    if(result.length == 0){
      return [];
    }

    if(assets.length == 0)
        return [];
    result.forEach((element) {
      assets.add(FundToBalance.fromMap(element));
    });
    return assets;
  }
}