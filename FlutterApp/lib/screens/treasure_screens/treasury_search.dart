import 'package:financial_app/components/progress.dart';
import 'package:financial_app/database/dao/treasuryCurrentValue_dao.dart';
import 'package:financial_app/functions/updateData.dart';
import 'package:financial_app/models/table_models/treasuryCurrentValue.dart';
import 'package:flutter/material.dart';

class TreasurySearchListForm extends StatefulWidget {
  @override
  _TreasurySearchListFormState createState() => _TreasurySearchListFormState();
}

class _TreasurySearchListFormState extends State<TreasurySearchListForm> {
  TextEditingController editingController = TextEditingController();

  final TreasuryCurrentValueDao _daoTreasuryList = TreasuryCurrentValueDao();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text("Escolha o Título"),
      ),
      body: SingleChildScrollView(
        scrollDirection: Axis.vertical,
        child: Padding(
          padding: const EdgeInsets.all(8.0),
          child: Container(
            child: FutureBuilder(
              future: _daoTreasuryList.findAll(),
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
                    final List<TreasuryCurrentValue> treasury = snapshot.data as
                    List<TreasuryCurrentValue>;
                    if(treasury.length == 0) {
                      updateTreasuryBond();
                      return Text("Não há dados para exibir.");
                    }
                    return new ListView.builder(
                          shrinkWrap: true,
                          physics: ClampingScrollPhysics(),
                          scrollDirection: Axis.vertical,
                          itemCount: treasury.length,
                          itemBuilder: (context, index) {
                            return ListTile(
                              title: Text('${treasury[index].treasuryBondName}'),
                              onTap: () {
                                TreasuryCurrentValue selected = treasury[index];
                                Navigator.pop(context, selected);
                              },
                            );
                          },
                        );
                }
                return Text('Erro Desconhecido');
              },
            ),
          ),
        ),
      ),
    );
  }
}
