
import 'package:financial_app/components/dialog.dart';
import 'package:financial_app/database/dao/treasury_dao.dart';
import 'package:financial_app/functions/treasuryCurrentPosition.dart';
import 'package:financial_app/models/models/treasuryTransaction.dart';
import 'package:financial_app/providers/treasuryTransactionList.dart';
import 'package:financial_app/screens/treasure_screens/treasury_form.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';

class TreasuryTransactionForm extends StatelessWidget {
  TreasuryTransactionForm(this.treasuryName);

  final String treasuryName;

  final TreasuryDao _dao = TreasuryDao();

  final currencyFormat = NumberFormat.currency(
      locale: "pt_BR",
      symbol: "R"
          "\$",
      decimalDigits: 2);

  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Tesouro Direto'),
      ),
      body: Container(
        height: MediaQuery.of(context).size.height*0.8,
        child: SingleChildScrollView(
          child: Padding(
            padding: const EdgeInsets.all(8.0),
            child: Consumer<TreasuryTransactionList>(
                builder: (context, assetTransactionList, snapshot) {
              List<TreasuryTransaction> treasuryList =
                  assetTransactionList.getList(treasuryName);

              return treasuryList.length == 0? Text("Não há dados para mostrar") :Column(
                children: [
                  Text(treasuryName, style: Theme.of(context).textTheme.headline4,),
                  Container(
                    width: (MediaQuery.of(context).size.width),
                    child: DataTable(
                        sortAscending: true,
                        headingRowHeight: 40,
                        dividerThickness: 1,
                        dataRowHeight: 40,
                        horizontalMargin: 5,
                        columnSpacing: 0,
                        showCheckboxColumn: false,
                        sortColumnIndex: 2,
                        columns: [
                          DataColumn(label: Text('Data'),),
                          DataColumn(label: Text('Op.'),),
                          DataColumn(label: Text('Qtd Restante'),),
                          DataColumn(label: Text('Preço Médio'),),
                          DataColumn(label: Text(''),),
                        ],
                        rows: treasuryList
                            .map<DataRow>(
                              (element) => DataRow(
                                onSelectChanged: (value) {
                                  Navigator.of(context).push(
                                    MaterialPageRoute(
                                      builder: (context) =>
                                          TreasuryForm(element.treasury),
                                    ),
                                  );
                                },
                                cells: [
                                  DataCell(Text(DateFormat("dd/MM/yy")
                                      .format(element.treasury.date)
                                      .toString())),
                                  DataCell(Text(element.operation.toString())),
                                  DataCell(Text(element.quantityCumulated.toString() + "  ( " + (element.operation == "C"? "+": "-")+
                                  (element.treasury.quantity.toString()+" )"))),
                                  DataCell(Text(currencyFormat
                                      .format(element.avgUnitPrice)
                                      .toString())),
                                  DataCell(
                                    Icon(Icons.delete),
                                    onTap: () {
                                      confirmDialog(context).then((response) {
                                        if (response!) {
                                          _dao.deleteRow(element.treasury);
                                          treasuryCurrentPosition(context);
                                        }
                                      },);
                                    },
                                  )
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
      ),
      floatingActionButton: FloatingActionButton(
          onPressed: () {
            Navigator.of(context).push(
              MaterialPageRoute(
                builder: (context) => TreasuryForm(),
              ),
            );
          },
          child: Icon(
            Icons.add,
          )),
    );
  }
}
