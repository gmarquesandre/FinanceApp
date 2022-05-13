import 'package:extended_masked_text/extended_masked_text.dart';
import 'package:financial_app/components/globalVariables.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';
import 'package:intl/intl.dart';
import 'package:shared_preferences/shared_preferences.dart';

class FgtsForm extends StatefulWidget {
  @override
  _FgtsFormState createState() => _FgtsFormState();
}

class _FgtsFormState extends State<FgtsForm> {

  var _totalAccountBalanceFGTS = MoneyMaskedTextController(leftSymbol: 'R\$ ');

  var _valueGrossIncome = MoneyMaskedTextController(leftSymbol: 'R\$ ');

  bool _birthdayWithdraw = false;

  DateTime _monthWithdrawFGTS = DateTime(1900, 1, 1);

  List<DateTime> months =
      new List<DateTime>.generate(12, (i) => DateTime(1900, i + 1, 1));

  final String totalAccountBalanceFGTS = GlobalVariables.totalAccountBalanceFGTS;

  final String dateLastUpdateFGTS = GlobalVariables.dateLastUpdateFGTS;

  final String birthdayWithDraw = GlobalVariables.birthdayWithDrawFGTS;

  final String monthWithdrawFGTS = GlobalVariables.monthWithdrawFGTS;

  final String valueGrossIncome = GlobalVariables.valueGrossIncome;

  DateTime _date = DateTime.now();

  void _loadValues() async {
    final prefs = await SharedPreferences.getInstance();
    setState(() {
      _totalAccountBalanceFGTS = MoneyMaskedTextController(
          leftSymbol: 'R\$ ',
          initialValue: prefs.getDouble(totalAccountBalanceFGTS) ?? 0);

      _birthdayWithdraw = prefs.getBool(birthdayWithDraw) ?? false;

      _date = DateTime.fromMillisecondsSinceEpoch(
          prefs.getInt(GlobalVariables.dateEpochLastUpdateBalanceValue) ?? 0);

      _valueGrossIncome = MoneyMaskedTextController(
          leftSymbol: 'R\$ ',
          initialValue: prefs.getDouble(valueGrossIncome) ?? 0);

      _monthWithdrawFGTS =
          DateTime(1900, prefs.getInt(monthWithdrawFGTS) ?? 1, 1);
    },);
  }

  void storeValue() async {
    final prefs = await SharedPreferences.getInstance();
    setState(() {
      prefs.setDouble(
          totalAccountBalanceFGTS, _totalAccountBalanceFGTS.numberValue);

      prefs.setDouble(valueGrossIncome, _valueGrossIncome.numberValue);

      prefs.setBool(birthdayWithDraw, _birthdayWithdraw);

      prefs.setInt(dateLastUpdateFGTS, DateTime.now().millisecondsSinceEpoch);

      prefs.setInt(monthWithdrawFGTS, _monthWithdrawFGTS.month);

      _date = DateTime.now();
    });
  }

  @override
  void initState() {
    super.initState();
    _loadValues();
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
                  controller: _valueGrossIncome,
                  decoration: InputDecoration(
                    labelText: 'Renda Bruta',
                  ),
                ),
              ),
              Padding(
                padding: const EdgeInsets.only(top: 8.0),
                child: TextFormField(
                  keyboardType: TextInputType.numberWithOptions(decimal: true),
                  controller: _totalAccountBalanceFGTS,
                  decoration: InputDecoration(
                    labelText: 'Saldo Total do FGTS',
                  ),
                ),
              ),
              _date.year <= 1970
                  ? Text(""):
              Text(
                "Ultima Atualização " +
                    DateFormat("dd/MM/yyyy").format(_date),
              ),
              CheckboxListTile(
                contentPadding: EdgeInsets.all(0),
                controlAffinity: ListTileControlAffinity.leading,
                title: Text("Saque Aniversário"),
                //    <-- label
                value: _birthdayWithdraw,
                onChanged: (newValue) {
                  setState(() {
                    _birthdayWithdraw = newValue!;
                  });
                },
              ),

              // CheckboxListTile(
              //   title: Text("title text"),
              //   value: false,
              //  <-- leading Checkbox
              // ),
              // Row(
              //   children: [
              //
              //     Text('Saque Aniversario',),
              //     Checkbox(
              //       checkColor: Colors.white,
              //       value: _birthdayWithdraw,
              //       onChanged: (bool? value) {
              //         setState((){
              //           _birthdayWithdraw = value!;
              //         });
              //       },
              //     ),
              //
              //   ],
              // ),
              // ),
              Visibility(
                visible: _birthdayWithdraw,
                child: Padding(
                  padding: const EdgeInsets.only(top: 8.0, left: 0),
                  child: DropdownButtonFormField<DateTime>(
                    value: _monthWithdrawFGTS,
                    decoration: InputDecoration(
                      labelText: 'Mês do Saque Aniversário',
                    ),
                    items: months
                        .map<DropdownMenuItem<DateTime>>((_monthWithdrawFGTS) {
                      return DropdownMenuItem<DateTime>(
                        value: _monthWithdrawFGTS,
                        child: Text(
                          DateFormat('MMMM', 'pt-br')
                              .format(_monthWithdrawFGTS),
                        ),
                      );
                    }).toList(),
                    onChanged: (DateTime? newValue) {
                      setState(() {
                        _monthWithdrawFGTS = newValue!;
                      });
                    },
                  ),
                ),
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
