import 'dart:convert';
import 'dart:io';
import 'package:finance_app/global_variables.dart';
import 'package:finance_app/route_generator.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:http/io_client.dart';
import 'package:http_status_code/http_status_code.dart';
// import 'package:http/https.dart' as http;

class DefaultClient {
  static String baseUrl = GlobalVariables.baseUrlLocal;

  bool trustSelfSigned = true;
  static final HttpClient httpClient = HttpClient()
    ..badCertificateCallback =
        ((X509Certificate cert, String host, int port) => true);

  IOClient https = IOClient(httpClient);
  final secureStorage = const FlutterSecureStorage();

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

  Future<dynamic> create(String path, String body) async {
    var uri = Uri.https(baseUrl, path);

    var headersToken = await getHeaders();

    final response = await https.post(uri, body: body, headers: headersToken);

    if (response.statusCode == StatusCode.CREATED) {
      return jsonDecode(response.body);
    }
    return jsonDecode(response.body);
  }

  Future<bool> delete(String path, Map<String, dynamic> queryParams) async {
    var uri = Uri.https(baseUrl, path, queryParams);
    var headersToken = await getHeaders();

    final response = await https.delete(uri, headers: headersToken);

    if (response.statusCode == StatusCode.OK) {
      return true;
    }
    return false;
  }

  Future<bool> update(String path, String body) async {
    var uri = Uri.https(baseUrl, path);

    var headersToken = await getHeaders();

    final response = await https.put(uri, body: body, headers: headersToken);

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
      var headersToken = await getHeaders();

      final response = await https
          .get(uri, headers: headersToken)
          .timeout(Duration(seconds: timeout));

      if (response.statusCode == StatusCode.OK) {
        return response.body;
      } else if (response.statusCode == StatusCode.UNAUTHORIZED) {
        //Tenta logar novamente
        await secureStorage.delete(key: GlobalVariables.userToken);
        //Enviar para tela de login
        navigator.currentState?.pushReplacementNamed(RouteName.login);
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

  getHeaders() async {
    String token = await secureStorage.read(key: "USER_TOKEN") ?? "";
    if (token == "") {
      navigator.currentState?.pushReplacementNamed(RouteName.login);
    }
    Map<String, String> headers = {
      'Content-Type': 'application/json',
      'Accept': 'application/json',
      'Authorization': 'Bearer $token'
    };
    return headers;
  }
}
