import 'package:financial_app/database/getDatabase.dart';
import 'package:financial_app/models/table_models/indexDailyInterest.dart';
import 'package:sqflite/sqflite.dart';

class IndexLastValueDao {
  static const String tableName = 'indexesLastValues';

  static const String id = 'id';
  static const String indexName = 'indexName';
  static const String date = 'date';

  static const String value = 'value';

  static const String tableSql = '''
         CREATE TABLE $tableName(
         $id INTEGER PRIMARY KEY,
         $indexName STRING UNIQUE,
         $date INTEGER,
         $value DOUBLE
         )
         ''';

  Future<int> save(IndexDaily index) async {
    final Database db = await getDatabase();
    final map = index.toMapLastValue();
    return db.insert(tableName, map);
  }

  Future<int> saveList(List<IndexDaily> list) async {
    final Database db = await getDatabase();

    list.forEach((element) {
      var map = element.toMapLastValue();
      db.insert(tableName, map, conflictAlgorithm: ConflictAlgorithm.replace);
    });

    return 1;
  }

  Future<List<IndexDaily>> findAll() async {
    final Database db = await getDatabase();
    final List<Map<String, dynamic>> result = await db.query(tableName);
    final List<IndexDaily> list = [];
    result.forEach(
      (element) {
        list.add(IndexDaily.fromMap(element));
      },
    );

    return list;
  }

  Future<int> deleteRow(IndexDaily value) async {
    var db = await getDatabase();
    final int result = await db.delete(
      tableName,
      where: "Id = ?",
      whereArgs: [value.id],
    );
    return result;
  }

  Future<int> deleteAll() async {
    var db = await getDatabase();

    return await db.delete(tableName);
  }
}
