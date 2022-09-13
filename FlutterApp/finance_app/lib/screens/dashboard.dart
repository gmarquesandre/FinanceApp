import 'package:finance_app/clients/forecast_client/forecast_client.dart';
import 'package:finance_app/components/app_bar.dart';
import 'package:finance_app/screens/currentBalance_screens/current_value_form.dart';
import 'package:finance_app/screens/fgts_screens/fgts_form.dart';
import 'package:finance_app/screens/forecast/forecast_charts.dart';
import 'package:finance_app/screens/forecast/forecast_options.dart';
import 'package:finance_app/screens/income/income_list.dart';
import 'package:finance_app/screens/loan_screens/loan_list.dart';
import 'package:finance_app/screens/spending_screens/spending_list.dart';
import 'package:flutter/material.dart';

import '../components/padding.dart';
import '../components/progress.dart';
import '../models/forecast/forecast_list.dart';

class Dashboard extends StatefulWidget {
  const Dashboard({Key? key}) : super(key: key);

  @override
  State<Dashboard> createState() => _DashboardState();
}

class _DashboardState extends State<Dashboard> {
  int _selectedIndex = 1;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: _getBody(_selectedIndex),
      bottomNavigationBar: BottomNavigationBar(
        currentIndex: _selectedIndex,
        onTap: (value) => setState(() => _selectedIndex = value),
        showSelectedLabels: false,
        showUnselectedLabels: true,
        items: const <BottomNavigationBarItem>[
          BottomNavigationBarItem(
            icon: Icon(Icons.layers),
            label: "Cadastro",
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.calendar_view_day_rounded),
            label: "Inicio",
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.stacked_line_chart_sharp),
            label: "Simulação",
          ),
        ],
        // fixedColor: Colors.white,
      ),
    );
  }

  Widget _getBody(int index) {
    switch (index) {
      case 1:
        return const _HomeScreen();
      case 0:
        return _TabList(); // Create this function, it should return your first page as a widget
      case 2:
        return const ForecastOptions(); // Create this function, it should return your second page as a widget
    }

    return const Center(
      child: Text("There is no page builder for this index."),
    );
  }
}

class _HomeScreen extends StatefulWidget {
  const _HomeScreen({Key? key}) : super(key: key);

  @override
  State<_HomeScreen> createState() => _HomeScreenState();
}

class _HomeScreenState extends State<_HomeScreen> {
  ForecastClient forecastClient = ForecastClient();
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: appBarLoggedInDefault("Inicio"),
      body: SingleChildScrollView(
        child: defaultBodyPadding(
          SingleChildScrollView(
            child: FutureBuilder(
              future: forecastClient.getTwoWeeks(),
              builder: (context, snapshot) {
                switch (snapshot.connectionState) {
                  case ConnectionState.none:
                    break;
                  case ConnectionState.waiting:
                    return const Progress();
                  case ConnectionState.active:
                    break;
                  case ConnectionState.done:
                    final List<ForecastList> spending =
                        snapshot.data as List<ForecastList>;
                    if (spending.isEmpty) {
                      return const Text("Não há dados para mostrar.");
                    }
                    return Column(
                      children: [
                        GetBalanceChart(
                            spending.firstWhere((a) => a.type == 0),
                            spending.firstWhere((a) => a.type == 1),
                            spending.firstWhere((a) => a.type == 9999)),
                      ],
                    );
                }
                return const Text('Erro Desconhecido');
              },
            ),
          ),
        ),
      ),
    );
  }
}

class _TabList extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return DefaultTabController(
      length: _tabs.length,
      child: Scaffold(
        appBar: AppBar(
          title: const Text("Cadastros"),
          bottom: TabBar(
            indicatorColor: Colors.blueGrey,
            isScrollable: true,
            tabs: _tabs,
          ),
        ),
        body: const TabBarView(
          children: [
            CurrentBalanceForm(),
            IncomeList(),
            SpendingList(),
            // TreasuryList(),
            // FixedInterestList(),
            // AssetsList(),
            // FundList(),
            LoanList(),
            FGTSForm(),
          ],
        ),
      ),
    );
  }

  List<Widget> get _tabs {
    return const [
      Tab(text: "Saldo Corrente"),
      Tab(text: "Rendas"),
      Tab(text: "Gastos"),
      // Tab(text: "Tesouro Direto"),
      // Tab(text: "Renda Fixa"),
      // Tab(text: "Ativos"),
      // Tab(text: "Fundos"),
      Tab(text: "Empréstimo/Financiamento"),
      Tab(text: "FGTS"),
    ];
  }
}
