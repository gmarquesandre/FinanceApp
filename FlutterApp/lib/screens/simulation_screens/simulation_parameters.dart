import 'package:financial_app/components/globalVariables.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';
import 'package:shared_preferences/shared_preferences.dart';

@override
class SimulationParameters extends StatefulWidget {
  @override
  State<SimulationParameters> createState() => _SimulationParametersState();
}

class _SimulationParametersState extends State<SimulationParameters> {
  final currencyFormat = NumberFormat.currency(locale: "pt_BR", symbol: "R\$");
  bool _updateValueWithCdi = false;


  void storeValue() async {
    final prefs = await SharedPreferences.getInstance();
    setState(() {

      prefs.setBool(
          GlobalVariables.updateCurrentValueWithCdi, _updateValueWithCdi);
    });
  }

  Widget build(BuildContext context) {

    return Scaffold(
      appBar: AppBar(
        title: Text('Balanço Mensal'),
      ),
      body: Padding(
        padding: const EdgeInsets.all(8.0),
        child: Column(
          children: [
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
    );
  }
}