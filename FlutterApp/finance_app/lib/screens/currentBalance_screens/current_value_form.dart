import 'package:extended_masked_text/extended_masked_text.dart';
import 'package:finance_app/components/progress.dart';
import 'package:finance_app/controllers/current_balance_client.dart';
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
  bool isLoading = false;

  DateTime _date = DateTime.now();
  CurrentBalanceClient client = CurrentBalanceClient();

  void _loadBalance() async {
    setLoading();
    var balance = await client.get();
    setState(
      () {
        _value = MoneyMaskedTextController(
          leftSymbol: 'R\$ ',
          initialValue: balance.value,
        );

        _date = balance.updateDateTime;

        _updateValueWithCdi = balance.updateValueWithCdiIndex;
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
        percentageCdi: 1,
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
      appBar: AppBar(
        title: const Text('Conta Corrente'),
      ),
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
                        decoration: InputDecoration(
                          labelText: 'Saldo Conta Corrente Atual',
                        ),
                      ),
                    ),
                    _date.year <= 1970
                        ? Text("")
                        : Text(
                            "Ultima Atualização " +
                                DateFormat("dd/MM/yyyy").format(_date),
                          ),
                    CheckboxListTile(
                      contentPadding: EdgeInsets.all(0),
                      controlAffinity: ListTileControlAffinity.leading,
                      title: Text("Atualizar Saldo pelo CDI"),
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
                          child: Text('Atualizar'),
                          onPressed: () async {
                            bool success = await storeValue();
                            if (success) {
                              var snackBar = SnackBar(
                                duration: const Duration(seconds: 2),
                                content: Text('Atualizado Com Sucesso.'),
                              );
                              ScaffoldMessenger.of(context)
                                  .showSnackBar(snackBar);
                            }
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
