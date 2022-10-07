import 'package:flutter/material.dart';

final GlobalKey<NavigatorState> navigator = GlobalKey<NavigatorState>();

class GlobalVariables {
  static String baseUrl = 'xx';

  // static String baseUrlLocal = 'ec2-3-85-109-240.compute-1.amazonaws.com:5000';
  // static String baseUrlLocal = 'localhost:7167';
  static String baseUrlLocal = '10.0.2.2:7167';

  static int requestTimeout = 100;

  static String userToken = 'USER_TOKEN';
}
