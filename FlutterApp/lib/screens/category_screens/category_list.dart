import 'package:financial_app/components/progress.dart';
import 'package:financial_app/database/dao/category_dao.dart';
import 'package:financial_app/models/table_models/category.dart';
import 'package:financial_app/screens/category_screens/category_form.dart';
import 'package:flutter/material.dart';

class CategoryList extends StatefulWidget {
  @override
  _CategoryListState createState() => _CategoryListState();
}

class _CategoryListState extends State<CategoryList> {
  final CategoryDao _daoCategory = CategoryDao();

  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Categorias'),
      ),
      body: SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.all(8.0),
          child: Container(
            child: FutureBuilder(
              future: _daoCategory.findAll(),
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
                    final List<Category> categories = snapshot.data as List<Category>;

                    if (categories.length == 0) {
                      return Text("Não há dados para mostrar.");
                    }
                    return Container(
                      width: (MediaQuery.of(context).size.width),
                      child: DataTable(
                          sortAscending: true,
                          sortColumnIndex: 2,
                          columnSpacing: 20,
                          showCheckboxColumn: false,
                          columns: [
                            DataColumn(label: Text('Id')),
                            DataColumn(tooltip: 'ae', label: Text('Ativo')),
                            // DataColumn(label: Text('Quantidade')),
                            // DataColumn(label: Text('Preço Médio')),
                            DataColumn(label: Text('')),
                          ],
                          rows: categories
                              .map<DataRow>((element) => DataRow(
                                    onSelectChanged: (value) {
                                      },
                                    cells: [
                                      DataCell(Text(element.id.toString())),
                                      DataCell(Text(element.name.toString())),
                                      // DataCell(Text(element.quantity.toString())),
                                      // DataCell(Text(currencyFormat
                                      //     .format(element.avgBuyPrice)
                                      //     .toString())),
                                      DataCell(Icon(Icons.delete), onTap: () {
                                        setState(() {
                                          Widget cancelaButton = TextButton(
                                            child: Text("Cancelar"),
                                            onPressed: () {
                                              Navigator.of(context).pop();
                                            },
                                          );
                                          Widget continuaButton = TextButton(
                                            child: Text("Deletar"),
                                            onPressed: () {
                                              setState(() {
                                                _daoCategory.deleteRow(element);
                                                Navigator.of(context).pop();
                                              });
                                            },
                                          );
                                          //configura o AlertDialog
                                          AlertDialog alert = AlertDialog(
                                            title: Text("Confirmar Exclusão"),
                                            content: Text(
                                                "Deseja excluir o resgistro?"),
                                            actions: [
                                              cancelaButton,
                                              continuaButton,
                                            ],
                                          );
                                          //exibe o diálogo
                                          showDialog(
                                            context: context,
                                            builder: (BuildContext context) {
                                              return alert;
                                            },
                                          );
                                        });
                                      }),
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
      ),
      floatingActionButton: FloatingActionButton(
          onPressed: () {
            Navigator.of(context)
                .push(
                  MaterialPageRoute(
                    builder: (context) => CategoryForm(),
                  ),
                )
                .then(
                  (newCategory) => setState(() {}),
                );
          },
          child: Icon(
            Icons.add,
          )),
    );
  }
}
