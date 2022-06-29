import 'package:financial_app/components/dialog.dart';
import 'package:financial_app/database/dao/asset_dao.dart';
import 'package:financial_app/functions/assetCurrentPosition.dart';
import 'package:financial_app/models/models/assetTransaction.dart';
import 'package:financial_app/providers/assetTransactionList.dart';
import 'package:financial_app/screens/asset_screens/asset_form.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';

class AssetTransactionForm extends StatelessWidget {
  AssetTransactionForm(this.assetCode);

  final String assetCode;

  final AssetDao _dao = AssetDao();

  final currencyFormat = NumberFormat.currency(
      locale: "pt_BR",
      symbol: "R"
          "\$",
      decimalDigits: 2);

  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Renda Variavel'),
      ),
      body: SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.all(8.0),
          child: Consumer<AssetTransactionList>(
            builder: (context, assetTransactionList, snapshot) {
              List<AssetTransaction> assets =
                  assetTransactionList.getList(assetCode);
              return assets.length == 0
                  ? Text("Não há dados para mostrar")
                  : Column(
                      children: [
                        Text(
                          assetCode,
                          style: Theme.of(context).textTheme.headline4,
                          textAlign: TextAlign.right,
                        ),
                        Container(
                          width: (MediaQuery.of(context).size.width),
                          child: DataTable(
                              sortAscending: true,
                              sortColumnIndex: 1,
                              headingRowHeight: 40,
                              dividerThickness: 0,
                              dataRowHeight: 40,
                              horizontalMargin: 5,
                              columnSpacing: 0,
                              showCheckboxColumn: false,
                              columns: [
                                DataColumn(label: Text('Data'),),
                                DataColumn(label: Text('Op.'),),
                                DataColumn(label: Text('Acumulado'),),
                                DataColumn(label: Text('Preço Médio'),),
                                DataColumn(label: Text('')),
                                // DataColumn(label: Text('Operação')),
                              ],
                              rows: assets
                                  .map<DataRow>(
                                    (element) => DataRow(
                                      onSelectChanged: (value) {
                                        Navigator.of(context).push(
                                          MaterialPageRoute(
                                            builder: (context) =>
                                                AssetForm(element.asset),
                                          ),
                                        );
                                      },
                                      cells: [
                                        DataCell(Text(DateFormat("dd/MM/yy")
                                            .format(element.asset.date)
                                            .toString())),
                                        DataCell(
                                            Text(element.operation.toString())),
                                        DataCell(Align(
                                          child: Text(
                                              element.quantityCumulatedLabel(), textAlign: TextAlign.center,),
                                        ),),
                                        DataCell(
                                            Align(child: Text(element.avgUnitPriceLabel(),textAlign: TextAlign.center,),),),
                                        DataCell(
                                          Icon(Icons.delete),
                                          onTap: () {
                                            confirmDialog(context).then(
                                              (response) async {
                                                if (response!) {
                                                  await _dao
                                                      .deleteRow(element.asset);
                                                  assetCurrentPosition(context);
                                                }
                                              },
                                            );
                                            //
                                          },
                                        )
                                      ],
                                    ),
                                  )
                                  .toList()),
                        ),
                      ],
                    );
            },
          ),
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
          )),
    );
  }
}
