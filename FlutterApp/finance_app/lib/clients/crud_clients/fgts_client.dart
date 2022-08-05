import 'package:finance_app/clients/default_request/default_client.dart';
import 'package:finance_app/models/fgts/create_or_update_fgts.dart';
import 'package:finance_app/models/fgts/fgts.dart';

class FGTSClient {
  static DefaultClient client = DefaultClient();

  static String controller = 'FGTS';

  Future<FGTS> get() async {
    var responseJson = await client.getSingle('$controller/Get');

    var balance = FGTS.fromJson(responseJson);

    return balance;
  }

  Future<bool> create(CreateOrUpdateFGTS item) async {
    final String body = item.toJson().toString();
    final String path = '$controller/CreateOrUpdate';
    var teste = client.create(path, body);

    return teste;
  }
}
