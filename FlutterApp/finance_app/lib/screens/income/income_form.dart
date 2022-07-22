import 'package:flutter_masked_text2/flutter_masked_text2.dart';
import 'package:finance_app/common_lists.dart';
import 'package:finance_app/clients/crud_clients/income_client.dart';
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
  IncomeFormState createState() => IncomeFormState();
}

class IncomeFormState extends State<IncomeForm> {
  var _incomeValueController = MoneyMaskedTextController(leftSymbol: 'R\$ ');

  bool _firstPress = true;
  DateTime? _dateTimeInitial;
  DateTime? _dateTimeFinal;
  final TextEditingController _nameController = TextEditingController();
  final TextEditingController _initialDateController = TextEditingController();
  final TextEditingController _endDateController = TextEditingController();
  Recurrence? _recurrenceController = CommonLists.recurrenceList.first;
  final TextEditingController _timesRecurrenceController =
      TextEditingController();

  String radioItem = 'Item 1';
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

      _nameController.text = widget.income!.name;
      _dateTimeFinal = widget.income!.endDate;
      _endDateController.text = (widget.income!.endDate != null
          ? DateFormat('dd/MM/yyyy').format(widget.income!.endDate!)
          : '');

      radioItem = widget.income!.recurrence == 1
          ? ''
          : widget.income!.isEndless
              ? 'forever'
              : widget.income!.timesRecurrence > 0
                  ? 'recurrenceNumber'
                  : 'endDate';
    }
  }

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
                _padding(
                  nameTextFormField(),
                ),
                _padding(
                  valueFormField(),
                ),
                _padding(
                  recurrenceDropdown(),
                ),
                _padding(
                  TextFormField(
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
                  child: _padding(
                    radioItemColumn(),
                  ),
                ),
                Visibility(
                  visible: _recurrenceController != null &&
                      _recurrenceController!.id! > 1 &&
                      radioItem ==
                          'recurre'
                              'nceNumber',
                  child: _padding(
                    recurrenceTextFormField(),
                  ),
                ),
                Visibility(
                  visible: _recurrenceController != null &&
                      _recurrenceController!.id != 1 &&
                      radioItem == 'endDate',
                  child: _padding(
                    TextFormField(
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
                      onPressed: () async {
                        if (_formKey.currentState!.validate() && _firstPress) {
                          final String name = _nameController.text;
                          final int? recurrenceId = int.tryParse(
                              _recurrenceController!.id.toString());
                          final double incomeValue =
                              _incomeValueController.numberValue;

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

                            await _daoIncome.create(newIncome).then((created) =>
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
                            await _daoIncome.update(newIncome).then((id) =>
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

  TextFormField recurrenceTextFormField() {
    return TextFormField(
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
    );
  }

  Column radioItemColumn() {
    return Column(
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
    );
  }

  DropdownButtonFormField<Recurrence> recurrenceDropdown() {
    return DropdownButtonFormField<Recurrence>(
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
      items: recurrenceList
          .map<DropdownMenuItem<Recurrence>>((recurrenceController) {
        return DropdownMenuItem<Recurrence>(
          value: recurrenceController,
          child: Text(
            recurrenceController.name,
          ),
        );
      }).toList(),
      onChanged: (Recurrence? newValue) {
        setState(() {
          _recurrenceController = newValue!;
        });
      },
    );
  }

  TextFormField valueFormField() {
    return TextFormField(
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
      keyboardType: const TextInputType.numberWithOptions(decimal: true),
    );
  }

  Padding _padding(Widget form) {
    return Padding(
      padding: const EdgeInsets.only(top: 8.0),
      child: form,
    );
  }

  TextFormField nameTextFormField() {
    return TextFormField(
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
    );
  }
}
