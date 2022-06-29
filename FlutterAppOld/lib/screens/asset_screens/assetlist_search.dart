import 'dart:async';
import 'package:financial_app/http/webclients/asset_webclient.dart';
import 'package:financial_app/models/table_models/assetCurrentValue.dart';
import 'package:flutter/material.dart';

class AssetSearchListForm extends StatefulWidget {
  @override
  _AssetSearchListFormState createState() => _AssetSearchListFormState();
}

class _AssetSearchListFormState extends State<AssetSearchListForm> {
  TextEditingController editingController = TextEditingController();

  List<AssetCurrentValue> _list = [];

  late StreamController<List<AssetCurrentValue>> _listStock;

  void _loadData(String filter) async {

    if(filter.length < 2) {
      _list = [];
      _listStock.add(_list);
    }
    else if (filter.length > 1) {
      await TransactionWebClient().getAsset(filter).then((values) {
        _list = [];

        values.forEach((element) {
          _list.add(element);

        });
      });


      _listStock.add(_list);
    }

  }


  @override
  initState() {
    super.initState();
    {
      _listStock = StreamController<List<AssetCurrentValue>>();
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text("Selecionar Ativo"),
      ),
      body: Column(
        children: [
          Padding(
            padding: const EdgeInsets.all(8.0),
            child: TextField(
              onChanged: (value) {
                _loadData(editingController.text);
              },
              controller: editingController,
              decoration: InputDecoration(
                labelText: "Pesquisa",
                hintText: "Digite o código do ativo",
                prefixIcon: Icon(Icons.search),
                // border: OutlineInputBorder(
                //     borderRadius: BorderRadius.all(Radius.circular(10.0))),
              ),
            ),
          ),
          StreamBuilder(
            stream: _listStock.stream.distinct(),

            builder: (context, snapshot) {

              List<AssetCurrentValue> list = [];

              list = snapshot.hasData? snapshot.data as
              List<AssetCurrentValue> : [];

              list = list.toSet().toList();


              return snapshot.hasData && editingController.text.length > 1? Expanded(
                child: ListView.builder(
                  shrinkWrap: true,
                  itemCount: list.toSet().toList().length,
                  itemBuilder: (context, index) {
                    return ListTile(
                      title: Text('${list[index].assetCode}'),
                      subtitle: Text('${list[index].companyName}'),
                      onTap: () {
                        AssetCurrentValue stockSelected = list[index];
                        Navigator.pop(context, stockSelected);

                      },
                    );
                  },
                ),
              ):

              Text('É necessário digitar pelo menos 2 caracteres');
            },
          ),
        ],
      ),
    );
  }
}
