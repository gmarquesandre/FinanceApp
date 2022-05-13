import 'package:extended_masked_text/extended_masked_text.dart';
import 'package:financial_app/database/dao/fund_simulation.dart';
import 'package:financial_app/functions/lists/common_lists.dart';
import 'package:financial_app/models/table_models/fundSimulation.dart';
import 'package:financial_app/models/table_models/index.dart';
import 'package:financial_app/screens/simulation_screens/fund_simulation/simulation_fundlist_search.dart';
import 'package:flutter/material.dart';

class FundSimulationForm extends StatefulWidget {
  final FundSimulation? fundSimulation;

  FundSimulationForm([this.fundSimulation]);

  @override
  _FundSimulationForm createState() => _FundSimulationForm();
}

class _FundSimulationForm extends State<FundSimulationForm> {
  final _formKey = GlobalKey<FormState>();


  FundSimulationDao _dao = FundSimulationDao();

  TextEditingController _fundCnpjController = TextEditingController();
  TextEditingController _nameshortController = TextEditingController();

  var _additionalFixedInterestController =
      MoneyMaskedTextController(initialValue: 0);
  Index? _indexController;
  List<Index> indexList = [];

  @override
  void initState() {
    // TODO: implement initState

    indexList = CommonLists.indexList;
    super.initState();

    if (widget.fundSimulation != null) {

      debugPrint(widget.fundSimulation!.toString());

      _nameshortController.text = widget.fundSimulation!.nameShort;
      _fundCnpjController.text = widget.fundSimulation!.cnpj;

      _additionalFixedInterestController = MoneyMaskedTextController(
          initialValue: (widget.fundSimulation!.fixedYearGain*100));

      _indexController = indexList.firstWhere(
          (element) => element.name == widget.fundSimulation!.indexName);
    }
  }

  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(title: Text("Adicionar Rendimento")),
      body: SingleChildScrollView(
        child: Form(
          key: _formKey,
          child: Padding(
            padding: const EdgeInsets.fromLTRB(16.0, 0.0, 16.0, 0.0),
            child: SingleChildScrollView(
              child: Column(
                children: [
                  Padding(
                    padding: const EdgeInsets.only(top: 8.0),
                    child: TextFormField(
                      validator: (value) {
                        if (value == '' || value == null) {
                          return 'É necessario escolher um fundo';
                        }
                        return null;
                      },
                      readOnly: true,
                      controller: _fundCnpjController,
                      onTap: () {
                        Navigator.of(context)
                            .push(
                          MaterialPageRoute(
                            builder: (context) => FundSimulationSearchListForm(),
                          ),
                        )
                            .then(
                              (fundSearch) => setState(() {
                            _fundCnpjController.text =
                                fundSearch.cnpj.toString();

                            _nameshortController.text = fundSearch.nameShort.toString();


                          }),
                        );
                      },
                      decoration: InputDecoration(
                        labelText: 'CNPJ',
                      ),
                    ),
                  ),
                  Padding(
                    padding: const EdgeInsets.only(top: 8.0),
                    child: TextFormField(
                      validator: (value) {
                        if (value == '' || value == null) {
                          return 'É necessario escolher um fundo';
                        }
                        return null;
                      },
                      readOnly: true,
                      controller: _nameshortController,
                      onTap: () {
                        Navigator.of(context)
                            .push(
                          MaterialPageRoute(
                            builder: (context) => FundSimulationSearchListForm(),
                          ),
                        )
                            .then(
                              (fundSearch) => setState(() {
                            _fundCnpjController.text =
                                fundSearch.cnpj.toString();

                            _nameshortController.text = fundSearch.nameShort.toString();


                          }),
                        );
                      },
                      decoration: InputDecoration(
                        labelText: 'Nome do Fundo',
                      ),
                    ),
                  ),
                  Padding(
                    padding: const EdgeInsets.only(top: 8.0, left: 0),
                    child: DropdownButtonFormField<Index>(
                      value: _indexController,
                      decoration: InputDecoration(
                        labelText: 'Indice',
                      ),
                      items: indexList
                          .map<DropdownMenuItem<Index>>((_indexController) {
                        return DropdownMenuItem<Index>(
                          value: _indexController,
                          child: Text(
                            _indexController.name,
                          ),
                        );
                      }).toList(),
                      onChanged: (Index? newValue) {
                        setState(
                          () {
                            _indexController = newValue!;
                          },
                        );
                      },
                    ),
                  ),
                  Padding(
                    padding: const EdgeInsets.only(top: 8.0),
                    child: new TextField(
                      controller: _additionalFixedInterestController,
                      autocorrect: true,
                      decoration: InputDecoration(
                        labelText: 'Rendimento Fixo Anua (%)',
                      ),
                      keyboardType:
                      TextInputType.numberWithOptions(decimal: true),
                    ),
                  ),
                  Padding(
                    padding: const EdgeInsets.only(top: 16.0),
                    child: SizedBox(
                      width: double.maxFinite,
                      child: ElevatedButton(
                        onPressed: () async {
                          if (_formKey.currentState!.validate()) {
                            final String fundCnpj = _fundCnpjController.text;
                            final String nameShort = _nameshortController.text;

                            final FundSimulation newFund = FundSimulation(
                              cnpj: fundCnpj,
                                nameShort: nameShort,
                                indexName: _indexController!.name,
                                fixedYearGain: _additionalFixedInterestController.numberValue/100,
                            );

                            if (widget.fundSimulation == null) {
                              await _dao.save(newFund).then((id) async {

                                Navigator.pop(context);
                              });
                            } else {
                              await _dao.updateRow(newFund).then((id) async {
                                Navigator.pop(context, newFund.toString());
                              });
                            }
                          }
                        },
                        child: Text(widget.fundSimulation == null? "Adicionar" : "Atualizar"),
                      ),
                    ),
                  )
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }
}
