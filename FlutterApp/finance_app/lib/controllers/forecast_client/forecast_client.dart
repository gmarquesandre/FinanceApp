import 'package:finance_app/controllers/default_request/default_client.dart';
import 'package:finance_app/models/forecast/forecast_list.dart';

class ForecastClient {
  static DefaultClient client = DefaultClient();

  static String controller = 'Forecast';

  Future<List<ForecastList>> get() async {
    var responseJson = await client.getMany('$controller/Get');

    var items = responseJson
        .map((dynamic json) => ForecastList.fromJson(json))
        .toList();

    return items;
  }
}
