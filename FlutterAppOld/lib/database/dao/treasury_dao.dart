import 'package:financial_app/database/dao/treasuryCurrentValue_dao.dart';
import 'package:financial_app/database/getDatabase.dart';
import 'package:financial_app/models/table_models/treasury.dart';
import 'package:sqflite/sqflite.dart';

class TreasuryDao {


  static const String tableName = 'treasury';
  static const String operation = 'operation';
  static const String id = 'id';
  static const String treasuryBondName = 'treasuryBondName';
  static const String unitPrice = 'unitPrice' ;
  static const String quantity = 'quantity';
  static const String date = 'date';


      static const String tableSql = '''
         CREATE TABLE $tableName (
         $id INTEGER PRIMARY KEY,
         $treasuryBondName STRING,     
         $unitPrice DOUBLE,
         $quantity DOUBLE,
         $date INTEGER,
         $operation INTEGER,
         FOREIGN KEY($treasuryBondName) REFERENCES ${TreasuryCurrentValueDao.tableName}(${TreasuryCurrentValueDao.treasuryBondName})
         )
         ''';

  Future<int> save(Treasury treasury) async {
    final Database db = await getDatabase();
    final treasuryMap = treasury.toMap();
    return db.insert(tableName, treasuryMap);
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

  Future<List<Treasury>> findAll() async {
    final Database db = await getDatabase();
    final List<Map<String, dynamic>> result = await db.query(tableName);
    final List<Treasury> treasuryList = [];
    result.forEach((element) {
      treasuryList.add(Treasury.fromMap(element));
    });

    return treasuryList;
  }

  Future<int> deleteRow(Treasury treasury) async {


    var db = await getDatabase();
    final int result = await db.delete(
      tableName,
      where: "id = ?",
      whereArgs: [treasury.id],
    );

    return result;
  }

  Future<double> availableQuantity(int? id, DateTime date) async{

    List<Treasury> list = await findAll();


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

        quantity += element.operation == 1? element.quantity: -element.quantity;
      }

    });

    return quantity;
  }

  Future<int> updateRow(Treasury treasury) async {

    var db = await getDatabase();
    final int result = await db.update(
      tableName,
      treasury.toMap(),
      where: "id = ?",
      whereArgs: [treasury.id],
    );
    return result;
  }

}