

import 'package:financial_app/database/getDatabase.dart';
import 'package:financial_app/models/table_models/loan.dart';
import 'package:sqflite/sqflite.dart';

class LoanDao {

  static const String tableName = 'loan';

  static const String id = "id";
  static const String date = "date";
  static const String name = "name";
  static const String totalValue = "totalValue";
  static const String months = "months";
  static const String interestRate = "interestRate";
  static const String paymentType = "paymentType";

  static const String tableSql = '''
         CREATE TABLE $tableName(
         $id INTEGER PRIMARY KEY,
         $date INTEGER,
         $name STRING,
         $totalValue DOUBLE,
         $months INTEGER,
         $interestRate DOUBLE,
         $paymentType INTEGER
         )
         ''';

  Future<int> save(Loan item) async {
    final Database db = await getDatabase();
    final assetMap = item.toMap();

    return db.insert(tableName, assetMap);
  }

  Future<List<Loan>> findAll() async {
    final Database db = await getDatabase();
    final List<Map<String, dynamic>> result = await db.query(tableName);
    final List<Loan> list = [];
    result.forEach((element) {
      list.add(Loan.fromMap(element));
    });
    return list;
  }

  Future<int> deleteRow(Loan item) async {

    var db = await getDatabase();
    final int result = await db.delete(
      tableName,
      where: "Id = ?",
      whereArgs: [item.id],
    );

    return result;
  }

  Future<Loan> findById(int id) async {
    var db = await getDatabase();
    final List<Map<String, Object?>> list = await db.query(
      tableName,
      where: "Id = ?",
      whereArgs: [id],
    );

    var obj = list.first;

    return Loan.fromMap(obj);
  }


  Future<int> updateRow(Loan item) async {
    var db = await getDatabase();
    final int result = await db.update(
      tableName,
      item.toMap(),
      where: "Id = ?",
      whereArgs: [item.id],
    );
    return result;
  }
}
