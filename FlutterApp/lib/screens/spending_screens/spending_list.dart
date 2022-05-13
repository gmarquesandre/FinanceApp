import 'package:financial_app/components/dialog.dart';
import 'package:financial_app/components/progress.dart';
import 'package:financial_app/database/dao/spending_dao.dart';
import 'package:financial_app/functions/lists/common_lists.dart';
import 'package:financial_app/models/table_models/recurrence.dart';
import 'package:financial_app/models/table_models/spending.dart';
import 'package:financial_app/screens/spending_screens/spending_form.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';

class SpendingList extends StatefulWidget {
  @override
  _SpendingListState createState() => _SpendingListState();
}

class _SpendingListState extends State<SpendingList> {
  final SpendingDao _daoSpending = SpendingDao();
  var currencyFormat = NumberFormat.currency(locale: "pt_BR", symbol: "R\$");

  List<Recurrence> recurrenceList = CommonLists.recurrenceList;

  @override
  void initState() {
    // TODO: implement initState
    super.initState();
  }

  Widget build(BuildContext context) {
    return Scaffold(
      body: Padding(
        padding: const EdgeInsets.all(8.0),
        child: SingleChildScrollView(
          child: Container(
            child: FutureBuilder(
              future: _daoSpending.findAll(),
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
                    final List<Spending> spending =
                        snapshot.data as List<Spending>;
                    if (spending.length == 0) {
                      return Text("Não há dados para mostrar.");
                    }
                    return Container(
                      width: (MediaQuery.of(context).size.width),
                      child: DataTable(
                        sortColumnIndex: 2,
                        headingRowHeight: 40,
                        dividerThickness: 1,
                        dataRowHeight: 40,
                        horizontalMargin: 5,
                        columnSpacing: 0,
                        showCheckboxColumn: false,
                        columns: [
                          DataColumn(
                            label: Container(
                              width: 60,
                              child: Text('Nome', textAlign: TextAlign.left),
                            ),
                          ),
                          DataColumn(
                            label: Expanded(
                              child: Text('Data Inicial',
                                  textAlign: TextAlign.center),
                            ),
                          ),
                          DataColumn(
                            label: Expanded(
                              child: Text('Valor', textAlign: TextAlign.center),
                            ),
                          ),
                          DataColumn(
                            label: Text('Recorrência'),
                          ),
                          DataColumn(
                            label: Text(''),
                          ),
                        ],
                        rows: spending
                            .map<DataRow>(
                              (element) => DataRow(
                                onSelectChanged: (value) {
                                  Navigator.of(context)
                                      .push(
                                        MaterialPageRoute(
                                          builder: (context) =>
                                              SpendingForm(element),
                                        ),
                                      )
                                      .then(
                                        (newSpend) => setState(() {}),
                                      );
                                },
                                cells: [
                                  DataCell(
                                    Container(
                                      width: 70,
                                      child: Text(
                                        element.name.toString(),
                                      ),
                                    ),
                                  ),
                                  DataCell(
                                    Align(
                                      child: Text(
                                          DateFormat('dd/MM/yy')
                                              .format(element.initialDate),
                                          textAlign: TextAlign.center),
                                    ),
                                  ),
                                  DataCell(
                                    Text(
                                      currencyFormat
                                          .format(element.spendingValue)
                                          .toString(),
                                    ),
                                  ),
                                  DataCell(
                                    Align(
                                      child: Text(
                                        recurrenceList
                                            .firstWhere((value) =>
                                                value.id ==
                                                element.recurrenceId)
                                            .name
                                            .toString(),
                                        textAlign: TextAlign.center,
                                      ),
                                    ),
                                  ),
                                  DataCell(
                                    Icon(Icons.delete),
                                    onTap: () {
                                      confirmDialog(context).then(
                                        (response) {
                                          if (response!) {
                                            setState(
                                              () {
                                                _daoSpending.deleteRow(element);
                                              },
                                            );
                                          }
                                        },
                                      );
                                      //
                                    },
                                  ),
                                ],
                              ),
                            )
                            .toList(),
                      ),
                    );
                }
                return Text('Erro Desconhecido');
              },
            ),
          ),
        ),
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: () {
          Navigator.of(context)
              .push(
                MaterialPageRoute(
                  builder: (context) => SpendingForm(),
                ),
              )
              .then(
                (newSpend) => setState(() {}),
              );
        },
        child: Icon(
          Icons.add,
        ),
      ),
    );
  }
}
