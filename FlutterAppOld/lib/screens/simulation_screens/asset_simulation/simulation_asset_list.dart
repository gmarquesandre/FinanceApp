import 'package:financial_app/components/dialog.dart';
import 'package:financial_app/components/progress.dart';
import 'package:financial_app/database/dao/asset_simulation.dart';
import 'package:financial_app/models/table_models/assetSimulation.dart';
import 'package:financial_app/screens/simulation_screens/asset_simulation/simulation_asset_form.dart';
import 'package:flutter/material.dart';

class AssetSimulationConfig extends StatefulWidget {
  const AssetSimulationConfig({Key? key}) : super(key: key);

  @override
  _AssetSimulationConfigState createState() => _AssetSimulationConfigState();
}

class _AssetSimulationConfigState extends State<AssetSimulationConfig> {
  AssetSimulationDao _dao = AssetSimulationDao();

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
                  final List<AssetSimulation> assets =
                      snapshot.data as List<AssetSimulation>;
                  if (assets.length == 0) {
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
                                child: Text('Ativo', textAlign: TextAlign.left),
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
                                child: Text('Ganho Fixo',
                                    textAlign: TextAlign.left),
                              ),
                            ),
                            DataColumn(
                                label: Expanded(
                                    child:
                                        Text('', textAlign: TextAlign.left))),
                          ],
                          rows: assets
                              .map<DataRow>(
                                (element) => DataRow(
                                  onSelectChanged: (value) {
                                    Navigator.of(context)
                                        .push(
                                          MaterialPageRoute(
                                            builder: (context) =>
                                                AssetSimulationForm(element),
                                          ),
                                        )
                                        .then(
                                          (newIncome) => setState(() {}),
                                        );
                                  },
                                  cells: [
                                    DataCell(
                                      Text(
                                        element.assetCode.toString(),
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
        onPressed: () async {
          await Navigator.of(context)
              .push(
                MaterialPageRoute(
                  builder: (context) => AssetSimulationForm(),
                ),
              )
              .then(
                (newAssetSimulation) => setState(() {}),
              );
        },
        child: Icon(
          Icons.add,
        ),
      ),
    );
  }
}
