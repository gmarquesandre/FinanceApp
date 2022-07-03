import 'package:flutter/material.dart';

final GlobalKey<NavigatorState> navigator = GlobalKey<NavigatorState>();

class GlobalVariables {
  static String baseUrl = '';

  static String baseUrlLocal = 'https://localhost:7167';

  static int requestTimeout = 3000;
}
