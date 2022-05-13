import 'package:extended_masked_text/extended_masked_text.dart';
import 'package:financial_app/database/dao/assetCurrentValue_dao.dart';
import 'package:financial_app/database/dao/asset_dao.dart';
import 'package:financial_app/functions/assetCurrentPosition.dart';
import 'package:financial_app/functions/dateFunctions.dart';
import 'package:financial_app/models/table_models/asset.dart';
import 'package:financial_app/models/table_models/assetCurrentValue.dart';
import 'package:financial_app/screens/asset_screens/assetlist_search.dart';
import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:intl/intl.dart';

class AssetForm extends StatefulWidget {
  final Asset? asset;

  AssetForm([this.asset]);

  @override
  _AssetFormState createState() => _AssetFormState();
}

class _AssetFormState extends State<AssetForm> {
  var _averagePriceController = MoneyMaskedTextController(leftSymbol: 'R\$ ');

  TextEditingController _assetCodeController = TextEditingController();
  final TextEditingController _quantityController = TextEditingController();
  String? assetId;

  final TextEditingController _investmentDateController =
      TextEditingController();

  DateTime? _dateInicial;

  bool _firstPress = true;

  int operationCode = 1;
  final AssetDao _dao = AssetDao();

  final _formKey = GlobalKey<FormState>();

  AssetCurrentValue _assetCurrentValue = AssetCurrentValue(
      assetCode: "",
      unitPrice: 0,
      companyName: "",
      dateLastUpdate: DateTime.now());

  final AssetCurrentValueDao _daoCurrentValue = AssetCurrentValueDao();

  @override
  initState() {
    super.initState();
    {
      if (widget.asset != null) {
        _assetCodeController.text = widget.asset!.assetCode;
        _investmentDateController.text =
            DateFormat('dd/MM/yyy').format(widget.asset!.date);

        _quantityController.text = widget.asset!.quantity.toString();
        _averagePriceController = MoneyMaskedTextController(
            leftSymbol: 'R\$ ', initialValue: widget.asset!.unitPrice);
        operationCode = widget.asset!.operation;

        _dateInicial = widget.asset!.date;
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text((widget.asset == null ? "Adicionar" : "Editar") + " Ativo"),
      ),
      body: Form(
        key: _formKey,
        child: Padding(
          padding: const EdgeInsets.fromLTRB(16.0, 0.0, 16.0, 0.0),
          child: SingleChildScrollView(
            child: Column(
              children: [
                Padding(
                  padding: const EdgeInsets.only(top: 8.0),
                  child: Row(
                    children: [
                      Expanded(
                        child: ElevatedButton(
                          onPressed: () {
                            setState(
                              () {
                                operationCode = 1;
                              },
                            );
                          },
                          child: Text("Compra",
                              style: TextStyle(
                                  color: operationCode == 1
                                      ? Colors.white
                                      : Colors.black)),
                          style: ButtonStyle(
                            backgroundColor: MaterialStateProperty.all(
                                operationCode == 1
                                    ? Colors.blueGrey
                                    : Colors.white),
                          ),
                        ),
                      ),
                      Expanded(
                        child: ElevatedButton(
                          onPressed: () {
                            setState(() {
                              operationCode = 2;
                            });
                          },
                          child: Text("Venda",
                              style: TextStyle(
                                  color: operationCode == 2
                                      ? Colors.white
                                      : Colors.black)),
                          style: ButtonStyle(
                            backgroundColor: MaterialStateProperty.all(
                                operationCode == 2
                                    ? Colors.blueGrey
                                    : Colors.white),
                          ),
                        ),
                      ),
                    ],
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.only(top: 8.0),
                  child: new TextFormField(
                    readOnly: true,
                    validator: (value) {
                      if (value == '') {
                        return 'Data de ' +
                            (operationCode == 1 ? 'Compra' : 'Venda') +
                            'deve estar '
                                'preenchida';
                      } else if (DateFormat('dd/MM/yyy')
                                  .parse(value!)
                                  .weekday ==
                              6 ||
                          DateFormat('dd/MM/yyy').parse(value).weekday == 7) {
                        return 'A data não pode ser um final de semana';
                      }
                      return null;
                    },
                    controller: _investmentDateController,
                    onTap: () async {
                      FocusScope.of(context).requestFocus(new FocusNode());
                      final DateTime? picked = await showDatePicker(
                        context: (context),
                        initialDate: _dateInicial == null
                            ? getInitialDatePicker()
                            : _dateInicial!,
                        selectableDayPredicate: predicate,
                        firstDate: DateTime(2010),
                        lastDate: getInitialDatePicker(),
                      );
                      if (picked != null) {
                        setState(() {
                          _dateInicial = picked;
                        });
                      }
                      _investmentDateController.text =
                          DateFormat('dd/MM/yyyy').format(_dateInicial!);
                    },
                    decoration: InputDecoration(
                      labelText: 'Data de ' +
                          (operationCode == 1 ? 'Compra' : 'Venda'),
                    ),
                  ),
                ),
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
                              builder: (context) => AssetSearchListForm(),
                            ),
                          )
                          .then(
                            (assetSearch) => setState(() {
                              _assetCodeController.text =
                                  assetSearch.assetCode.toString();
                              _assetCurrentValue = assetSearch;
                            }),
                          );
                    },
                    decoration: InputDecoration(
                      labelText: 'Nome do Ativo',
                    ),
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.only(top: 8.0),
                  child: new TextFormField(
                    validator: (value) {
                      double valueCompare = _averagePriceController.numberValue;
                      if (valueCompare < 0.01) {
                        return 'O Valor deve ser maior que zero';
                      }
                      return null;
                    },
                    controller: _averagePriceController,
                    autocorrect: true,
                    decoration: InputDecoration(
                      labelText: 'Preço Unitário',
                    ),
                    keyboardType:
                        TextInputType.numberWithOptions(decimal: true),
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.only(top: 8.0),
                  child: TextFormField(
                    validator: (value) {
                      if (value == '' || int.tryParse(value!)! < 1) {
                        return 'O Valor deve ser maior que zero';
                      }
                      return null;
                    },
                    controller: _quantityController,
                    inputFormatters: [FilteringTextInputFormatter.digitsOnly],
                    decoration: InputDecoration(
                      //border: OutlineInputBorder(),
                      labelText: 'Quantidade',
                    ),
                    keyboardType: TextInputType.number,
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.only(top: 16.0),
                  child: SizedBox(
                    width: double.maxFinite,
                    child: ElevatedButton(

                      onPressed: () async {
                        if (_formKey.currentState!.validate() && _firstPress) {
                          final String assetCode = _assetCodeController.text;

                          final int quantity =
                              int.tryParse(_quantityController.text)!;

                          final double averageBuyPrice =
                              _averagePriceController.numberValue;

                          final Asset newAsset = Asset(
                              date: _dateInicial!,
                              assetCode: assetCode,
                              quantity: quantity,
                              unitPrice: averageBuyPrice,
                              operation: operationCode);

                          _firstPress = false;

                          if (widget.asset == null) {
                            await _dao.save(newAsset).then((id) async {
                              await _daoCurrentValue.save(_assetCurrentValue);
                              await assetCurrentPosition(context);
                              Navigator.pop(context, newAsset.toString());
                            });
                          } else {
                            newAsset.id = widget.asset!.id;
                            await _dao.updateRow(newAsset).then((id) async {
                              await assetCurrentPosition(context);
                              Navigator.pop(context, newAsset.toString());
                            });
                          }
                        }
                      },
                      child: Text(
                          widget.asset == null ? "Adicionar" : "Atualizar"),
                    ),
                  ),
                )
              ],
            ),
          ),
        ),
      ),
    );
  }
}
