import 'package:finance_app/common_lists.dart';
import 'package:finance_app/components/dialog.dart';
import 'package:finance_app/components/progress.dart';
import 'package:finance_app/clients/crud_clients/income_client.dart';
import 'package:finance_app/models/income/income.dart';
import 'package:finance_app/models/recurrence.dart';
import 'package:finance_app/screens/income/income_form.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';

class IncomeList extends StatefulWidget {
  const IncomeList({Key? key}) : super(key: key);

  @override
  IncomeListState createState() => IncomeListState();
}

class IncomeListState extends State<IncomeList> {
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
                      columns: header,
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
                                  DataCell(Text(element.name.toString(),
                                      textAlign: TextAlign.left)),
                                  DataCell(Align(
                                      child: Text(
                                          DateFormat('dd/MM/yy')
                                              .format(element.initialDate),
                                          textAlign: TextAlign.left))),
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
                                          textAlign: TextAlign.left))),
                                  DataCell(
                                    const Icon(Icons.delete),
                                    onTap: () {
                                      confirmDialog(context)
                                          .then((response) async {
                                        if (response!) {
                                          await _dao.delete(element.id);
                                          setState(() {});
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
            return const Text('Erro Desconhecido');
          },
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

  List<String> get headerTexts =>
      ['Nome', 'Data Inicial', 'Valor', 'Recorrência', ''];

  List<DataColumn> get header {
    return headerTexts
        .map(
          (headerText) => DataColumn(
            label: Expanded(
              child: Text(headerText, textAlign: TextAlign.left),
            ),
          ),
        )
        .toList();
  }
}
