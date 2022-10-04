import 'package:finance_app/clients/crud_clients/creditcard_client.dart';
import 'package:finance_app/components/padding.dart';
import 'package:finance_app/models/category/category.dart';
import 'package:finance_app/models/credit_card/credit_card.dart';
import 'package:finance_app/screens/creditcard/creditcard_form.dart';
import 'package:flutter_masked_text2/flutter_masked_text2.dart';
import 'package:finance_app/common_lists.dart';
import 'package:finance_app/clients/crud_clients/spending_client.dart';
import 'package:finance_app/models/recurrence.dart';
import 'package:finance_app/models/spending/create_spending.dart';
import 'package:finance_app/models/spending/spending.dart';
import 'package:finance_app/models/spending/update_spending.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:intl/intl.dart';

class SpendingForm extends StatefulWidget {
  final Spending? spending;

  const SpendingForm([this.spending]);

  @override
  SpendingFormState createState() => SpendingFormState();
}

class SpendingFormState extends State<SpendingForm> {
  var _spendingValueController = MoneyMaskedTextController(leftSymbol: 'R\$ ');

  DateTime? _dateTimeInitial;
  DateTime? _dateTimeFinal;

  final TextEditingController _nameController = TextEditingController();
  final TextEditingController _initialDateController = TextEditingController();
  final TextEditingController _endDateController = TextEditingController();
  final TextEditingController _timesRecurrenceController =
      TextEditingController();
  Recurrence? recurrenceController;
  CreditCard? creditCardController;
  Category? _categoryController;

  bool _isRequiredSpending = false;

  List<Category> categoryList = [];
  List<CreditCard> creditCardList = [];
  List<Recurrence> recurrenceList = CommonLists.recurrenceList;
  int typePayment = 1;
  String radioItem = 'Item 1';

  final _formKey = GlobalKey<FormState>();

  final SpendingClient _daoSpending = SpendingClient();
  final CreditCardClient _daoCreditCard = CreditCardClient();

  _loadCreditCards() async {
    var getCards = await _daoCreditCard.get();

    if (creditCardController != null) creditCardList.add(creditCardController!);

    creditCardList.addAll(
        getCards.where((element) => element.id != creditCardController?.id));

    setState(() {});
  }

  @override
  void initState() {
    super.initState();

    _loadCreditCards();

    if (widget.spending != null) {
      recurrenceController = recurrenceList
          .firstWhere((element) => element.id == widget.spending!.recurrence);
      _dateTimeInitial = widget.spending!.initialDate;

      _isRequiredSpending = widget.spending!.isRequired;

      _spendingValueController = MoneyMaskedTextController(
          leftSymbol: 'R\$ ', initialValue: widget.spending!.amount);
      _dateTimeFinal =
          (widget.spending!.recurrence != 1 ? widget.spending!.endDate : null);
      _nameController.text = widget.spending!.name;
      _initialDateController.text =
          DateFormat('dd/MM/yyyy').format(widget.spending!.initialDate);

      _endDateController.text = (widget.spending!.recurrence != 1
          ? DateFormat('dd/MM/yyyy').format(widget.spending!.endDate!)
          : '');

      // _isEndlessController = widget.spending!.isEndless == 1;
      _timesRecurrenceController.text =
          widget.spending!.timesRecurrence.toString();

      radioItem = widget.spending!.recurrence == 1
          ? ''
          : widget.spending!.isEndless
              ? 'forever'
              : widget.spending!.timesRecurrence > 0
                  ? 'recurrenceNumber'
                  : 'endDate';

      typePayment = widget.spending!.payment;

      creditCardController = widget.spending!.creditCard;
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text(
            "${widget.spending == null ? "Adicionar" : "Atualizar"} Gasto"),
      ),
      body: Form(
        key: _formKey,
        child: defaultBodyPadding(
          SingleChildScrollView(
            child: Column(
              children: [
                defaultInputPadding(
                  TextFormField(
                    validator: (value) {
                      if (value!.isEmpty) {
                        return 'É necessario um nome';
                      }
                      return null;
                    },
                    controller: _nameController,
                    decoration: const InputDecoration(
                      labelText: 'Nome do Gasto',
                    ),
                  ),
                ),
                defaultInputPadding(
                  Row(
                    children: [
                      Expanded(
                        child: ElevatedButton(
                          onPressed: () async {
                            setState(() {
                              typePayment = 1;
                            });
                          },
                          style: ButtonStyle(
                            backgroundColor: MaterialStateProperty.all(
                                typePayment == 1
                                    ? Colors.blueGrey
                                    : Colors.white),
                          ),
                          child: Text(
                            "Débito",
                            style: TextStyle(
                                color: typePayment == 1
                                    ? Colors.white
                                    : Colors.black),
                          ),
                        ),
                      ),
                      Expanded(
                        child: ElevatedButton(
                          onPressed: () async {
                            setState(() {
                              typePayment = 2;
                            });
                          },
                          style: ButtonStyle(
                            backgroundColor: MaterialStateProperty.all(
                                typePayment == 2
                                    ? Colors.blueGrey
                                    : Colors.white),
                          ),
                          child: Text(
                            "Crédito",
                            style: TextStyle(
                                color: typePayment == 2
                                    ? Colors.white
                                    : Colors.black),
                          ),
                        ),
                      ),
                    ],
                  ),
                ),
                typePayment == 1
                    ? const SizedBox()
                    : defaultInputPadding(
                        DropdownButtonFormField<CreditCard>(
                          value: creditCardController,
                          validator: (value) {
                            if (value == null) {
                              return 'Selecione uma recorrencia';
                            }
                            return null;
                          },
                          decoration: InputDecoration(
                            labelText: 'Cartão de Crédito',
                            suffixIcon: IconButton(
                              icon: const Icon(Icons.add),
                              tooltip: 'Novo Cartão',
                              onPressed: () {
                                setState(() {
                                  Navigator.of(context)
                                      .push(
                                        MaterialPageRoute(
                                          builder: (context) =>
                                              CreditCardForm(),
                                        ),
                                      )
                                      .then(
                                        (newCredit) => setState(() {
                                          creditCardList.add(newCredit);
                                          creditCardController = newCredit;
                                        }),
                                      );
                                });
                              },
                            ),
                          ),
                          items: creditCardList
                              .map<DropdownMenuItem<CreditCard>>(
                                  (creditCardController) {
                            return DropdownMenuItem<CreditCard>(
                              value: creditCardController,
                              child: Text(
                                "Cart. ${creditCardController.name} Fech. ${creditCardController.invoiceClosingDay} Pgto. ${creditCardController.invoicePaymentDay}",
                              ),
                            );
                          }).toList(),
                          onChanged: (CreditCard? newValue) {
                            setState(() {
                              creditCardController = newValue!;
                            });
                          },
                        ),
                      ),
                defaultInputPadding(
                  TextFormField(
                    validator: (value) {
                      double? valueCompare = double.tryParse(
                        value
                            .toString()
                            .replaceAll('R\$ ', '')
                            .replaceAll(".", "")
                            .replaceAll(",", "."),
                      );
                      if (valueCompare! < 0.01) {
                        return 'O Valor deve ser maior que zero';
                      }
                      return null;
                    },
                    controller: _spendingValueController,
                    autocorrect: true,
                    decoration: const InputDecoration(
                      labelText: 'Valor',
                    ),
                    keyboardType:
                        const TextInputType.numberWithOptions(decimal: true),
                  ),
                ),
                defaultInputPadding(
                  DropdownButtonFormField<Recurrence>(
                    value: recurrenceController,
                    validator: (value) {
                      if (value == null) {
                        return 'Selecione uma recorrencia';
                      }
                      return null;
                    },
                    decoration: const InputDecoration(
                      labelText: 'Recorrência',
                    ),
                    items: recurrenceList.map<DropdownMenuItem<Recurrence>>(
                        (recurrenceController) {
                      return DropdownMenuItem<Recurrence>(
                        value: recurrenceController,
                        child: Text(
                          recurrenceController.name,
                        ),
                      );
                    }).toList(),
                    onChanged: (Recurrence? newValue) {
                      setState(() {
                        recurrenceController = newValue!;
                      });
                    },
                  ),
                ),
                defaultInputPadding(
                  TextFormField(
                    readOnly: true,
                    validator: (value) {
                      if (value == '') {
                        return 'Insira a data do gasto';
                      }
                      return null;
                    },
                    controller: _initialDateController,
                    onTap: () async {
                      FocusScope.of(context).requestFocus(
                        FocusNode(),
                      );
                      final DateTime? picked = await showDatePicker(
                        context: (context),
                        initialDate: _dateTimeInitial == null
                            ? DateTime.now()
                            : _dateTimeInitial!,
                        firstDate: DateTime(1990),
                        lastDate: DateTime(DateTime.now().year + 100),
                      );
                      if (picked != null) {
                        setState(() {
                          _dateTimeInitial = picked;
                        });
                      }
                      _initialDateController.text =
                          DateFormat('dd/MM/yyyy').format(_dateTimeInitial!);
                    },
                    decoration: const InputDecoration(
                      labelText: 'Data Inicial',
                    ),
                  ),
                ),
                Visibility(
                  visible: recurrenceController != null &&
                      recurrenceController!.id! > 1,
                  child: defaultInputPadding(
                    Column(
                      mainAxisAlignment: MainAxisAlignment.start,
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: <Widget>[
                        RadioListTile(
                          groupValue: radioItem,
                          title: const Text(
                            'Número de Recorrencias',
                          ),
                          value: 'recurrenceNumber',
                          onChanged: (val) {
                            setState(() {
                              radioItem = val as String;
                            });
                          },
                        ),
                        RadioListTile(
                          groupValue: radioItem,
                          title: const Text(
                            'Data Final',
                          ),
                          value: 'endDate',
                          onChanged: (val) {
                            setState(() {
                              _endDateController.text = '';
                              radioItem = val as String;
                            });
                          },
                        ),
                        RadioListTile(
                          groupValue: radioItem,
                          title: const Text(
                            'Para Sempre',
                          ),
                          value: 'forever',
                          onChanged: (val) {
                            setState(() {
                              radioItem = val as String;
                            });
                          },
                        ),
                      ],
                    ),
                  ),
                ),
                Visibility(
                  visible: recurrenceController != null &&
                      recurrenceController!.id! > 1 &&
                      radioItem ==
                          'recurre'
                              'nceNumber',
                  child: defaultInputPadding(
                    TextFormField(
                      validator: (value) {
                        if (value == null || value == '') {
                          return 'É necessario um número';
                        } else if (int.tryParse(value)! <= 1) {
                          return 'Recorrencia deve ser maior que 1';
                        }
                        return null;
                      },
                      controller: _timesRecurrenceController,
                      decoration: const InputDecoration(
                        labelText: 'Número de recorrencias',
                      ),
                      keyboardType: TextInputType.number,
                      inputFormatters: <TextInputFormatter>[
                        FilteringTextInputFormatter.allow(
                          RegExp(
                            r'[0-9]',
                          ),
                        )
                      ],
                    ),
                  ),
                ),
                Visibility(
                  visible: recurrenceController != null &&
                      recurrenceController!.id != 1 &&
                      radioItem == 'endDate',
                  child: defaultInputPadding(
                    TextFormField(
                      validator: (value) {
                        if (value == '') {
                          return 'Insira a data final';
                        }
                        return null;
                      },
                      controller: _endDateController,
                      onTap: () async {
                        FocusScope.of(context).requestFocus(
                          FocusNode(),
                        );
                        final DateTime? picked = await showDatePicker(
                          context: (context),
                          initialDate: DateTime.now(),
                          firstDate: DateTime(1900),
                          lastDate: DateTime(DateTime.now().year + 100),
                        );
                        if (picked != null) {
                          setState(() {
                            _dateTimeFinal = picked;
                          });
                        }
                        _endDateController.text =
                            DateFormat('dd/MM/yyyy').format(_dateTimeFinal!);
                      },
                      decoration: const InputDecoration(
                        labelText: 'Data Final',
                      ),
                    ),
                  ),
                ),
                defaultBodyPadding(
                  SizedBox(
                    width: double.maxFinite,
                    child: ElevatedButton(
                      onPressed: () async {
                        if (_formKey.currentState!.validate()) {
                          final String name = _nameController.text;
                          final int? recurrenceId = int.tryParse(
                            recurrenceController!.id.toString(),
                          );
                          final int? categoryId = _categoryController?.id;
                          final double spendingValue =
                              _spendingValueController.numberValue;
                          final DateTime endDate =
                              recurrenceController!.id == 1 ||
                                      _dateTimeFinal == null ||
                                      radioItem != 'endDate'
                                  ? DateTime(
                                      1900,
                                      1,
                                    )
                                  : _dateTimeFinal!;
                          final DateTime initialDate = _dateTimeInitial!;

                          final int timesRecurrence =
                              radioItem == 'recurrenceNumber'
                                  ? int.parse(_timesRecurrenceController.text)
                                  : 0;

                          final bool isEndless =
                              radioItem == 'forever' ? true : false;
                          final int? creditCardId = creditCardController != null
                              ? creditCardController!.id
                              : null;
                          if (widget.spending == null) {
                            final CreateSpending newSpend = CreateSpending(
                                creditCardId: creditCardId,
                                payment: typePayment,
                                name: name,
                                amount: spendingValue,
                                initialDate: initialDate,
                                endDate: endDate,
                                recurrence: recurrenceId!,
                                categoryId: categoryId,
                                isEndless: isEndless,
                                isRequired: _isRequiredSpending,
                                timesRecurrence: timesRecurrence);

                            await _daoSpending
                                .create(newSpend)
                                .then((id) => Navigator.pop(
                                      context,
                                      newSpend.toString(),
                                    ));
                          } else {
                            final UpdateSpending newSpend = UpdateSpending(
                                payment: typePayment,
                                id: widget.spending!.id,
                                creditCardId: creditCardId,
                                name: name,
                                amount: spendingValue,
                                initialDate: initialDate,
                                endDate: endDate,
                                recurrence: recurrenceId!,
                                categoryId: categoryId,
                                isEndless: isEndless,
                                isRequired: _isRequiredSpending,
                                timesRecurrence: timesRecurrence);

                            await _daoSpending
                                .update(newSpend)
                                .then((id) => Navigator.pop(
                                      context,
                                      newSpend.toString(),
                                    ));
                          }
                        }
                      },
                      child: const Text('Adicionar'),
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
