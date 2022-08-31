import 'dart:convert';
import 'package:finance_app/clients/default_request/default_client.dart';
import 'package:finance_app/models/credit_card/create_credit_card.dart';
import 'package:finance_app/models/credit_card/credit_card.dart';

class CreditCardClient {
  static DefaultClient client = DefaultClient();

  static String controller = 'CreditCard';

  Future<List<CreditCard>> get() async {
    var responseJson = await client.getMany('$controller/Get');

    var items =
        responseJson.map((dynamic json) => CreditCard.fromJson(json)).toList();

    return items;
  }

  dynamic myEncode(dynamic item) {
    if (item is DateTime) {
      return item.toIso8601String();
    }
    return item;
  }

  Future<CreditCard> create(CreateCreditCard item) async {
    final String body = jsonEncode(item.toJson());

    final String path = '$controller/Create';
    var responseBody = await client.create(path, body);

    return CreditCard.fromJson(responseBody);
  }

  Future<bool> delete(int id) async {
    final String path = '$controller/Delete';

    var success = await client.delete(path, {'id': id.toString()});

    return success;
  }

  // Future<bool> update(UpdateCreditCard item) async {
  //   final String body = jsonEncode(item.toJson());

  //   final String path = '$controller/Update';
  //   var success = await client.update(path, body);

  //   return success;
  // }
}
