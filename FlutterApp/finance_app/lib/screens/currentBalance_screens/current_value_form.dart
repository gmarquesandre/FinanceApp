import 'package:finance_app/models/current_balance/create_or_update_current_balance.dart';
import 'package:finance_app/providers/current_balance_provider.dart';
import 'package:flutter/material.dart';
import 'package:extended_masked_text/extended_masked_text.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';

class CurrentBalanceForm extends StatelessWidget {
  const CurrentBalanceForm({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(title: const Text('Saldo Conta Corrente')),
        body: Padding(
            padding: const EdgeInsets.fromLTRB(16.0, 0.0, 16.0, 0.0),
            child: Consumer<CurrentBalanceProvider>(
                builder: (context, balanceProvider, snapshot) {
              balanceProvider.getValue();

              // balanceProvider.updateValue(CreateOrUpdateCurrentBalance(
              //   percentageCdi: 1,
              //   value: 155.21,
              //   updateValueWithCdiIndex: false,
              // ));

              var value = MoneyMaskedTextController(
                  leftSymbol: 'R\$ ',
                  initialValue: balanceProvider.balance.value);

              var updateValueWithCdi =
                  balanceProvider.balance.updateValueWithCdiIndex;

              return SingleChildScrollView(
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Padding(
                      padding: const EdgeInsets.only(top: 8.0),
                      child: TextFormField(
                        keyboardType: const TextInputType.numberWithOptions(
                            decimal: true),
                        controller: value,
                        decoration: const InputDecoration(
                          labelText: 'Saldo Conta Corrente Atual',
                        ),
                      ),
                    ),
                    balanceProvider.balance.updateDateTime.year <= 1970
                        ? const Text("")
                        : Text(
                            "Ultima Atualização ${DateFormat("dd/MM/yyyy").format(balanceProvider.balance.updateDateTime)}",
                          ),
                    CheckboxListTile(
                      contentPadding: const EdgeInsets.all(0),
                      controlAffinity: ListTileControlAffinity.leading,
                      title: const Text("Atualizar Saldo pelo CDI"),
                      //    <-- label
                      value: updateValueWithCdi,
                      onChanged: (bool? value) {
                        balanceProvider.changeCdi();
                      },
                    ),
                    Padding(
                      padding: const EdgeInsets.only(top: 16.0),
                      child: SizedBox(
                        width: double.maxFinite,
                        child: ElevatedButton(
                          child: const Text('Atualizar'),
                          onPressed: () async {
                            // _createOrUpdate(CreateOrUpdateCurrentBalance(
                            //     percentageCdi: 1,
                            //     value: _value.numberValue,
                            //     updateValueWithCdiIndex: _updateValueWithCdi));
                            const snackBar = SnackBar(
                              duration: Duration(seconds: 2),
                              content: Text('Atualizado Com Sucesso.'),
                            );
                            ScaffoldMessenger.of(context)
                                .showSnackBar(snackBar);
                          },
                        ),
                      ),
                    ),
                  ],
                ),
              );
            })));
  }
}

// class CurrentBalanceForm extends StatefulWidget {
//   const CurrentBalanceForm({Key? key}) : super(key: key);

//   @override
//   CurrentBalanceFormState createState() {
//     return CurrentBalanceFormState();
//   }
// }



// class CurrentBalanceFormState extends State<CurrentBalanceForm> {
//   var _value = MoneyMaskedTextController(
//     leftSymbol: 'R\$ ',
//   );

//   late CurrentBalance balance;

//   bool _updateValueWithCdi = false;

//   DateTime _date = DateTime(1900, 1, 1);
//   void _createOrUpdate(
//       CreateOrUpdateCurrentBalance createOrUpdateCurrentBalance) async {
//     CurrentBalanceClient client = CurrentBalanceClient();

//     await client.create(createOrUpdateCurrentBalance);

//     _loadBalance;
//   }

//   void _loadBalance() async {
//     CurrentBalanceClient client = CurrentBalanceClient();

//     balance = await client.get();
  
//         _value = MoneyMaskedTextController(
//           leftSymbol: 'R\$ ',
//           initialValue: balance.value,
//         );

//         _date = balance.updateDateTime;

//         _updateValueWithCdi = balance.updateValueWithCdiIndex;
      
//     );
//   }

//   @override
//   void initState() {
//     super.initState();
//     _loadBalance();
//   }

//   @override
//   Widget build(BuildContext context) {
//     return Scaffold(
//       appBar: AppBar(title: Text('Saldo Conta Corrente')),
//       body: Padding(
//         padding: const EdgeInsets.fromLTRB(16.0, 0.0, 16.0, 0.0),
//         child: Consumer<CurrentBalanceProvider>(
//             builder: (context, balanceProvider, snapshot) {
//           balanceProvider.setValue(balance);

//           return Text(balanceProvider.value.value.toString());
//         }

              
//             ),
//       ),
//     );
//   }
// }
