import 'package:finance_app/global_variables.dart';
import 'package:finance_app/http/current_balance_client.dart';
import 'package:finance_app/routes.dart';
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
    return MaterialApp(
      navigatorKey: navigator,
      onGenerateRoute: RouteGenerator.generateRoute,
      localizationsDelegates: const [
        GlobalMaterialLocalizations.delegate,
        GlobalWidgetsLocalizations.delegate,
        // SfGlobalLocalizations.delegate,
      ],
      supportedLocales: const [
        Locale('pt'),
      ],
      locale: const Locale('pt'),
      themeMode: ThemeMode.dark,
      darkTheme: ThemeData(
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
        floatingActionButtonTheme: const FloatingActionButtonThemeData(
            backgroundColor: Colors.blueGrey),
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
          helperStyle: const TextStyle(color: Colors.white),
          floatingLabelStyle: const TextStyle(color: Colors.white),
          counterStyle: const TextStyle(color: Colors.white),
          suffixStyle: const TextStyle(color: Colors.white),
          errorStyle: const TextStyle(color: Colors.white),
          prefixStyle: const TextStyle(color: Colors.white),
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
          unselectedIconTheme: IconThemeData(color: Colors.white, size: 32),
          selectedIconTheme:
              IconThemeData(color: Colors.yellowAccent, size: 42),
          unselectedLabelStyle: TextStyle(
            color: Colors.white,
            fontSize: 12,
          ),
          selectedLabelStyle: TextStyle(color: Colors.blue, fontSize: 12),
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
        iconTheme: const IconThemeData(color: Colors.white),
        cardTheme: const CardTheme(
          color: Colors.blueGrey,
          elevation: 12,
        ),
        cardColor: Colors.white,
        textTheme: const TextTheme(
          subtitle1: TextStyle(
              fontSize: 18, fontFamily: 'Roboto', color: Colors.white),
          subtitle2: TextStyle(
              fontSize: 18, fontFamily: 'Roboto', color: Colors.white),
          headline1: TextStyle(
              fontSize: 24, fontFamily: 'Roboto', color: Colors.white),
          headline2: TextStyle(
              fontSize: 24, fontFamily: 'Roboto', color: Colors.white),
          headline3: TextStyle(
              fontSize: 24, fontFamily: 'Roboto', color: Colors.white),
          headline4: TextStyle(
              fontSize: 24, fontFamily: 'Roboto', color: Colors.white),
          headline5: TextStyle(
              fontSize: 24, fontFamily: 'Roboto', color: Colors.white),
          headline6: TextStyle(
              fontSize: 24, fontFamily: 'Roboto', color: Colors.white),
          bodyText1: TextStyle(
              fontSize: 18, fontFamily: 'Roboto', color: Colors.white),
          bodyText2: TextStyle(
              fontSize: 12, fontFamily: 'Roboto', color: Colors.white),
        ),
      ),
      home: const MyHomePage(title: 'Flutter Demo Home Page'),
    );
  }
}

class MyHomePage extends StatefulWidget {
  const MyHomePage({Key? key, required this.title}) : super(key: key);

  // This widget is the home page of your application. It is stateful, meaning
  // that it has a State object (defined below) that contains fields that affect
  // how it looks.

  // This class is the configuration for the state. It holds the values (in this
  // case the title) provided by the parent (in this case the App widget) and
  // used by the build method of the State. Fields in a Widget subclass are
  // always marked "final".

  final String title;

  @override
  State<MyHomePage> createState() => _MyHomePageState();
}

class _MyHomePageState extends State<MyHomePage> {
  int _counter = 0;
  var client = CurrentBalanceClient();
  void _incrementCounter() {
    setState(() async {
      await client.get();
      // This call to setState tells the Flutter framework that something has
      // changed in this State, which causes it to rerun the build method below
      // so that the display can reflect the updated values. If we changed
      // _counter without calling setState(), then the build method would not be
      // called again, and so nothing would appear to happen.
      _counter++;
    });
  }

  @override
  Widget build(BuildContext context) {
    // This method is rerun every time setState is called, for instance as done
    // by the _incrementCounter method above.
    //
    // The Flutter framework has been optimized to make rerunning build methods
    // fast, so that you can just rebuild anything that needs updating rather
    // than having to individually change instances of widgets.
    return Scaffold(
      appBar: AppBar(
        // Here we take the value from the MyHomePage object that was created by
        // the App.build method, and use it to set our appbar title.
        title: Text(widget.title),
      ),
      body: Center(
        // Center is a layout widget. It takes a single child and positions it
        // in the middle of the parent.
        child: Column(
          // Column is also a layout widget. It takes a list of children and
          // arranges them vertically. By default, it sizes itself to fit its
          // children horizontally, and tries to be as tall as its parent.
          //
          // Invoke "debug painting" (press "p" in the console, choose the
          // "Toggle Debug Paint" action from the Flutter Inspector in Android
          // Studio, or the "Toggle Debug Paint" command in Visual Studio Code)
          // to see the wireframe for each widget.
          //
          // Column has various properties to control how it sizes itself and
          // how it positions its children. Here we use mainAxisAlignment to
          // center the children vertically; the main axis here is the vertical
          // axis because Columns are vertical (the cross axis would be
          // horizontal).
          mainAxisAlignment: MainAxisAlignment.center,
          children: <Widget>[
            const Text(
              'You have pushed the button this many times:',
            ),
            Text(
              '$_counter',
              style: Theme.of(context).textTheme.headline4,
            ),
          ],
        ),
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: _incrementCounter,
        tooltip: 'Increment',
        backgroundColor: Colors.green,
        child: const Icon(Icons.add),
      ), // This trailing comma makes auto-formatting nicer for build methods.
    );
  }
}
