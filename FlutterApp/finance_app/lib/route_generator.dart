import 'package:finance_app/login.dart';
import 'package:finance_app/main.dart';
import 'package:finance_app/screens/currentBalance_screens/current_value_form.dart';
import 'package:finance_app/screens/dashboard.dart';
import 'package:flutter/material.dart';

class RouteName {
  static const String home = 'home';

  static const String alert = 'alert';

  static const String currentBalance = 'currentBalance';

  static const String dashboard = 'dashboard';

  static const String login = 'login';
}

class RouteGenerator {
  static Route<dynamic> generateRoute(RouteSettings settings) {
    // final args = settings.arguments;

    switch (settings.name) {
      case RouteName.home:
        return MaterialPageRoute(builder: (_) => const MyWidget());
      case RouteName.alert:
        return MaterialPageRoute(builder: (_) => const Popup());
      case RouteName.currentBalance:
        return MaterialPageRoute(builder: (_) => const CurrentBalanceForm());
      case RouteName.dashboard:
        return MaterialPageRoute(builder: (_) => const Dashboard());
      case RouteName.login:
        return MaterialPageRoute(builder: (_) => const Login());
      default:
        return _errorRoute();
    }
  }

  static Route<dynamic> _errorRoute() {
    return MaterialPageRoute(builder: (_) {
      return Scaffold(
          appBar: AppBar(
            backgroundColor: Colors.green,
            title: const Text('Erro'),
          ),
          body: const Center(
            child: Text("Erro: Rota não encotrada"),
          ));
    });
  }
}
