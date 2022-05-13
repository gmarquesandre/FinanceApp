import 'package:financial_app/components/progress.dart';
import 'package:financial_app/database/dao/investmentFundCurrentValue_dao.dart';
import 'package:financial_app/models/table_models/investmentFundCurrentValue.dart';
import 'package:flutter/material.dart';

class FundSimulationSearchListForm extends StatefulWidget {
  @override
  _FundSimulationSearchListFormState createState() =>
      _FundSimulationSearchListFormState();
}

class _FundSimulationSearchListFormState
    extends State<FundSimulationSearchListForm> {
  TextEditingController editingController = TextEditingController();

  InvestmentFundCurrentValueDao _dao = InvestmentFundCurrentValueDao();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text("Selecionar Ativo"),
      ),
      body: SingleChildScrollView(
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
                  final List<InvestmentFundCurrentValue> funds =
                      snapshot.data as List<InvestmentFundCurrentValue>;
                  if (funds.length == 0) {
                    return Text("Não há dados para mostrar.");
                  }
                  return ListView.builder(
                    shrinkWrap: true,
                    itemCount: funds.toSet().toList().length,
                    itemBuilder: (context, index) {
                      return ListTile(
                        title: Text('${funds[index].cnpj}'),
                        subtitle: Text('${funds[index].nameShort}'),
                        onTap: () {
                          InvestmentFundCurrentValue stockSelected =
                              funds[index];
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
