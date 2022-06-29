import 'dart:async';
import 'package:financial_app/http/webclients/asset_webclient.dart';
import 'package:financial_app/models/table_models/investmentFundCurrentValue.dart';
import 'package:flutter/material.dart';

class InvestmentFundListForm extends StatefulWidget {
  @override
  _InvestmentFundListFormState createState() => _InvestmentFundListFormState();
}

class _InvestmentFundListFormState extends State<InvestmentFundListForm> {
  TextEditingController editingController = TextEditingController();

  List<InvestmentFundCurrentValue> _list = [];

  late StreamController<List<InvestmentFundCurrentValue>> _listStock;

  void _loadData(String filter) async {

    if(filter.length < 5) {
      _list = [];
      _listStock.add(_list);
    }
    else if (filter.length >=5 ) {
      await TransactionWebClient().getFund(filter).then((values) {
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
      _listStock = StreamController<List<InvestmentFundCurrentValue>>();
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text("Selecionar Fundo"),
      ),
      body: Column(
        children: [
          Padding(
            padding: const EdgeInsets.all(8.0),
            child: TextField(
              onChanged: (value) {
                setState(() {});
              },
              controller: editingController,
              decoration: InputDecoration(
                // labelText: "Pesquisa",
                hintText: "Digite o CNPJ ou nome do fundo",
                suffix: ElevatedButton(
                  onPressed: () async {
                    _loadData(editingController.text);
                  },
                  child: Text('Pesquisar'),
                ),
                // suffixIcon: IconButton(
                //   icon: Icon(Icons.search),
                //   onPressed: () {
                //     _loadData(editingController.text);
                //     // do something
                //   },
                // ),
                // border: OutlineInputBorder(
                //     borderRadius: BorderRadius.all(Radius.circular(10.0))),
              ),
            ),
          ),
          StreamBuilder(
            stream: _listStock.stream,
            builder: (context, snapshot) {
              List<InvestmentFundCurrentValue> list = snapshot.hasData
                  ? snapshot.data as List<InvestmentFundCurrentValue>
                  : [];
              return snapshot.hasData && editingController.text.length > 1
                  ? Expanded(
                      child: ListView.builder(
                        shrinkWrap: true,
                        itemCount: list.length,
                        itemBuilder: (context, index) {
                          return ListTile(
                            title: Text('${list[index].nameShort}'),
                            subtitle: Text('${list[index].cnpj}'),
                            onTap: () {
                              InvestmentFundCurrentValue selected = list[index];
                              Navigator.pop(context, selected);
                            },
                          );
                        },
                      ),
                    )
                  : Text(editingController.text.length < 5
                      ? 'É necessário digitar mais ${(5 - editingController.text.length).toString()} caracteres'
                      : '');
            },
          ),
        ],
      ),
    );
  }
}
