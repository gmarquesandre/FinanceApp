import 'package:finance_app/common_lists.dart';
import 'package:finance_app/components/dialog.dart';
import 'package:finance_app/components/progress.dart';
import 'package:finance_app/controllers/cruc_clients/income_client.dart';
import 'package:finance_app/models/income/income.dart';
import 'package:finance_app/models/recurrence.dart';
import 'package:finance_app/screens/income/income_form.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';

class IncomeList extends StatefulWidget {
  @override
  _IncomeListState createState() => _IncomeListState();
}

class _IncomeListState extends State<IncomeList> {
  final IncomeClient _dao = IncomeClient();

  var currencyFormat = NumberFormat.currency(locale: "pt_BR", symbol: "R\$");

  List<Recurrence> recurrenceList = CommonLists.recurrenceList;

  @override
  void initState() {
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Padding(
        padding: const EdgeInsets.all(8.0),
        child: SingleChildScrollView(
          child: FutureBuilder(
            future: _dao.get(),
            builder: (context, snapshot) {
              switch (snapshot.connectionState) {
                case ConnectionState.none:
                  break;
                case ConnectionState.waiting:
                  return const Progress();
                case ConnectionState.active:
                  break;
                case ConnectionState.done:
                  final List<Income> incomes = snapshot.data as List<Income>;
                  if (incomes.isEmpty) {
                    return const Text("Não há dados para mostrar.");
                  }
                  return SizedBox(
                    width: (MediaQuery.of(context).size.width),
                    child: DataTable(
                        sortColumnIndex: 2,
                        headingRowHeight: 40,
                        dividerThickness: 1,
                        dataRowHeight: 40,
                        horizontalMargin: 5,
                        columnSpacing: 0,
                        showCheckboxColumn: false,
                        columns: const [
                          DataColumn(
                              label: Expanded(
                                  child:
                                      Text('Nome', textAlign: TextAlign.left))),
                          DataColumn(
                              label: Expanded(
                                  child: Text('Data Inicial',
                                      textAlign: TextAlign.center))),
                          DataColumn(
                              label: Expanded(
                                  child: Text('Valor',
                                      textAlign: TextAlign.center))),
                          DataColumn(label: Text('Recorrência')),
                          DataColumn(label: Text('')),
                        ],
                        rows: incomes
                            .map<DataRow>((element) => DataRow(
                                  onSelectChanged: (value) {
                                    Navigator.of(context)
                                        .push(
                                          MaterialPageRoute(
                                            builder: (context) =>
                                                IncomeForm(element),
                                          ),
                                        )
                                        .then(
                                          (newIncome) => setState(() {}),
                                        );
                                  },
                                  cells: [
                                    // DataCell(Text(element.id.toString())),
                                    DataCell(Text(element.name.toString())),
                                    DataCell(Align(
                                        child: Text(
                                            DateFormat('dd/MM/yy')
                                                .format(element.initialDate),
                                            textAlign: TextAlign.center))),
                                    DataCell(Text(currencyFormat
                                        .format(element.amount)
                                        .toString())),
                                    DataCell(Align(
                                        child: Text(
                                            recurrenceList
                                                .firstWhere((value) =>
                                                    value.id ==
                                                    element.recurrence)
                                                .name
                                                .toString(),
                                            textAlign: TextAlign.center))),
                                    DataCell(
                                      Icon(Icons.delete),
                                      onTap: () {
                                        confirmDialog(context).then((response) {
                                          if (response!) {
                                            // setState(() {
                                            //   _dao.deleteRow(element);
                                            // });
                                          }
                                        });
                                        //
                                      },
                                    ),
                                  ],
                                ))
                            .toList()),
                  );
              }
              return Text('Erro Desconhecido');
            },
          ),
        ),
      ),
      floatingActionButton: FloatingActionButton(
        onPressed: () {
          Navigator.of(context)
              .push(
                MaterialPageRoute(
                  builder: (context) => IncomeForm(),
                ),
              )
              .then(
                (newIncome) => setState(() {}),
              );
        },
        child: const Icon(
          Icons.add,
        ),
      ),
    );
  }
}
