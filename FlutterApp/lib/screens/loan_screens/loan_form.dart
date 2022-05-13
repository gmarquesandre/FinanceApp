import 'dart:math';

import 'package:extended_masked_text/extended_masked_text.dart';
import 'package:financial_app/database/dao/loanDao.dart';
import 'package:financial_app/functions/lists/common_lists.dart';
import 'package:financial_app/models/models/paymentType.dart';
import 'package:financial_app/models/table_models/loan.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:intl/intl.dart';

class LoanForm extends StatefulWidget {
  final Loan? loan;

  LoanForm([this.loan]);

  @override
  _LoanFormState createState() => _LoanFormState();
}

class _LoanFormState extends State<LoanForm> {
  final LoanDao _dao = LoanDao();
  var _totalValueController = MoneyMaskedTextController(leftSymbol: 'R\$ ');
  final _formKey = GlobalKey<FormState>();

  var _interestRateController = MoneyMaskedTextController(initialValue: 0);

  LoanPaymentType _paymentType = CommonLists.loanPaymentType.first;

  List<LoanPaymentType> paymentTypeList = CommonLists.loanPaymentType;

  DateTime? _date;
  final TextEditingController _monthsController = TextEditingController();

  TextEditingController _nameController = TextEditingController();

  TextEditingController _paymentMonthly = MoneyMaskedTextController(
      leftSymbol: 'R\$ ');

  TextEditingController _paymentTotal = MoneyMaskedTextController(
      leftSymbol: 'R\$ ');

  TextEditingController _lastPayment = MoneyMaskedTextController(
      leftSymbol: 'R\$ ');

  TextEditingController _dateController = TextEditingController();

  @override
  void initState() {
    // TODO: implement initState]
    super.initState();

    if (widget.loan != null) {
      _paymentType = paymentTypeList
          .firstWhere((element) => element.id == widget.loan!.paymentType);

      _nameController.text = widget.loan!.name;

      _totalValueController = MoneyMaskedTextController(
          leftSymbol: 'R\$ ', initialValue: widget.loan!.totalValue);

      _monthsController.text = widget.loan!.months.toString();

      _interestRateController = MoneyMaskedTextController(
          leftSymbol: '', initialValue: widget.loan!.interestRate);

      _date = widget.loan!.date;

      _dateController.text = DateFormat('dd/MM/yyyy').format(widget.loan!.date);

      getPaymentValue(
        _totalValueController.numberValue,
        int.tryParse(_monthsController.text),
        _interestRateController.numberValue / 100,
        _paymentType.id,
      );
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text((widget.loan == null? "Adicionar" : "Editar" )+ " Empréstimo"),
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
                      labelText: 'Nome',
                    ),
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.only(top: 8.0),
                  child: new TextFormField(
                    validator: (value) {
                      double valueCompare = _totalValueController.numberValue;
                      if (valueCompare < 0.01) {
                        return 'O Valor deve ser maior que zero';
                      }
                      return null;
                    },
                    controller: _totalValueController,
                    autocorrect: true,
                    decoration: InputDecoration(
                      labelText: 'Valor Total',
                    ),
                    onChanged: (value){
                      setState(() {
                        getPaymentValue(
                          _totalValueController.numberValue,
                          int.tryParse(_monthsController.text),
                          _interestRateController.numberValue / 100,
                          _paymentType.id,
                        );
                      });
                    },
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
                    controller: _monthsController,
                    inputFormatters: [FilteringTextInputFormatter.digitsOnly],
                    decoration: InputDecoration(
                      //border: OutlineInputBorder(),
                      labelText: 'Meses de pagamento',
                    ),
                    onChanged: (value) {
                      setState(() {
                        getPaymentValue(
                          _totalValueController.numberValue,
                          int.tryParse(_monthsController.text),
                          _interestRateController.numberValue / 100,
                          _paymentType.id,
                        );
                      });
                    },
                    keyboardType: TextInputType.number,
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.only(top: 8.0),
                  child: new TextFormField(
                    readOnly: true,
                    validator: (value) {
                      if (value == '') {
                        return 'Insira a data inicial';
                      }

                      return null;
                    },
                    controller: _dateController,
                    onTap: () async {
                      FocusScope.of(context).requestFocus(new FocusNode());
                      final DateTime? picked = await showDatePicker(
                          context: (context),
                          initialDate: _date == null ? DateTime.now() : _date!,
                          firstDate: DateTime(2000),
                          lastDate: DateTime(DateTime.now().year + 100));
                      if (picked != null) {
                        setState(() {
                          _date = picked;
                        });
                      }
                      _dateController.text =
                          DateFormat('dd/MM/yyyy').format(_date!);
                    },
                    decoration: InputDecoration(
                      labelText: 'Data Inicial',
                    ),
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.only(top: 8.0),
                  child: new TextField(
                    controller: _interestRateController,
                    autocorrect: true,
                    decoration: InputDecoration(
                      labelText: 'Juros Anual (%)',
                    ),
                    onChanged: (value) {
                      setState(() {
                        getPaymentValue(
                          _totalValueController.numberValue,
                          int.tryParse(_monthsController.text),
                          _interestRateController.numberValue / 100,
                          _paymentType.id,
                        );
                      });
                    },
                    keyboardType:
                        TextInputType.numberWithOptions(decimal: true),
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.only(top: 8.0, left: 0),
                  child: Column(
                    children: [
                      DropdownButtonFormField<LoanPaymentType>(
                        value: _paymentType,
                        validator: (value) {
                          if (value == null) {
                            return 'Selecione um tipo';
                          }
                          return null;
                        },
                        decoration: InputDecoration(
                          labelText: 'Forma de Pagamento',
                        ),
                        items: paymentTypeList
                            .map<DropdownMenuItem<LoanPaymentType>>(
                                (_paymentController) {
                          return DropdownMenuItem<LoanPaymentType>(
                            value: _paymentController,
                            child: Text(
                              _paymentController.name,
                              style: Theme.of(context).textTheme.subtitle2,

                            ),
                          );
                        }).toList(),
                        onChanged: (newValue) {
                          setState(() {
                            _paymentType = newValue!;
                            getPaymentValue(
                              _totalValueController.numberValue,
                              int.tryParse(_monthsController.text),
                              _interestRateController.numberValue / 100,
                              _paymentType.id,
                            );
                          });
                        },
                      ),
                      Container(width: double.maxFinite,
                        child: Text(_paymentType.description, textAlign: TextAlign.left,
                        style: Theme.of(context).textTheme.bodyText2),),
                    ],
                  ),
                ),

                Padding(
                  padding: const EdgeInsets.only(top: 8.0, left: 0),
                  child: new TextFormField(
                    controller: _paymentMonthly,
                    enabled: false,
                    decoration: InputDecoration(
                      labelText: _paymentType.id == 1
                          ? 'Parcela Fixa'
                          : 'Primeira Parcela',
                    ),

                    keyboardType:
                        TextInputType.numberWithOptions(decimal: true),
                  ),
                ),
                Visibility(
                  visible: _paymentType.id == 2,
                  child: Padding(
                    padding: const EdgeInsets.only(top: 8.0, left: 0),
                    child: new TextFormField(
                      controller: _lastPayment,
                      enabled: false,
                      decoration: InputDecoration(
                        labelText: 'Ultima Parcela'
                      ),
                      keyboardType:
                          TextInputType.numberWithOptions(decimal: true),
                    ),
                  ),
                ),
                Padding(
                  padding: const EdgeInsets.only(top: 8.0, left: 0),
                  child: new TextFormField(
                    controller: _paymentTotal,
                    enabled: false,
                    decoration: InputDecoration(
                      labelText: 'Pagamento Total'
                    ),

                    keyboardType:
                        TextInputType.numberWithOptions(decimal: true),
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

                          final double totalValue =
                              _totalValueController.numberValue;

                          final int months =
                              int.tryParse(_monthsController.text)!;

                          final double interestRate =
                              _interestRateController.numberValue;

                          final DateTime date = _date!;

                          final int paymentType = _paymentType.id;

                          final Loan newLoan = Loan(
                              name: name,
                              totalValue: totalValue,
                              date: date,
                              months: months,
                              interestRate: interestRate,
                              paymentType: paymentType);
                          if (widget.loan == null) {
                            _dao.save(newLoan).then((id) =>
                                Navigator.pop(context, newLoan.toString()));
                          } else {
                            newLoan.id = widget.loan!.id;
                            _dao.updateRow(newLoan).then((id) =>
                                Navigator.pop(context, newLoan.toString()));
                          }
                        }
                      },
                      child: Text(widget.loan == null? "Adicionar" : "Atualizar"),
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

  void getPaymentValue(
      double totalValue, int? nMonths, double interestRate, int id) {


    double paymentMonthly = 0;
    double lastPayment = 0;
    double paymentTotal = 0;

    if (nMonths != null && totalValue != 0.00 && interestRate != 0.00) {
      num interestRateMonth = pow(1 + interestRate, 1 / 12) - 1;



      if (id == 1) {
        paymentMonthly = (totalValue *
                (((pow((1 + interestRateMonth), nMonths) * interestRateMonth)) /
                    (pow((1 + interestRateMonth), nMonths) - 1)));


        lastPayment = paymentMonthly;

        paymentTotal = (paymentMonthly*nMonths);
      }
      if (id == 2) {

        paymentMonthly = (totalValue / nMonths +
                (totalValue * (1 + interestRateMonth) - totalValue));


        lastPayment = ((totalValue/nMonths)*(1+interestRateMonth));

        paymentTotal = ((totalValue + totalValue*interestRateMonth*(nMonths+1)/2));


      }
    }
    _paymentMonthly = MoneyMaskedTextController(
        leftSymbol: 'R\$ ', initialValue: paymentMonthly);

    _lastPayment = MoneyMaskedTextController(
        leftSymbol: 'R\$ ', initialValue: lastPayment);

    _paymentTotal = MoneyMaskedTextController(
        leftSymbol: 'R\$ ', initialValue: paymentTotal);



  }
}
