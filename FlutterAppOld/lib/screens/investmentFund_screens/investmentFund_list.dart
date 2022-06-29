import 'package:financial_app/models/models/fundCurrentPosition.dart';
import 'package:financial_app/providers/investmentFundCurrentPositionList.dart';
import 'package:financial_app/screens/investmentFund_screens/investmentFund_form.dart';
import 'package:financial_app/screens/investmentFund_screens/investmentFund_transaction.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';

bool _ascending = true;

class FundList extends StatelessWidget {
  final currencyFormat = NumberFormat.currency(locale: "pt_BR", symbol: "R\$");

  Widget build(BuildContext context) {
    return Scaffold(
      body: SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.all(8.0),
          child: Consumer<FundCurrentPositionList>(
              builder: (context, fundCurrentPositionList, snapshot) {
                List<FundCurrentPosition> fundList =
                fundCurrentPositionList.getList();

                return fundList.length == 0? Text("Não há dados para mostrar") :Container(
                  width: (MediaQuery.of(context).size.width),
                  child: DataTable(
                      sortColumnIndex: 1,
                      sortAscending: _ascending,
                      columnSpacing: 0,
                      headingRowHeight: 40,
                      dividerThickness: 1,
                      dataRowHeight: 50,
                      horizontalMargin: 5,
                      showCheckboxColumn: false,
                      columns: [
                        // DataColumn(label: Text('Id')),
                        DataColumn(label: Text('Ativo'),),
                        DataColumn(
                            label: Text('Quantidade'),
                            onSort: (columnIndex, ascending) {
                              _ascending = _ascending == true ? false : true;
                              fundCurrentPositionList.sortByQtd(ascending);
                            }),
                        DataColumn(label: Text('Preço Médio'),),
                      ],
                      rows: fundList
                          .map<DataRow>(
                            (element) => DataRow(
                          onSelectChanged: (value) {
                            Navigator.of(context).push(
                              MaterialPageRoute(
                                builder: (context) =>
                                    FundTransactionForm(element.cnpj),
                              ),
                            );
                          },
                          cells: [
                            // DataCell(Text(element.id.toString())),
                            DataCell(Container(width: 130, child: Text(element.name),),),
                            DataCell(Align(child: Text(element.quantity.toString(), textAlign: TextAlign.center),),),
                            DataCell(Text(currencyFormat.format(element.avgUnitPrice).toString(),),),
                          ],
                        ),
                      )
                          .toList()),
                );
              }),
        ),
      ),
      floatingActionButton: FloatingActionButton(
          onPressed: () {
            Navigator.of(context).push(
              MaterialPageRoute(
                builder: (context) => InvestmentFundForm(),
              ),
            );
          },
          child: Icon(
            Icons.add,
          )),
    );
  }
}
