import 'package:extended_masked_text/extended_masked_text.dart';
import 'package:financial_app/database/dao/income_dao.dart';
import 'package:financial_app/functions/lists/common_lists.dart';
import 'package:financial_app/models/table_models/income.dart';
import 'package:financial_app/models/table_models/recurrence.dart';
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

  final IncomeDao _daoIncome = IncomeDao();

  @override
  void initState() {
    // TODO: implement initState
    super.initState();
    recurrenceList = CommonLists.recurrenceList;

    if (widget.income != null) {
      _recurrenceController = recurrenceList
          .firstWhere((element) => element.id == widget.income!.recurrenceId);
      _dateTimeInitial = widget.income!.initialDate;
      _initialDateController.text =
          DateFormat('dd/MM/yyyy').format(widget.income!.initialDate);
      _timesRecurrenceController.text =
          widget.income!.timesRecurrence.toString();
      _incomeValueController = MoneyMaskedTextController(
          leftSymbol: 'R\$ ', initialValue: widget.income!.incomeValue);
      // _isEndlessController = widget.income!.isEndless == 1;

      _nameController.text = widget.income!.name;
      _dateTimeFinal = widget.income!.endDate;
      _endDateController.text = (widget.income!.recurrenceId != 1
          ? DateFormat('dd/MM/yyyy').format(widget.income!.endDate)
          : '');

      radioItem = widget.income!.recurrenceId == 1
          ? ''
          : widget.income!.isEndless == 1
              ? 'forever'
              : widget.income!.timesRecurrence > 0
                  ? 'recurrenceNumber'
                  : 'endDate';
    }
  }

  String radioItem = 'Item 1';
  final _formKey = GlobalKey<FormState>();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text((widget.income == null? "Adicionar" : "Editar" )+ " Renda"),
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
                      if (value!.length == 0) {
                        return 'É necessário um nome';
                      }
                      return null;
                    },
                    controller: _nameController,
                    decoration: InputDecoration(
                      labelText: 'Nome da Renda',
                    ),
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.only(top: 8.0),
                  child: new TextFormField(
                    validator: (value) {
                      double valueCompare = _incomeValueController.numberValue;
                      if (valueCompare < 0.01) {
                        return 'O Valor deve ser maior que zero';
                      }
                      return null;
                    },
                    controller: _incomeValueController,
                    autocorrect: true,
                    decoration: InputDecoration(
                      labelText: 'Renda Liquida',
                    ),
                    keyboardType:
                        TextInputType.numberWithOptions(decimal: true),
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
                    decoration: InputDecoration(
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
                  child: new TextFormField(
                    readOnly: true,
                    validator: (value) {
                      if (value == '') {
                        return 'Insira a data do investimento';
                      }
                      return null;
                    },
                    controller: _initialDateController,
                    onTap: () async {
                      FocusScope.of(context).requestFocus(new FocusNode());
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
                          title: Text(
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
                          title: Text(
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
                          title: Text(
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
                        if (value == '' || value == null)
                          return 'É necessario um número';
                        else if (int.tryParse(value)! <= 1) {
                          return 'Recorrencia deve ser maior que 1';
                        }
                        return null;
                      },
                      controller: _timesRecurrenceController,
                      decoration: InputDecoration(
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
                    child: new TextFormField(
                      validator: (value) {
                        if (value == '') {
                          return 'Insira a data final';
                        }
                        return null;
                      },
                      controller: _endDateController,
                      onTap: () async {
                        FocusScope.of(context).requestFocus(new FocusNode());
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
                      decoration: InputDecoration(
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

                          final int isEndless = radioItem == 'forever' ? 1 : 0;

                          final Income newIncome = Income(
                              name: name,
                              incomeValue: incomeValue,
                              initialDate: initialDate,
                              endDate: endDate,
                              recurrenceId: recurrenceId,
                              isEndless: isEndless,
                              timesRecurrence: timesRecurrence);

                          _firstPress = false;

                          if (widget.income == null) {
                            _daoIncome.save(newIncome).then((id) =>
                                Navigator.pop(context, newIncome.toString()));
                          } else {
                            newIncome.id = widget.income!.id;
                            _daoIncome.updateRow(newIncome).then((id) =>
                                Navigator.pop(context, newIncome.toString()));
                          }
                        }
                      },
                      child: Text(widget.income == null? "Adicionar" : "Atualizar"),
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
