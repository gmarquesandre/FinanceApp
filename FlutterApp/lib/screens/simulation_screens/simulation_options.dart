import 'package:financial_app/screens/simulation_screens/asset_simulation/simulation_asset_list.dart';
import 'package:financial_app/screens/simulation_screens/fund_simulation/simulation_fund_list.dart';
import 'package:financial_app/screens/simulation_screens/simulation.dart';
import 'package:flutter/material.dart';

class PreDashboardScreen extends StatelessWidget {
  const PreDashboardScreen({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text("Simulação"),
      ),
      body: Container(
        child: SingleChildScrollView(
          child: Column(
            children: [
              _CardButton(
                Icons.insert_chart,
                "Rendimento Ações",
                AssetSimulationConfig(),
              ),
              _CardButton(
                Icons.insert_chart,
                "Rendimento Fundos",
                FundSimulationConfig(),
              ),
              _CardButton(
                Icons.attach_money,
                "Simulação",
                SimulationScreen(),
              ),
            ],
          ),
        ),
      ),
    );
  }
}

class _CardButton extends StatelessWidget {
  final IconData _iconName;
  final String _textName;
  final _route;

  _CardButton(this._iconName, this._textName, this._route);

  @override
  Widget build(BuildContext context) {
    return Card(
      child: ListTile(
        onTap: () {
          Navigator.of(context).push(
            MaterialPageRoute(
              builder: (context) => _route,
            ),
          );
        },
        trailing: Icon(_iconName, size: 32),
        title: Text(
          _textName,
        ),
      ),
    );
  }
}
