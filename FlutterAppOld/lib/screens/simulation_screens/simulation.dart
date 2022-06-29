import 'package:financial_app/components/globalVariables.dart';
import 'package:financial_app/components/progress.dart';
import 'package:financial_app/functions/dashboard.dart';
import 'package:financial_app/models/dto_models/balanceMonth.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:syncfusion_flutter_charts/charts.dart';

@override
class SimulationScreen extends StatefulWidget {
  @override
  State<SimulationScreen> createState() => _SimulationScreenState();
}

class _SimulationScreenState extends State<SimulationScreen> {
  final currencyFormat = NumberFormat.currency(locale: "pt_BR", symbol: "R\$");

  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Simulação'),
      ),
      body: Padding(
        padding: const EdgeInsets.all(8.0),
        child: SingleChildScrollView(
          child: FutureBuilder(
            future: getDashboardData(context),
            builder: (context, snapshot) {
              switch (snapshot.connectionState) {
                case ConnectionState.none:
                  break;
                case ConnectionState.waiting:
                  return Progress();
                case ConnectionState.active:
                  // TODO: Handle this case.
                  break;
                case ConnectionState.done:
                  final List<BalanceMonth> spending =
                      snapshot.data as List<BalanceMonth>;
                  if (spending.length == 0) {
                    return Text("Não há dados para mostrar.");
                  }
                  return Container(
                    child: Column(
                      children: [
                        GetPatrimony(spending),
                        GetLiquidityBalance(spending),
                        GetValueByTypeBalance(spending),
                        GetBalanceChart(spending),
                        GetSpendingChart(spending),
                        GetFgtsChart(spending),
                      ],
                    ),
                  );
              }
              return Text('Erro Desconhecido');
            },
          ),
        ),
      ),
    );
  }
}

class GetLiquidityBalance extends StatefulWidget {
  GetLiquidityBalance(this.spending);

  final List<BalanceMonth> spending;

  @override
  State<GetLiquidityBalance> createState() => _GetLiquidityBalanceState();
}

class _GetLiquidityBalanceState extends State<GetLiquidityBalance> {
  bool showAsPercentage = false;

  final TooltipBehavior _tooltipBehavior = TooltipBehavior(
    enable: true,
    shared: true,
    duration: GlobalVariables.durationTooltip,
  );

  @override
  Widget build(BuildContext context) {
    return ExpansionTile(
      initiallyExpanded: false,
      collapsedTextColor: Colors.white,
      textColor: Colors.white,
      collapsedIconColor: Colors.white,
      iconColor: Colors.white,
      title: Text(
        "Patrimônio por Liquidez",
      ),
      leading: Icon(Icons.bar_chart_outlined, color: Colors.white),
      children: [
        Visibility(
          visible:
              !widget.spending.any((element) => element.totalAvailable < 0),
          child: SizedBox(
            child: ElevatedButton(
                child: Text(showAsPercentage ? "Valor" : "Percentual"),
                onPressed: () async {
                  showAsPercentage = !showAsPercentage;
                  setState(() {});
                }),
          ),
        ),
        SfCartesianChart(
          legend: Legend(
              isVisible: true,
              position: LegendPosition.bottom,
              overflowMode: LegendItemOverflowMode.wrap),
          tooltipBehavior: _tooltipBehavior,
          series: <ChartSeries>[
            StepLineSeries<BalanceMonth, DateTime>(
              dataSource: widget.spending,
              name: 'Total',
              isVisibleInLegend: false,
              color: Colors.black,
              opacity: 0,
              xValueMapper: (BalanceMonth value, _) => value.date,
              yValueMapper: (BalanceMonth value, _) => showAsPercentage? 1 : value.totalPatrimony,
            ),
            StackedColumnSeries<BalanceMonth, DateTime>(
                name: 'Saldo Disponível',
                dataSource: widget.spending,
                color: Colors.deepPurple,
                xValueMapper: (BalanceMonth value, _) => value.date,
                yValueMapper: (BalanceMonth value, _) => showAsPercentage
                    ? value.totalAvailable / value.totalPatrimony
                    : value.totalAvailable,
                enableTooltip: true),
            StackedColumnSeries<BalanceMonth, DateTime>(
                name: 'Saldo Liquido',
                dataSource: widget.spending,
                color: Colors.greenAccent,
                xValueMapper: (BalanceMonth value, _) => value.date,
                yValueMapper: (BalanceMonth value, _) => showAsPercentage
                    ? (value.totalPatrimonyLiquid - value.totalAvailable) /
                        (value.totalPatrimony)
                    : (value.totalPatrimonyLiquid - value.totalAvailable),
                enableTooltip: true),
            StackedColumnSeries<BalanceMonth, DateTime>(
                name: 'Saldo Não Liquido',
                dataSource: widget.spending,
                color: Colors.deepOrangeAccent,
                xValueMapper: (BalanceMonth value, _) => value.date,
                yValueMapper: (BalanceMonth value, _) => showAsPercentage
                    ? (value.totalPatrimony - value.totalPatrimonyLiquid) /
                        (value.totalPatrimony)
                    : (value.totalPatrimony - value.totalPatrimonyLiquid),
                enableTooltip: true),
          ],
          primaryXAxis: DateTimeCategoryAxis(
            edgeLabelPlacement: EdgeLabelPlacement.shift,
            dateFormat: DateFormat("d/M/yy"),
            intervalType: DateTimeIntervalType.months,
          ),
          primaryYAxis: NumericAxis(
            numberFormat: showAsPercentage
                ? NumberFormat.decimalPercentPattern()
                : NumberFormat.compact(),
          ),
        ),
      ],
    );
  }
}

class GetValueByTypeBalance extends StatefulWidget {
  GetValueByTypeBalance(this.spending);

  final List<BalanceMonth> spending;

  @override
  State<GetValueByTypeBalance> createState() => _GetValueByTypeBalance();
}

class _GetValueByTypeBalance extends State<GetValueByTypeBalance> {
  bool showAsPercentage = false;
  int i = -1;
  final TooltipBehavior _tooltipBehavior = TooltipBehavior(
    enable: true,
    shared: true,
    duration: GlobalVariables.durationTooltip,
  );

  @override
  Widget build(BuildContext context) {
    return ExpansionTile(
      initiallyExpanded: false,
      collapsedTextColor: Colors.white,
      textColor: Colors.white,
      collapsedIconColor: Colors.white,
      iconColor: Colors.white,
      title: Text(
        "Patrimônio Por Tipo",
      ),
      leading: Icon(Icons.bar_chart_outlined, color: Colors.white),
      children: [
        Visibility(
          visible:
              !widget.spending.any((element) => element.totalAvailable < 0),
          child: SizedBox(
              child: ElevatedButton(
                  child: Text(showAsPercentage ? "Valor" : "Percentual"),
                  onPressed: () async {
                    showAsPercentage = !showAsPercentage;

                    setState(() {
                      i = -1;
                    });
                  })),
        ),
        SfCartesianChart(
          legend: Legend(
              isVisible: true,
              position: LegendPosition.bottom,
              overflowMode: LegendItemOverflowMode.wrap),
          tooltipBehavior: _tooltipBehavior,
          series: <ChartSeries>[
            StepLineSeries<BalanceMonth, DateTime>(
              dataSource: widget.spending,
              name: 'Total',
              isVisibleInLegend: false,
              color: Colors.black,
              opacity: 0,
              xValueMapper: (BalanceMonth value, _) => value.date,
              yValueMapper: (BalanceMonth value, _) => showAsPercentage? 1 : value.totalPatrimony,
            ),
            StackedColumnSeries<BalanceMonth, DateTime>(
                name: 'Conta Corrente',
                dataSource: widget.spending,
                color: Colors.accents[++i],
                xValueMapper: (BalanceMonth value, _) => value.date,
                yValueMapper: (BalanceMonth value, _) => showAsPercentage
                    ? value.currentBalance / value.totalPatrimony
                    : value.currentBalance,
                enableTooltip: true),
            StackedColumnSeries<BalanceMonth, DateTime>(
                name: 'Ativos',
                dataSource: widget.spending,
                color: Colors.accents[++i],
                xValueMapper: (BalanceMonth value, _) => value.date,
                yValueMapper: (BalanceMonth value, _) => showAsPercentage
                    ? value.stocksValue / value.totalPatrimony
                    : value.stocksValue,
                enableTooltip: true),
            StackedColumnSeries<BalanceMonth, DateTime>(
                name: 'Tesouro Direto',
                dataSource: widget.spending,
                color: Colors.accents[++i],
                xValueMapper: (BalanceMonth value, _) => value.date,
                yValueMapper: (BalanceMonth value, _) => showAsPercentage
                    ? value.treasuryBondValue / value.totalPatrimony
                    : value.treasuryBondValue,
                enableTooltip: true),
            StackedColumnSeries<BalanceMonth, DateTime>(
                name: 'Renda Fixa Não Liquida',
                dataSource: widget.spending,
                color: Colors.accents[++i],
                xValueMapper: (BalanceMonth value, _) => value.date,
                yValueMapper: (BalanceMonth value, _) => showAsPercentage
                    ? value.fixedIncomeValueLiquidityOnExpire /
                        value.totalPatrimony
                    : value.fixedIncomeValueLiquidityOnExpire,
                enableTooltip: true),
            StackedColumnSeries<BalanceMonth, DateTime>(
                name: 'Renda Fixa Liquida',
                dataSource: widget.spending,
                color: Colors.accents[++i],
                xValueMapper: (BalanceMonth value, _) => value.date,
                yValueMapper: (BalanceMonth value, _) => showAsPercentage
                    ? value.fixedIncomeValueFreeLiquidity / value.totalPatrimony
                    : value.fixedIncomeValueFreeLiquidity,
                enableTooltip: true),
            StackedColumnSeries<BalanceMonth, DateTime>(
                name: 'Fundos',
                dataSource: widget.spending,
                color: Colors.accents[++i],
                xValueMapper: (BalanceMonth value, _) => value.date,
                yValueMapper: (BalanceMonth value, _) => showAsPercentage
                    ? value.fundsValue / value.totalPatrimony
                    : value.fundsValue,
                enableTooltip: true),
            StackedColumnSeries<BalanceMonth, DateTime>(
                name: 'FGTS',
                dataSource: widget.spending,
                color: Colors.accents[++i],
                xValueMapper: (BalanceMonth value, _) => value.date,
                yValueMapper: (BalanceMonth value, _) => showAsPercentage
                    ? value.fgtsValue / value.totalPatrimony
                    : value.fgtsValue,
                enableTooltip: true),
          ],
          primaryXAxis: DateTimeCategoryAxis(
            edgeLabelPlacement: EdgeLabelPlacement.shift,
            dateFormat: DateFormat("d/M/yy"),
            intervalType: DateTimeIntervalType.months,
          ),
          primaryYAxis: NumericAxis(
            numberFormat: showAsPercentage
                ? NumberFormat.decimalPercentPattern()
                : NumberFormat.compact(),
          ),
        ),
      ],
    );
  }
}

class GetSpendingChart extends StatelessWidget {
  GetSpendingChart(this.spending);

  final List<BalanceMonth> spending;
  final TooltipBehavior _tooltipBehavior = TooltipBehavior(
    enable: true,
    shared: true,
    duration: GlobalVariables.durationTooltip,
  );

  @override
  Widget build(BuildContext context) {
    return ExpansionTile(
      initiallyExpanded: false,
      collapsedTextColor: Colors.white,
      textColor: Colors.white,
      collapsedIconColor: Colors.white,
      iconColor: Colors.white,

      // initiallyExpanded:  true,
      title: Text(
        "Gastos por Tipo",
      ),
      leading: Icon(Icons.bar_chart_outlined, color: Colors.white),
      children: [
        SfCartesianChart(
          enableAxisAnimation: true,
          legend: Legend(
              isVisible: true,
              position: LegendPosition.bottom,
              overflowMode: LegendItemOverflowMode.wrap),
          tooltipBehavior: _tooltipBehavior,
          series: <ChartSeries>[
            StepLineSeries<BalanceMonth, DateTime>(
              dataSource: spending,
              name: 'Total',
              isVisibleInLegend: false,
              color: Colors.black,
              opacity: 0,
              xValueMapper: (BalanceMonth value, _) => value.date,
              yValueMapper: (BalanceMonth value, _) =>
                  value.spending + value.loan,
            ),
            StackedColumnSeries<BalanceMonth, DateTime>(
                name: 'Gasto Obrigatórios',
                dataSource: spending,
                color: Colors.red[900],
                xValueMapper: (BalanceMonth value, _) => value.date,
                yValueMapper: (BalanceMonth value, _) => value.spendingRequired,
                enableTooltip: true),
            StackedColumnSeries<BalanceMonth, DateTime>(
                name: 'Outros Gastos',
                dataSource: spending,
                color: Colors.red[500],
                xValueMapper: (BalanceMonth value, _) => value.date,
                yValueMapper: (BalanceMonth value, _) =>
                    value.spending - value.spendingRequired,
                enableTooltip: true),
            StackedColumnSeries<BalanceMonth, DateTime>(
                name: 'Empréstimo/Financiamento',
                dataSource: spending,
                color: Colors.red[1000],
                xValueMapper: (BalanceMonth value, _) => value.date,
                yValueMapper: (BalanceMonth value, _) => value.loan,
                enableTooltip: true),
          ],
          primaryXAxis: DateTimeCategoryAxis(
            edgeLabelPlacement: EdgeLabelPlacement.shift,
            dateFormat: DateFormat("d/M/yy"),
            intervalType: DateTimeIntervalType.months,
          ),
          primaryYAxis: NumericAxis(
              numberFormat: NumberFormat.compact(),
              decimalPlaces: 2,
              maximumLabels: 4),
        ),
      ],
    );
  }
}

class GetPatrimony extends StatelessWidget {
  GetPatrimony(this.spending);

  final List<BalanceMonth> spending;
  final TooltipBehavior _tooltipBehavior = TooltipBehavior(
    enable: true,
    shared: true,
    duration: GlobalVariables.durationTooltip,
  );

  @override
  Widget build(BuildContext context) {
    return ExpansionTile(
      initiallyExpanded: false,
      collapsedTextColor: Colors.white,
      textColor: Colors.white,
      collapsedIconColor: Colors.white,
      iconColor: Colors.white,
      title: Text(
        "Patrimônio",
      ),
      leading: Icon(Icons.bar_chart_outlined, color: Colors.white),
      children: [
        SfCartesianChart(
          legend: Legend(
              // isVisible: true,
              position: LegendPosition.bottom,
              overflowMode: LegendItemOverflowMode.wrap),
          tooltipBehavior: _tooltipBehavior,
          series: <ChartSeries>[
            StackedColumnSeries<BalanceMonth, DateTime>(
                name: 'Patrimônio',
                dataSource: spending,
                color: Colors.white,
                xValueMapper: (BalanceMonth value, _) => value.date,
                yValueMapper: (BalanceMonth value, _) => value.totalPatrimony,
                enableTooltip: true),
          ],
          primaryXAxis: DateTimeCategoryAxis(
            edgeLabelPlacement: EdgeLabelPlacement.shift,
            dateFormat: DateFormat("d/M/yy"),
            intervalType: DateTimeIntervalType.months,
          ),
          primaryYAxis: NumericAxis(
            numberFormat: NumberFormat.compact(),
          ),
          axes: <ChartAxis>[
            NumericAxis(
                name: 'um',
                minimum: 0.00,
                maximum: 110.00,
                labelFormat: '{value}%',
                opposedPosition: true,
                maximumLabels: 3)
          ],
        ),
      ],
    );
  }
}

class GetBalanceChart extends StatelessWidget {
  GetBalanceChart(this.spending);

  final List<BalanceMonth> spending;
  final TooltipBehavior _tooltipBehavior = TooltipBehavior(
    enable: true,
    shared: true,
    duration: GlobalVariables.durationTooltip,
  );

  @override
  Widget build(BuildContext context) {
    return ExpansionTile(
      initiallyExpanded: false,
      collapsedTextColor: Colors.white,
      textColor: Colors.white,
      collapsedIconColor: Colors.white,
      iconColor: Colors.white,

      title: Text(
        "Gastos vs Receitas",
      ),
      leading: Icon(Icons.bar_chart_outlined, color: Colors.white),
      children: [
        SfCartesianChart(
          zoomPanBehavior: ZoomPanBehavior(
            enablePanning: true,
          ),
          legend: Legend(
              isVisible: true,
              position: LegendPosition.bottom,
              overflowMode: LegendItemOverflowMode.wrap),
          tooltipBehavior: _tooltipBehavior,
          series: <ChartSeries>[
            StackedColumnSeries<BalanceMonth, DateTime>(
                name: 'Receita',
                dataSource: spending,
                color: Colors.green,
                xValueMapper: (BalanceMonth value, _) => value.date,
                yValueMapper: (BalanceMonth value, _) => value.income,
                enableTooltip: true),
            StackedColumnSeries<BalanceMonth, DateTime>(
                name: 'Financiamento',
                dataSource: spending,
                color: Colors.red[900],
                xValueMapper: (BalanceMonth value, _) => value.date,
                yValueMapper: (BalanceMonth value, _) => -value.loan,
                enableTooltip: true),
            StackedColumnSeries<BalanceMonth, DateTime>(
                name: 'Gasto',
                dataSource: spending,
                color: Colors.red,
                xValueMapper: (BalanceMonth value, _) => value.date,
                yValueMapper: (BalanceMonth value, _) => -value.spending,
                enableTooltip: true),
            LineSeries<BalanceMonth, DateTime>(
                name: 'Acumulado',
                dataSource: spending,
                yAxisName: 'um',
                markerSettings: MarkerSettings(isVisible: true),
                color: Colors.grey,
                xValueMapper: (BalanceMonth value, _) => value.date,
                yValueMapper: (BalanceMonth value, _) =>
                    value.incomeCumulated - value.spendingCumulated,
                enableTooltip: true),
          ],
          primaryXAxis: DateTimeCategoryAxis(
            edgeLabelPlacement: EdgeLabelPlacement.shift,
            dateFormat: DateFormat("d/M/yy"),
            intervalType: DateTimeIntervalType.months,
          ),
          primaryYAxis: NumericAxis(
              numberFormat: NumberFormat.compact(),
              decimalPlaces: 2,
              maximumLabels: 4),
          axes: <ChartAxis>[
            NumericAxis(
                name: 'um',
                numberFormat: NumberFormat.compact(),
                decimalPlaces: 2,
                opposedPosition: true,
                maximumLabels: 3),
          ],
        ),
      ],
    );
  }
}

class GetFgtsChart extends StatelessWidget {
  GetFgtsChart(this.spending);

  final List<BalanceMonth> spending;
  final TooltipBehavior _tooltipBehavior = TooltipBehavior(
    enable: true,
    shared: true,
    duration: GlobalVariables.durationTooltip,
  );

  @override
  Widget build(BuildContext context) {
    return ExpansionTile(
        initiallyExpanded: false,
        collapsedTextColor: Colors.white,
        textColor: Colors.white,
        collapsedIconColor: Colors.white,
        iconColor: Colors.white,
        title: Text("FGTS"),
      leading: Icon(Icons.bar_chart_outlined, color: Colors.white),
      children: [
        SfCartesianChart(
          legend: Legend(
              isVisible: true,
              position: LegendPosition.bottom,
              overflowMode: LegendItemOverflowMode.wrap),
          tooltipBehavior: _tooltipBehavior,
          series: <ChartSeries>[
            StackedColumnSeries<BalanceMonth, DateTime>(
                name: 'Saque FGTS',
                dataSource: spending,
                // isVisibleInLegend: showIncome,
                color: Colors.grey,
                xValueMapper: (BalanceMonth value, _) => value.date,
                yValueMapper: (BalanceMonth value, _) => value.fgtsWithdraw,
                enableTooltip: true),
            StackedColumnSeries<BalanceMonth, DateTime>(
                name: 'Saldo FGTS',
                dataSource: spending,
                // isVisibleInLegend: showIncome,
                color: Colors.green,
                xValueMapper: (BalanceMonth value, _) => value.date,
                yValueMapper: (BalanceMonth value, _) => value.fgtsValue,
                enableTooltip: true),
          ],
          primaryXAxis: DateTimeCategoryAxis(
            edgeLabelPlacement: EdgeLabelPlacement.shift,
            dateFormat: DateFormat("d/M/yy"),
            intervalType: DateTimeIntervalType.months,
          ),
          primaryYAxis: NumericAxis(
              numberFormat: NumberFormat.compact(),
              decimalPlaces: 2,
              maximumLabels: 4),
          axes: <ChartAxis>[
            NumericAxis(
                name: 'um',
                numberFormat: NumberFormat.compact(),
                associatedAxisName: 'um',
                decimalPlaces: 2,
                opposedPosition: true,
                maximumLabels: 3)
          ],
        ),
      ],
    );
  }
}
