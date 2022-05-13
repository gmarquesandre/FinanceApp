import 'package:financial_app/database/getDatabase.dart';
import 'package:financial_app/models/table_models/treasuryCurrentValue.dart';
import 'package:sqflite/sqflite.dart';

class TreasuryCurrentValueDao {


  static const String tableName = 'treasuryCurrentValue';
  static const String codeISIN = 'codeISIN';
  static const String treasuryBondName = 'treasuryBondName' ;
  static const String dateLastUpdate = 'dateLastUpdate' ;
  static const String unitPriceBuy = 'unitPriceBuy ';
  static const String unitPriceSell = 'unitPriceSell ';
  static const String expirationDate = 'expirationDate' ;
  static const String indexName = 'indexName';
  static const String fixedInterestValueSell = 'fixedInterestValueSell';
  static const String fixedInterestValueBuy = 'fixedInterestValueBuy';
  static const String lastAvailableDate = 'lastAvailableDate';


static const String tableSql = '''
         CREATE TABLE $tableName (
         $codeISIN STRING PRIMARY KEY,
         $treasuryBondName STRING,
         $dateLastUpdate INTEGER,
         $unitPriceBuy DOUBLE,
         $unitPriceSell DOUBLE,
         $expirationDate INTEGER,
         $indexName STRING,
         $fixedInterestValueSell DOUBLE,     
         $fixedInterestValueBuy DOUBLE,
         $lastAvailableDate INTEGER
         )
         ''';

  Future<int> save(TreasuryCurrentValue value) async {
    final Database db = await getDatabase();
    final treasuryMap = value.toMap();
    return db.insert(tableName, treasuryMap, conflictAlgorithm: ConflictAlgorithm.replace);
  }

  Future<List<TreasuryCurrentValue>> findAll() async {
    final Database db = await getDatabase();
    final List<Map<String, dynamic>> result = await db.query(tableName);
    final List<TreasuryCurrentValue> treasuryTypeList = [];
    result.forEach((element) {
      treasuryTypeList.add(TreasuryCurrentValue.fromMap(element));
    });

    return treasuryTypeList;
  }

  Future<DateTime> minDate() async{
    final Database db = await getDatabase();

    final List<Map<String, dynamic>> result = await db.rawQuery("select min($dateLastUpdate) as minUpdate from $tableName");

    DateTime date = result[0]['minUpdate'] != null? DateTime.fromMillisecondsSinceEpoch(result[0]['minUpdate']): DateTime(1900,1,1);

    return date;

  }

  Future<int> saveList(List<TreasuryCurrentValue> list) async {
    final Database db = await getDatabase();
    list.forEach((element) {
      var map = element.toMap();


      db.insert(tableName, map, conflictAlgorithm: ConflictAlgorithm.replace);
    });

    return 1;
  }

  Future<int> deleteAll() async{
    var db = await getDatabase();

    return await db.delete(
        tableName
    );

  }



  Future<int> deleteRow(TreasuryCurrentValue row) async {
    var db = await getDatabase();
    final int result = await db.delete(
      tableName,
      where: "Id = ?",
      whereArgs: [row.treasuryBondName],
    );
    return result;
  }

  Future<int> updateRow(TreasuryCurrentValue row) async {
    var db = await getDatabase();
    final int result = await db.update(
      tableName,
      row.toMap(),
      where: "Id = ?",
      whereArgs: [row.treasuryBondName],
    );
    return result;
  }

}