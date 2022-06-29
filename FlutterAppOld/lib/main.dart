import 'package:financial_app/functions/assetCurrentPosition.dart';
import 'package:financial_app/functions/fixedInterestPosition.dart';
import 'package:financial_app/functions/fundCurrentPosition.dart';
import 'package:financial_app/functions/treasuryCurrentPosition.dart';
import 'package:financial_app/providers/assetCurrentPositionList.dart';
import 'package:financial_app/providers/assetTransactionList.dart';
import 'package:financial_app/providers/balanceMonthList.dart';
import 'package:financial_app/providers/fixedInterestListProvider.dart';
import 'package:financial_app/providers/investmentFundCurrentPositionList.dart';
import 'package:financial_app/providers/investmentFundTransactionList.dart';
import 'package:financial_app/providers/treasuryCurrentPositionList.dart';
import 'package:financial_app/providers/treasuryTransactionList.dart';
import 'package:financial_app/screens/dashboard.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:syncfusion_localizations/syncfusion_localizations.dart';
import 'package:flutter_localizations/flutter_localizations.dart';

void main() {
  runApp(MultiProvider(providers: [
    ChangeNotifierProvider(
      create: (context) => BalanceMonthList(),
    ),
    ChangeNotifierProvider(
      create: (context) => AssetTransactionList(),
    ),
    ChangeNotifierProvider(
      create: (context) => AssetCurrentPositionList(),
    ),
    ChangeNotifierProvider(
      create: (context) => TreasuryTransactionList(),
    ),
    ChangeNotifierProvider(
      create: (context) => TreasuryCurrentPositionList(),
    ),
    ChangeNotifierProvider(
      create: (context) => FundCurrentPositionList(),
    ),
    ChangeNotifierProvider(
      create: (context) => FundTransactionList(),
    ),
    ChangeNotifierProvider(
      create: (context) => FixedInterestListProvider(),
    ),
  ], child: MyApp()));
}

class MyApp extends StatelessWidget {
  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    fixedInterestPosition(context);
    assetCurrentPosition(context);
    treasuryCurrentPosition(context);
    fundCurrentPosition(context);

    return MaterialApp(
      localizationsDelegates: [
        GlobalMaterialLocalizations.delegate,
        GlobalWidgetsLocalizations.delegate,
        SfGlobalLocalizations.delegate,
      ],
      supportedLocales: [
        const Locale('pt'),
      ],
      locale: const Locale('pt'),
      themeMode: ThemeMode.dark,
      darkTheme: ThemeData(
        brightness: Brightness.dark,
        canvasColor: Colors.blueGrey,
        timePickerTheme: TimePickerThemeData(),
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
        buttonTheme: ButtonThemeData(
          buttonColor: Colors.blueGrey,
        ),
        floatingActionButtonTheme:
            FloatingActionButtonThemeData(backgroundColor: Colors.blueGrey),
        snackBarTheme: SnackBarThemeData(),
        inputDecorationTheme: InputDecorationTheme(
          hoverColor: Colors.white,
          filled: false,
          border:  OutlineInputBorder(borderRadius: BorderRadius.circular(10),),
          enabledBorder: UnderlineInputBorder(
            borderSide: BorderSide(color: Colors.blueGrey, width: 2),
          ),
          disabledBorder: UnderlineInputBorder(

            borderSide: BorderSide(color: Colors.white, width: 2),
          ),
          helperStyle: TextStyle(color: Colors.white),
          floatingLabelStyle: TextStyle(color: Colors.white),
          counterStyle: TextStyle(color: Colors.white),
          suffixStyle: TextStyle(color: Colors.white),
          errorStyle: TextStyle(color: Colors.white),
          prefixStyle: TextStyle(color: Colors.white),
          labelStyle: TextStyle(
            color: Colors.white,
          ),
          hintStyle: TextStyle(
            color: Colors.white,
          ),
          fillColor: Colors.greenAccent,
          focusColor: Colors.green,
        ),
        unselectedWidgetColor: Colors.indigoAccent,
        appBarTheme: AppBarTheme(
          backgroundColor: Colors.black,
        ),

        scaffoldBackgroundColor: Colors.black,
        backgroundColor: Colors.black,
        tabBarTheme: TabBarTheme(
          labelColor: Colors.blueGrey,
          unselectedLabelColor: Colors.white,
        ),
        bottomAppBarColor: Colors.blueGrey[1200],
        bottomAppBarTheme: BottomAppBarTheme(
          elevation: 120,
          shape: CircularNotchedRectangle(

          ),
        ),
        bottomNavigationBarTheme: BottomNavigationBarThemeData(
          backgroundColor: Colors.blueGrey,
          unselectedItemColor: Colors.white,
          selectedItemColor: Colors.white,
          unselectedIconTheme: IconThemeData(color: Colors.white, size: 32),
          selectedIconTheme: IconThemeData(color: Colors.yellowAccent, size: 42),
          unselectedLabelStyle: TextStyle(
            color: Colors.white,
            fontSize: 12,
          ),
          selectedLabelStyle: TextStyle(color: Colors.blue, fontSize: 12),
        ),

        dataTableTheme: DataTableThemeData(
          headingRowHeight: 100 ,
          headingTextStyle: TextStyle(
            fontSize: 14,

          ),
          dataTextStyle: TextStyle(
            fontSize: 12,
          ),
          dividerThickness: 30,
          // headingRowColor: MaterialStateProperty.all(Colors.blueGrey),
          dataRowColor: MaterialStateProperty.all(Colors.blueGrey[900]),

        ),
        iconTheme: IconThemeData(
          color: Colors.white
        ),
        cardTheme: CardTheme(
          color: Colors.blueGrey,
          elevation: 12,
        ),
        cardColor: Colors.white,
        textTheme: TextTheme(
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
              fontSize: 18  , fontFamily: 'Roboto', color: Colors.white),
          bodyText2: TextStyle(
              fontSize: 12, fontFamily: 'Roboto', color: Colors.white),
        ),

      ),
      home: Dashboard(),
    );
  }
}

