import 'dart:convert';
import 'dart:io';
import 'package:finance_app/global_variables.dart';
import 'package:finance_app/models/login_return.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:http/io_client.dart';
import 'package:http_status_code/http_status_code.dart';
// import 'package:http/http.dart' as http;

class LoginClient {
  static String baseUrl = GlobalVariables.baseUrlLocal;

  static Map<String, String> headers = {
    'Content-Type': 'application/json',
    'Accept': 'application/json'
  };

  bool trustSelfSigned = true;
  static final HttpClient httpClient = HttpClient()
    ..badCertificateCallback =
        ((X509Certificate cert, String host, int port) => true);

  IOClient http = IOClient(httpClient);

  static int timeout = GlobalVariables.requestTimeout;

  Future<bool> login(String username, String password) async {
    String path = "Login";
    var uri = Uri.http(baseUrl, path);

    var body = {"username": username, "password": password};

    const secureStorage = FlutterSecureStorage();

    final response =
        await http.post(uri, body: json.encode(body), headers: headers);

    if (response.statusCode == StatusCode.OK) {
      var loginReturn = LoginReturn.fromJson(jsonDecode(response.body));
      await secureStorage.write(
          key: GlobalVariables.userToken, value: loginReturn.message);
      return true;
    }
    await secureStorage.deleteAll();
    return false;
  }
}
