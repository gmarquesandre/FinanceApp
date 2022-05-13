import 'package:financial_app/components/dialog.dart';
import 'package:financial_app/components/progress.dart';
import 'package:financial_app/database/dao/loanDao.dart';
import 'package:financial_app/models/table_models/loan.dart';
import 'package:financial_app/screens/loan_screens/loan_form.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';

class LoanList extends StatefulWidget {
  @override
  _LoanListState createState() => _LoanListState();
}

class _LoanListState extends State<LoanList> {
  final LoanDao _dao = LoanDao();
  var currencyFormat = NumberFormat.currency(locale: "pt_BR", symbol: "R\$");

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
              future: _dao.findAll(),
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
                    final List<Loan> spending =
                        snapshot.data as List<Loan>;
                    if (spending.length == 0) {
                      return Text("Não há dados para mostrar.");
                    }
                    return Container(
                      width: (MediaQuery.of(context).size.width),
                      child: DataTable(
                        headingTextStyle:TextStyle(color: Colors.white),
                        dataTextStyle: TextStyle(color: Colors.white),
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
                              child: Text('Nome', textAlign: TextAlign.left),
                            ),
                          ),
                          DataColumn(
                            label: Expanded(
                              child: Text('Parcelas',
                                  textAlign: TextAlign.center),
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
                                              LoanForm(element),
                                        ),
                                      )
                                      .then(
                                        (newSpend) => setState(() {}),
                                      );
                                },
                                cells: [
                                  DataCell(
                                    Container(
                                      width: 50,
                                      child: Text(
                                        element.name.toString(),
                                      ),
                                    ),
                                  ),
                                  DataCell(
                                    Align(
                                      child: Text(
                                          element.months.toString(),
                                          textAlign: TextAlign.center),
                                    ),
                                  ),
                                  DataCell(
                                    Align(
                                      child: Text(
                                          DateFormat('dd/MM/yy')
                                              .format(element.date),
                                          textAlign: TextAlign.center),
                                    ),
                                  ),
                                  DataCell(
                                    Text(
                                      currencyFormat
                                          .format(element.totalValue)
                                          .toString(),
                                    ),
                                  ),
                                  DataCell(
                                    Icon(Icons.delete, color: Colors.white,),
                                    onTap: () {
                                      confirmDialog(context).then(
                                        (response) {
                                          if (response!) {
                                            setState(
                                              () {
                                                _dao.deleteRow(element);
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
                  builder: (context) => LoanForm(),
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
