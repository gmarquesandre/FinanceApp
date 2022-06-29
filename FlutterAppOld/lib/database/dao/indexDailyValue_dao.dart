import 'package:financial_app/database/getDatabase.dart';
import 'package:financial_app/models/models/IndexesAndDate.dart';
import 'package:financial_app/models/models/interestAndDays.dart';
import 'package:financial_app/models/table_models/indexDailyInterest.dart';
import 'package:flutter/material.dart';
import 'package:sqflite/sqflite.dart';

class IndexDailyValueDao {

  static const String tableName = 'indexesDailyValues';

  static const String id = 'id';
  static const String indexName = 'indexName';
  static const String date ='date' ;
  static const String value = 'value';
  static const String uniqueKey = 'uniqueKey';

  static const String tableSql = '''
         CREATE TABLE $tableName(
         $id INTEGER PRIMARY KEY,
         $uniqueKey STRING UNIQUE,
         $indexName STRING,
         $date INTEGER,
         $value DOUBLE
         )
         ''';

  Future<int> save(IndexDaily index) async {
    final Database db = await getDatabase();
    final map = index.toMap();
    return db.insert(tableName, map);
  }

  Future<int> saveList(List<IndexDaily> list) async {


    final Database db = await getDatabase();

    Batch batch = db.batch();
    list.forEach((element) {
      var map = element.toMap();
      batch.insert(tableName, map, conflictAlgorithm: ConflictAlgorithm.replace);
    });

    batch.commit();

    return 1;
  }


  Future<List<IndexDaily>> findAll() async {
    final Database db = await getDatabase();
    final List<Map<String, dynamic>> result =
    await db.query(tableName);
    final List<IndexDaily> list = [];
  try {
    result.forEach((element) {
      list.add(IndexDaily.fromMap(element));
    });
  }
  on Exception catch(e){
    debugPrint(e.toString());

  }
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

  Future<dynamic> deleteOutofRangeDate(IndexAndDate value) async {
    var db = await getDatabase();
     var result = await db.rawQuery("Delete from $tableName where $indexName = '${value.indexName}' and ($date < ${value.minDate!.millisecondsSinceEpoch} or $date > ${value.maxDate!.millisecondsSinceEpoch})");

    return result;
  }

  Future<dynamic> deleteWithIndexName(IndexAndDate value) async {
    var db = await getDatabase();
      var result = await db.rawQuery("Delete from $tableName where $indexName = '${value.indexName}'");

    return result;
  }


  Future<int> deleteAll() async{
    var db = await getDatabase();

    return await db.delete(
      tableName
    );

  }


  Future<int> updateRow(IndexDaily value) async {
    var db = await getDatabase();
    final int result = await db.update(
      tableName,
      value.toMap(),
      where: "Id = ?",
      whereArgs: [value.id],
    );
    return result;
  }

  Future<DateTime> getMaxDate(int indexId) async {
    var db = await getDatabase();

    String query = "select max($date) as $date from $tableName where indexId ='$indexId'";

    final List<Map<String, dynamic>> result =
    await db.rawQuery(query);
    DateTime maxDate;

    maxDate = DateTime.fromMillisecondsSinceEpoch(result.first['$date']);

    return maxDate;

  }

  Future<DateTime> getMinDate(int indexId) async {
    var db = await getDatabase();

    String query = 'select min(date) as date from $tableName where indexId = $indexId';

    final List<Map<String, dynamic>> result =
    await db.rawQuery(query);
    DateTime date;

    date = DateTime.fromMillisecondsSinceEpoch(result.first['date']);

    return date;

  }

  Future<List<IndexAndDate>> getIndexesList() async{
    var db = await getDatabase();
    final List<Map<String, dynamic>> result =
    await db.rawQuery("select $indexName, min($date) as minDate, max($date) as maxDate from $tableName group by $indexName");
    final List<IndexAndDate> list = [];
    result.forEach((element) {
      list.add(IndexAndDate.fromMap(element));
    });
    return list;

  }


  Future<List<InterestAndNDays>> getIndexValues(int indexId, DateTime? initialDate, DateTime? endDate) async {
    var db = await getDatabase();
    String query = 'SELECT value as interestRate, count(distinct date) as workingDays from $tableName where indexId = $indexId';
    if(initialDate != null)
    {
        query+= ' and date >= ${initialDate.millisecondsSinceEpoch}';
    }
    if(endDate != null)
    {
      query+= ' and date <= ${endDate.millisecondsSinceEpoch}';
    }
    query += ' GROUP BY value';
    final List<Map<String, dynamic>> result = await db.rawQuery(query);

    final List<InterestAndNDays> list = [];
    result.forEach((element) {
      list.add(InterestAndNDays.fromMap(element));
    });

    return list;
  }
}

