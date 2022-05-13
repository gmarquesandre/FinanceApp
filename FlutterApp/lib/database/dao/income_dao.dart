import 'package:financial_app/database/getDatabase.dart';
import 'package:financial_app/models/table_models/income.dart';
import 'package:sqflite/sqflite.dart';

class IncomeDao {

  static const String tableName = 'income';

  static const String id = 'id';
  static const String name = 'name';
  static const String incomeValue = 'incomeValue';
  static const String initialDate = 'initialDate';
  static const String endDate = 'endDate';
  static const String recurrenceId = 'recurrenceId';
  static const String isEndless = 'isEndless';
  static const String timesRecurrence = 'timesRecurrence' ;


  static const String tableSql = '''
         CREATE TABLE $tableName(
         $id INTEGER PRIMARY KEY,
         $name TEXT,
         $incomeValue DOUBLE,
         $initialDate INTEGER,
         $endDate INTEGER,
         $recurrenceId INTEGER,
         $isEndless INTEGER,
         $timesRecurrence INTEGER

         )
         ''';

  // FOREIGN KEY($recurrenceId) REFERENCES ${RecurrenceDao.tableName}(${RecurrenceDao.id})

  Future<int> save(Income income) async {
    final Database db = await getDatabase();
    final fixedInterestMap = income.toMap();
    return db.insert(tableName, fixedInterestMap);
  }

  Future<List<Income>> findAll() async {
    final Database db = await getDatabase();
        final List<Map<String, dynamic>> result = await db.query(tableName);
        final List<Income> incomes = [];
        result.forEach((element) {
      incomes.add(Income.fromMap(element));
    });
    return incomes;
  }

  Future<int> deleteRow(Income income) async {
    var db = await getDatabase();
    final int result = await db.delete(
      tableName,
      where: "Id = ?",
      whereArgs: [income.id],
    );
    return result;
  }


  Future<int> updateRow(Income income) async {
    var db = await getDatabase();
    final int result = await db.update(
      tableName,
      income.toMap(),
      where: "Id = ?",
      whereArgs: [income.id],
    );
    return result;
  }
}