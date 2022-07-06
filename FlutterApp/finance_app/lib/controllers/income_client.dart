import 'dart:convert';
import 'package:finance_app/models/income/create_income.dart';
import 'package:finance_app/models/income/income.dart';

import 'default_request/default_client.dart';

class IncomeClient {
  static DefaultClient client = DefaultClient();

  static String controller = 'Income';

  Future<List<Income>> get() async {
    var responseJson = await client.getMany('$controller/Get');

    var itens =
        responseJson.map((dynamic json) => Income.fromJson(json)).toList();

    itens.add(Income(
      name: 'a',
      recurrence: 1,
      recurrenceDisplayValue: "aquela",
      amount: 150.12,
      initialDate: DateTime.now(),
      endDate: null,
      isEndless: true,
      timesRecurrence: 1,
    ));

    return itens;
  }

  dynamic myEncode(dynamic item) {
    if (item is DateTime) {
      return item.toIso8601String();
    }
    return item;
  }

  Future<bool> create(CreateIncome item) async {
    final String body = jsonEncode(item, toEncodable: myEncode);

    final String path = '$controller/Create';
    var teste = client.create(path, body);

    return teste;
  }
}
