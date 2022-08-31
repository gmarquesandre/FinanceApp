import 'package:extended_masked_text/extended_masked_text.dart';
import 'package:financial_app/database/dao/category_dao.dart';
import 'package:financial_app/database/dao/spending_dao.dart';
import 'package:financial_app/functions/lists/common_lists.dart';
import 'package:financial_app/models/table_models/category.dart';
import 'package:financial_app/models/table_models/recurrence.dart';
import 'package:financial_app/models/table_models/spending.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:intl/intl.dart';

class SpendingForm extends StatefulWidget {
  final Spending? spending;

  SpendingForm([this.spending]);

  @override
  _SpendingFormState createState() => _SpendingFormState();
}

class _SpendingFormState extends State<SpendingForm> {
  var _spendingValueController = MoneyMaskedTextController(leftSymbol: 'R\$ ');

  DateTime? _dateTimeInitial;
  DateTime? _dateTimeFinal;

  TextEditingController _nameController = TextEditingController();
  TextEditingController _initialDateController = TextEditingController();
  TextEditingController _endDateController = TextEditingController();
  TextEditingController _timesRecurrenceController = TextEditingController();
  Recurrence? _recurrenceController;
  Category? _categoryController;
  // bool _isEndlessController = false;
  bool _isRequiredSpending = false;

  List<Category> categoryList = [];
  List<Recurrence> recurrenceList = CommonLists.recurrenceList;

  String radioItem = 'Item 1';

  final _formKey = GlobalKey<FormState>();

  final SpendingDao _daoSpending = SpendingDao();
  final CategoryDao _daoCategory = CategoryDao();

  Future getCategory() async {
    var list = await _daoCategory.findAll();

    setState(
      () {
        categoryList = list;

        if (widget.spending != null) {
          _categoryController = categoryList.firstWhere(
              (element) => element.id == widget.spending!.categoryId);
        }
      },
    );
    return '';
  }

  @override
  void initState() {
    // TODO: implement initState
    super.initState();

    getCategory();

    if (widget.spending != null) {
      _recurrenceController = recurrenceList
          .firstWhere((element) => element.id == widget.spending!.recurrenceId);
      _dateTimeInitial = widget.spending!.initialDate;

      _isRequiredSpending = widget.spending!.isRequiredSpending == 1;

      _spendingValueController = MoneyMaskedTextController(
          leftSymbol: 'R\$ ', initialValue: widget.spending!.spendingValue);
      _dateTimeFinal = (widget.spending!.recurrenceId != 1
          ? widget.spending!.endDate
          : null);
      _nameController.text = widget.spending!.name;
      _initialDateController.text =
          DateFormat('dd/MM/yyyy').format(widget.spending!.initialDate);

      _endDateController.text = (widget.spending!.recurrenceId != 1
          ? DateFormat('dd/MM/yyyy').format(widget.spending!.endDate)
          : '');

      // _isEndlessController = widget.spending!.isEndless == 1;
      _timesRecurrenceController.text =
          widget.spending!.timesRecurrence.toString();

      radioItem = widget.spending!.recurrenceId == 1
          ? ''
          : widget.spending!.isEndless == 1
              ? 'forever'
              : widget.spending!.timesRecurrence > 0
                  ? 'recurrenceNumber'
                  : 'endDate';
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title:
            Text((widget.spending == null ? "Adicionar" : "Editar") + " Gasto"),
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
                        return 'É necessario um nome';
                      }
                      return null;
                    },
                    controller: _nameController,
                    decoration: InputDecoration(
                      labelText: 'Nome do Gasto',
                    ),
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.only(top: 8.0),
                  child: Row(
                    children: [
                      Checkbox(
                        checkColor: Colors.white,
                        value: _isRequiredSpending,
                        onChanged: (bool? value) {
                          setState(
                            () {
                              _isRequiredSpending = value!;
                            },
                          );
                        },
                      ),
                      Text('Gasto Obrigatório'),
                    ],
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
                        return 'O Valor deve ser maior que zero';
                      }
                      return null;
                    },
                    controller: _spendingValueController,
                    autocorrect: true,
                    decoration: InputDecoration(
                      labelText: 'Valor',
                    ),
                    keyboardType:
                        TextInputType.numberWithOptions(decimal: true),
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.only(top: 8.0, left: 0),
                  child: DropdownButtonFormField<Category>(
                    value: _categoryController,
                    validator: (value) {
                      if (value == null) {
                        return 'Selecione uma categoria';
                      }
                      return null;
                    },
                    decoration: InputDecoration(
                      labelText: 'Categoria',
                    ),
                    items: categoryList
                        .map<DropdownMenuItem<Category>>((_categoryController) {
                      return DropdownMenuItem<Category>(
                        value: _categoryController,
                        child: Text(
                          _categoryController.name,
                        ),
                      );
                    }).toList(),
                    onChanged: (newValue) {
                      setState(() {
                        _categoryController = newValue!;
                      });
                    },
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
                        return 'Insira a data do gasto';
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
                      labelText: 'Data Inicial',
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
                          onChanged: (val) {
                            setState(() {
                              radioItem = val as String;
                            });
                          },
                        ),
                        RadioListTile(
                          groupValue: radioItem,
                          title: Text(
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
                          title: Text(
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
                  visible: _recurrenceController != null &&
                      _recurrenceController!.id! > 1 &&
                      radioItem ==
                          'recurre'
                              'nceNumber',
                  child: Padding(
                    padding: const EdgeInsets.only(top: 8.0),
                    child: TextFormField(
                      validator: (value) {
                        if (value == null || value == '')
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
                        if (_formKey.currentState!.validate()) {
                          final String name = _nameController.text;
                          final int? recurrenceId = int.tryParse(
                              _recurrenceController!.id.toString());
                          final int categoryId =
                              int.tryParse(_categoryController!.id.toString())!;
                          final double spendingValue =
                              _spendingValueController.numberValue;
                          final DateTime endDate =
                              _recurrenceController!.id == 1 ||
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

                          final int isEndless = radioItem == 'forever' ? 1 : 0;

                          final int isRequiredSpending =
                              _isRequiredSpending == true ? 1 : 0;

                          final Spending newSpend = Spending(
                              name: name,
                              spendingValue: spendingValue,
                              initialDate: initialDate,
                              endDate: endDate,
                              recurrenceId: recurrenceId,
                              categoryId: categoryId,
                              isEndless: isEndless,
                              isRequiredSpending: isRequiredSpending,
                              timesRecurrence: timesRecurrence);
                          if (widget.spending == null) {
                            _daoSpending.save(newSpend).then((id) =>
                                Navigator.pop(context, newSpend.toString()));
                          } else {
                            newSpend.id = widget.spending!.id;
                            _daoSpending.updateRow(newSpend).then((id) =>
                                Navigator.pop(context, newSpend.toString()));
                          }
                        }
                      },
                      child: Text('Adicionar'),
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
