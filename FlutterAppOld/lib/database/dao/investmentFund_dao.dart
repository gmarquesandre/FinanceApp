import 'package:financial_app/database/getDatabase.dart';
import 'package:financial_app/models/table_models/investmentFund.dart';
import 'package:sqflite/sqflite.dart';

class InvestmentFundDao {

  static const String tableName = 'investmentFund';

  static const String id = 'id';
  static const String date = 'date';
  static const String cnpj = 'cnpj';
  static const String name = 'name' ;
  static const String totalInvestment = 'totalInvestment';
  static const String unitPrice = 'unitPrice' ;
  static const String operation = 'operation' ;
  static const String quantity = 'quantity' ;

  static const String tableSql = '''
         CREATE TABLE $tableName(
         $id INTEGER PRIMARY KEY,
         $date INTEGER,
         $cnpj STRING,
         $name STRING,
         $totalInvestment DOUBLE,
         $operation INTEGER,
         $unitPrice DOUBLE,
         $quantity DOUBLE
         )
         ''';

  Future<int> save(InvestmentFund obj) async {
    final Database db = await getDatabase();
    final assetMap = obj.toMap();
    return db.insert(tableName, assetMap);
  }

  Future<List<InvestmentFund>> findAll() async {

    final Database db = await getDatabase();
    final List<Map<String, dynamic>> result = await db.query(tableName);
    final List<InvestmentFund> list = [];
    result.forEach((element) {
      list.add(InvestmentFund.fromMap(element));
    });
    return list;
  }

  Future<List<String>> getFundList() async{
    final Database db = await getDatabase();
    final List<Map<String, dynamic>> result = await db.rawQuery("select distinct $cnpj from $tableName");
    final List<String> assets = [];

    result.forEach((element) {
      assets.add(element['$cnpj'].toString());
    });
    return assets;
  }
  Future<int> getMaxId() async{
    final Database db = await getDatabase();
    final List<Map<String, dynamic>> result = await db.rawQuery("select max"
        "($id) as id from $tableName");
    if(result.length == 0)
      return 0;

    var item = result.first;

    int maxId = item['id'];

    return maxId;

  }
  Future<double> availableQuantity(int? id, DateTime date) async{

    List<InvestmentFund> list = await findAll();


    if(id == null){
      id = await getMaxId() ;
      id+=1;
    }

    list = list.where((element) => element.date.compareTo(date) <
        0 || (element.date.compareTo(date) ==
        0 && element.id! < id!) ).toList();
    double quantity = 0.00;
    list.forEach((element) {
      if(element.id != id){

        quantity += element.operation == 1? element.quantity!: -element
            .quantity!;
      }

    });

    return quantity;
  }


  Future<int> deleteRow(InvestmentFund obj) async {
    var db = await getDatabase();
    final int result = await db.delete(
      tableName,
      where: "Id = ?",
      whereArgs: [obj.id],
    );
    return result;
  }

  Future<int> updateRow(InvestmentFund obj) async {
    var db = await getDatabase();
    final int result = await db.update(
      tableName,
      obj.toMap(),
      where: "Id = ?",
      whereArgs: [obj.id],
    );
    return result;
  }
}
