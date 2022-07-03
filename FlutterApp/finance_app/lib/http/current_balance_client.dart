import 'dart:convert';

import 'package:finance_app/global_variables.dart';
import 'package:finance_app/routes.dart';
import 'package:http_status_code/http_status_code.dart';
import 'package:http/http.dart' as http;

class CurrentBalanceClient {
  static String baseUrl = GlobalVariables.baseUrlLocal;

  static int timeout = GlobalVariables.requestTimeout;

  Future<void> get() async {
    final Map<String, dynamic> queryParameters = <String, dynamic>{};

    var uri = Uri.https(baseUrl, '/mvp/GetWorkingDaysByYear', queryParameters);

    final response = await http.get(uri).timeout(Duration(seconds: timeout));

    if (response.statusCode == StatusCode.UNAUTHORIZED) {
      navigator.currentState?.pushNamed(RouteName.home);
    }

    if (response.statusCode == StatusCode.OK) {
      final List<dynamic> decodedJson = jsonDecode(response.body);

      // return decodedJson
      //     .map((dynamic json) => WorkingDaysByYear.fromJson(json))
      //     .toList();
    } else if (response.statusCode == 204) {
      // return [];
    } else {
      throw Exception('Failed');
    }
  }
}
