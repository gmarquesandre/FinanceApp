import 'package:financial_app/database/getDatabase.dart';
import 'package:financial_app/models/table_models/assetChanges.dart';
import 'package:sqflite/sqflite.dart';

class AssetChangesDao {

  static const String tableName = 'assetChanges';

  static const String id = 'id';
  static const String type = 'type';
  static const String assetCode = 'assetCode';
  static const String assetCodeISIN = 'assetCodeISIN';
  static const String declarationDate = 'declarationDate';
  static const String exDate = 'exDate';
  static const String groupingFactor = 'groupingFactor';
  static const String toAssetISIN = 'toAssetISIN';
  static const String notes = 'notes';
  static const String hash = 'hash';


  static const String tableSql = '''
         CREATE TABLE $tableName(
         $id INTEGER PRIMARY KEY,
         $assetCode STRING,
         $assetCodeISIN STRING,
         $type STRING,
         $declarationDate INTEGER,
         $exDate INTEGER,
         $groupingFactor DOUBLE,
         $toAssetISIN STRING,
         $hash STRING UNIQUE
         )
         ''';


  Future<int> save(AssetChanges asset) async {
    final Database db = await getDatabase();
    final map = asset.toMap();
    return db.insert(tableName, map, conflictAlgorithm: ConflictAlgorithm.replace);
  }

  Future<int> saveList(List<AssetChanges> assetList) async {
    final Database db = await getDatabase();
    assetList.forEach((element) {
      var map = element.toMap();
      db.insert(tableName, map, conflictAlgorithm: ConflictAlgorithm.replace);
    });

    return 1;
  }

  Future<int> deleteAll() async{
    var db = await getDatabase();

    await db.delete(
        tableName
    );

    return 1;
  }

  Future<List<AssetChanges>> findAll() async {
    final Database db = await getDatabase();
    final List<Map<String, dynamic>> result = await db.query(tableName);
    final List<AssetChanges> assetLists = [];
    result.forEach((element) {
      assetLists.add(AssetChanges.fromMap(element));
    });


    return assetLists;
  }

  Future<List<AssetChanges>> findAssetAndMaxDate() async {
    final Database db = await getDatabase();
    final List<Map<String, dynamic>> result = await db.rawQuery("select $assetCodeISIN from ");
    final List<AssetChanges> assetLists = [];
    result.forEach((element) {
      assetLists.add(AssetChanges.fromMap(element));
    });


    return assetLists;
  }




  Future<int> deleteRow(AssetChanges assetList) async {
    var db = await getDatabase();
    final int result = await db.delete(
      tableName,
      where: "id = ?",
      whereArgs: [assetList.id],
    );
    return result;
  }

  Future<int> updateRow(AssetChanges assetList) async {
    var db = await getDatabase();
    final int result = await db.update(
      tableName,
      assetList.toMap(),
      where: "hash = ?",

      whereArgs: [assetList.hash],
    );
    return result;
  }

}
