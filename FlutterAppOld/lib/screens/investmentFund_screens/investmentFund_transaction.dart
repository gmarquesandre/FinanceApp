import 'package:financial_app/components/dialog.dart';
import 'package:financial_app/database/dao/investmentFund_dao.dart';
import 'package:financial_app/functions/fundCurrentPosition.dart';
import 'package:financial_app/models/models/fundTransaction.dart';
import 'package:financial_app/providers/investmentFundTransactionList.dart';
import 'package:financial_app/screens/investmentFund_screens/investmentFund_form.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';

class FundTransactionForm extends StatelessWidget {
  FundTransactionForm(this.cnpj);

  final String cnpj;

  final InvestmentFundDao _dao = InvestmentFundDao();

  final currencyFormat = NumberFormat.currency(
      locale: "pt_BR",
      symbol: "R"
          "\$",
      decimalDigits: 2);

  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Fundos'),
      ),
      body: SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.all(8.0),
          child: Consumer<FundTransactionList>(
            builder: (context, fundTransactionList, snapshot) {
              List<FundTransaction> fundList =
                  fundTransactionList.getList(cnpj);

              return fundList.length == 0
                  ? Text("Não há dados para mostrar")
                  : Column(
                      children: [
                        Text(
                          cnpj,
                        ),
                        Container(
                          width: (MediaQuery.of(context).size.width),
                          child: DataTable(
                            sortAscending: true,
                            sortColumnIndex: 2,
                            columnSpacing: 0,
                            headingRowHeight: 40,
                            dividerThickness: 1,
                            dataRowHeight: 50,
                            horizontalMargin: 5,
                            showCheckboxColumn: false,
                            columns: [
                              DataColumn(
                                label: Text('Data'),
                              ),
                              DataColumn(
                                label: Text('Qtd Acumulada'),
                              ),
                              DataColumn(
                                label: Text('Preço Médio'),
                              ),
                              DataColumn(
                                label: Text(''),
                              ),
                            ],
                            rows: fundList
                                .map<DataRow>(
                                  (element) => DataRow(
                                    onSelectChanged: (value) {
                                      Navigator.of(context).push(
                                        MaterialPageRoute(
                                          builder: (context) =>
                                              InvestmentFundForm(element.fund),
                                        ),
                                      );
                                    },
                                    cells: [
                                      DataCell(Text(DateFormat("dd/MM/yy")
                                          .format(element.fund.date)
                                          .toString(),),),
                                      DataCell(
                                        Text(
                                          element.quantityCumulated.toString()
                                              + "  ( " + (element.operation == "C"? "+": "-")+
                                          element.fund.quantity!.toStringAsFixed(2) + " )",
                                        ),
                                      ),
                                      DataCell(
                                        Text(
                                          currencyFormat
                                              .format(element.avgUnitPrice)
                                              .toString(),
                                        ),
                                      ),
                                      DataCell(
                                        Icon(Icons.delete),
                                        onTap: () {
                                          confirmDialog(context).then(
                                            (response) {
                                              if (response!) {
                                                _dao.deleteRow(element.fund);
                                                fundCurrentPosition(context);
                                              }
                                            },
                                          );
                                          //
                                        },
                                      )
                                    ],
                                  ),
                                )
                                .toList(),
                          ),
                        ),
                      ],
                    );
            },
          ),
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
        ),
      ),
    );
  }
}
