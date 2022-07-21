import 'package:finance_app/controllers/crud_clients/forecast_parameters_client.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';

@override
class ForecastParameters extends StatefulWidget {
  ForecastParameters({Key? key}) : super(key: key);

  @override
  State<ForecastParameters> createState() => _ForecastParametersState();
}

class _ForecastParametersState extends State<ForecastParameters> {
  final currencyFormat = NumberFormat.currency(locale: "pt_BR", symbol: "R\$");
  bool _updateValueWithCdi = false;
  ForecastParametersClient client = ForecastParametersClient();

  void _loadBalance() async {
    var balance = await client.get();
    setState(() {});
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
        title: const Text('Balanço Mensal'),
      ),
      body: Padding(
        padding: const EdgeInsets.all(8.0),
        child: Column(
          children: [
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
                  onPressed: () {
                    const snackBar = SnackBar(
                      duration: Duration(seconds: 2),
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
