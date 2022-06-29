import 'package:financial_app/components/dialog.dart';
import 'package:financial_app/components/progress.dart';
import 'package:financial_app/database/dao/fund_simulation.dart';
import 'package:financial_app/models/table_models/fundSimulation.dart';
import 'package:financial_app/screens/simulation_screens/fund_simulation/simulation_fund_form.dart';
import 'package:flutter/material.dart';

class FundSimulationConfig extends StatefulWidget {
  const FundSimulationConfig({Key? key}) : super(key: key);

  @override
  _FundSimulationConfigState createState() => _FundSimulationConfigState();
}

class _FundSimulationConfigState extends State<FundSimulationConfig> {
  FundSimulationDao _dao = FundSimulationDao();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text("Simulação Ativos"),
      ),
      body: SingleChildScrollView(
        child: Container(
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
                  final List<FundSimulation> incomes =
                      snapshot.data as List<FundSimulation>;
                  if (incomes.length == 0) {
                    return Text("Não há dados para mostrar.");
                  }
                  return Container(
                      width: (MediaQuery.of(context).size.width),
                      child: DataTable(
                          headingRowHeight: 40,
                          dividerThickness: 1,
                          dataRowHeight: 40,
                          horizontalMargin: 5,
                          columnSpacing: 0,
                          showCheckboxColumn: false,
                          columns: [
                            DataColumn(
                              label: Expanded(
                                child: Text('CNPJ', textAlign: TextAlign.left),
                              ),
                            ),
                            DataColumn(
                              label: Expanded(
                                child: Text('Nome', textAlign: TextAlign.left),
                              ),
                            ),
                            DataColumn(
                              label: Expanded(
                                child:
                                    Text('Indice', textAlign: TextAlign.left),
                              ),
                            ),
                            DataColumn(
                              label: Expanded(
                                child:
                                    Text('Fixo %', textAlign: TextAlign.left),
                              ),
                            ),
                            DataColumn(
                                label: Expanded(
                                    child:
                                        Text('', textAlign: TextAlign.left))),
                          ],
                          rows: incomes
                              .map<DataRow>(
                                (element) => DataRow(
                                  onSelectChanged: (value) {
                                    Navigator.of(context)
                                        .push(
                                          MaterialPageRoute(
                                            builder: (context) =>
                                                FundSimulationForm(element),
                                          ),
                                        )
                                        .then(
                                          (newIncome) => setState(() {}),
                                        );
                                  },
                                  cells: [
                                    // DataCell(Text(element.id.toString())),
                                    DataCell(
                                      Text(element.cnpj.toString(),
                                          textAlign: TextAlign.center),
                                    ),
                                    DataCell(
                                      Container(
                                        width: 100,
                                        child: Text(
                                          element.nameShort
                                              .substring(0, 30)
                                              .toString(),
                                        ),
                                      ),
                                    ),
                                    DataCell(
                                      Text(
                                        element.indexName.toString(),
                                      ),
                                    ),
                                    DataCell(
                                      Text((element.fixedYearGain * 100)
                                              .toString() +
                                          " %"),
                                    ),
                                    DataCell(
                                      Icon(Icons.delete),
                                      onTap: () {
                                        confirmDialog(context).then(
                                          (response) {
                                            if (response!) {
                                              setState(
                                                () {
                                                  _dao.deleteRow(element);
                                                },
                                              );
                                            }
                                          },
                                        );
                                      },
                                    ),
                                  ],
                                ),
                              )
                              .toList()));
              }
              return Text("Erro Desconhecido");
            },
          ),
        ),
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: () {
          Navigator.of(context).push(
            MaterialPageRoute(
              builder: (context) => FundSimulationForm(),
            ),
          );
        },
        child: Icon(
          Icons.add,
        ),
      ),
    );
  }
}
