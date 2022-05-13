import 'package:financial_app/components/progress.dart';
import 'package:financial_app/components/dialog.dart';
import 'package:financial_app/database/dao/income_dao.dart';
import 'package:financial_app/functions/lists/common_lists.dart';
import 'package:financial_app/models/table_models/income.dart';
import 'package:financial_app/models/table_models/recurrence.dart';
import 'package:financial_app/screens/income_screens/income_form.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';

class IncomeList extends StatefulWidget {
  @override
  _IncomeListState createState() => _IncomeListState();
}

class _IncomeListState extends State<IncomeList> {
  final IncomeDao _dao = IncomeDao();

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
                    final List<Income> incomes = snapshot.data as List<Income>;
                    if(incomes.length == 0) {
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
                            DataColumn(label: Expanded(child: Text('Nome', textAlign: TextAlign.left))),
                            DataColumn(label: Expanded(child: Text('Data Inicial', textAlign: TextAlign.center))),
                            DataColumn(label: Expanded(child: Text('Valor', textAlign: TextAlign.center))),
                            DataColumn(label: Text('Recorrência')),
                            DataColumn(label: Text('')),
                          ],
                          rows:
                          incomes.map<DataRow>((element) => DataRow(
                            onSelectChanged: (value) {

                              Navigator.of(context)
                                  .push(
                                MaterialPageRoute(
                                  builder: (context) => IncomeForm(element),
                                ),
                              )
                                  .then(
                                    (newIncome) => setState(() {}),
                              );

                            },
                            cells:[
                              // DataCell(Text(element.id.toString())),
                              DataCell(Text(element.name.toString())),
                              DataCell(Align(child: Text(DateFormat('dd/MM/yy').format(element.initialDate), textAlign: TextAlign.center))),
                              DataCell(Text(currencyFormat.format(element.incomeValue).toString())),
                              DataCell(Align(child: Text(recurrenceList.firstWhere((value) => value.id == element.recurrenceId).name.toString(),
                                  textAlign: TextAlign.center))),
                              DataCell(
                                Icon(Icons.delete),
                                onTap: () {
                                  confirmDialog(context)
                                      .then((response) {
                                    if (response!) {
                                      setState(() {
                                        _dao.deleteRow(element);
                                      });
                                    }
                                  });
                                  //
                                },
                              ),
                            ],

                          )).toList()
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
                builder: (context) => IncomeForm(),
              ),
            )
                .then(
                  (newIncome) => setState(() {}),
            );
          },
          child: Icon(
            Icons.add,
          ),),
    );
  }
}

