import 'package:financial_app/database/dao/income_dao.dart';
import 'package:financial_app/database/dao/spending_dao.dart';
import 'package:financial_app/database/getDatabase.dart';
import 'package:financial_app/models/dto_models/spendingAndIncome.dart';
import 'package:sqflite/sqflite.dart';

class SpendingAndIncomeDto{
  String query = '''
      SELECT 
        ${SpendingDao.name}, 
        ${SpendingDao.spendingValue} as amount, 
        1 as isSpending, 
        ${SpendingDao.initialDate}, 
        ${SpendingDao.endDate}, 
        ${SpendingDao.isRequiredSpending}, 
        ${SpendingDao.recurrenceId}, 
        ${SpendingDao.isEndless}, 
        ${SpendingDao.timesRecurrence} 
      FROM ${SpendingDao.tableName}
      UNION ALL
      SELECT 
        ${IncomeDao.name}, 
        ${IncomeDao.incomeValue} as amount, 
        0 as isSpending, 
        ${IncomeDao.initialDate},
        ${IncomeDao.endDate}, 
        0 as isRequiredSpending, 
        ${IncomeDao.recurrenceId}, 
        ${IncomeDao.isEndless}, 
        ${IncomeDao.timesRecurrence} 
      FROM ${IncomeDao.tableName}
  ''';

  Future<List<SpendingAndIncome>> findAll() async {
    final Database db = await getDatabase();
    final List<Map<String, dynamic>> result = await db.rawQuery(query);
    final List<SpendingAndIncome> assets = [];

    result.forEach((element) {
      assets.add(SpendingAndIncome.fromMap(element));
    });
    return assets;
  }
}