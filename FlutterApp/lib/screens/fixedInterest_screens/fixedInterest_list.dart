import 'package:financial_app/components/dialog.dart';
import 'package:financial_app/database/dao/fixedInterest_dao.dart';
import 'package:financial_app/functions/fixedInterestPosition.dart';
import 'package:financial_app/functions/lists/common_lists.dart';
import 'package:financial_app/models/models/fixedInterestMonthly.dart';
import 'package:financial_app/models/table_models/fixedInterestType.dart';
import 'package:financial_app/models/table_models/index.dart';
import 'package:financial_app/providers/fixedInterestListProvider.dart';
import 'package:financial_app/screens/fixedInterest_screens/fixedInterest_form.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';

class FixedInterestList extends StatelessWidget {
  final FixedInterestDao _dao = FixedInterestDao();

  final currencyFormat = NumberFormat.currency(
    locale: "pt_BR",
    symbol: "R\$",
  );

  final List<FixedInterestType> fixedInterestTypeList =
      CommonLists.fixedInterestTypeList;
  final List<Index> indexList = CommonLists.indexList;

  final List<DateTime> dates = [DateTime.now()];


  Widget build(BuildContext context) {
    return Scaffold(
      body: Padding(
        padding: const EdgeInsets.all(8.0),
        child: SingleChildScrollView(
          child: Container(
            child: Consumer<FixedInterestListProvider>(
              builder: (context, assetCurrentPositionList, snapshot) {

                List<FixedInterestMonthly> list =
                    assetCurrentPositionList.getList();
                return Container(
                  width: (MediaQuery.of(context).size.width),
                  child: DataTable(
                    headingRowHeight: 40,
                    dividerThickness: 1,
                    dataRowHeight: 40,
                    horizontalMargin: 5,
                    columnSpacing: 0,
                    showCheckboxColumn: false,
                    columns: [
                      DataColumn(
                        label: Expanded(
                          child: Text('Nome', textAlign: TextAlign.left),
                        ),
                      ),
                      DataColumn(
                        label: Expanded(
                          child:
                              Text('Rentabilidade', textAlign: TextAlign.left),
                        ),
                      ),
                      DataColumn(
                        label: Text('Período'),
                      ),
                      DataColumn(
                        label: Container(
                          child: Text('Valor Atual \n ( Investido )',
                              textAlign: TextAlign.center),
                        ),
                      ),
                      DataColumn(
                        label: Text(''),
                      ),
                    ],
                    rows: list
                        .map<DataRow>((element) => DataRow(
                              onSelectChanged: (value) {
                                Navigator.of(context).push(
                                  MaterialPageRoute(
                                    builder: (context) => FixedInterestForm(
                                        element.fixedInterestElement),
                                  ),
                                );
                              },
                              cells: [
                                DataCell(
                                  Container(
                                    width: 60,
                                    child: Text(element.name,
                                        textAlign: TextAlign.left),
                                  ),
                                ),
                                DataCell(
                                  Container(
                                    width: 90,
                                    child: Text(
                                        element.fixedInterestElement
                                            .investmentInterest(),
                                        textAlign: TextAlign.left),
                                  ),
                                ),
                                DataCell(
                                  Container(
                                    width: 70,
                                    child: Text(
                                      element.fixedInterestElement.period(),
                                    ),
                                  ),
                                ),
                                DataCell(
                                  Text(
                                    currencyFormat
                                            .format(element.todayValue)
                                            .toString() +
                                        "\n" +
                                        "( " +
                                        currencyFormat
                                            .format(element.amount)
                                            .toString() +
                                        " )",
                                  ),
                                ),
                                DataCell(
                                  Icon(Icons.delete),
                                  onTap: () {
                                    confirmDialog(context).then(
                                      (response) async {
                                        if (response!) {
                                          await _dao.deleteRow(
                                              element.fixedInterestElement).then(
                                                  (value) async => await fixedInterestPosition(
                                              context));
                                        }
                                      },
                                    );
                                  },
                                ),
                              ],
                            ))
                        .toList(),
                  ),
                );
              },
            ),
          ),
        ),
      ),
      floatingActionButton: Column(
        mainAxisAlignment: MainAxisAlignment.end,
        children: [
// FloatingActionButton(
//   heroTag: "Calculator",
//   onPressed: () {
//     Navigator.of(context).push(
//       MaterialPageRoute(
//         builder: (context) => FixedInterestCompare(),
//       ),
//     );
//   },
//   child: Icon(
//     Icons.calculate,
//     size: 40,
//   ),
// ),

          Padding(
            padding: const EdgeInsets.only(top: 8.0),
            child: FloatingActionButton(
              heroTag: "AddInvestment",
              onPressed: () {
                Navigator.of(context).push(
                  MaterialPageRoute(
                    builder: (context) => FixedInterestForm(),
                  ),
                );
              },
              child: Icon(
                Icons.add,
              ),
            ),
          ),
        ],
      ),
    );
  }
}
