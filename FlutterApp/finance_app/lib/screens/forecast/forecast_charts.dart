import 'package:finance_app/components/app_bar.dart';
import 'package:finance_app/components/padding.dart';
import 'package:finance_app/components/progress.dart';
import 'package:finance_app/clients/forecast_client/forecast_client.dart';
import 'package:finance_app/models/forecast/forecast_item.dart';
import 'package:finance_app/models/forecast/forecast_list.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:syncfusion_flutter_charts/charts.dart';

@override
class ForecastCharts extends StatefulWidget {
  const ForecastCharts({Key? key}) : super(key: key);

  @override
  State<ForecastCharts> createState() => _ForecastChartsState();
}

class _ForecastChartsState extends State<ForecastCharts> {
  final currencyFormat = NumberFormat.currency(locale: "pt_BR", symbol: "R\$");

  ForecastClient forecastClient = ForecastClient();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: appBarLoggedInDefault("Simulação"),
      body: defaultBodyPadding(
        SingleChildScrollView(
          child: FutureBuilder(
            future: forecastClient.get(),
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
                      // Text(spending.first),
                      GetPatrimony(spending.first),
                      GetTest(spending.first)
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
  GetPatrimony(this.spending, {Key? key}) : super(key: key);

  final ForecastList spending;
  final TooltipBehavior _tooltipBehavior = TooltipBehavior(
    enable: true,
    shared: true,
    // duration: GlobalVariables.durationTooltip,
  );

  @override
  Widget build(BuildContext context) {
    return ExpansionTile(
      initiallyExpanded: true,
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
                yValueMapper: (ForecastItem value, _) => value.realAmount,
                enableTooltip: true),
          ],
          primaryXAxis: DateTimeCategoryAxis(
            title: AxisTitle(
                text: 'Mês', textStyle: const TextStyle(fontSize: 12)),
            edgeLabelPlacement: EdgeLabelPlacement.shift,
            dateFormat: DateFormat("M/yy"),
            intervalType: DateTimeIntervalType.months,
          ),
          primaryYAxis: NumericAxis(
            decimalPlaces: 0,
            numberFormat: NumberFormat.compactCurrency(
                decimalDigits: 0, locale: 'pt-BR', symbol: 'R\$'),
          ),
          axes: <ChartAxis>[
            NumericAxis(
                name: 'um',
                minimum: 0.00,
                maximum: 110.00,
                // labelFormat: '{value}%',
                opposedPosition: true,
                maximumLabels: 2)
          ],
        ),
      ],
    );
  }
}

class GetTest extends StatelessWidget {
  GetTest(this.spending, {Key? key}) : super(key: key);

  final ForecastList spending;
  final TooltipBehavior _tooltipBehavior = TooltipBehavior(
    enable: true,
    shared: true,
    // duration: GlobalVariables.durationTooltip,
  );

  @override
  Widget build(BuildContext context) {
    return ExpansionTile(
      initiallyExpanded: true,
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
          tooltipBehavior: _tooltipBehavior,
          plotAreaBorderWidth: 0,
          title: ChartTitle(text: 'Inflation - Consumer price'),
          legend: Legend(
              isVisible: true, overflowMode: LegendItemOverflowMode.wrap),
          primaryXAxis: DateTimeCategoryAxis(
            title: AxisTitle(
                text: 'Mês', textStyle: const TextStyle(fontSize: 12)),
            edgeLabelPlacement: EdgeLabelPlacement.shift,
            dateFormat: DateFormat("M/yy"),
            intervalType: DateTimeIntervalType.months,
          ),
          series: <SplineSeries<ForecastItem, DateTime>>[
            SplineSeries<ForecastItem, DateTime>(
              dataSource: spending.items,
              xValueMapper: (ForecastItem sales, _) => sales.dateReference,
              yValueMapper: (ForecastItem sales, _) => sales.realAmount,
              markerSettings: const MarkerSettings(isVisible: true),
              name: 'Valor Real',
            ),
            SplineSeries<ForecastItem, DateTime>(
              dataSource: spending.items,
              name: 'Valor Nominal',
              markerSettings: const MarkerSettings(isVisible: true),
              xValueMapper: (ForecastItem sales, _) => sales.dateReference,
              yValueMapper: (ForecastItem sales, _) => sales.nominalAmount,
            )
          ],
        )
      ],
    );
  }
}
