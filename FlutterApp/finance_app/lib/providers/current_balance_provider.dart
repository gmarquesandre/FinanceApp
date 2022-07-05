import 'package:finance_app/controllers/current_balance_client.dart';
import 'package:finance_app/models/current_balance/create_or_update_current_balance.dart';
import 'package:finance_app/models/current_balance/current_balance.dart';
import 'package:flutter/cupertino.dart';

class CurrentBalanceProvider extends ChangeNotifier {
  CurrentBalance balance = CurrentBalance(
      creationDateTime: DateTime(1900, 1, 1),
      percentageCdi: 0.00,
      updateDateTime: DateTime(1900, 1, 1),
      updateValueWithCdiIndex: false,
      value: 0.00);

  bool updated = false;

  CurrentBalanceClient client = CurrentBalanceClient();

  getValue() async {
    if (!updated) {
      balance = await client.get();
      updated = true;
      notifyListeners();
    }
  }

  updateValue(CreateOrUpdateCurrentBalance newValue) async {
    await client.create(newValue);

    balance = CurrentBalance(
      creationDateTime: DateTime.now(),
      percentageCdi: newValue.percentageCdi,
      updateDateTime: DateTime.now(),
      updateValueWithCdiIndex: newValue.updateValueWithCdiIndex,
      value: newValue.value,
    );

    notifyListeners();
  }

  changeCdi() {
    balance.updateValueWithCdiIndex = !balance.updateValueWithCdiIndex;
    notifyListeners();
  }
}
