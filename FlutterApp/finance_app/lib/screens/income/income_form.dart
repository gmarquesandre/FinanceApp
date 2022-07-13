import 'package:extended_masked_text/extended_masked_text.dart';
import 'package:finance_app/common_lists.dart';
import 'package:finance_app/controllers/crud_clients/income_client.dart';
import 'package:finance_app/models/income/create_income.dart';
import 'package:finance_app/models/income/income.dart';
import 'package:finance_app/models/income/update_income.dart';
import 'package:finance_app/models/recurrence.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:intl/intl.dart';

class IncomeForm extends StatefulWidget {
  final Income? income;

  IncomeForm([this.income]);

  @override
  _IncomeFormState createState() => _IncomeFormState();
}

class _IncomeFormState extends State<IncomeForm> {
  var _incomeValueController = MoneyMaskedTextController(leftSymbol: 'R\$ ');

  bool _firstPress = true;
  DateTime? _dateTimeInitial;
  DateTime? _dateTimeFinal;
  TextEditingController _nameController = TextEditingController();
  TextEditingController _initialDateController = TextEditingController();
  TextEditingController _endDateController = TextEditingController();
  Recurrence? _recurrenceController = CommonLists.recurrenceList.first;
  TextEditingController _timesRecurrenceController = TextEditingController();

  // bool _isEndlessController = false;

  List<Recurrence> recurrenceList = [];

  final IncomeClient _daoIncome = IncomeClient();

  @override
  void initState() {
    super.initState();
    recurrenceList = CommonLists.recurrenceList;

    if (widget.income != null) {
      _recurrenceController = recurrenceList
          .firstWhere((element) => element.id == widget.income!.recurrence);
      _dateTimeInitial = widget.income!.initialDate;
      _initialDateController.text =
          DateFormat('dd/MM/yyyy').format(widget.income!.initialDate);
      _timesRecurrenceController.text =
          widget.income!.timesRecurrence.toString();
      _incomeValueController = MoneyMaskedTextController(
          leftSymbol: 'R\$ ', initialValue: widget.income!.amount);
      // _isEndlessController = widget.income!.isEndless == 1;

      _nameController.text = widget.income!.name;
      _dateTimeFinal = widget.income!.endDate;
      _endDateController.text = (widget.income!.endDate != null
          ? DateFormat('dd/MM/yyyy').format(widget.income!.endDate!)
          : '');

      if (widget.income!.recurrence == 1) {
        radioItem = '';
      } else {
        radioItem = widget.income!.isEndless
            ? 'forever'
            : widget.income!.timesRecurrence > 0
                ? 'recurrenceNumber'
                : 'endDate';
      }
    }
  }

  String radioItem = 'Item 1';
  final _formKey = GlobalKey<FormState>();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text("${widget.income == null ? "Adicionar" : "Editar"} Renda"),
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
                  child: TextFormField(
                    validator: (value) {
                      if (value!.isEmpty) {
                        return 'É necessário um nome';
                      }
                      return null;
                    },
                    controller: _nameController,
                    decoration: const InputDecoration(
                      labelText: 'Nome da Renda',
                    ),
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.only(top: 8.0),
                  child: TextFormField(
                    validator: (value) {
                      double valueCompare = _incomeValueController.numberValue;
                      if (valueCompare < 0.01) {
                        return 'O Valor deve ser maior que zero';
                      }
                      return null;
                    },
                    controller: _incomeValueController,
                    autocorrect: true,
                    decoration: const InputDecoration(
                      labelText: 'Renda Liquida',
                    ),
                    keyboardType:
                        const TextInputType.numberWithOptions(decimal: true),
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.only(top: 8.0, left: 0),
                  child: DropdownButtonFormField<Recurrence>(
                    value: _recurrenceController,
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
                        (_recurrenceController) {
                      return DropdownMenuItem<Recurrence>(
                        value: _recurrenceController,
                        child: Text(
                          _recurrenceController.name,
                        ),
                      );
                    }).toList(),
                    onChanged: (Recurrence? newValue) {
                      setState(() {
                        _recurrenceController = newValue!;
                      });
                    },
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.only(top: 8.0),
                  child: TextFormField(
                    readOnly: true,
                    validator: (value) {
                      if (value == '') {
                        return 'Insira a data do investimento';
                      }
                      return null;
                    },
                    controller: _initialDateController,
                    onTap: () async {
                      FocusScope.of(context).requestFocus(FocusNode());
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
                    decoration: InputDecoration(
                      labelText: _recurrenceController != null &&
                              _recurrenceController!.id != 1
                          ? 'Data Inicial'
                          : 'Data',
                    ),
                  ),
                ),
                Visibility(
                  visible: _recurrenceController != null &&
                      _recurrenceController!.id! > 1,
                  child: Padding(
                    padding: const EdgeInsets.only(top: 8.0),
                    child: Column(
                      mainAxisAlignment: MainAxisAlignment.start,
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: <Widget>[
                        RadioListTile(
                          groupValue: radioItem,
                          title: const Text(
                            'Número de Recorrencias',
                          ),
                          value: 'recurrenceNumber',
                          onChanged: (String? val) {
                            setState(() {
                              radioItem = val!;
                            });
                          },
                        ),
                        RadioListTile(
                          groupValue: radioItem,
                          title: const Text(
                            'Data Final',
                          ),
                          value: 'endDate',
                          onChanged: (String? val) {
                            setState(() {
                              _endDateController.text = '';
                              radioItem = val!;
                            });
                          },
                        ),
                        RadioListTile(
                          groupValue: radioItem,
                          title: const Text(
                            'Para Sempre',
                          ),
                          value: 'forever',
                          onChanged: (String? val) {
                            setState(() {
                              radioItem = val!;
                            });
                          },
                        ),
                      ],
                    ),
                  ),
                ),
                Visibility(
                  visible: _recurrenceController != null &&
                      _recurrenceController!.id! > 1 &&
                      radioItem ==
                          'recurre'
                              'nceNumber',
                  child: Padding(
                    padding: const EdgeInsets.only(top: 8.0),
                    child: TextFormField(
                      validator: (value) {
                        if (value == '' || value == null) {
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
                        FilteringTextInputFormatter.allow(RegExp(r'[0-9]'))
                      ],
                    ),
                  ),
                ),
                Visibility(
                  visible: _recurrenceController != null &&
                      _recurrenceController!.id != 1 &&
                      radioItem == 'endDate',
                  child: Padding(
                    padding: const EdgeInsets.only(top: 8.0),
                    child: TextFormField(
                      validator: (value) {
                        if (value == '') {
                          return 'Insira a data final';
                        }
                        return null;
                      },
                      controller: _endDateController,
                      onTap: () async {
                        FocusScope.of(context).requestFocus(FocusNode());
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
                Padding(
                  padding: const EdgeInsets.only(top: 24.0),
                  child: SizedBox(
                    width: double.maxFinite,
                    child: ElevatedButton(
                      onPressed: () {
                        if (_formKey.currentState!.validate() && _firstPress) {
                          final String name = _nameController.text;
                          final int? recurrenceId = int.tryParse(
                              _recurrenceController!.id.toString());
                          final double incomeValue =
                              _incomeValueController.numberValue;

                          final DateTime endDate =
                              _recurrenceController!.id == 1 ||
                                      _dateTimeFinal == null ||
                                      radioItem != 'endDate'
                                  ? DateTime(1900, 1, 1)
                                  : _dateTimeFinal!;
                          final int timesRecurrence =
                              radioItem == 'recurrenceNumber'
                                  ? int.parse(_timesRecurrenceController.text)
                                  : 0;
                          final DateTime initialDate = _dateTimeInitial!;

                          final bool isEndless = radioItem == 'forever';

                          if (widget.income == null) {
                            final CreateIncome newIncome = CreateIncome(
                                name: name,
                                amount: incomeValue,
                                initialDate: initialDate,
                                endDate: _dateTimeFinal,
                                recurrence: recurrenceId!,
                                isEndless: isEndless,
                                timesRecurrence: timesRecurrence);

                            _firstPress = false;

                            _daoIncome.create(newIncome).then((created) =>
                                Navigator.pop(context, newIncome.toString()));
                          } else {
                            final UpdateIncome newIncome = UpdateIncome(
                                id: widget.income!.id,
                                name: name,
                                amount: incomeValue,
                                initialDate: initialDate,
                                endDate: _dateTimeFinal,
                                recurrence: recurrenceId!,
                                isEndless: isEndless,
                                timesRecurrence: timesRecurrence);

                            newIncome.id = widget.income!.id;
                            _daoIncome.update(newIncome).then((id) =>
                                Navigator.pop(context, newIncome.toString()));
                          }
                        }
                      },
                      child: Text(
                          widget.income == null ? "Adicionar" : "Atualizar"),
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
