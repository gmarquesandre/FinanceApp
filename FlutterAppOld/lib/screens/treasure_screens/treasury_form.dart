import 'package:extended_masked_text/extended_masked_text.dart';
import 'package:financial_app/database/dao/treasury_dao.dart';
import 'package:financial_app/functions/dateFunctions.dart';
import 'package:financial_app/functions/treasuryCurrentPosition.dart';
import 'package:financial_app/http/webclients/asset_webclient.dart';
import 'package:financial_app/models/table_models/treasury.dart';
import 'package:financial_app/screens/treasure_screens/treasury_search.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';

class TreasuryForm extends StatefulWidget {
  final Treasury? treasury;

  TreasuryForm([this.treasury]);

  @override
  _TreasuryFormState createState() => _TreasuryFormState();
}

class _TreasuryFormState extends State<TreasuryForm> {
  var _unitValueController = MoneyMaskedTextController(leftSymbol: 'R\$');

  var _quantityController =MoneyMaskedTextController(
      initialValue: 0.00);

  DateTime? _dateTimeInitial;

  TextEditingController _initialDateController = TextEditingController();

  final _formKey = GlobalKey<FormState>();
  String codeISIN = '';
  final TreasuryDao _daoTreasury = TreasuryDao();

  TextEditingController _treasuryNameController = TextEditingController();

  int operationCode = 1;

  @override
  void initState() {
    // TODO: implement initState
    super.initState();

    if (widget.treasury != null) {
      _dateTimeInitial = widget.treasury!.date;

      _unitValueController = MoneyMaskedTextController(
          leftSymbol: 'R\$'
              ' ',
          initialValue: widget.treasury!.unitPrice);

      _initialDateController.text =
          DateFormat('dd/MM/yyyy').format(widget.treasury!.date);

      _treasuryNameController.text = widget.treasury!.treasuryBondName;

      _quantityController = MoneyMaskedTextController(
          leftSymbol: 'R\$'
              ' ',
          initialValue: widget.treasury!.quantity);

      operationCode = widget.treasury!.operation;
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(
            (widget.treasury == null ? "Adicionar" : "Editar") + " Tesouro"),
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
                          onPressed: () async {
                            setState(() {
                              operationCode = 1;
                            });
                            if (_dateTimeInitial != null && codeISIN != '') {
                              _unitValueController = await getValue(context,
                                  _dateTimeInitial!, codeISIN, operationCode);
                              setState(() {});
                            }
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
                          onPressed: () async {
                            setState(() {
                              operationCode = 2;
                            });
                            if (_dateTimeInitial != null && codeISIN != '') {
                              _unitValueController = await getValue(context,
                                  _dateTimeInitial!, codeISIN, operationCode);
                              setState(() {});
                            }
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
                    controller: _initialDateController,
                    onTap: () async {
                      FocusScope.of(context).requestFocus(new FocusNode());
                      final DateTime? picked = await showDatePicker(
                        context: (context),
                        initialDate: _dateTimeInitial == null
                            ? getInitialDatePicker()
                            : _dateTimeInitial!,
                        firstDate: DateTime(1990),
                        selectableDayPredicate: predicate,
                        lastDate: DateTime.now(),
                      );
                      if (picked != null) {
                        setState(() {
                          _dateTimeInitial = picked;
                        });
                      }

                      _initialDateController.text =
                          DateFormat('dd/MM/yyyy').format(_dateTimeInitial!);
                      if (_dateTimeInitial != null && codeISIN != '') {
                        _unitValueController = await getValue(context,
                            _dateTimeInitial!, codeISIN, operationCode);
                        setState(() {});
                      }
                    },
                    decoration: InputDecoration(
                      labelText: 'Data ' +
                          (operationCode == 1
                              ? 'Compra'
                              : 'Vend'
                                  'a'),
                    ),
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.only(top: 8.0),
                  child: TextFormField(
                    validator: (value) {
                      if (value == '' || value == null) {
                        return 'É necessario escolher um título';
                      }
                      return null;
                    },
                    readOnly: true,
                    controller: _treasuryNameController,
                    onTap: () async {
                      Navigator.of(context)
                          .push(
                        MaterialPageRoute(
                          builder: (context) => TreasurySearchListForm(),
                        ),
                      )
                          .then((newTreasury) async {
                        setState(() {
                          _treasuryNameController.text =
                              newTreasury.treasuryBondName.toString();
                          codeISIN = newTreasury.codeISIN.toString();
                        });
                        if (_dateTimeInitial != null && codeISIN != '') {
                          _unitValueController = await getValue(context,
                              _dateTimeInitial!, codeISIN, operationCode);
                          setState(() {});
                        }
                      });
                    },
                    decoration: InputDecoration(
                      labelText: 'Nome do Título',
                    ),
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.only(top: 8.0),
                  child: new TextFormField(
                    validator: (value) {
                      double? valueCompare = double.tryParse(value
                          .toString()
                          .replaceAll('R\$ ', '')
                          .replaceAll(".", "")
                          .replaceAll(",", "."));
                      if (valueCompare! < 0.01) {
                        return 'A quantidade deve ser maior que zero';
                      }
                      return null;
                    },
                    controller: _quantityController,
                    autocorrect: true,
                    decoration: InputDecoration(
                      labelText: 'Quantidade ' +
                          (operationCode == 1
                              ? 'Compra'
                              : 'Vend'
                                  'a'),
                    ),
                    keyboardType:
                        TextInputType.numberWithOptions(decimal: true),
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.only(top: 8.0),
                  child: TextFormField(
                    validator: (value) {
                      if (_unitValueController.numberValue < 0.01) {
                        return 'O Valor deve ser maior que zero';
                      }
                      return null;
                    },
                    enabled: false,
                    controller: _unitValueController,
                    decoration: InputDecoration(
                      labelText: 'Preço Unitário de ' +
                          (operationCode == 1 ? 'Compra' : 'Venda'),
                    ),
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.only(top: 24.0),
                  child: SizedBox(
                    width: double.maxFinite,
                    child: ElevatedButton(
                      onPressed: () async {
                        if (_formKey.currentState!.validate()) {
                          final double unitPrice =
                              _unitValueController.numberValue;

                          final double quantity =_quantityController.numberValue;

                          final DateTime date = _dateTimeInitial!;

                          final String treasuryBondName =
                              _treasuryNameController.text;

                          final Treasury newTreasury = Treasury(
                              treasuryBondName: treasuryBondName,
                              unitPrice: unitPrice,
                              operation: operationCode,
                              quantity: quantity,
                              date: date);

                          if (widget.treasury == null) {
                            await _daoTreasury
                                .save(newTreasury)
                                .then((id) async {
                              await treasuryCurrentPosition(context);
                              // Navigator.pushAndRemoveUntil(
                              //   context,
                              //   MaterialPageRoute(builder: (context) => TreasuryForm()),
                              //       (Route<dynamic> route) => false,
                              // );

                              Navigator.pop(context, newTreasury.toString());
                            });
                          } else {
                            newTreasury.id = widget.treasury!.id;
                            await _daoTreasury
                                .updateRow(newTreasury)
                                .then((id) async {
                              await treasuryCurrentPosition(context);
                              Navigator.pop(context, newTreasury.toString());
                            });
                          }
                        } else {
                          final snackBar = SnackBar(
                            content:
                                Text('A quantidade de venda é maior do que a '
                                    'quantidade disponivel.'),
                            action: SnackBarAction(
                              label: 'Entendi',
                              onPressed: () {
                                // Some code to undo the change.
                              },
                            ),
                          );
                          ScaffoldMessenger.of(context).showSnackBar(snackBar);
                        }
                      },
                      child: Text(
                          widget.treasury == null ? "Adicionar" : "Atualizar"),
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

Future<MoneyMaskedTextController> getValue(BuildContext context, DateTime date,
    String codeISIN, int operationCode) async {
  if (await isHoliday(date)) {
    final snackBar = SnackBar(
      content: Text('A data é um feriado.'),
      action: SnackBarAction(
        label: 'Entendi',
        onPressed: () {
          // Some code to undo the change.
        },
      ),
    );
    ScaffoldMessenger.of(context).showSnackBar(snackBar);

    return MoneyMaskedTextController(leftSymbol: 'R\$', initialValue: 0);
  }

  double? value = await TransactionWebClient()
      .getTreasuryBondValueDay(date, codeISIN, operationCode);

  if (value == null) {
    final snackBar = SnackBar(
      content: Text('Valor não encontrado para operação e data selecionada'),
      action: SnackBarAction(
        label: 'Entendi',
        onPressed: () {
          // Some code to undo the change.
        },
      ),
    );
    ScaffoldMessenger.of(context).showSnackBar(snackBar);
  }

  return MoneyMaskedTextController(
      leftSymbol: 'R\$'
          ' ',
      initialValue: value);
}
