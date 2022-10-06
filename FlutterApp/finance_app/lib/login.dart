import 'package:finance_app/clients/login_client.dart';
import 'package:finance_app/components/padding.dart';
import 'package:finance_app/components/progress.dart';
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
  bool savePassword = true;
  bool isLoading = false;

  onSubmit() async {
    if (savePassword) {
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
      body: isLoading
          ? const Progress()
          : defaultBodyPadding(
              Column(
                children: [
                  defaultInputPadding(
                    TextField(
                      controller: username,
                      decoration: const InputDecoration(
                        labelText: 'Usuário',
                      ),
                    ),
                  ),
                  defaultInputPadding(
                    TextField(
                      controller: password,
                      obscureText: true,
                      enableSuggestions: false,
                      autocorrect: false,
                      decoration: const InputDecoration(
                        labelText: 'Senha',
                      ),
                    ),
                  ),
                  defaultInputPadding(
                    SizedBox(
                      width: double.maxFinite,
                      child: ElevatedButton(
                        child: const Text('Entrar'),
                        onPressed: () async {
                          isLoading = true;
                          setState(() {});
                          await loginClient
                              .login(username.text, password.text)
                              .catchError((err) => isLoading = false)
                              .then(
                            (resp) async {
                              if (resp) {
                                await onSubmit();

                                Navigator.of(context)
                                    .pushReplacementNamed(RouteName.dashboard);

                                const snackBar = SnackBar(
                                  duration: Duration(seconds: 2),
                                  content: Text('Bem Vindo.'),
                                );
                                ScaffoldMessenger.of(context)
                                    .showSnackBar(snackBar);
                              } else {
                                const snackBar = SnackBar(
                                  duration: Duration(seconds: 2),
                                  content: Text('Usuário ou senha incorretos'),
                                );
                                ScaffoldMessenger.of(context)
                                    .showSnackBar(snackBar);
                              }
                              isLoading = false;
                              setState(() {});
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
