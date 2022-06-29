import 'package:financial_app/components/progress.dart';
import 'package:financial_app/database/dao/assetCurrentValue_dao.dart';
import 'package:financial_app/models/table_models/assetCurrentValue.dart';
import 'package:flutter/material.dart';

class AssetSimulationSearchListForm extends StatefulWidget {
  @override
  _AssetSimulationSearchListFormState createState() => _AssetSimulationSearchListFormState();
}

class _AssetSimulationSearchListFormState extends State<AssetSimulationSearchListForm> {
  TextEditingController editingController = TextEditingController();


  AssetCurrentValueDao _dao = AssetCurrentValueDao();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text("Selecionar Ativo"),
      ),
      body:  SingleChildScrollView(
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
                  final List<AssetCurrentValue> assets =
                  snapshot.data as List<AssetCurrentValue>;
                  if (assets.length == 0) {
                    return Text("Não há dados para mostrar.");
                  }
                  return
                    ListView.builder(
                      shrinkWrap: true,
                      itemCount: assets.toSet().toList().length,
                      itemBuilder: (context, index) {
                        return ListTile(
                          title: Text('${assets[index].assetCode}'),
                          subtitle: Text('${assets[index].companyName}'),
                          onTap: () {
                            AssetCurrentValue stockSelected = assets[index];
                            Navigator.pop(context, stockSelected);

                          },
                        );
                      },
                    );

              }
              return Text("Erro Desconhecido");
            },
          ),
        ),
      ),
    );
  }
}
