import 'package:finance_app/components/padding.dart';
import 'package:flutter_masked_text2/flutter_masked_text2.dart';
import 'package:finance_app/components/progress.dart';
import 'package:finance_app/clients/crud_clients/current_balance_client.dart';
import 'package:finance_app/models/current_balance/create_or_update_current_balance.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';

class CurrentBalanceForm extends StatefulWidget {
  const CurrentBalanceForm({Key? key}) : super(key: key);

  @override
  CurrentBalanceFormState createState() => CurrentBalanceFormState();
}

class CurrentBalanceFormState extends State<CurrentBalanceForm> {
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
                    !_updateValueWithCdi
                        ? const SizedBox()
                        : defaultInputPadding(
                            indexPercentageInput(),
                          ),
                    !_updateValueWithCdi
                        ? const Text("")
                        : const Text(
                            "Será considerado IR de 22,5% em qualquer prazo do investimento",
                          ),
                    _date.year <= 1970
                        ? const Text("")
                        : Text(
                            "Ultima Atualização ${DateFormat("dd/MM/yyyy").format(_date)}",
                          ),
                    CheckboxListTile(
                      contentPadding: const EdgeInsets.all(0),
                      controlAffinity: ListTileControlAffinity.leading,
                      title: const Text("Atualizar Saldo pelo CDI"),
                      value: _updateValueWithCdi,
                      onChanged: (bool? value) {
                        setState(() {
                          _updateValueWithCdi = value!;
                        });
                      },
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

  TextField indexPercentageInput() {
    return TextField(
      controller: _interestRateController,
      autocorrect: true,
      decoration: const InputDecoration(
        labelText: 'CDI ( % )',
      ),
      keyboardType: const TextInputType.numberWithOptions(decimal: true),
    );
  }

  TextFormField currentBalanceInput() {
    return TextFormField(
      keyboardType: const TextInputType.numberWithOptions(decimal: true),
      controller: _value,
      decoration: const InputDecoration(
        labelText: 'Saldo Conta Corrente Atual',
      ),
    );
  }

  var _value = MoneyMaskedTextController(
    leftSymbol: 'R\$ ',
  );

  bool _updateValueWithCdi = false;
  int _id = 0;
  bool isLoading = false;

  DateTime _date = DateTime.now();
  CurrentBalanceClient client = CurrentBalanceClient();
  var _interestRateController = MoneyMaskedTextController(initialValue: 0);

  void _getBalance() async {
    setLoading();
    var balance = await client.get();
    setState(
      () {
        _value = MoneyMaskedTextController(
          leftSymbol: 'R\$ ',
          initialValue: balance.value,
        );

        _interestRateController = MoneyMaskedTextController(
            precision: 2,
            leftSymbol: '',
            initialValue: balance.percentageCdi != null
                ? balance.percentageCdi! * 100
                : 0);

        _date = balance.updateDateTime;

        _updateValueWithCdi = balance.updateValueWithCdiIndex;

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

    CreateOrUpdateCurrentBalance newValue = CreateOrUpdateCurrentBalance(
        id: _id,
        percentageCdi: _interestRateController.numberValue / 100,
        value: _value.numberValue,
        updateValueWithCdiIndex: _updateValueWithCdi);
    await client.create(newValue);

    unsetLoading();

    return true;
  }
}
