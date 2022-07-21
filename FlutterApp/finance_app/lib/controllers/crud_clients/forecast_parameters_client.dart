import 'package:finance_app/controllers/default_request/default_client.dart';
import 'package:finance_app/models/forecast_parameters/create_or_update_forecast_parameters.dart';
import 'package:finance_app/models/forecast_parameters/forecast_parameters.dart';

class ForecastParametersClient {
  static DefaultClient client = DefaultClient();

  static String controller = 'ForecastParameters';

  Future<ForecastParameters> get() async {
    var responseJson = await client.getSingle('$controller/Get');

    var balance = ForecastParameters.fromJson(responseJson);

    return balance;
  }

  Future<bool> create(CreateOrUpdateForecastParameters item) async {
    final String body = item.toJson().toString();
    final String path = '$controller/CreateOrUpdate';
    var teste = client.create(path, body);

    return teste;
  }
}
