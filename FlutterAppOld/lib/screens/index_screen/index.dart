import 'package:financial_app/components/progress.dart';
import 'package:financial_app/database/dao/indexProspect.dart';
import 'package:financial_app/models/table_models/indexProspect.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:syncfusion_flutter_charts/charts.dart';

class IndexScreen extends StatefulWidget {
  @override
  _IndexScreenState createState() => _IndexScreenState();
}

class ObjectGraph {
  DateTime date;
  double value;

  ObjectGraph(this.date, this.value);
}

class _IndexScreenState extends State<IndexScreen> {
  final IndexProspectDao _dao = IndexProspectDao();

  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Indices Futuros'),
      ),
      body: Padding(
        padding: const EdgeInsets.all(8.0),
        child: SingleChildScrollView(
          child: FutureBuilder(
            future: _dao.findAll(),
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
                  final List<IndexProspect> index =
                      snapshot.data as List<IndexProspect>;
                  if (index.length == 0) {
                    return Text("Não há dados para mostrar.");
                  }
                  List<ObjectGraph> listIpca = index
                      .where((element) => element.indexName == "IPCA")
                      .map((e) => ObjectGraph(e.dateStart, e.median))
                      .toList();
                  List<ObjectGraph> listSelic = index
                      .where((element) => element.indexName == "SELIC")
                      .map((e) => ObjectGraph(e.dateStart, e.median))
                      .toList();

                  List<ObjectGraph> listIGPM = index
                      .where((element) => element.indexName == "IGP-M")
                      .map((e) => ObjectGraph(e.dateStart, e.median))
                      .toList();

                  DateTime dateResearch = index.first.dateResearch;

                  return Container(
                    child:

                        Column(children: [
                          Text("Os dados utilizados tem como fonte a pesquisa FOCUS do Banco Central do Brasil"),
                          Text("Data da Pesquisa ${DateFormat("dd/MM/yyyy").format(dateResearch)}"),
                          GetPatrimony(listSelic, "Selic Over / CDI Anual", "Selic Over / CDI", "d/M/yy"),
                          GetPatrimony(listIpca, "IPCA Mensal", "IPCA", "M/yy"),
                          GetPatrimony(listIGPM, "IGPM Mensal", "IGPM", "M/yy"),

                        ]),
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

class GetPatrimony extends StatelessWidget {
  GetPatrimony(this.index, this.title, this.name, this.dateFormat);

  final TooltipBehavior _tooltipBehavior =
      TooltipBehavior(enable: true, shared: true, duration: 30000);

  final List<ObjectGraph> index;
  final String title;
  final String name;
  final String dateFormat;

  @override
  Widget build(BuildContext context) {
    return SfCartesianChart(
      title: ChartTitle(text: title),
      legend: Legend(
          position: LegendPosition.bottom,
          overflowMode: LegendItemOverflowMode.wrap),
      tooltipBehavior: _tooltipBehavior,
      series: <ChartSeries>[
        ColumnSeries<ObjectGraph, DateTime>(
            name: name,
            dataSource: index,
            color: Colors.black,
            xValueMapper: (ObjectGraph value, _) => value.date,
            yValueMapper: (ObjectGraph value, _) => value.value,
            enableTooltip: true),
      ],
      primaryXAxis: DateTimeCategoryAxis(
        edgeLabelPlacement: EdgeLabelPlacement.shift,
        dateFormat: DateFormat(dateFormat),
        intervalType: DateTimeIntervalType.months,
      ),
      primaryYAxis: NumericAxis(
        labelFormat: '{value}%',
        decimalPlaces: 4,
      ),
    );
  }
}