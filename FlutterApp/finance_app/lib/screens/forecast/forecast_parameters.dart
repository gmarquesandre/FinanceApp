import 'package:finance_app/components/progress.dart';
import 'package:finance_app/clients/crud_clients/forecast_parameters_client.dart';
import 'package:finance_app/models/forecast_parameters/create_or_update_forecast_parameters.dart';
import 'package:flutter/material.dart';
import 'package:flutter_masked_text2/flutter_masked_text2.dart';
import 'package:intl/intl.dart';

@override
class ForecastParameters extends StatefulWidget {
  ForecastParameters({Key? key}) : super(key: key);

  @override
  State<ForecastParameters> createState() => _ForecastParametersState();
}

class _ForecastParametersState extends State<ForecastParameters> {
  final currencyFormat = NumberFormat.currency(locale: "pt_BR", symbol: "R\$");

  ForecastParametersClient client = ForecastParametersClient();
  bool isLoading = false;

  var percentageCdiFixedInteresIncometSavings =
      MoneyMaskedTextController(initialValue: 0);

  int id = 0;

  void _loadBalance() async {
    setLoading();
    var base = await client.get();

    setState(() {
      id = base.id;
      percentageCdiFixedInteresIncometSavings = MoneyMaskedTextController(
          precision: 2,
          rightSymbol: '%',
          initialValue: base.percentageCdiFixedInteresIncometSavings * 100);
      base.percentageCdiFixedInteresIncometSavings;
    });
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

    CreateOrUpdateForecastParameters newValue =
        CreateOrUpdateForecastParameters(
      id: id,
      monthsSavingWarning: 0,
      percentageCdiFixedInteresIncometSavings:
          percentageCdiFixedInteresIncometSavings.numberValue / 100,
      percentageCdiLoan: 3,
      percentageCdiVariableIncome: 0,
      savingsLiquidPercentage: 1,
    );
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
        title: const Text('Parametros de Simulação'),
      ),
      body: Padding(
        padding: const EdgeInsets.all(8.0),
        child: isLoading
            ? const Progress()
            : Column(
                children: [
                  Padding(
                    padding: const EdgeInsets.only(top: 8.0),
                    child: TextField(
                      controller: percentageCdiFixedInteresIncometSavings,
                      autocorrect: true,
                      decoration: const InputDecoration(
                        labelText:
                            'Rentabilidade Recebimentos Futuros - CDI ( % )',
                      ),
                      keyboardType:
                          const TextInputType.numberWithOptions(decimal: true),
                    ),
                  ),
                  Padding(
                    padding: const EdgeInsets.only(top: 16.0),
                    child: SizedBox(
                      width: double.maxFinite,
                      child: ElevatedButton(
                        child: const Text('Atualizar'),
                        onPressed: () async {
                          await storeValue();
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
