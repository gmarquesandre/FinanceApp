import 'package:extended_masked_text/extended_masked_text.dart';
import 'package:financial_app/database/dao/asset_simulation.dart';
import 'package:financial_app/functions/lists/common_lists.dart';
import 'package:financial_app/models/table_models/assetSimulation.dart';
import 'package:financial_app/models/table_models/index.dart';
import 'package:financial_app/screens/simulation_screens/asset_simulation/simulation_assetlist_search.dart';
import 'package:flutter/material.dart';

class AssetSimulationForm extends StatefulWidget {
  final AssetSimulation? assetSimulation;

  AssetSimulationForm([this.assetSimulation]);

  @override
  _AssetSimulationForm createState() => _AssetSimulationForm();
}

class _AssetSimulationForm extends State<AssetSimulationForm> {
  final _formKey = GlobalKey<FormState>();


  AssetSimulationDao _dao = AssetSimulationDao();

  TextEditingController _assetCodeController = TextEditingController();

  var _additionalFixedInterestController =
      MoneyMaskedTextController(initialValue: 0);
  Index? _indexController;
  List<Index> indexList = [];

  @override
  void initState() {
    // TODO: implement initState

    indexList = CommonLists.indexList;
    super.initState();

    if (widget.assetSimulation != null) {

      _assetCodeController.text = widget.assetSimulation!.assetCode;

      _additionalFixedInterestController = MoneyMaskedTextController(
          initialValue: (widget.assetSimulation!.fixedYearGain*100));

      _indexController = indexList.firstWhere(
          (element) => element.name == widget.assetSimulation!.indexName);
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
                          return 'É necessario escolher um ativo';
                        }
                        return null;
                      },
                      readOnly: true,
                      controller: _assetCodeController,
                      onTap: () {
                        Navigator.of(context)
                            .push(
                          MaterialPageRoute(
                            builder: (context) => AssetSimulationSearchListForm(),
                          ),
                        )
                            .then(
                              (assetSearch) => setState(() {
                            _assetCodeController.text =
                                assetSearch.assetCode.toString();

                          }),
                        );
                      },
                      decoration: InputDecoration(
                        labelText: 'Nome do Ativo',
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
                        labelText: 'Rendimento Fixo Anual(%)',
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
                            final String assetCode = _assetCodeController.text;

                            final AssetSimulation newAsset = AssetSimulation(
                                assetCode: assetCode,
                                indexName: _indexController!.name,
                                fixedYearGain: _additionalFixedInterestController.numberValue/100
                            );

                            if (widget.assetSimulation == null) {
                              await _dao.save(newAsset).then((id) async {

                                Navigator.pop(context, newAsset.toString());
                              });
                            } else {
                              await _dao.updateRow(newAsset).then((id) async {
                                Navigator.pop(context, newAsset.toString());
                              });
                            }
                          }
                        },
                        child: Text(widget.assetSimulation == null? "Adicionar" : "Atualizar"),
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
