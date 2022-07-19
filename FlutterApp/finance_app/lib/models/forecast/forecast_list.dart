import 'package:finance_app/models/forecast/forecast_item.dart';

class ForecastList {
  int type;
  String typeDisplayValue;
  List<ForecastItem> items;

  ForecastList(
      {required this.type,
      required this.typeDisplayValue,
      required this.items});

  ForecastList.fromJson(Map<String, dynamic> json)
      : type = json['type'],
        typeDisplayValue = json['typeDisplayValue'],
        items = json['items']
            .map<ForecastItem>((dynamic item) => ForecastItem.fromJson(item))
            .toList();

  // var items =
  // responseJson.map((dynamic json) => Income.fromJson(json)).toList();
}
