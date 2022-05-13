import 'package:extended_masked_text/extended_masked_text.dart';
import 'package:financial_app/database/dao/fixedInterest_dao.dart';
import 'package:financial_app/functions/dateFunctions.dart';
import 'package:financial_app/functions/fixedInterestPosition.dart';
import 'package:financial_app/functions/lists/common_lists.dart';
import 'package:financial_app/models/table_models/fixedInterest.dart';
import 'package:financial_app/models/table_models/fixedInterestType.dart';
import 'package:financial_app/models/table_models/index.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';

class FixedInterestForm extends StatefulWidget {
  final FixedInterest? fixedInterest;

  FixedInterestForm([this.fixedInterest]);

  @override
  _FixedInterestFormState createState() => _FixedInterestFormState();
}

class _FixedInterestFormState extends State<FixedInterestForm> {
  final FixedInterestDao _daoFixedInterest = FixedInterestDao();

  var _amountController = MoneyMaskedTextController(leftSymbol: 'R\$ ');

  var _indexPercentageController = MoneyMaskedTextController(initialValue: 0);
  var _additionalFixedInterestController =
      MoneyMaskedTextController(initialValue: 0);
  DateTime? _dateInicial;

  DateTime? _expirationDate;

  bool _liquidityOnExpirationController = true;

  final TextEditingController _nameController = TextEditingController();

  final TextEditingController _investmentDateController =
      TextEditingController();

  final TextEditingController _expirationDateController =
      TextEditingController();

  FixedInterestType? _fixedTypeController;

  List<FixedInterestType> fixedInterestTypeList =
      CommonLists.fixedInterestTypeList;

  Index? _indexController;

  bool _firstPress = true;

  bool _preFixedInvestmentController = false;

  final _formKey = GlobalKey<FormState>();

  List<Index> indexList = [];

  @override
  void initState() {
    // TODO: implement initState
    super.initState();

    indexList = CommonLists.indexList;
    if (widget.fixedInterest != null) {
      _amountController = MoneyMaskedTextController(
          leftSymbol: 'R\$ ', initialValue: widget.fixedInterest!.amount);

      _indexPercentageController = MoneyMaskedTextController(
          initialValue: (widget.fixedInterest!.indexPercentage * 100));

      _indexController = indexList.firstWhere(
          (element) => element.name == widget.fixedInterest!.indexName);

      _additionalFixedInterestController = MoneyMaskedTextController(
          initialValue: (widget.fixedInterest!.additionalFixedInterest * 100));

      _dateInicial = widget.fixedInterest!.investmentDate;
      _expirationDate = widget.fixedInterest!.expirationDate;

      _liquidityOnExpirationController =
          widget.fixedInterest!.liquidityOnExpiration == 1;
      _preFixedInvestmentController =
          widget.fixedInterest!.preFixedInvestment == 1;

      _fixedTypeController = fixedInterestTypeList.firstWhere(
          (element) => element.id == widget.fixedInterest!.typeFixedInterestId);

      _nameController.text = widget.fixedInterest!.name;
      _investmentDateController.text =
          DateFormat('dd/MM/yyyy').format(widget.fixedInterest!.investmentDate);
      _expirationDateController.text =
          DateFormat('dd/MM/yyyy').format(widget.fixedInterest!.expirationDate);
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text((widget.fixedInterest == null? "Adicionar" : "Editar")+ " Renda Fixa"),
      ),
      body: Form(
        key: _formKey,
        child: Padding(
          padding: const EdgeInsets.fromLTRB(16.0, 0.0, 16.0, 0.0),
          child: SingleChildScrollView(
            child: Column(
              children: [
                Padding(
                  padding: const EdgeInsets.only(top: 8.0, left: 0),
                  child: DropdownButtonFormField<FixedInterestType>(
                    value: _fixedTypeController,
                    decoration: InputDecoration(
                      labelText: 'Tipo de Investimento',
                    ),
                    validator: (value) {
                      if (_fixedTypeController == null) {
                        return 'É necessário selecionar uma opção';
                      }
                      return null;
                    },
                    items: fixedInterestTypeList
                        .map<DropdownMenuItem<FixedInterestType>>(
                            (_fixedTypeController) {
                      return DropdownMenuItem<FixedInterestType>(
                        value: _fixedTypeController,
                        child: Text(
                          _fixedTypeController.name,
                        ),
                      );
                    }).toList(),
                    onChanged: (FixedInterestType? newValue) {
                      setState(
                        () {
                          _fixedTypeController = newValue!;
                        },
                      );
                    },
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.only(top: 8.0),
                  child: TextFormField(
                    validator: (value) {
                      if (value == '') {
                        return 'Coloque um nome de identificação (Ex: Nome do'
                            ' banco)';
                      }
                      return null;
                    },
                    controller: _nameController,
                    decoration: InputDecoration(
                      labelText: 'Nome',
                    ),

                  ),
                ),
                Padding(
                  padding: const EdgeInsets.only(top: 8.0),
                  child: new TextFormField(
                    validator: (value) {
                      double valueCompare = _amountController.numberValue;
                      if (valueCompare < 0.01) {
                        return 'O Valor deve ser maior que zero';
                      }
                      return null;
                    },
                    controller: _amountController,
                    autocorrect: true,
                    decoration: InputDecoration(
                      labelText: 'Valor Investido',
                    ),

                    keyboardType:
                        TextInputType.numberWithOptions(decimal: true),
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.only(top: 8.0),
                  child: new TextFormField(
                    readOnly: true,
                    validator: (value) {
                      if (value == '') {
                        return 'Insira a data do investimento';
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
                        firstDate: DateTime(1960),
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
                      labelText: 'Data do Investimento',
                    ),

                  ),
                ),
                Visibility(
                  visible: !(_fixedTypeController != null &&
                      _fixedTypeController!.name == "Poupança"),
                  child: Padding(
                    padding: const EdgeInsets.only(top: 8.0),
                    child: new TextFormField(
                      readOnly: true,
                      validator: (value) {
                        if (value == '') {
                          return 'Insira a data de vencimento do título';
                        } else if (DateFormat('dd/MM/yyy')
                                    .parse(value!)
                                    .weekday ==
                                6 ||
                            DateFormat('dd/MM/yyy').parse(value).weekday == 7) {
                          return 'A data não pode ser um final de semana';
                        }
                        return null;
                      },
                      controller: _expirationDateController,
                      onTap: () async {
                        FocusScope.of(context).requestFocus(new FocusNode());
                        final DateTime? picked = await showDatePicker(
                          context: (context),
                          initialDate: _expirationDate == null
                              ? getInitialDatePicker()
                              : _expirationDate!,
                          firstDate: getInitialDatePicker(),
                          selectableDayPredicate: predicate,
                          lastDate: DateTime(DateTime.now().year + 100),
                        );
                        if (picked != null) {
                          setState(
                            () {
                              _expirationDate = picked;
                            },
                          );
                        }
                        _expirationDateController.text =
                            DateFormat('dd/MM/yyyy').format(_expirationDate!);
                      },
                      decoration: InputDecoration(
                        labelText: 'Data de Vencimento',
                      ),
                    ),
                  ),
                ),
                Visibility(
                  visible: !(_fixedTypeController != null &&
                      _fixedTypeController!.name == "Poupança"),
                  child: Padding(
                    padding: const EdgeInsets.only(top: 8.0),
                    child: Row(
                      children: [
                        Checkbox(
                          checkColor: Colors.white,
                          value: _liquidityOnExpirationController,
                          onChanged: (bool? value) {
                            setState(
                              () {
                                _liquidityOnExpirationController = value!;
                              },
                            );
                          },
                        ),
                        Text('Liquidez no vencimento',),
                      ],
                    ),
                  ),
                ),
                Visibility(
                  visible: !(_fixedTypeController != null &&
                      _fixedTypeController!.name == "Poupança"),
                  child: Padding(
                    padding: const EdgeInsets.only(top: 8.0),
                    child: new TextField(
                      controller: _additionalFixedInterestController,
                      autocorrect: true,
                      decoration: InputDecoration(
                        labelText: 'Rendimento Fixo (%)',
                      ),
                      keyboardType:
                          TextInputType.numberWithOptions(decimal: true),
                    ),
                  ),
                ),
                Visibility(
                  visible: !(_fixedTypeController != null &&
                      _fixedTypeController!.name == "Poupança"),
                  child: Padding(
                    padding: const EdgeInsets.only(top: 8.0),
                    child: Row(
                      children: [
                        Checkbox(
                          checkColor: Colors.white,
                          value: _preFixedInvestmentController,
                          onChanged: (bool? value) {
                            setState(
                              () {
                                _preFixedInvestmentController = value!;
                              },
                            );
                          },
                        ),
                        Text('Pré Fixado'),
                      ],
                    ),
                  ),
                ),
                Visibility(
                  visible: _preFixedInvestmentController == false &&
                      !(_fixedTypeController != null &&
                          _fixedTypeController!.name == "Poupança"),
                  child: Padding(
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
                ),
                Visibility(
                  visible: _preFixedInvestmentController == false &&
                      !(_fixedTypeController != null &&
                          _fixedTypeController!.name == "Poupança"),
                  child: Padding(
                    padding: const EdgeInsets.only(top: 8.0),
                    child: new TextField(
                      controller: _indexPercentageController,
                      autocorrect: true,
                      decoration: InputDecoration(
                        labelText: _indexController != null
                            ? '% ' + _indexController!.name
                            : 'Indice (%)',
                      ),
                      keyboardType:
                          TextInputType.numberWithOptions(decimal: true),
                    ),
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.only(top: 16.0),
                  child: SizedBox(
                    width: double.maxFinite,
                    child: ElevatedButton(
                      onPressed: () async {
                        if (_formKey.currentState!.validate() && _firstPress) {
                          if (_fixedTypeController!.name != "Poupança" &&
                              _indexPercentageController.numberValue == 0 &&
                              _additionalFixedInterestController.numberValue ==
                                  0) {
                            final snackBar = SnackBar(
                              content: Text(
                                  'É necessario valor de rendimento fixo ou atrelado a um indice'),
                              action: SnackBarAction(
                                label: 'Entendi',
                                onPressed: () {
                                  // Some code to undo the change.
                                },
                              ),
                            );
                            ScaffoldMessenger.of(context)
                                .showSnackBar(snackBar);
                          } else {
                            if (_fixedTypeController!.name != "Poupança" &&
                                _expirationDate!.millisecondsSinceEpoch <
                                    _dateInicial!.millisecondsSinceEpoch) {
                              final snackBar = SnackBar(
                                content: Text(
                                    'A data inicial deve ser menor do que a final'),
                                action: SnackBarAction(
                                  label: 'Entendi',
                                  onPressed: () {
                                    // Some code to undo the change.
                                  },
                                ),
                              );
                              ScaffoldMessenger.of(context)
                                  .showSnackBar(snackBar);
                            } else {
                              if (_indexPercentageController.numberValue >
                                      0.00 &&
                                  _indexController == null) {
                                final SnackBar snackBar = SnackBar(
                                  content: Text('Selecione um indice'),
                                  action: SnackBarAction(
                                    label: "Entendi",
                                    onPressed: () {
                                      // Some code to undo the change.
                                    },
                                  ),
                                );
                                ScaffoldMessenger.of(context)
                                    .showSnackBar(snackBar);
                              } else {
                                final int typeFixedInterestId =
                                    _fixedTypeController!.id;
                                final String name = _nameController.text;

                                final double amount =
                                    _amountController.numberValue;

                                final double additionalFixedInterest =
                                    _fixedTypeController!.name == "Poupança"
                                        ? 0
                                        : _additionalFixedInterestController
                                                .numberValue /
                                            100;

                                final DateTime investmentDate = _dateInicial!;

                                final DateTime expirationDate =
                                    _fixedTypeController!.name == "Poupança"
                                        ? DateTime(DateTime.now().year + 100)
                                        : _expirationDate!;
                                final int preFixedInvestment =
                                    _fixedTypeController!.name == "Poupança"
                                        ? 0
                                        : _preFixedInvestmentController == true
                                            ? 1
                                            : 0;

                                final String indexName = preFixedInvestment == 1
                                    ? ""
                                    : _indexController!.name;
                                final double indexPercentage =
                                    _fixedTypeController!.name == "Poupança"
                                        ? 0
                                        : _preFixedInvestmentController
                                                    .toString() ==
                                                'true'
                                            ? 0
                                            : _indexPercentageController
                                                    .numberValue /
                                                100;
                                final int liquidityOnExpiration =
                                    _liquidityOnExpirationController
                                                .toString() ==
                                            'true'
                                        ? 1
                                        : 0;

                                bool investmentDateIsHoliday =
                                    await isHoliday(investmentDate);

                                bool expirationDateIsHoliday =
                                    await isHoliday(expirationDate);

                                if (expirationDateIsHoliday ||
                                    investmentDateIsHoliday) {

                                  String warningText = expirationDateIsHoliday && investmentDateIsHoliday?
                                    "As data de investimento e vencimento não podem ser em feriados":
                                  expirationDateIsHoliday? "A data de vencimento não pode ser um feriado":
                                  investmentDateIsHoliday? "A data de investimento não pode ser um feriado": "";
                                  final snackBar = SnackBar(
                                    content:
                                    Text(warningText),
                                    action: SnackBarAction(
                                      label: 'Entendi',
                                      onPressed: () {
                                        // Some code to undo the change.
                                      },
                                    ),
                                  );
                                  ScaffoldMessenger.of(context)
                                      .showSnackBar(snackBar);


                                } else {
                                  final FixedInterest newInvestment =
                                      FixedInterest(
                                          name: name,
                                          typeFixedInterestId:
                                              typeFixedInterestId,
                                          amount: amount,
                                          indexName: indexName,
                                          indexPercentage: indexPercentage,
                                          additionalFixedInterest:
                                              additionalFixedInterest,
                                          investmentDate: investmentDate,
                                          expirationDate: expirationDate,
                                          liquidityOnExpiration:
                                              liquidityOnExpiration,
                                          preFixedInvestment:
                                              preFixedInvestment);

                                  _firstPress = false;

                                  if (widget.fixedInterest == null) {

                                    await _daoFixedInterest.save(newInvestment).then(
                                        (id) async {
                                          try {
                                            await fixedInterestPosition(
                                                context);
                                          }
                                          catch(e)
                                          {
                                            debugPrint(e.toString());
                                          }
                                        Navigator.pop(
                                            context, newInvestment.toString());
                                        } );

                                  } else {

                                    newInvestment.id = widget.fixedInterest!.id;
                                    await _daoFixedInterest
                                        .updateRow(newInvestment)
                                        .then((id) async {
                                          print("ae");
                                      await fixedInterestPosition(context);
                                      Navigator.pop(
                                          context, newInvestment.toString());
                                    } );
                                    
                                  }
                                }
                              }
                            }
                          }
                        }
                      },
                      child: Text(widget.fixedInterest == null? "Adicionar" : "Atualizar"),
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
