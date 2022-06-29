import 'package:financial_app/database/getDatabase.dart';
import 'package:financial_app/models/models/IndexesAndDate.dart';
import 'package:financial_app/models/table_models/fixedInterest.dart';

import 'package:sqflite/sqflite.dart';

class FixedInterestDao {

  static const String tableName = 'fixedInterest';

  static const String id = 'id';
  static const String name = 'name';
  static const String preFixedInvestment = 'preFixedInvestment';
  static const String typeFixedInterestId = 'typeFixedInterestId';
  static const String amount = 'amount';
  static const String indexName = 'indexName';
  static const String indexPercentage = 'indexPercentage';
  static const String investmentDate = 'investmentDate';
  static const String additionalFixedInterest = 'additionalFixedInterest';
  static const String expirationDate = 'expirationDate';
  static const String liquidityOnExpiration = 'liquidityOnExpiration';

  static const String tableSql = '''
         CREATE TABLE $tableName(
         $id INTEGER PRIMARY KEY,
         $name STRING,
         $preFixedInvestment INTEGER,
         $typeFixedInterestId INTEGER,
         $amount DOUBLE,
         $indexName STRING,
         $indexPercentage DOUBLE,
         $additionalFixedInterest DOUBLE,
         $investmentDate INTEGER,
         $expirationDate INTEGER,
         $liquidityOnExpiration INTEGER
         )
         ''';

  Future<int> save(FixedInterest fixedInterest) async {
    final Database db = await getDatabase();
    final fixedInterestMap = fixedInterest.toMap();
    return db.insert(tableName, fixedInterestMap);
  }

  Future<List<FixedInterest>> findAll() async {
    final Database db = await getDatabase();
    final List<Map<String, dynamic>> result = await db.query(tableName);
    final List<FixedInterest> fixedInterestList = [];

    result.forEach((element) {
      fixedInterestList.add(FixedInterest.fromMap(element));
    });
    return fixedInterestList;
  }


  Future<List<IndexAndDate>> getIndexesList() async{
    var db = await getDatabase();
    final List<Map<String, dynamic>> result =
    await db.rawQuery("select $indexName, min($investmentDate) as minDate, max($expirationDate) as maxDate from $tableName group by $indexName");
    final List<IndexAndDate> list = [];
    result.forEach((element) {
      list.add(IndexAndDate.fromMap(element));
    });
    return list;

  }


  Future<int> deleteRow(FixedInterest fixedInterest) async {
    var db = await getDatabase();
    final int result = await db.delete(
      tableName,
      where: "Id = ?",
      whereArgs: [fixedInterest.id],
    );
    return result;
  }

  Future<int> updateRow(FixedInterest fixedInterest) async {
    var db = await getDatabase();
    final int result = await db.update(
      tableName,
      fixedInterest.toMap(),
      where: "Id = ?",
      whereArgs: [fixedInterest.id],
    );
    return result;
  }


}