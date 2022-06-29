import 'package:financial_app/database/getDatabase.dart';
import 'package:financial_app/models/table_models/workingDaysByYear.dart';
import 'package:sqflite/sqflite.dart';

class WorkingDaysByYearDao {

  static const String tableName = 'workingDaysByYear';


  static const String year = "year";
  static const String workingDays = "workingDays";
  static const String dateLastUpdate = "dateLastUpdate";

  static const String tableSql = '''
         CREATE TABLE $tableName(
         $year INTEGER PRIMARY KEY,
         $workingDays INTEGER,
         $dateLastUpdate INTEGER
         )
         ''';

  Future<int> save(WorkingDaysByYear item) async {
    final Database db = await getDatabase();
    final map = item.toMap();
    return db.insert(tableName, map, conflictAlgorithm: ConflictAlgorithm.replace);
  }

  Future<int> saveList(List<WorkingDaysByYear> list) async {


    final Database db = await getDatabase();
    list.forEach((element) {
      var map = element.toMap();

      db.insert(tableName, map, conflictAlgorithm: ConflictAlgorithm.replace);
    });

    return 1;
  }

  Future<List<WorkingDaysByYear>> findAll(int? initialYear, int?
  endYear)
  async {

    if(initialYear == null){
      initialYear = 1900;
    }

    if(endYear == null){
      endYear = 3000;
    }

    final Database db = await getDatabase();


    String query = "select * from $tableName where $year >= $initialYear"
        " and $year <= $endYear";

    final List<Map<String, dynamic>> result = await db.rawQuery(query);


    final List<WorkingDaysByYear> list = [];
    result.forEach((element) {

      list.add(WorkingDaysByYear.fromMap(element));
    });
    return list;
  }

  void deleteAll() async{
    var db = await getDatabase();

    await db.delete(
        tableName
    );
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

  Future<int> getWorkingDays(int yearFind) async {
    var db = await getDatabase();

    String query = "select * from $tableName where $year "
        "= $yearFind";

    final List<Map<String, dynamic>> result =
    await db.rawQuery(query);
    int value;
    value = result.first['$workingDays'];

    return value;

  }


}
