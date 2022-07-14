import 'dart:convert';
import 'package:finance_app/controllers/default_request/default_client.dart';
import 'package:finance_app/models/income/create_income.dart';
import 'package:finance_app/models/income/income.dart';
import 'package:finance_app/models/income/update_income.dart';

class IncomeClient {
  static DefaultClient client = DefaultClient();

  static String controller = 'Income';

  Future<List<Income>> get() async {
    var responseJson = await client.getMany('$controller/Get');

    var items =
        responseJson.map((dynamic json) => Income.fromJson(json)).toList();

    return items;
  }

  dynamic myEncode(dynamic item) {
    if (item is DateTime) {
      return item.toIso8601String();
    }
    return item;
  }

  Future<bool> create(CreateIncome item) async {
    final String body = jsonEncode(item.toJson());

    final String path = '$controller/Create';
    var success = await client.create(path, body);

    return success;
  }

  Future<bool> delete(int id) async {
    final String path = '$controller/Delete';

    var success = await client.delete(path, {'id': id.toString()});

    return success;
  }

  Future<bool> update(UpdateIncome item) async {
    final String body = jsonEncode(item.toJson());

    final String path = '$controller/Update';
    var success = await client.update(path, body);

    return success;
  }
}
