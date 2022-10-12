import 'dart:io';
import 'package:finance_app/global_variables.dart';
import 'package:finance_app/login.dart';
import 'package:finance_app/route_generator.dart';
import 'package:flutter/material.dart';
import 'package:flutter_localizations/flutter_localizations.dart';

void main() {
  runApp(const MyApp());
}

class MyApp extends StatelessWidget {
  const MyApp({Key? key}) : super(key: key);

  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    var materialApp = MaterialApp(
      navigatorKey: navigator,
      onGenerateRoute: RouteGenerator.generateRoute,
      // localizationsDelegates: const [
      //   GlobalMaterialLocalizations.delegate,
      //   GlobalWidgetsLocalizations.delegate,
      // SfGlobalLocalizations.delegate,
      // ],
      // supportedLocales: const [
      //   Locale('pt'),
      // ],
      // locale: const Locale('pt'),
      themeMode: ThemeMode.dark,
      darkTheme: _darkTheme,
      home: const Login(),
    );
    return materialApp;
  }
}

class MyHttpOverrides extends HttpOverrides {
  @override
  HttpClient createHttpClient(SecurityContext? context) {
    return super.createHttpClient(context)
      ..badCertificateCallback =
          (X509Certificate cert, String host, int port) => true;
  }
}

final ThemeData _darkTheme = ThemeData(
  pageTransitionsTheme: const PageTransitionsTheme(builders: {
    TargetPlatform.iOS: CupertinoPageTransitionsBuilder(),
    TargetPlatform.android: OpenUpwardsPageTransitionsBuilder(),
  }),
  brightness: Brightness.dark,
  canvasColor: Colors.blueGrey,
  timePickerTheme: const TimePickerThemeData(),
  checkboxTheme: CheckboxThemeData(
    fillColor: MaterialStateProperty.all(Colors.blueGrey),
  ),
  primaryColor: Colors.blueGrey,
  elevatedButtonTheme: ElevatedButtonThemeData(
    style: ElevatedButton.styleFrom(
      shadowColor: Colors.blueGrey,
      primary: Colors.blueGrey,
      // side: BorderSide(color: Colors.blueGrey, width: 2),
    ),
  ),
  buttonTheme: const ButtonThemeData(
    buttonColor: Colors.blueGrey,
  ),
  floatingActionButtonTheme:
      const FloatingActionButtonThemeData(backgroundColor: Colors.blueGrey),
  snackBarTheme: const SnackBarThemeData(),
  inputDecorationTheme: InputDecorationTheme(
    hoverColor: Colors.white,
    filled: false,
    border: OutlineInputBorder(
      borderRadius: BorderRadius.circular(10),
    ),
    enabledBorder: const UnderlineInputBorder(
      borderSide: BorderSide(color: Colors.blueGrey, width: 2),
    ),
    disabledBorder: const UnderlineInputBorder(
      borderSide: BorderSide(color: Colors.white, width: 2),
    ),
    helperStyle: const TextStyle(
      color: Colors.white,
    ),
    floatingLabelStyle: const TextStyle(
      color: Colors.white,
    ),
    counterStyle: const TextStyle(
      color: Colors.white,
    ),
    suffixStyle: const TextStyle(
      color: Colors.white,
    ),
    errorStyle: const TextStyle(
      color: Colors.white,
    ),
    prefixStyle: const TextStyle(
      color: Colors.white,
    ),
    labelStyle: const TextStyle(
      color: Colors.white,
    ),
    hintStyle: const TextStyle(
      color: Colors.white,
    ),
    fillColor: Colors.greenAccent,
    focusColor: Colors.green,
  ),
  unselectedWidgetColor: Colors.indigoAccent,
  appBarTheme: const AppBarTheme(
    backgroundColor: Colors.black,
  ),
  scaffoldBackgroundColor: Colors.black,
  backgroundColor: Colors.black,
  tabBarTheme: const TabBarTheme(
    labelColor: Colors.blueGrey,
    unselectedLabelColor: Colors.white,
  ),
  bottomAppBarColor: Colors.blueGrey[1200],
  bottomAppBarTheme: const BottomAppBarTheme(
    elevation: 120,
    shape: CircularNotchedRectangle(),
  ),
  bottomNavigationBarTheme: const BottomNavigationBarThemeData(
    backgroundColor: Colors.blueGrey,
    unselectedItemColor: Colors.white,
    selectedItemColor: Colors.white,
    unselectedIconTheme: IconThemeData(
      color: Colors.white,
      size: 32,
    ),
    selectedIconTheme: IconThemeData(
      color: Colors.yellowAccent,
      size: 42,
    ),
    unselectedLabelStyle: TextStyle(
      color: Colors.white,
      fontSize: 12,
    ),
    selectedLabelStyle: TextStyle(
      color: Colors.blue,
      fontSize: 12,
    ),
  ),
  dataTableTheme: DataTableThemeData(
    headingRowHeight: 100,
    headingTextStyle: const TextStyle(
      fontSize: 14,
    ),
    dataTextStyle: const TextStyle(
      fontSize: 12,
    ),
    dividerThickness: 30,
    // headingRowColor: MaterialStateProperty.all(Colors.blueGrey),
    dataRowColor: MaterialStateProperty.all(Colors.blueGrey[900]),
  ),
  iconTheme: const IconThemeData(
    color: Colors.white,
  ),
  cardTheme: const CardTheme(
    color: Colors.blueGrey,
    elevation: 12,
  ),
  expansionTileTheme: const ExpansionTileThemeData(
    collapsedTextColor: Colors.white,
    textColor: Colors.white,
    collapsedIconColor: Colors.white,
    iconColor: Colors.white,
  ),
  cardColor: Colors.white,
  textTheme: const TextTheme(
    subtitle1: TextStyle(
      fontSize: 18,
      fontFamily: 'Roboto',
      color: Colors.white,
    ),
    subtitle2: TextStyle(
      fontSize: 18,
      fontFamily: 'Roboto',
      color: Colors.white,
    ),
    headline1: TextStyle(
      fontSize: 24,
      fontFamily: 'Roboto',
      color: Colors.white,
    ),
    headline2: TextStyle(
      fontSize: 24,
      fontFamily: 'Roboto',
      color: Colors.white,
    ),
    headline3: TextStyle(
      fontSize: 24,
      fontFamily: 'Roboto',
      color: Colors.white,
    ),
    headline4: TextStyle(
      fontSize: 24,
      fontFamily: 'Roboto',
      color: Colors.white,
    ),
    headline5: TextStyle(
      fontSize: 24,
      fontFamily: 'Roboto',
      color: Colors.white,
    ),
    headline6: TextStyle(
      fontSize: 24,
      fontFamily: 'Roboto',
      color: Colors.white,
    ),
    bodyText1: TextStyle(
      fontSize: 18,
      fontFamily: 'Roboto',
      color: Colors.white,
    ),
    bodyText2: TextStyle(
      fontSize: 12,
      fontFamily: 'Roboto',
      color: Colors.white,
    ),
  ),
);
