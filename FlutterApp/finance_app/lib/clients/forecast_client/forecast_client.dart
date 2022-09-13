import 'package:finance_app/clients/default_request/default_client.dart';
import 'package:finance_app/models/forecast/forecast_list.dart';
import 'package:intl/intl.dart';

class ForecastClient {
  static DefaultClient client = DefaultClient();

  static String controller = 'Forecast';

  Future<List<ForecastList>> get() async {
    Map<String, String> queryParameters = {
      'currentDate': DateFormat('yyyy-MM-dd').format(DateTime.now()).toString()
    };

    var responseJson = await client.getMany('$controller/Get', queryParameters);

    var items = responseJson
        .map((dynamic json) => ForecastList.fromJson(json))
        .toList();

    return items;
  }

  Future<List<ForecastList>> getTwoWeeks() async {
    Map<String, String> queryParameters = {
      'currentDate': DateFormat('yyyy-MM-dd').format(DateTime.now()).toString()
    };

    var responseJson = await client.getMany(
        '$controller/GetTwoWeeksForecast', queryParameters);

    var items = responseJson
        .map((dynamic json) => ForecastList.fromJson(json))
        .toList();

    return items;
  }
}
