import 'package:finance_app/components/progress.dart';
import 'package:finance_app/controllers/forecast_client/forecast_client.dart';
import 'package:finance_app/models/forecast/forecast_item.dart';
import 'package:finance_app/models/forecast/forecast_list.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:syncfusion_flutter_charts/charts.dart';

@override
class ForecastScreen extends StatefulWidget {
  @override
  State<ForecastScreen> createState() => _ForecastScreenState();
}

class _ForecastScreenState extends State<ForecastScreen> {
  final currencyFormat = NumberFormat.currency(locale: "pt_BR", symbol: "R\$");

  ForecastClient forecastClient = ForecastClient();

  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Simulação'),
      ),
      body: Padding(
        padding: const EdgeInsets.all(8.0),
        child: SingleChildScrollView(
          child: FutureBuilder(
            future: forecastClient.get(),
            builder: (context, snapshot) {
              switch (snapshot.connectionState) {
                case ConnectionState.none:
                  break;
                case ConnectionState.waiting:
                  return const Progress();
                case ConnectionState.active:
                  // TODO: Handle this case.
                  break;
                case ConnectionState.done:
                  final List<ForecastList> spending =
                      snapshot.data as List<ForecastList>;
                  if (spending.isEmpty) {
                    return const Text("Não há dados para mostrar.");
                  }
                  return Column(
                    children: [
                      // Text(spending.first),
                      GetPatrimony(spending.first),
                    ],
                  );
              }
              return const Text('Erro Desconhecido');
            },
          ),
        ),
      ),
    );
  }
}

class GetPatrimony extends StatelessWidget {
  GetPatrimony(this.spending);

  final ForecastList spending;
  final TooltipBehavior _tooltipBehavior = TooltipBehavior(
    enable: true,
    shared: true,
    // duration: GlobalVariables.durationTooltip,
  );

  @override
  Widget build(BuildContext context) {
    return ExpansionTile(
      initiallyExpanded: false,
      collapsedTextColor: Colors.white,
      textColor: Colors.white,
      collapsedIconColor: Colors.white,
      iconColor: Colors.white,
      title: const Text(
        "Patrimônio Liquido",
      ),
      leading: const Icon(Icons.bar_chart_outlined, color: Colors.white),
      children: [
        SfCartesianChart(
          legend: Legend(
              // isVisible: true,
              position: LegendPosition.bottom,
              overflowMode: LegendItemOverflowMode.wrap),
          tooltipBehavior: _tooltipBehavior,
          series: <ChartSeries>[
            StackedColumnSeries<ForecastItem, DateTime>(
                name: 'Patrimônio Liquido',
                dataSource: spending.items,
                color: Colors.white,
                xValueMapper: (ForecastItem value, _) => value.dateReference,
                yValueMapper: (ForecastItem value, _) => value.amount,
                enableTooltip: true),
          ],
          primaryXAxis: DateTimeCategoryAxis(
            edgeLabelPlacement: EdgeLabelPlacement.shift,
            dateFormat: DateFormat("d/M/yy"),
            intervalType: DateTimeIntervalType.months,
          ),
          primaryYAxis: NumericAxis(
            numberFormat: NumberFormat.simpleCurrency(),
          ),
          axes: <ChartAxis>[
            NumericAxis(
                name: 'um',
                minimum: 0.00,
                maximum: 110.00,
                labelFormat: '{value}%',
                opposedPosition: true,
                maximumLabels: 2)
          ],
        ),
      ],
    );
  }
}
