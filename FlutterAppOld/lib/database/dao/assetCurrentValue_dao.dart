import 'package:financial_app/database/getDatabase.dart';
import 'package:financial_app/models/table_models/assetCurrentValue.dart';
import 'package:flutter/material.dart';
import 'package:sqflite/sqflite.dart';

class AssetCurrentValueDao {

  static const String tableName = 'assetCurrentValue';

  static const String assetCode = 'assetCode';
  static const String companyName = 'companyName';
  static const String dateLastUpdate = 'dateLastUpdate';
  static const String dateLastLocalUpdate = 'dateLastLocalUpdate';
  static const String unitPrice = 'unitPrice';


  static const String tableSql = '''
         CREATE TABLE $tableName(
         $assetCode STRING PRIMARY KEY,
         $companyName STRING,
         $dateLastUpdate INTEGER,
         $unitPrice DOUBLE
         )
         ''';


  Future<int> save(AssetCurrentValue asset) async {
    final Database db = await getDatabase();
    final map = asset.toMap();
    return db.insert(tableName, map, conflictAlgorithm: ConflictAlgorithm.replace);
  }

  Future<DateTime> maxDate() async{
    final Database db = await getDatabase();
    try {

      return DateTime.now();

      final List<Map<String, dynamic>> result = await db.rawQuery(
          "select max($dateLastUpdate) as maxUpdate from $tableName");

      DateTime date = result[0]['maxUpdate'] != null ? DateTime
          .fromMillisecondsSinceEpoch(result[0]['maxUpdate']) : DateTime(
          1900, 1, 1);

      return date;
    }
    on Exception catch(e){
      debugPrint(e.toString());
      return DateTime.now();
    }
  }

  Future<List<String>> getAssetList() async{
    final Database db = await getDatabase();
    final List<Map<String, dynamic>> result = await db.rawQuery("select $assetCode from $tableName");
    final List<String> assets = [];

     result.forEach((element) {
      assets.add(element['$assetCode'].toString());
    });
    return assets;
  }

  Future<int> saveList(List<AssetCurrentValue> assetList) async {
    final Database db = await getDatabase();
    assetList.forEach((element) {
      var map = element.toMap();
      db.insert(tableName, map, conflictAlgorithm: ConflictAlgorithm.replace);
    });

    return 1;
  }



  Future<List<AssetCurrentValue>> findAll() async {
    final Database db = await getDatabase();
    final List<Map<String, dynamic>> result = await db.query(tableName);
    final List<AssetCurrentValue> assetLists = [];
    result.forEach((element) {
      assetLists.add(AssetCurrentValue.fromMap(element));
    });


    return assetLists;
  }

  Future<int> deleteRow(AssetCurrentValue assetList) async {
    var db = await getDatabase();
    final int result = await db.delete(
      tableName,
      where: "assetCode = ?",
      whereArgs: [assetList.assetCode],
    );
    return result;
  }

  Future<int> deleteRowWithName(String assetCode) async {
    var db = await getDatabase();
    final int result = await db.delete(
      tableName,
      where: "assetCode = ?",
      whereArgs: [assetCode],
    );
    return result;
  }


  Future<int> updateRow(AssetCurrentValue assetList) async {
    var db = await getDatabase();
    final int result = await db.update(
      tableName,
      assetList.toMap(),
      where: "assetCode = ?",

      whereArgs: [assetList.assetCode],
    );
    return result;
  }

}
