import 'package:finance_app/common_lists.dart';
import 'package:finance_app/components/dialog.dart';
import 'package:finance_app/components/progress.dart';
import 'package:finance_app/controllers/crud_clients/spending_client.dart';
import 'package:finance_app/models/recurrence.dart';
import 'package:finance_app/models/spending/spending.dart';
import 'package:finance_app/screens/spending_screens/spending_form.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';

class SpendingList extends StatefulWidget {
  @override
  SpendingListState createState() => SpendingListState();
}

class SpendingListState extends State<SpendingList> {
  final SpendingClient _daoSpending = SpendingClient();
  var currencyFormat = NumberFormat.currency(locale: "pt_BR", symbol: "R\$");

  List<Recurrence> recurrenceList = CommonLists.recurrenceList;

  @override
  void initState() {
    super.initState();
  }

  Widget build(BuildContext context) {
    return Scaffold(
      body: Padding(
        padding: const EdgeInsets.all(8.0),
        child: SingleChildScrollView(
          child: Container(
            child: FutureBuilder(
              future: _daoSpending.get(),
              builder: (context, snapshot) {
                switch (snapshot.connectionState) {
                  case ConnectionState.none:
                    break;
                  case ConnectionState.waiting:
                    return const Progress();
                  case ConnectionState.active:
                    // TODO: Handle this case.
                    break;
                  case ConnectionState.done:
                    final List<Spending> spending =
                        snapshot.data as List<Spending>;
                    if (spending.isEmpty) {
                      return const Text("Não há dados para mostrar.");
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
                              child:
                                  const Text('Nome', textAlign: TextAlign.left),
                            ),
                          ),
                          const DataColumn(
                            label: const Expanded(
                              child: Text('Data Inicial',
                                  textAlign: TextAlign.center),
                            ),
                          ),
                          const DataColumn(
                            label: Expanded(
                              child: const Text('Valor',
                                  textAlign: TextAlign.center),
                            ),
                          ),
                          const DataColumn(
                            label: Text('Recorrência'),
                          ),
                          const DataColumn(
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
                                          .format(element.amount)
                                          .toString(),
                                    ),
                                  ),
                                  DataCell(
                                    Align(
                                      child: Text(
                                        recurrenceList
                                            .firstWhere((value) =>
                                                value.id == element.recurrence)
                                            .name
                                            .toString(),
                                        textAlign: TextAlign.center,
                                      ),
                                    ),
                                  ),
                                  DataCell(
                                    const Icon(Icons.delete),
                                    onTap: () {
                                      confirmDialog(context).then(
                                        (response) {
                                          if (response!) {
                                            setState(
                                              () {
                                                _daoSpending.delete(element.id);
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
                return const Text('Erro Desconhecido');
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
        child: const Icon(
          Icons.add,
        ),
      ),
    );
  }
}
