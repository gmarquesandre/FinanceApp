import 'package:finance_app/route_generator.dart';
import 'package:flutter/material.dart';

class Popup extends StatelessWidget {
  const Popup({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return AlertDialog(
      title: const Text('Falha'),
      content: Column(
        mainAxisSize: MainAxisSize.min,
        crossAxisAlignment: CrossAxisAlignment.start,
        children: const <Widget>[
          Text("Erro ao buscar dados"),
        ],
      ),
      actions: <Widget>[
        TextButton(
          onPressed: () {
            Navigator.of(context).pushReplacementNamed(RouteName.login);
          },
          child: const Text('Close'),
        ),
      ],
    );
  }
}
