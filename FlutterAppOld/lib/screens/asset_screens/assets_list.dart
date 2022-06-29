import 'package:financial_app/models/models/assetCurrentPosition.dart';
import 'package:financial_app/models/table_models/assetCurrentValue.dart';
import 'package:financial_app/providers/assetCurrentPositionList.dart';
import 'package:financial_app/screens/asset_screens/asset_form.dart';
import 'package:financial_app/screens/asset_screens/asset_transaction.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';

bool _ascending = true;

class AssetsList extends StatelessWidget {
  final currencyFormat = NumberFormat.currency(locale: "pt_BR", symbol: "R\$");

  final List<AssetCurrentValue> listCurrentValue = [];

  Widget build(BuildContext context) {
    return Scaffold(
      body: SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.all(8.0),
          child: Consumer<AssetCurrentPositionList>(
              builder: (context, assetCurrentPositionList, snapshot) {
            List<AssetCurrentPosition> assets =
                assetCurrentPositionList.getList();

            return assets.length == 0
                ? Text("Não há dados para mostrar")
                : Column(
                    children: [
                      Container(
                        width: (MediaQuery.of(context).size.width),
                        child: DataTable(
                            sortColumnIndex: 2,
                            headingRowHeight: 40,
                            dividerThickness: 1,
                            dataRowHeight: 40,
                            horizontalMargin: 5,
                            sortAscending: _ascending,
                            columnSpacing: 0,
                            showCheckboxColumn: false,
                            columns: [
                              DataColumn(
                                label: Expanded(
                                  child: Text(
                                    'Ativo',
                                    textAlign: TextAlign.left,
                                  ),
                                ),
                              ),
                              DataColumn(
                                  label: Container(
                                    width: 80,
                                    child: Text(
                                      'Quantidade',
                                      textAlign: TextAlign.center,
                                    ),
                                  ),
                                  onSort: (columnIndex, ascending) {
                                    _ascending =
                                        _ascending == true ? false : true;
                                    assetCurrentPositionList
                                        .sortByQtd(ascending);
                                  }),
                              DataColumn(
                                label: Expanded(
                                  child: Text(
                                    'Preço Médio',
                                    textAlign: TextAlign.left,
                                  ),
                                ),
                              ),
                              DataColumn(
                                label: Expanded(
                                  child: Text(
                                    'Preço Atual',
                                    textAlign: TextAlign.left,
                                  ),
                                ),
                              ),
                              DataColumn(
                                label: Expanded(
                                  child: Text(
                                    '',
                                    textAlign: TextAlign.center,
                                  ),
                                ),
                              ),
                            ],
                            rows: assets
                                .map<DataRow>(
                                  (element) => DataRow(
                                    onSelectChanged: (value) {
                                      Navigator.of(context).push(
                                        MaterialPageRoute(
                                          builder: (context) =>
                                              AssetTransactionForm(
                                                  element.assetCode),
                                        ),
                                      );
                                    },
                                    cells: [
                                      DataCell(
                                        Text(
                                          element.assetCode,
                                        ),
                                      ),
                                      DataCell(
                                        Container(
                                          width: 80,
                                          child: Text(
                                            element.quantity.toString(),
                                            textAlign: TextAlign.center,
                                          ),
                                        ),
                                      ),
                                      DataCell(Align(
                                        child: Text(
                                            currencyFormat
                                                .format(element.avgUnitPrice),
                                            textAlign: TextAlign.left),
                                      )),
                                      DataCell(Align(
                                        child: Text(
                                            currencyFormat
                                                .format(element.currentPrice)
                                                .toString(),
                                            textAlign: TextAlign.right),
                                      )),
                                      DataCell(
                                        Icon(
                                            element.currentPrice ==
                                                    element.avgUnitPrice
                                                ? Icons.horizontal_rule
                                                : element.currentPrice <
                                                        element.avgUnitPrice
                                                    ? Icons.arrow_drop_down
                                                    : Icons.arrow_drop_up,
                                            color: element.currentPrice ==
                                                    element.avgUnitPrice
                                                ? Colors.grey
                                                : element.currentPrice >
                                                        element.avgUnitPrice
                                                    ? Colors.green
                                                    : Colors.red),
                                      ),
                                    ],
                                  ),
                                )
                                .toList()),
                      ),
                    ],
                  );
          }),
        ),
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: () {
          Navigator.of(context).push(
            MaterialPageRoute(
              builder: (context) => AssetForm(),
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
