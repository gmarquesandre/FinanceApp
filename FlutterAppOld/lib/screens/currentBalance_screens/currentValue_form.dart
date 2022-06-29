import 'package:extended_masked_text/extended_masked_text.dart';
import 'package:financial_app/components/globalVariables.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:intl/intl.dart';
import 'package:shared_preferences/shared_preferences.dart';

class CurrentBalanceForm extends StatefulWidget {
  @override
  _CurrentBalanceFormState createState() => _CurrentBalanceFormState();
}

class _CurrentBalanceFormState extends State<CurrentBalanceForm> {
  var _value = MoneyMaskedTextController(
    leftSymbol: 'R\$ ',
  );
  bool _updateValueWithCdi = false;

  DateTime _date = DateTime.now();

  void _loadBalance() async {
    final prefs = await SharedPreferences.getInstance();
    setState(
      () {
        _value = MoneyMaskedTextController(
          leftSymbol: 'R\$ ',
          initialValue:
              prefs.getDouble(GlobalVariables.currentBalanceValue) ?? 0,
        );

        _date = DateTime.fromMillisecondsSinceEpoch(
            prefs.getInt(GlobalVariables.dateEpochLastUpdateBalanceValue) ?? 0);

        _updateValueWithCdi =
            prefs.getBool(GlobalVariables.updateCurrentValueWithCdi) ?? false;
      },
    );
  }

  void storeValue() async {
    final prefs = await SharedPreferences.getInstance();
    setState(
      () {
        prefs.setDouble(
          GlobalVariables.currentBalanceValue,
          _value.numberValue,
        );

        prefs.setBool(
          GlobalVariables.updateCurrentValueWithCdi,
          _updateValueWithCdi,
        );

        prefs.setInt(
            GlobalVariables.dateEpochLastUpdateBalanceValue,
            DateTime(
              DateTime.now().year,
              DateTime.now().month,
              DateTime.now().day,
            ).millisecondsSinceEpoch);
        _date = DateTime.now();
      },
    );
  }

  @override
  void initState() {
    super.initState();
    _loadBalance();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Padding(
        padding: const EdgeInsets.fromLTRB(16.0, 0.0, 16.0, 0.0),
        child: SingleChildScrollView(
          child: Column(
            crossAxisAlignment: CrossAxisAlignment.start,
            children: [
              Padding(
                padding: const EdgeInsets.only(top: 8.0),
                child: TextFormField(
                  keyboardType: TextInputType.numberWithOptions(decimal: true),
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
                    onPressed: () {
                      storeValue();
                      final snackBar = SnackBar(
                        duration: const Duration(seconds: 2),
                        content: Text('Atualizado Com Sucesso.'),
                      );
                      ScaffoldMessenger.of(context).showSnackBar(snackBar);
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
