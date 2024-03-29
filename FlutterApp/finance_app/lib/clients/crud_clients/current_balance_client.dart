import 'package:finance_app/clients/default_request/default_client.dart';
import 'package:finance_app/models/current_balance/create_or_update_current_balance.dart';
import 'package:finance_app/models/current_balance/current_balance.dart';

class CurrentBalanceClient {
  static DefaultClient client = DefaultClient();

  static String controller = 'CurrentBalance';

  Future<CurrentBalance> get() async {
    var responseJson = await client.getSingle('$controller/Get');

    var balance = CurrentBalance.fromJson(responseJson);

    return balance;
  }

  Future<CurrentBalance> create(CreateOrUpdateCurrentBalance item) async {
    final String body = item.toJson().toString();
    final String path = '$controller/CreateOrUpdate';
    var teste = await client.create(path, body);

    return CurrentBalance.fromJson(teste);
  }
}
