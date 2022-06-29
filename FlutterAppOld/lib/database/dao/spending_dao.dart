import 'package:financial_app/database/dao/category_dao.dart';
import 'package:financial_app/database/getDatabase.dart';
import 'package:financial_app/models/table_models/spending.dart';
import 'package:sqflite/sqflite.dart';

class SpendingDao {


  static const String tableName = 'spending';
  static const String id = 'id';
  static const String name = 'name' ;
  static const String spendingValue = 'spendingValue';
  static const String initialDate = 'initialDate' ;
  static const String endDate = 'endDate' ;
  static const String recurrenceId = 'recurrenceId' ;
  static const String categoryId = 'categoryId' ;
  static const String isEndless = 'isEndless' ;
  static const String timesRecurrence = 'timesRecurrence' ;
  static const String isRequiredSpending = 'isRequiredSpending' ;

  static const String tableSql = '''
         CREATE TABLE $tableName (
         $id INTEGER PRIMARY KEY,
         $name TEXT,
         $spendingValue DOUBLE,
         $initialDate INTEGER,
         $endDate INTEGER,
         $recurrenceId INTEGER,
         $categoryId INTEGER,
         $isEndless INTEGER,
         $timesRecurrence INTEGER,
         $isRequiredSpending INTEGER,
         FOREIGN KEY($categoryId) REFERENCES ${CategoryDao.tableName}(${CategoryDao.id})
         )
         ''';



  Future<int> save(Spending spending) async {
    final Database db = await getDatabase();
    final spendingMap = spending.toMap();
    return db.insert(tableName, spendingMap);
  }

  Future<List<Spending>> findAll() async {
    final Database db = await getDatabase();
    final List<Map<String, dynamic>> result = await db.query(tableName);
    final List<Spending> spendingList = [];
    result.forEach((element) {
      spendingList.add(Spending.fromMap(element));
    });
    return spendingList;
  }

  Future<int> deleteRow(Spending spending) async {
    var db = await getDatabase();
    final int result = await db.delete(
      tableName,
      where: "Id = ?",
      whereArgs: [spending.id],
    );
    return result;
  }

  Future<int> updateRow(Spending spending) async {
    var db = await getDatabase();
    final int result = await db.update(
      tableName,
      spending.toMap(),
      where: "Id = ?",
      whereArgs: [spending.id],
    );
    return result;
  }

}