import 'package:finance_app/global_variables.dart';
import 'package:finance_app/route_generator.dart';
import 'package:flutter/material.dart';

AppBar appBarLoggedInDefault(String title) {
  return AppBar(
    title: Text(title),
    actions: [
      IconButton(
        icon: const Icon(Icons.exit_to_app),
        onPressed: () {
          navigator.currentState?.pushReplacementNamed(RouteName.login);
        },
      ),
    ],
  );
}
