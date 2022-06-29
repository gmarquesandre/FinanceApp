import 'package:financial_app/database/getDatabase.dart';
import 'package:financial_app/models/table_models/assetSimulation.dart';
import 'package:flutter/material.dart';
import 'package:sqflite/sqflite.dart';

class AssetSimulationDao {

  static const String tableName = 'assetsSimulation';

  static const String assetCode = "assetCode";
  static const String indexName = "indexName";
  static const String fixedYearGain = "fixedYearGain";


  static const String tableSql = '''
         CREATE TABLE $tableName(
         $assetCode STRING PRIMARY KEY,
         $indexName STRING,          
         $fixedYearGain DOUBLE          
         )
         ''';

  Future<int> save(AssetSimulation asset) async {
    final Database db = await getDatabase();
    final assetMap = asset.toMap();

    return db.insert(tableName, assetMap);
  }

  Future<List<AssetSimulation>> findAll() async {
    final Database db = await getDatabase();
    final List<Map<String, dynamic>> result = await db.query(tableName);
    final List<AssetSimulation> assets = [];
    result.forEach((element) {
      assets.add(AssetSimulation.fromMap(element));
    });
    return assets;
  }


  Future<int> deleteRow(AssetSimulation asset) async {


    var db = await getDatabase();
    final int result = await db.delete(
      tableName,
      where: "$assetCode = ?",
      whereArgs: [asset.assetCode],
    );

    return result;
  }



  Future<int> updateRow(AssetSimulation asset) async {
    var db = await getDatabase();
    final int result = await db.update(
      tableName,
      asset.toMap(),
      where: "$assetCode = ?",
      whereArgs: [asset.assetCode],
    );
    return result;
  }
}
