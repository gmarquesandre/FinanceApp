import 'package:financial_app/database/getDatabase.dart';
import 'package:financial_app/screens/asset_screens/assets_list.dart';
import 'package:financial_app/screens/currentBalance_screens/currentValue_form.dart';
import 'package:financial_app/screens/fgts_screens/fgts_form.dart';
import 'package:financial_app/screens/fixedInterest_screens/fixedInterest_list.dart';
import 'package:financial_app/screens/income_screens/income_list.dart';
import 'package:financial_app/screens/investmentFund_screens/investmentFund_list.dart';
import 'package:financial_app/screens/loan_screens/loan_list.dart';
import 'package:financial_app/screens/simulation_screens/simulation_options.dart';
import 'package:financial_app/screens/spending_screens/spending_list.dart';
import 'package:financial_app/screens/treasure_screens/treasury_list.dart';
import 'package:flutter/material.dart';

class Dashboard extends StatefulWidget {
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
        items: <BottomNavigationBarItem>[
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
        return _HomeScreen();
      case 0:
        return _TabList(); // Create this function, it should return your first page as a widget
      case 2:
        return PreDashboardScreen(); // Create this function, it should return your second page as a widget
    }

    return Center(
      child: Text("There is no page builder for this index."),
    );
  }
}

class _HomeScreen extends StatelessWidget {
  const _HomeScreen({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text("Inicio")),
      body: SingleChildScrollView(
        child: Column(
          children: [
            // ElevatedButton(child: Text('Resetar Banco de Dados'),
            //   onPressed: () async {
            //     await resetDatabaseFunc();
            //     final snackBar = SnackBar(
            //       duration: const Duration(seconds: 2),
            //       content: Text('Banco Resetado.'),
            //     );
            //     ScaffoldMessenger.of(context).showSnackBar(snackBar);
            //   },),
            Padding(
              padding: const EdgeInsets.only(top: 10),
              child: Container(
                width: MediaQuery
                    .of(context)
                    .size
                    .width,
                height: 100,
                decoration: ShapeDecoration(
                  color: Colors.white60,
                  shape: OutlineInputBorder(
                    borderRadius: BorderRadius.circular(10),
                  ),
                ),
              ),
            ),
            Padding(
              padding: const EdgeInsets.only(top: 10),
              child: Container(
                width: MediaQuery
                    .of(context)
                    .size
                    .width,
                height: 300,
                decoration: ShapeDecoration(
                  color: Colors.white60,
                  shape: OutlineInputBorder(
                    borderRadius: BorderRadius.circular(10),
                  ),
                ),
              ),
            ),
            Padding(
              padding: const EdgeInsets.only(top: 10),
              child: Container(
                width: MediaQuery
                    .of(context)
                    .size
                    .width,
                height: 300,
                decoration: ShapeDecoration(
                  color: Colors.white60,
                  shape: OutlineInputBorder(
                    borderRadius: BorderRadius.circular(10),
                  ),
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}

class _TabList extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return DefaultTabController(
      length: 9,
      child: Scaffold(
        appBar: AppBar(
          title: Text("Cadastros"),
          bottom: TabBar(
            indicatorColor: Colors.blueGrey,
            isScrollable: true,
            onTap: (value) {
              debugPrint(value.toString());
            },
            tabs: [
              Tab(text: "Saldo Corrente"),
              Tab(text: "Rendas"),
              Tab(text: "Gastos"),
              Tab(text: "Tesouro Direto"),
              Tab(text: "Renda Fixa"),
              Tab(text: "Ativos"),
              Tab(text: "Fundos"),
              Tab(text: "Empréstimo/Financiamento"),
              Tab(text: "FGTS"),
            ],
          ),
        ),
        body: TabBarView(
          children: [
            CurrentBalanceForm(),
            IncomeList(),
            SpendingList(),
            TreasuryList(),
            FixedInterestList(),
            AssetsList(),
            FundList(),
            LoanList(),
            FgtsForm(),
          ],
        ),
      ),
    );
  }
}
