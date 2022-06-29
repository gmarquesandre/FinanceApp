
import 'package:financial_app/database/getDatabase.dart';
import 'package:financial_app/models/table_models/holiday.dart';
import 'package:sqflite/sqflite.dart';

class HolidaysDao {

  static const String tableName = 'holidays';


  static const String id = "id";
  static const String countryCode = "countryCode";
  static const String dateLastUpdate = "dateLastUpdate";
  static const String date = "date";

  static const String tableSql = '''
         CREATE TABLE $tableName(
         $id INTEGER PRIMARY KEY,
         $countryCode STRING,
         $date INTEGER UNIQUE,
         $dateLastUpdate  INTEGER
         )
         ''';

  Future<int> save(Holiday item) async {
    final Database db = await getDatabase();
    final map = item.toMap();
    return db.insert(tableName, map, conflictAlgorithm: ConflictAlgorithm.replace);
  }

  Future<int> saveList(List<Holiday> list) async {


    final Database db = await getDatabase();
    Batch batch = db.batch();

    list.forEach((element) {
      var map = element.toMap();

      batch.insert(tableName, map, conflictAlgorithm: ConflictAlgorithm
          .replace);
    });

    batch.commit();

    return 1;
  }

  Future<List<Holiday>> findAll(DateTime? initialDate, DateTime? endDate)
  async {

    if(initialDate == null){
      initialDate = DateTime(1900,1,1);
    }
    if(endDate == null){
      endDate = DateTime(DateTime.now().year + 100,1,1);
    }
    final Database db = await getDatabase();


    String query = "select * from $tableName where $date >= ${initialDate
        .millisecondsSinceEpoch} and $date <= ${endDate.millisecondsSinceEpoch}";

    final List<Map<String, dynamic>> result = await db.rawQuery(query);


    final List<Holiday> list = [];
    result.forEach((element) {

      list.add(Holiday.fromMap(element));
    });
    return list;
  }

  Future<int> deleteAll() async{
    var db = await getDatabase();

    await db.delete(
        tableName
    );
    return 1;
  }

  Future<DateTime> getLastUpdateDate() async {
    var db = await getDatabase();

    String query = "select max($dateLastUpdate) as $dateLastUpdate from $tableName ";

    final List<Map<String, dynamic>> result =
        await db.rawQuery(query);
    DateTime maxDate;

    maxDate = result.first['$dateLastUpdate'] == null? DateTime(1900,1,1):
    DateTime
        .fromMillisecondsSinceEpoch(result
        .first['$dateLastUpdate']);

    return maxDate;

  }

  Future<bool> findDate(DateTime dateFind) async {
    var db = await getDatabase();

    String query = "select $dateLastUpdate from $tableName where $date "
        "== ${dateFind.millisecondsSinceEpoch}";

    final List<Map<String, dynamic>> result =
    await db.rawQuery(query);
    bool found;

    found = result.length == 0 ? false:
    true;

    return found;

  }


}
