
import 'package:financial_app/database/getDatabase.dart';
import 'package:financial_app/models/table_models/investmentFundCurrentValue.dart';
import 'package:sqflite/sqflite.dart';

class InvestmentFundCurrentValueDao {


  static const String tableName = 'investmentFundCurrentValue';

  static const String cnpj = 'cnpj';
  static const String nameShort = 'nameShort' ;
  static const String name = 'name' ;
  static const String situation = 'situation' ;
  static const String taxLongTerm = 'taxLongTerm' ;
  static const String administrationFee = 'administrationFee' ;
  static const String fundTypeName = 'fundTypeName' ;
  static const String unitPrice = 'unitPrice' ;
  static const String dateLastUpdate = 'dateLastUpdate' ;

  static const String tableSql = '''
         CREATE TABLE $tableName(
         $cnpj STRING PRIMARY KEY,
         $dateLastUpdate INTEGER,
         $name STRING,
         $fundTypeName STRING,
         $administrationFee DOUBLE,
         $taxLongTerm INTEGER,
         $situation STRING,
         $nameShort STRING,
         $unitPrice DOUBLE     
         )
         ''';

  Future<int> save(InvestmentFundCurrentValue obj) async {
    final Database db = await getDatabase();
    final assetMap = obj.toMap();
    return db.insert(tableName, assetMap, conflictAlgorithm: ConflictAlgorithm.replace);
  }

  Future<List<String>> getFundList() async{
    final Database db = await getDatabase();
    final List<Map<String, dynamic>> result = await db.rawQuery("select $cnpj from $tableName");
    final List<String> assets = [];

    result.forEach((element) {
      assets.add(element['$cnpj'].toString());
    });
    return assets;
  }


  Future<int> saveList(List<InvestmentFundCurrentValue> fundList) async {
    final Database db = await getDatabase();
    fundList.forEach((element) {
      var map = element.toMap();
      db.insert(tableName, map, conflictAlgorithm: ConflictAlgorithm.replace);
    });

    return 1;
  }

  Future<DateTime> minDate() async{
    final Database db = await getDatabase();

    final List<Map<String, dynamic>> result = await db.rawQuery("select min($dateLastUpdate) as minUpdate from $tableName");

    DateTime date = result[0]['minUpdate'] != null? DateTime.fromMillisecondsSinceEpoch(result[0]['minUpdate']): DateTime(1900,1,1);
    return date;

  }

  Future<List<InvestmentFundCurrentValue>> findAll() async {

    final Database db = await getDatabase();
    final List<Map<String, dynamic>> result = await db.query(tableName);
    final List<InvestmentFundCurrentValue> list = [];
    result.forEach((element) {
      list.add(InvestmentFundCurrentValue.fromMap(element));
    });
    return list;
  }

  Future<int> deleteRow(InvestmentFundCurrentValue obj) async {
    var db = await getDatabase();
    final int result = await db.delete(
      tableName,
      where: "Id = ?",
      whereArgs: [obj.cnpj],
    );
    return result;
  }

  Future<int> deleteRowWithCnpj(String cnpj) async {
    var db = await getDatabase();

    final int result = await db.delete(
      tableName,
      where: "cnpj = ?",
      whereArgs: [cnpj],
    );
    return result;
  }


  Future<int> updateRow(InvestmentFundCurrentValue obj) async {
    var db = await getDatabase();
    final int result = await db.update(
      tableName,
      obj.toMap(),
      where: "Id = ?",
      whereArgs: [obj.cnpj],
    );
    return result;
  }
}
