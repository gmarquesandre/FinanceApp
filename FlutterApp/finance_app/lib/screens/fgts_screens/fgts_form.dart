import 'package:finance_app/clients/crud_clients/fgts_client.dart';
import 'package:finance_app/components/padding.dart';
import 'package:finance_app/models/fgts/create_or_update_fgts.dart';
import 'package:flutter_masked_text2/flutter_masked_text2.dart';
import 'package:finance_app/components/progress.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';

class FGTSForm extends StatefulWidget {
  const FGTSForm({Key? key}) : super(key: key);

  @override
  FGTSFormState createState() => FGTSFormState();
}

class FGTSFormState extends State<FGTSForm> {
  @override
  void initState() {
    super.initState();
    _getBalance();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: defaultBodyPadding(
        isLoading
            ? const Progress()
            : SingleChildScrollView(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    defaultInputPadding(
                      currentBalanceInput(),
                    ),
                    defaultInputPadding(
                      monthlyGrossIncomeInput(),
                    ),
                    _date.year <= 1970
                        ? const Text("")
                        : Text(
                            "Ultima Atualização ${DateFormat("dd/MM/yyyy").format(_date)}",
                          ),
                    CheckboxListTile(
                      contentPadding: const EdgeInsets.all(0),
                      controlAffinity: ListTileControlAffinity.leading,
                      title: const Text("Saque Aniversário"),
                      value: _anniversaryWithdraw,
                      onChanged: (bool? value) {
                        setState(() {
                          _anniversaryWithdraw = value!;
                        });
                      },
                    ),
                    Visibility(
                      visible: _anniversaryWithdraw,
                      child: defaultInputPadding(
                        DropdownButtonFormField<DateTime>(
                          value: monthWithdrawFGTS,
                          decoration: const InputDecoration(
                            labelText: "Mês do Saque Aniversário",
                          ),
                          items: months.map<DropdownMenuItem<DateTime>>(
                              (monthWithdrawFGTS) {
                            return DropdownMenuItem<DateTime>(
                              value: monthWithdrawFGTS,
                              child: Text(
                                DateFormat('MMMM', 'pt-br')
                                    .format(monthWithdrawFGTS),
                              ),
                            );
                          }).toList(),
                          onChanged: (DateTime? newValue) {
                            setState(() {
                              monthWithdrawFGTS = newValue!;
                            });
                          },
                        ),
                      ),
                    ),
                    defaultButtonPadding(
                      SizedBox(
                        width: double.maxFinite,
                        child: ElevatedButton(
                          child: const Text('Atualizar'),
                          onPressed: () async {
                            saveValue().then((success) {
                              if (success) {
                                var snackBar = const SnackBar(
                                  duration: Duration(seconds: 2),
                                  content: Text('Atualizado Com Sucesso.'),
                                );
                                ScaffoldMessenger.of(context)
                                    .showSnackBar(snackBar);
                              }
                            });
                          },
                        ),
                      ),
                    ),
                  ],
                ),
              ),
      ),
    );
  }

  TextFormField currentBalanceInput() {
    return TextFormField(
      keyboardType: const TextInputType.numberWithOptions(decimal: true),
      controller: _currentBalance,
      decoration: const InputDecoration(
        labelText: 'Saldo Atual FGTS',
      ),
    );
  }

  TextFormField monthlyGrossIncomeInput() {
    return TextFormField(
      keyboardType: const TextInputType.numberWithOptions(decimal: true),
      controller: _valueGrossIncome,
      decoration: const InputDecoration(
        labelText: 'Renda Bruta Mensal',
      ),
    );
  }

  DateTime monthWithdrawFGTS = DateTime(1900, 1, 1);

  var _currentBalance = MoneyMaskedTextController(
    leftSymbol: 'R\$ ',
  );

  List<DateTime> months =
      List<DateTime>.generate(12, (i) => DateTime(1900, i + 1, 1));

  var _valueGrossIncome =
      MoneyMaskedTextController(leftSymbol: 'R\$ ', initialValue: 0.00);

  bool _anniversaryWithdraw = false;
  int _id = 0;
  bool isLoading = false;

  DateTime _date = DateTime.now();
  FGTSClient client = FGTSClient();

  void _getBalance() async {
    setLoading();
    var balance = await client.get();
    setState(
      () {
        _currentBalance = MoneyMaskedTextController(
          leftSymbol: 'R\$ ',
          initialValue: balance.currentBalance,
        );

        _valueGrossIncome = MoneyMaskedTextController(
            leftSymbol: 'R\$ ', initialValue: balance.monthlyGrossIncome);

        _anniversaryWithdraw = balance.anniversaryWithdraw;

        monthWithdrawFGTS = DateTime(1900, balance.monthAniversaryWithdraw, 1);

        _date = balance.updateDateTime;

        _id = balance.id;
      },
    );
    unsetLoading();
  }

  void setLoading() {
    setState(() {
      isLoading = true;
    });
  }

  void unsetLoading() {
    setState(() {
      isLoading = false;
    });
  }

  Future<bool> saveValue() async {
    setLoading();

    CreateOrUpdateFGTS newValue = CreateOrUpdateFGTS(
        id: _id,
        monthlyGrossIncome: _valueGrossIncome.numberValue,
        anniversaryWithdraw: _anniversaryWithdraw,
        currentBalance: _currentBalance.numberValue,
        monthAniversaryWithdraw: monthWithdrawFGTS.month);
    var success = await client.create(newValue);

    unsetLoading();

    return success;
  }
}
