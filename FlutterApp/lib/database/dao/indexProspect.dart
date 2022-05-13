
import 'package:financial_app/database/getDatabase.dart';
import 'package:financial_app/models/table_models/indexProspect.dart';
import 'package:sqflite/sqflite.dart';

class IndexProspectDao {

  static const String tableName = 'indexesProspect';

  static const String id = 'id';
  static const String indexName = 'indexName';
  static const String dateResearch = 'dateResearch';
  static const String dateStart = 'dateStart';
  static const String dateEnd = 'dateEnd';
  static const String average = 'average';
  static const String median = 'median';
  static const String min = 'min';
  static const String max = 'max';
  static const String researchAnswers = 'researchAnswers';
  static const String baseCalculo = 'baseCalculo';
  static const String dateLastUpdate = 'dateLastUpdate';
  static const String uniqueKey = 'uniqueKey';

  static const String tableSql = '''
        CREATE TABLE $tableName(
        $id INTEGER PRIMARY KEY,
        $indexName STRING,
        $dateResearch INTEGER,
        $dateStart INTEGER,
        $dateEnd INTEGER,
        $average DOUBLE,
        $median DOUBLE,
        $min DOUBLE,
        $max DOUBLE,
        $researchAnswers INTEGER,
        $baseCalculo INTEGER,
        $dateLastUpdate INTEGER,
        $uniqueKey STRING UNIQUE
         )
         ''';

  Future<int> save(IndexProspect index) async {
    final Database db = await getDatabase();
    final map = index.toMap();
    return db.insert(tableName, map);
  }

  Future<int> saveList(List<IndexProspect> list) async {
    final Database db = await getDatabase();

    Batch batch = db.batch();

    list.forEach((element) {
      var map = element.toMap();
      batch.insert(tableName, map, conflictAlgorithm: ConflictAlgorithm.replace);
    });

    batch.commit();
    return 1;
  }


  Future<List<IndexProspect>> findAll() async {
    final Database db = await getDatabase();
    final List<Map<String, dynamic>> result =
    await db.query(tableName);
    final List<IndexProspect> list = [];

    result.forEach((element) {
      list.add(IndexProspect.fromMap(element));
    });


    return list;
  }

  Future<int> deleteAll() async{
    var db = await getDatabase();

    final int result = await db.delete(
      tableName
    );
    return result;
  }

  Future<DateTime> getLastUpdateDate() async {
    var db = await getDatabase();

    String query = 'select max($dateLastUpdate) as $dateLastUpdate from $tableName';

    final List<Map<String, dynamic>> result =
    await db.rawQuery(query);
    DateTime date;

    date = result.first['$dateLastUpdate'] != null ?DateTime
        .fromMillisecondsSinceEpoch(result.first['$dateLastUpdate']):
    DateTime(1900);


    return date;

  }

}

