import 'package:finance_app/components/app_bar.dart';
import 'package:finance_app/components/padding.dart';
import 'package:finance_app/components/progress.dart';
import 'package:finance_app/clients/crud_clients/forecast_parameters_client.dart';
import 'package:finance_app/models/forecast_parameters/create_or_update_forecast_parameters.dart';
import 'package:flutter/material.dart';
import 'package:flutter_masked_text2/flutter_masked_text2.dart';
import 'package:intl/intl.dart';

@override
class ForecastParameters extends StatefulWidget {
  const ForecastParameters({Key? key}) : super(key: key);

  @override
  State<ForecastParameters> createState() => _ForecastParametersState();
}

class _ForecastParametersState extends State<ForecastParameters> {
  final currencyFormat = NumberFormat.currency(locale: "pt_BR", symbol: "R\$");

  ForecastParametersClient client = ForecastParametersClient();
  bool isLoading = false;

  var percentageCdiFixedInteresIncometSavings =
      MoneyMaskedTextController(initialValue: 0);

  void _loadBalance() async {
    setLoading();
    var base = await client.get();

    setState(() {
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
      monthsSavingWarning: 0,
      percentageCdiFixedInteresIncometSavings:
          percentageCdiFixedInteresIncometSavings.numberValue / 100,
      percentageCdiLoan: 3,
      percentageCdiVariableIncome: 0,
      savingsLiquidPercentage: 1,
    );
    await client.create(newValue);

    unsetLoading();

    return true;
  }

  @override
  void initState() {
    super.initState();
    _loadBalance();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: appBarLoggedInDefault("Parametros de Simulação"),
      body: defaultBodyPadding(
        isLoading
            ? const Progress()
            : Column(
                children: [
                  defaultInputPadding(
                    TextField(
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
                  defaultButtonPadding(
                    SizedBox(
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
