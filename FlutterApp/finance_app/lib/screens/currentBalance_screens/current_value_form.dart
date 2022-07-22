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
  var _value = MoneyMaskedTextController(
    leftSymbol: 'R\$ ',
  );

  bool _updateValueWithCdi = false;
  int _id = 0;
  bool isLoading = false;

  DateTime _date = DateTime.now();
  CurrentBalanceClient client = CurrentBalanceClient();
  var _interestRateController = MoneyMaskedTextController(initialValue: 0);

  void _loadBalance() async {
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

  Future<bool> storeValue() async {
    setLoading();

    CreateOrUpdateCurrentBalance newValue = CreateOrUpdateCurrentBalance(
        id: _id,
        percentageCdi: _interestRateController.numberValue / 100,
        value: _value.numberValue,
        updateValueWithCdiIndex: _updateValueWithCdi);
    var success = await client.create(newValue);

    unsetLoading();

    return success;
  }

  @override
  void initState() {
    super.initState();
    _loadBalance();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      // appBar: AppBar(
      // title: const Text('Conta Corrente'),
      // ),
      body: Padding(
        padding: const EdgeInsets.fromLTRB(16.0, 0.0, 16.0, 0.0),
        child: isLoading
            ? const Progress()
            : SingleChildScrollView(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Padding(
                      padding: const EdgeInsets.only(top: 8.0),
                      child: TextFormField(
                        keyboardType: const TextInputType.numberWithOptions(
                            decimal: true),
                        controller: _value,
                        decoration: const InputDecoration(
                          labelText: 'Saldo Conta Corrente Atual',
                        ),
                      ),
                    ),
                    !_updateValueWithCdi
                        ? const SizedBox()
                        : Padding(
                            padding: const EdgeInsets.only(top: 8.0),
                            child: TextField(
                              controller: _interestRateController,
                              autocorrect: true,
                              decoration: const InputDecoration(
                                labelText: 'CDI ( % )',
                              ),
                              keyboardType:
                                  const TextInputType.numberWithOptions(
                                      decimal: true),
                            ),
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
                      //    <-- label
                      value: _updateValueWithCdi,
                      onChanged: (bool? value) {
                        setState(() {
                          _updateValueWithCdi = value!;
                        });
                      },
                    ),
                    Padding(
                      padding: const EdgeInsets.only(top: 16.0),
                      child: SizedBox(
                        width: double.maxFinite,
                        child: ElevatedButton(
                          child: const Text('Atualizar'),
                          onPressed: () async {
                            storeValue().then((success) {
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
}
