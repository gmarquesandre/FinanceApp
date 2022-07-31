import 'package:finance_app/components/dialog.dart';
import 'package:finance_app/components/progress.dart';
import 'package:finance_app/clients/crud_clients/loan_client.dart';
import 'package:finance_app/models/loan/loan.dart';
import 'package:finance_app/screens/loan_screens/loan_form.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';

class LoanList extends StatefulWidget {
  const LoanList({Key? key}) : super(key: key);

  @override
  LoanListState createState() => LoanListState();
}

class LoanListState extends State<LoanList> {
  final LoanClient _dao = LoanClient();
  var currencyFormat = NumberFormat.currency(locale: "pt_BR", symbol: "R\$");

  @override
  void initState() {
    super.initState();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: defaultBodyPadding(
        SingleChildScrollView(
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
                  final List<Loan> spending = snapshot.data as List<Loan>;
                  if (spending.isEmpty) {
                    return const Text("Não há dados para mostrar.");
                  }
                  return SizedBox(
                    width: (MediaQuery.of(context).size.width),
                    child: DataTable(
                      headingTextStyle: const TextStyle(color: Colors.white),
                      dataTextStyle: const TextStyle(color: Colors.white),
                      sortColumnIndex: 2,
                      headingRowHeight: 40,
                      dividerThickness: 1,
                      dataRowHeight: 40,
                      horizontalMargin: 5,
                      columnSpacing: 0,
                      showCheckboxColumn: false,
                      columns: header,
                      rows: spending
                          .map<DataRow>(
                            (element) => DataRow(
                              onSelectChanged: (value) {
                                Navigator.of(context)
                                    .push(
                                      MaterialPageRoute(
                                        builder: (context) => LoanForm(element),
                                      ),
                                    )
                                    .then(
                                      (newSpend) => setState(() {}),
                                    );
                              },
                              cells: [
                                DataCell(
                                  Text(
                                    element.name.toString(),
                                  ),
                                ),
                                DataCell(
                                  Align(
                                    child: Text(
                                        element.monthsPayment.toString(),
                                        textAlign: TextAlign.center),
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
                                        .format(element.loanValue)
                                        .toString(),
                                  ),
                                ),
                                DataCell(
                                  const Icon(
                                    Icons.delete,
                                    color: Colors.white,
                                  ),
                                  onTap: () {
                                    confirmDialog(context).then(
                                      (response) {
                                        if (response!) {
                                          setState(
                                            () {
                                              _dao.delete(element.id);
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
      floatingActionButton: FloatingActionButton(
        onPressed: () {
          Navigator.of(context)
              .push(
                MaterialPageRoute(
                  builder: (context) => LoanForm(),
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

  List<String> get headerTexts =>
      ['Nome', 'Parcelas', 'Data Inicial', 'Valor', ''];

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
