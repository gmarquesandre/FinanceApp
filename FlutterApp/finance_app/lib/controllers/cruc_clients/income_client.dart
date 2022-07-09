import 'dart:convert';
import 'package:finance_app/models/income/create_income.dart';
import 'package:finance_app/models/income/income.dart';

import 'default_request/default_client.dart';

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
    var success = client.create(path, body);

    return success;
  }
}
