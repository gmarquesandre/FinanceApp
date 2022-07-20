import 'dart:convert';
import 'dart:io';
import 'package:finance_app/global_variables.dart';
import 'package:finance_app/route_generator.dart';
import 'package:http/io_client.dart';
import 'package:http_status_code/http_status_code.dart';
// import 'package:http/http.dart' as http;

class DefaultClient {
  static String baseUrl = GlobalVariables.baseUrlLocal;

  static Map<String, String> headers = {
    'Content-Type': 'application/json',
    'Accept': 'application/json',
    'Authorization': 'Bearer $token',
  };

  bool trustSelfSigned = true;
  static final HttpClient httpClient = HttpClient()
    ..badCertificateCallback =
        ((X509Certificate cert, String host, int port) => true);

  IOClient http = IOClient(httpClient);

  static String token =
      "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiYWRtaW4iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEiLCJleHAiOjE2ODk3ODA4NzF9.PdBKeDR0njkS6-WZ_gpxI4vZRFLjN1bh4CsOwch5e6s";

  static int timeout = GlobalVariables.requestTimeout;

  Future<dynamic> getSingle(String path,
      {Map<String, String>? parameters}) async {
    String responseBody = await _get(path, parameters: parameters);

    dynamic decodedJson = jsonDecode(responseBody);

    return decodedJson;
  }

  Future<List<dynamic>> getMany(String path,
      [Map<String, String>? parameters]) async {
    String responseBody = await _get(path, parameters: parameters);

    List<dynamic> decodedJson = jsonDecode(responseBody);
    return decodedJson;
  }

  Future<bool> create(String path, String body) async {
    var uri = Uri.https(baseUrl, path);

    final response = await http.post(uri, body: body, headers: headers);

    if (response.statusCode == StatusCode.CREATED) {
      return true;
    }
    return false;
  }

  Future<bool> delete(String path, Map<String, dynamic> queryParams) async {
    var uri = Uri.https(baseUrl, path, queryParams);

    final response = await http.delete(uri, headers: headers);

    if (response.statusCode == StatusCode.OK) {
      return true;
    }
    return false;
  }

  Future<bool> update(String path, String body) async {
    var uri = Uri.https(baseUrl, path);

    final response = await http.put(uri, body: body, headers: headers);

    if (response.statusCode == StatusCode.OK) {
      return true;
    }
    if (response.statusCode == StatusCode.CREATED) {
      return true;
    }
    return false;
  }

  Future<String> _get(String path, {Map<String, String>? parameters}) async {
    var uri = Uri.https(baseUrl, path, parameters);
    try {
      final response = await http
          .get(uri, headers: headers)
          .timeout(Duration(seconds: timeout));

      if (response.statusCode == StatusCode.OK) {
        return response.body;
      } else if (response.statusCode == StatusCode.UNAUTHORIZED) {
        //Tenta logar novamente

        //Enviar para tela de login
        // navigator.currentState?.pushReplacementNamed(RouteName.home);
        throw Exception('Failed');
      } else {
        throw Exception('Failed');
      }
    } on Exception catch (_) {
      //Checar se tem internet ou a API que está fora.
      navigator.currentState?.pushReplacementNamed(RouteName.alert);
      throw Exception('Failed');
    }
  }

  // Future<void> post() async {
  //   var uri = Uri.https(baseUrl, '/mvp/PostAssetEarnings');

  //   final String json = jsonEncode(list.map((e) => e.toJson()).toList());

  //   final response = await http.post(
  //     uri,
  //     body: json.toString(),
  //     headers: {'Content-type': 'application/json'},
  //   );

  //   if (response.statusCode == 200) {
  //     final List<dynamic> decodedJson = jsonDecode(response.body);

  //     return decodedJson
  //         .map((dynamic json) => AssetEarnings.fromJson(json))
  //         .toList();
  //   } else {
  //     throw Exception('Failed');
  //   }
  // }
}
