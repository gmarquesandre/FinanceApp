import 'dart:math';
import 'package:finance_app/components/app_bar.dart';
import 'package:finance_app/components/padding.dart';
import 'package:flutter_masked_text2/flutter_masked_text2.dart';
import 'package:finance_app/common_lists.dart';
import 'package:finance_app/clients/crud_clients/loan_client.dart';
import 'package:finance_app/models/loan/loan.dart';
import 'package:finance_app/models/loan/create_loan.dart';
import 'package:finance_app/models/loan/update_loan.dart';
import 'package:finance_app/models/payment_type.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:intl/intl.dart';

class LoanForm extends StatefulWidget {
  final Loan? loan;

  LoanForm([this.loan]);

  @override
  LoanFormState createState() => LoanFormState();
}

class LoanFormState extends State<LoanForm> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: appBarLoggedInDefault(
          "${widget.loan == null ? "Adicionar" : "Atualizar"} Empréstimo"),
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
                      labelText: 'Nome',
                    ),
                  ),
                ),
                defaultInputPadding(
                  TextFormField(
                    validator: (value) {
                      double valueCompare = _totalValueController.numberValue;
                      if (valueCompare < 0.01) {
                        return 'O Valor deve ser maior que zero';
                      }
                      return null;
                    },
                    controller: _totalValueController,
                    autocorrect: true,
                    decoration: const InputDecoration(
                      labelText: 'Valor Total',
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
                        const TextInputType.numberWithOptions(decimal: true),
                  ),
                ),
                defaultInputPadding(
                  TextFormField(
                    validator: (value) {
                      if (value == '' || int.tryParse(value!)! < 1) {
                        return 'O Valor deve ser maior que zero';
                      }
                      return null;
                    },
                    controller: _monthsController,
                    inputFormatters: [FilteringTextInputFormatter.digitsOnly],
                    decoration: const InputDecoration(
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
                defaultInputPadding(
                  TextFormField(
                    readOnly: true,
                    validator: (value) {
                      if (value == '') {
                        return 'Insira a data inicial';
                      }

                      return null;
                    },
                    controller: _dateController,
                    onTap: () async {
                      FocusScope.of(context).requestFocus(FocusNode());
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
                    decoration: const InputDecoration(
                      labelText: 'Data Inicial',
                    ),
                  ),
                ),
                defaultInputPadding(
                  TextField(
                    controller: _interestRateController,
                    autocorrect: true,
                    decoration: const InputDecoration(
                      labelText: 'Juros Anual',
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
                        const TextInputType.numberWithOptions(decimal: true),
                  ),
                ),
                SizedBox(
                  width: double.maxFinite,
                  child: Text(
                      "*Para Financiamentos, Utilizar o Custo Efetivo Total (CET)",
                      textAlign: TextAlign.left,
                      style: Theme.of(context).textTheme.bodyText2),
                ),
                defaultInputPadding(
                  Column(
                    children: [
                      DropdownButtonFormField<LoanPaymentType>(
                        value: _paymentType,
                        validator: (value) {
                          if (value == null) {
                            return 'Selecione um tipo';
                          }
                          return null;
                        },
                        decoration: const InputDecoration(
                          labelText: 'Forma de Pagamento',
                        ),
                        items: paymentTypeList
                            .map<DropdownMenuItem<LoanPaymentType>>(
                                (paymentController) {
                          return DropdownMenuItem<LoanPaymentType>(
                            value: paymentController,
                            child: Text(
                              paymentController.name,
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
                      SizedBox(
                        width: double.maxFinite,
                        child: Text(_paymentType.description,
                            textAlign: TextAlign.left,
                            style: Theme.of(context).textTheme.bodyText2),
                      ),
                    ],
                  ),
                ),
                defaultInputPadding(
                  TextFormField(
                    controller: _paymentMonthly,
                    enabled: false,
                    decoration: InputDecoration(
                      labelText: _paymentType.id == 1
                          ? 'Parcela Fixa'
                          : 'Primeira Parcela',
                    ),
                    keyboardType:
                        const TextInputType.numberWithOptions(decimal: true),
                  ),
                ),
                Visibility(
                  visible: _paymentType.id == 0,
                  child: defaultInputPadding(
                    TextFormField(
                      controller: _lastPayment,
                      enabled: false,
                      decoration:
                          const InputDecoration(labelText: 'Ultima Parcela'),
                      keyboardType:
                          const TextInputType.numberWithOptions(decimal: true),
                    ),
                  ),
                ),
                defaultInputPadding(
                  TextFormField(
                    controller: _paymentTotal,
                    enabled: false,
                    decoration:
                        const InputDecoration(labelText: 'Pagamento Total'),
                    keyboardType:
                        const TextInputType.numberWithOptions(decimal: true),
                  ),
                ),
                defaultInputPadding(
                  SizedBox(
                    width: double.maxFinite,
                    child: ElevatedButton(
                      onPressed: () async {
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

                          if (widget.loan == null) {
                            final CreateLoan newLoan = CreateLoan(
                                name: name,
                                loanValue: totalValue,
                                initialDate: date,
                                monthsPayment: months,
                                interestRate: interestRate,
                                type: paymentType);
                            _dao.create(newLoan).then((id) =>
                                Navigator.pop(context, newLoan.toString()));
                          } else {
                            final UpdateLoan updateLoan = UpdateLoan(
                                id: widget.loan!.id,
                                name: name,
                                loanValue: totalValue,
                                initialDate: date,
                                monthsPayment: months,
                                interestRate: interestRate,
                                type: paymentType);

                            await _dao.update(updateLoan).then((id) =>
                                Navigator.pop(context, updateLoan.toString()));
                          }
                        }
                      },
                      child:
                          Text(widget.loan == null ? "Adicionar" : "Atualizar"),
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

      if (id == 0) {
        paymentMonthly = (totalValue / nMonths +
            (totalValue * (1 + interestRateMonth) - totalValue));

        lastPayment = ((totalValue / nMonths) * (1 + interestRateMonth));

        paymentTotal =
            ((totalValue + totalValue * interestRateMonth * (nMonths + 1) / 2));
      } else if (id == 1) {
        paymentMonthly = (totalValue *
            (((pow((1 + interestRateMonth), nMonths) * interestRateMonth)) /
                (pow((1 + interestRateMonth), nMonths) - 1)));

        lastPayment = paymentMonthly;

        paymentTotal = (paymentMonthly * nMonths);
      }
    }
    _paymentMonthly = MoneyMaskedTextController(
        leftSymbol: 'R\$ ', initialValue: paymentMonthly);

    _lastPayment = MoneyMaskedTextController(
        leftSymbol: 'R\$ ', initialValue: lastPayment);

    _paymentTotal = MoneyMaskedTextController(
        leftSymbol: 'R\$ ', initialValue: paymentTotal);
  }

  final LoanClient _dao = LoanClient();
  var _totalValueController = MoneyMaskedTextController(leftSymbol: 'R\$ ');
  final _formKey = GlobalKey<FormState>();

  var _interestRateController = MoneyMaskedTextController(initialValue: 0);

  LoanPaymentType _paymentType = CommonLists.loanPaymentType.first;

  List<LoanPaymentType> paymentTypeList = CommonLists.loanPaymentType;

  DateTime? _date;
  final TextEditingController _monthsController = TextEditingController();

  final TextEditingController _nameController = TextEditingController();

  TextEditingController _paymentMonthly =
      MoneyMaskedTextController(leftSymbol: 'R\$ ');

  TextEditingController _paymentTotal =
      MoneyMaskedTextController(leftSymbol: 'R\$ ');

  TextEditingController _lastPayment =
      MoneyMaskedTextController(leftSymbol: 'R\$ ');

  final TextEditingController _dateController = TextEditingController();

  @override
  void initState() {
    super.initState();

    if (widget.loan != null) {
      _paymentType = paymentTypeList
          .firstWhere((element) => element.id == widget.loan!.type);

      _nameController.text = widget.loan!.name;

      _totalValueController = MoneyMaskedTextController(
          leftSymbol: 'R\$ ', initialValue: widget.loan!.loanValue);

      _monthsController.text = widget.loan!.monthsPayment.toString();

      _interestRateController = MoneyMaskedTextController(
          precision: 2,
          leftSymbol: '',
          initialValue: widget.loan!.interestRate);

      _date = widget.loan!.initialDate;

      _dateController.text =
          DateFormat('dd/MM/yyyy').format(widget.loan!.initialDate);

      getPaymentValue(
        _totalValueController.numberValue,
        int.tryParse(_monthsController.text),
        _interestRateController.numberValue / 100,
        _paymentType.id,
      );
    }
  }
}
