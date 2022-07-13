import 'dart:convert';
import 'package:finance_app/models/spending/create_spending.dart';
import 'package:finance_app/models/spending/spending.dart';
import 'package:finance_app/models/spending/update_spending.dart';

import 'default_request/default_client.dart';

class SpendingClient {
  static DefaultClient client = DefaultClient();

  static String controller = 'Spending';

  Future<List<Spending>> get() async {
    var responseJson = await client.getMany('$controller/Get');

    var items =
        responseJson.map((dynamic json) => Spending.fromJson(json)).toList();

    return items;
  }

  dynamic myEncode(dynamic item) {
    if (item is DateTime) {
      return item.toIso8601String();
    }
    return item;
  }

  Future<bool> create(CreateSpending item) async {
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

  Future<bool> update(UpdateSpending item) async {
    final String body = jsonEncode(item.toJson());

    final String path = '$controller/Update';
    var success = await client.update(path, body);

    return success;
  }
}
