import 'package:financial_app/database/getDatabase.dart';
import 'package:financial_app/models/table_models/fundSimulation.dart';
import 'package:sqflite/sqflite.dart';

class FundSimulationDao {

  static const String tableName = 'fundSimulation';

  static const String cnpj = "cnpj";
  static const String nameShort = "nameShort";
  static const String indexName = "indexName";
  static const String fixedYearGain = "fixedYearGain";


  Future<int> save(FundSimulation fund) async {
    final Database db = await getDatabase();
    final fundMap = fund.toMap();

    return db.insert(tableName, fundMap);
  }

  static const String tableSql = '''
         CREATE TABLE $tableName(
         $cnpj STRING PRIMARY KEY,
         $nameShort STRING,
         $indexName STRING,          
         $fixedYearGain DOUBLE          
         )
         ''';

  Future<List<FundSimulation>> findAll() async {
    final Database db = await getDatabase();
    final List<Map<String, dynamic>> result = await db.query(tableName);
    final List<FundSimulation> funds = [];
    result.forEach((element) {
      funds.add(FundSimulation.fromMap(element));
    });
    return funds;
  }


  Future<int> deleteRow(FundSimulation fund) async {


    var db = await getDatabase();
    final int result = await db.delete(
      tableName,
      where: "$cnpj = ?",
      whereArgs: [fund.cnpj],
    );

    return result;
  }



  Future<int> updateRow(FundSimulation fund) async {
    var db = await getDatabase();
    final int result = await db.update(
      tableName,
      fund.toMap(),
      where: "$cnpj = ?",
      whereArgs: [fund.cnpj],
    );
    return result;
  }
}
