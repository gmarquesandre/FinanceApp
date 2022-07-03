import 'package:finance_app/main.dart';
import 'package:flutter/material.dart';

class RouteName {
  static const String home = 'home';
}

class RouteGenerator {
  static Route<dynamic> generateRoute(RouteSettings settings) {
    final args = settings.arguments;

    switch (settings.name) {
      case RouteName.home:
        return MaterialPageRoute(builder: (_) => const MyApp());
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
