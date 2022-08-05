import 'package:flutter/material.dart';

final GlobalKey<NavigatorState> navigator = GlobalKey<NavigatorState>();

class GlobalVariables {
  static String baseUrl = '';

  // static String baseUrlLocal = '10.0.2.2:7167';
  static String baseUrlLocal = 'localhost:7167';
  // static String baseUrlLocal = 'financeappapi20220804005248.azurewebsites.net';

  static int requestTimeout = 3000;

  static String userToken = 'USER_TOKEN';
}
