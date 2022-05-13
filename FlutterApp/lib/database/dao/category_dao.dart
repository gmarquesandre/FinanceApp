import 'package:financial_app/database/getDatabase.dart';
import 'package:financial_app/models/table_models/category.dart';
import 'package:sqflite/sqflite.dart';

class CategoryDao {

  static const String tableName = 'spending_category';

  static const String id = 'id';
  static const String name = 'name';

  static const String tableSql = '''
         CREATE TABLE $tableName(
         $id INTEGER PRIMARY KEY,
         $name TEXT
         )
         ''';

static List<Category> categoryList = [
  Category(name: "Alimentação"),
  Category(name: "Transporte"),
  Category(name: "Aluguel"),
  Category(name: "Lazer"),
  Category(name: "Outros")
];


  Future<int> save(Category category) async {
    final Database db = await getDatabase();
    final map = category.toMap();
    return db.insert(tableName, map);
  }

  Future<List<Category>> findAll() async {
    final Database db = await getDatabase();
    final List<Map<String, dynamic>> result = await db.query(tableName);
    final List<Category> list = [];
    result.forEach((element) {
      list.add(Category.fromMap(element));
    });
    return list;
  }

  Future<int> deleteRow(Category category) async {
    var db = await getDatabase();
    final int result = await db.delete(
      tableName,
      where: "Id = ?",
      whereArgs: [category.id],
    );
    return result;
  }

  Future<int> updateRow(Category category) async {
    var db = await getDatabase();
    final int result = await db.update(
      tableName,
      category.toMap(),
      where: "Id = ?",
      whereArgs: [category.id],
    );
    return result;
  }
}

