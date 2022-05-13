import 'package:financial_app/models/models/treasuryCurrentPosition.dart';
import 'package:financial_app/providers/treasuryCurrentPositionList.dart';
import 'package:financial_app/screens/treasure_screens/treasury_form.dart';
import 'package:financial_app/screens/treasure_screens/treasury_transaction.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';

bool _ascending = true;

class TreasuryList extends StatelessWidget {
  final currencyFormat = NumberFormat.currency(locale: "pt_BR", symbol: "R\$");

  Widget build(BuildContext context) {
    return Scaffold(
      body: Container(
        height: MediaQuery.of(context).size.height*0.8,
        child: SingleChildScrollView(
          child: Padding(
            padding: const EdgeInsets.all(8.0),
            child: Consumer<TreasuryCurrentPositionList>(
                builder: (context, treasuryCurrentPositionList, snapshot) {
              List<TreasuryCurrentPosition> treasuryList =
              treasuryCurrentPositionList.getList();

              return treasuryList.length == 0? Text("Não há dados para mostrar") :Container(
                width: (MediaQuery.of(context).size.width),
                child: DataTable(
                    sortColumnIndex: 1,
                    headingRowHeight: 40,
                    dividerThickness: 1,
                    dataRowHeight: 50,
                    horizontalMargin: 5,
                    columnSpacing: 0,
                    sortAscending: _ascending,
                    showCheckboxColumn: false,
                    columns: [
                      DataColumn(label: Text('Titulo')),
                      DataColumn(
                          label: Text('Quantidade'),
                          onSort: (columnIndex, ascending) {
                            _ascending = _ascending == true ? false : true;
                            treasuryCurrentPositionList.sortByQtd(ascending);
                          }),
                      DataColumn(label: Text('Preço Médio')),
                    ],
                    rows: treasuryList
                        .map<DataRow>(
                          (element) => DataRow(
                            onSelectChanged: (value) {
                              Navigator.of(context).push(
                                MaterialPageRoute(
                                  builder: (context) =>
                                      TreasuryTransactionForm(element.treasuryName),
                                ),
                              );
                            },
                            cells: [
                              DataCell(Container(width: 100, child: Text(element.treasuryName))),
                              DataCell(Text(element.quantity.toString())),
                              DataCell(Text(currencyFormat
                                  .format(element.avgUnitPrice)
                                  .toString(),),),
                            ],
                          ),
                        )
                        .toList()),
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
