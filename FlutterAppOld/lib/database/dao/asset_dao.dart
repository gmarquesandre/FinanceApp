import 'package:financial_app/database/getDatabase.dart';
import 'package:financial_app/models/table_models/asset.dart';
import 'package:sqflite/sqflite.dart';

class AssetDao {

  static const String tableName = 'assets';


  static const String id = "id";
  static const String assetCode = "assetCode";
  static const String quantity = "quantity";
  static const String unitPrice = "unitPrice";
  static const String date = "date";
  static const String operation = "operation";

  static const String tableSql = '''
         CREATE TABLE $tableName(
         $id INTEGER PRIMARY KEY,
         $date INTEGER,
         $assetCode STRING,
         $quantity INTEGER,
         $unitPrice DOUBLE,
         $operation INTEGER 
         )
         ''';

  Future<int> save(Asset asset) async {
    final Database db = await getDatabase();
    final assetMap = asset.toMap();

    return db.insert(tableName, assetMap);
  }

  Future<List<Asset>> findAll() async {
    final Database db = await getDatabase();
    final List<Map<String, dynamic>> result = await db.query(tableName);
    final List<Asset> assets = [];
    result.forEach((element) {
      assets.add(Asset.fromMap(element));
    });
    return assets;
  }

  Future<List<Asset>> getAssetAndDateList() async{
    final Database db = await getDatabase();
    final List<Map<String, dynamic>> result = await db.rawQuery("select $assetCode, min($date) as date from $tableName group by $assetCode");
    final List<Asset> assets = [];

    result.forEach((element) {
      assets.add(Asset(assetCode: element['$assetCode'].toString(),
        date: DateTime.fromMillisecondsSinceEpoch(element['$date']), quantity: 0 , operation: 0, unitPrice: 0,
      ),
      );
    });
    return assets;
  }

  Future<int> deleteRow(Asset asset) async {


    var db = await getDatabase();
    final int result = await db.delete(
      tableName,
      where: "Id = ?",
      whereArgs: [asset.id],
    );

    return result;
  }

  Future<Asset> findById(int assetId) async {
    var db = await getDatabase();
    final List<Map<String, Object?>> list = await db.query(
      tableName,
      where: "Id = ?",
      whereArgs: [assetId],
    );

    var obj = list.first;

    return Asset.fromMap(obj);
  }


  Future<int> updateRow(Asset asset) async {
    var db = await getDatabase();
    final int result = await db.update(
      tableName,
      asset.toMap(),
      where: "Id = ?",
      whereArgs: [asset.id],
    );
    return result;
  }
}
