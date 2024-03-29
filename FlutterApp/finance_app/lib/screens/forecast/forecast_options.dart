import 'package:finance_app/components/app_bar.dart';
import 'package:finance_app/screens/forecast/forecast_charts.dart';
import 'package:finance_app/screens/forecast/forecast_parameters.dart';
import 'package:flutter/material.dart';

class ForecastOptions extends StatelessWidget {
  const ForecastOptions({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: appBarLoggedInDefault("Simulação"),
      body: SingleChildScrollView(
        child: Column(
          children: const [
            _CardButton(
              Icons.keyboard_option_key,
              "Parâmetros",
              ForecastParameters(),
            ),
            _CardButton(
              Icons.insert_chart,
              "Gráficos",
              ForecastCharts(),
            ),
          ],
        ),
      ),
    );
  }
}

class _CardButton extends StatelessWidget {
  final IconData _iconName;
  final String _textName;
  final dynamic _route;

  const _CardButton(this._iconName, this._textName, this._route);

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
