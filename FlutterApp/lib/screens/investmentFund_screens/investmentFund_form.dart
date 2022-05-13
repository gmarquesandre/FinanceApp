import 'package:extended_masked_text/extended_masked_text.dart';
import 'package:financial_app/database/dao/investmentFundCurrentValue_dao.dart';
import 'package:financial_app/database/dao/investmentFund_dao.dart';
import 'package:financial_app/functions/dateFunctions.dart';
import 'package:financial_app/functions/fundCurrentPosition.dart';
import 'package:financial_app/models/table_models/investmentFund.dart';
import 'package:financial_app/models/table_models/investmentFundCurrentValue.dart';
import 'package:financial_app/screens/investmentFund_screens/investmentFund_search.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:intl/intl.dart';

class InvestmentFundForm extends StatefulWidget {
  final InvestmentFund? investmentFund;

  InvestmentFundForm([this.investmentFund]);

  @override
  _InvestmentFundFormState createState() => _InvestmentFundFormState();
}

class _InvestmentFundFormState extends State<InvestmentFundForm> {

  TextEditingController _fundCnpj = TextEditingController();

  final InvestmentFundCurrentValueDao _daoCurrentValue =
      InvestmentFundCurrentValueDao();
  bool _firstPress = true;
  TextEditingController _fundNameShort = TextEditingController();

  InvestmentFundCurrentValue _fundNew = new InvestmentFundCurrentValue(
    dateLastUpdate: DateTime.now(),
    cnpj: '',
    name: '',
    nameShort: '',
    unitPrice: 0,
    taxLongTerm: false,
    fundTypeName: '',
    administrationFee: 0,
    situation: '',
  );

  final InvestmentFundDao _dao = InvestmentFundDao();

  final _formKey = GlobalKey<FormState>();

  var _totalInvestment = MoneyMaskedTextController(leftSymbol: 'R\$ ');

  var _unitPriceBuy =
      MoneyMaskedTextController(leftSymbol: 'R\$ ', precision: 8);

  int operationCode = 1;

  TextEditingController _dateController = TextEditingController();

  DateTime? _date;

  @override
  initState() {
    super.initState();
    {
      if (widget.investmentFund != null) {
        _dateController.text =
            DateFormat('dd/MM/yyyy').format(widget.investmentFund!.date);
        _date = widget.investmentFund!.date;
        _fundCnpj.text = widget.investmentFund!.cnpj;

        operationCode = widget.investmentFund!.operation;

        _fundNameShort.text = widget.investmentFund!.name;

        _totalInvestment = MoneyMaskedTextController(
            leftSymbol: 'R\$',
            initialValue: widget.investmentFund!.totalInvestment);

        _unitPriceBuy = MoneyMaskedTextController(
            leftSymbol: 'R\$',
            initialValue: widget.investmentFund!.unitPrice,
            precision: 6);
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text((widget.investmentFund == null? "Adicionar" : "Editar" )+ " Fundo"),
      ),
      body: Form(
        key: _formKey,
        child: Padding(
          padding: const EdgeInsets.fromLTRB(16.0, 0.0, 16.0, 0.0),
          child: GestureDetector(
            onTap: () => FocusScope.of(context).unfocus,
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
                              setState(() {
                                operationCode = 1;
                              });
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
                      validator: (value) {
                        if (value == '') {
                          return 'Insira a data de '+ (operationCode == 1? 'Compra': 'Venda'
                          )+
                        'deve estar '
                        'preenchida';
                        }
                        return null;
                      },
                      controller: _dateController,
                      onTap: () async {
                        FocusScope.of(context).requestFocus(new FocusNode());
                        final DateTime? picked = await showDatePicker(
                          context: (context),
                          initialDate: getInitialDatePicker(),
                          selectableDayPredicate: predicate,
                          firstDate: DateTime(1960),
                          lastDate: getInitialDatePicker(),
                        );
                        if (picked != null) {
                          setState(() {
                            _date = picked;
                          });
                        }
                        _dateController.text =
                            DateFormat('dd/MM/yyyy').format(_date!);
                      },
                      decoration: InputDecoration(
                        labelText: 'Data de ' + (operationCode == 1? 'Compra': 'Venda'),
                      ),
                    ),
                  ),
                  Padding(
                    padding: const EdgeInsets.only(top: 8.0),
                    child: TextFormField(
                      validator: (value) {
                        if (value == '' || value == null) {
                          return 'É necessario escolher um fundo';
                        }
                        return null;
                      },
                      readOnly: true,
                      controller: _fundNameShort,
                      onTap: () {
                        Navigator.of(context)
                            .push(
                              MaterialPageRoute(
                                builder: (context) => InvestmentFundListForm(),
                              ),
                            )
                            .then(
                              (newInvestment) => setState(() {
                                _fundNew = newInvestment;
                                _fundCnpj.text = newInvestment.cnpj.toString();
                                _fundNameShort.text =
                                    newInvestment.nameShort.toString();
                              }),
                            );
                      },
                      decoration: InputDecoration(
                        labelText: 'Nome do Fundo',
                      ),
                    ),
                  ),
                  Padding(
                    padding: const EdgeInsets.only(top: 8.0),
                    child: TextFormField(
                      validator: (value) {
                        if (value!.length == 0) {
                          return 'É necessário escolher um fundo';
                        }
                        return null;
                      },
                      readOnly: true,
                      controller: _fundCnpj,
                      decoration: InputDecoration(
                        //border: OutlineInputBorder(),
                        labelText: 'CNPJ',
                      ),
                      keyboardType: TextInputType.number,
                    ),
                  ),
                  Padding(
                    padding: const EdgeInsets.only(top: 8.0),
                    child: new TextFormField(
                      validator: (value) {
                        debugPrint(_totalInvestment.numberValue.toString());
                        if (_totalInvestment.numberValue <= 0.0) {
                          return 'O Valor deve ser maior que zero';
                        }
                        return null;
                      },
                      controller: _totalInvestment,
                      autocorrect: true,
                      decoration: InputDecoration(
                        labelText: 'Total '+ (operationCode == 1? 'Investido': 'Retirado'),
                      ),
                      keyboardType:
                          TextInputType.numberWithOptions(decimal: true),
                    ),
                  ),
                  Padding(
                    padding: const EdgeInsets.only(top: 8.0),
                    child: new TextFormField(
                      validator: (value) {

                        if (_unitPriceBuy.numberValue <= 0.0) {
                          return 'O Valor deve ser maior que zero';
                        }
                        return null;
                      },
                      controller: _unitPriceBuy,
                      autocorrect: true,
                      decoration: InputDecoration(
                        labelText: 'Preço Unitário de '+ (operationCode == 1? 'Compra': 'Venda'),
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
                            final String name = _fundNameShort.text;
                            final String cnpj = _fundCnpj.text;
                            final double totalInvestment =
                                _totalInvestment.numberValue;
                            final double unitPriceBuy =
                                _unitPriceBuy.numberValue;
                            final DateTime date = _date!;

                              final InvestmentFund newInvestmentFund =
                                  InvestmentFund(
                                      operation: operationCode,
                                      date: date,
                                      cnpj: cnpj,
                                      name: name,
                                      totalInvestment: totalInvestment,
                                      unitPrice: unitPriceBuy);
                              _firstPress = false;
                              if (widget.investmentFund == null) {
                                await _daoCurrentValue.save(_fundNew);
                                await _dao
                                    .save(newInvestmentFund)
                                    .then((id) async {
                                  await fundCurrentPosition(context);
                                  Navigator.pop(
                                      context, newInvestmentFund.toString());
                                });
                              } else {
                                newInvestmentFund.id =
                                    widget.investmentFund!.id;
                                await _daoCurrentValue.save(_fundNew);

                                await _dao
                                    .updateRow(newInvestmentFund)
                                    .then((id) async {

                                  await fundCurrentPosition(context);

                                  Navigator.pop(
                                      context, newInvestmentFund.toString());
                                });
                              }
                            }
                          },
                        child: Text(widget.investmentFund == null? "Adicionar" : "Atualizar"),
                      ),
                    ),
                  )
                ],
              ),
            ),
          ),
        ),
      ),
    );
  }
}
