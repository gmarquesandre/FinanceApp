import 'package:flutter/material.dart';

final GlobalKey<NavigatorState> navigator = GlobalKey<NavigatorState>();

class GlobalVariables {
  static String baseUrl = '';

  // static String baseUrlLocal = '10.0.2.2:7167';
  static String baseUrlLocal = 'localhost:7167';

  static int requestTimeout = 3000;
}
