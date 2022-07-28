import 'package:finance_app/clients/login_client.dart';
import 'package:finance_app/route_generator.dart';
import 'package:flutter/material.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';

class Login extends StatefulWidget {
  const Login({Key? key}) : super(key: key);
  @override
  State<Login> createState() => _LoginState();
}

class _LoginState extends State<Login> {
  final secureStorage = const FlutterSecureStorage();
  var loginClient = LoginClient();
  var password = TextEditingController();
  var username = TextEditingController();
  final bool _savePassword = true;

  onSubmit() async {
    if (_savePassword) {
      await secureStorage.write(key: "USER_NAME", value: username.text);
      await secureStorage.write(key: "USER_PASSWORD", value: password.text);
    }
  }

  _readFromStorage() async {
    username.text = await secureStorage.read(key: "USER_NAME") ?? "";
    password.text = await secureStorage.read(key: "USER_PASSWORD") ?? "";
  }

  @override
  void initState() {
    super.initState();
    _readFromStorage();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("Login"),
      ),
      body: Padding(
        padding: const EdgeInsets.all(8.0),
        child: Column(
          children: [
            Padding(
              padding: const EdgeInsets.only(top: 8.0),
              child: TextField(
                controller: username,
                decoration: const InputDecoration(
                  labelText: 'Usuário',
                ),
              ),
            ),
            Padding(
              padding: const EdgeInsets.only(top: 8.0),
              child: TextField(
                controller: password,
                obscureText: true,
                enableSuggestions: false,
                autocorrect: false,
                decoration: const InputDecoration(
                  labelText: 'Senha',
                ),
              ),
            ),
            Padding(
              padding: const EdgeInsets.only(top: 16.0),
              child: SizedBox(
                width: double.maxFinite,
                child: ElevatedButton(
                  child: const Text('Entrar'),
                  onPressed: () async {
                    await loginClient.login(username.text, password.text).then(
                      (resp) async {
                        if (resp) {
                          await onSubmit();

                          Navigator.of(context)
                              .pushReplacementNamed(RouteName.dashboard);

                          const snackBar = SnackBar(
                            duration: Duration(seconds: 2),
                            content: Text('Bem Vindo.'),
                          );
                          ScaffoldMessenger.of(context).showSnackBar(snackBar);
                        } else {
                          const snackBar = SnackBar(
                            duration: Duration(seconds: 2),
                            content: Text('Usuário ou senha incorretos'),
                          );
                          ScaffoldMessenger.of(context).showSnackBar(snackBar);
                        }
                      },
                    );
                  },
                ),
              ),
            )
          ],
        ),
      ),
    );
  }
}
