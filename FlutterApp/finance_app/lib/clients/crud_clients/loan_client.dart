import 'dart:convert';
import 'package:finance_app/clients/default_request/default_client.dart';
import 'package:finance_app/models/loan/loan.dart';
import 'package:finance_app/models/loan/update_loan.dart';
import 'package:finance_app/models/loan/create_loan.dart';

class LoanClient {
  static DefaultClient client = DefaultClient();

  static String controller = 'Loan';

  Future<List<Loan>> get() async {
    var responseJson = await client.getMany('$controller/Get');

    var items =
        responseJson.map((dynamic json) => Loan.fromJson(json)).toList();

    return items;
  }

  dynamic myEncode(dynamic item) {
    if (item is DateTime) {
      return item.toIso8601String();
    }
    return item;
  }

  Future<Loan> create(CreateLoan item) async {
    final String body = jsonEncode(item.toJson());

    final String path = '$controller/Create';
    var success = await client.create(path, body);
    return Loan.fromJson(success);
  }

  Future<bool> delete(int id) async {
    final String path = '$controller/Delete';

    var success = await client.delete(path, {'id': id.toString()});

    return success;
  }

  Future<bool> update(UpdateLoan item) async {
    final String body = jsonEncode(item.toJson());

    final String path = '$controller/Update';
    var success = await client.update(path, body);

    return success;
  }
}
