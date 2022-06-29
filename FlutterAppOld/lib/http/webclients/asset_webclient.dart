import 'dart:convert';
import 'package:financial_app/models/models/IndexesAndDate.dart';
import 'package:financial_app/models/table_models/assetChangeInput.dart';
import 'package:financial_app/models/table_models/assetChanges.dart';
import 'package:financial_app/models/table_models/assetCurrentValue.dart';
import 'package:financial_app/models/table_models/assetEarnings.dart';
import 'package:financial_app/models/table_models/holiday.dart';
import 'package:financial_app/models/table_models/indexDailyInterest.dart';
import 'package:financial_app/models/table_models/indexProspect.dart';
import 'package:financial_app/models/table_models/investmentFundCurrentValue.dart';
import 'package:financial_app/models/table_models/treasuryCurrentValue.dart';
import 'package:financial_app/models/table_models/workingDaysByYear.dart';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;

class TransactionWebClient {
  // static String baseUrl = '10.0.2.2:65458';
  static String baseUrl = 'myfinanceapi.azurewebsites.net';

  static int timeout = 300;

  Future<List<WorkingDaysByYear>> getWorkingDaysByYear(
      DateTime lastUpdateDate) async {
    final queryParameters = {'lastUpdateDate': lastUpdateDate.toString()};

    var uri = Uri.https(baseUrl, '/mvp/GetWorkingDaysByYear', queryParameters);

    final response = await http.get(uri).timeout(Duration(seconds: timeout));

    if (response.statusCode == 200) {
      final List<dynamic> decodedJson = jsonDecode(response.body);

      return decodedJson
          .map((dynamic json) => WorkingDaysByYear.fromJson(json))
          .toList();
    } else if (response.statusCode == 204) {
      return [];
    } else {
      throw Exception('Failed');
    }
  }

  Future<double?> getTreasuryBondValueDay(
      DateTime date, String codeisin, int operation) async {
    final queryParameters = {'codeisin': codeisin, 'date': date.toString()};

    var uri =
        Uri.https(baseUrl, '/mvp/GetTreasuryBondValueDay', queryParameters);

    final response = await http.get(uri).timeout(Duration(seconds: timeout));

    if (response.statusCode == 200) {
      final dynamic decodedJson = jsonDecode(response.body);

      if (operation == 2) {
        return decodedJson['unitPriceSell'];
      }
      try {
        return decodedJson['unitPriceBuy'];
      } catch (e) {
        return 0;
      }
    } else if (response.statusCode == 204) {
      return null;
    } else {
      throw Exception('Failed');
    }
  }

  Future<List<Holiday>> getHolidays(DateTime lastUpdateDate) async {
    final queryParameters = {'lastUpdateDate': lastUpdateDate.toString()};

    var uri = Uri.https(baseUrl, '/mvp/GetHolidays', queryParameters);

    final response = await http.get(uri).timeout(Duration(seconds: timeout));

    if (response.statusCode == 200) {
      final List<dynamic> decodedJson = jsonDecode(response.body);

      return decodedJson.map((dynamic json) => Holiday.fromJson(json)).toList();
    } else if (response.statusCode == 204) {
      return [];
    } else {
      throw Exception('Failed');
    }
  }

  Future<List<AssetCurrentValue>> getAsset(String assetCode) async {
    final queryParameters = {'stock': assetCode};

    debugPrint(queryParameters.toString());

    var uri = Uri.https(baseUrl, '/mvp/GetStock', queryParameters);

    final response = await http.get(uri).timeout(Duration(seconds: timeout));

    if (response.statusCode == 200) {
      final List<dynamic> decodedJson = jsonDecode(response.body);

      return decodedJson
          .map((dynamic json) => AssetCurrentValue.fromJson(json))
          .toList();
    } else {
      throw Exception('Failed');
    }
  }

  Future<List<AssetCurrentValue>> getAssetWithList(
      List<String> assetList) async {
    String param = assetList.join(",");

    final queryParameters = {'listStocks': param};

    var uri = Uri.https(baseUrl, '/mvp/GetStockWithList', queryParameters);

    final response = await http.get(uri);

    if (response.statusCode == 200) {
      final List<dynamic> decodedJson = jsonDecode(response.body);

      return decodedJson
          .map((dynamic json) => AssetCurrentValue.fromJson(json))
          .toList();
    } else {
      throw Exception('Failed');
    }
  }

  Future<List<AssetEarnings>> postAssetEarnings(
      List<AssetChangeInput> listChangeInput) async {
    var uri = Uri.https(baseUrl, '/mvp/PostAssetEarnings');

    final String json =
        jsonEncode(listChangeInput.map((e) => e.toJson()).toList());

    final response = await http.post(
      uri,
      body: json.toString(),
      headers: {'Content-type': 'application/json'},
    );

    if (response.statusCode == 200) {
      final List<dynamic> decodedJson = jsonDecode(response.body);

      return decodedJson
          .map((dynamic json) => AssetEarnings.fromJson(json))
          .toList();
    } else {
      throw Exception('Failed');
    }
  }

  Future<List<AssetChanges>> postAssetChanges(
      List<AssetChangeInput> listChangeInput) async {
    var uri = Uri.https(baseUrl, '/mvp/PostAssetChanges');

    final String json =
        jsonEncode(listChangeInput.map((e) => e.toJson()).toList());

    try {
      final response = await http.post(uri,
          body: json.toString(), headers: {'Content-type': 'application/json'});

      if (response.statusCode == 200) {
        final List<dynamic> decodedJson = jsonDecode(response.body);

        return decodedJson
            .map((dynamic json) => AssetChanges.fromJson(json))
            .toList();
      } else {
        throw Exception('Failed');
      }
    } on Exception catch (e) {
      debugPrint(e.toString());
      return [];
    }
  }

  Future<List<TreasuryCurrentValue>> getTreasury() async {
    var uri = Uri.https(baseUrl, '/mvp/GetTreasuryBond');

    final response = await http.get(uri).timeout(Duration(seconds: timeout));

    if (response.statusCode == 200) {
      final List<dynamic> decodedJson = jsonDecode(response.body);

      return decodedJson
          .map((dynamic json) => TreasuryCurrentValue.fromJson(json))
          .toList();
    } else {
      throw Exception('Failed');
    }
  }

  Future<List<InvestmentFundCurrentValue>> getFund(String filter) async {
    final queryParameters = {'value': filter};

    var uri = Uri.https(baseUrl, '/mvp/GetFundList', queryParameters);

    final response = await http.get(uri).timeout(Duration(seconds: timeout));

    if (response.statusCode == 200) {
      final List<dynamic> decodedJson = jsonDecode(response.body);

      return decodedJson
          .map((dynamic json) => InvestmentFundCurrentValue.fromJson(json))
          .toList();
    } else {
      throw Exception('Failed');
    }
  }

  Future<List<InvestmentFundCurrentValue>> getFundsWithList(
      List<String> assetList) async {
    String param = assetList.join(",");

    final queryParameters = {'listFund': param};

    var uri = Uri.https(baseUrl, '/mvp/GetFundWithList', queryParameters);

    final response = await http.get(uri).timeout(Duration(seconds: timeout));

    if (response.statusCode == 200) {
      final List<dynamic> decodedJson = jsonDecode(response.body);

      return decodedJson
          .map((dynamic json) => InvestmentFundCurrentValue.fromJson(json))
          .toList();
    } else {
      throw Exception('Failed');
    }
  }

  Future<List<IndexDaily>> getIndexLastValues() async {
    var uri = Uri.https(baseUrl, '/mvp/GetLastValueIndex');

    final response = await http.get(uri).timeout(Duration(seconds: timeout));

    if (response.statusCode == 200) {
      final List<dynamic> decodedJson = jsonDecode(response.body);

      return decodedJson
          .map((dynamic json) => IndexDaily.fromJson(json))
          .toList();
    } else {
      throw Exception('Failed');
    }
  }

  Future<List<IndexDaily>> getIndexValues(IndexAndDate item) async {
    final queryParameters = {
      'indexName': item.indexName,
      'minDate': item.minDate.toString(),
      'maxDate': item.maxDate.toString(),
    };

    var uri = Uri.https(baseUrl, '/mvp/GetIndex', queryParameters);

    try {
      final response = await http.get(uri).timeout(Duration(seconds: timeout));

      if (response.statusCode == 200) {
        final List<dynamic> decodedJson = jsonDecode(response.body);

        if (decodedJson.length > 0) {
          return decodedJson
              .map((dynamic json) => IndexDaily.fromJson(json))
              .toList();
        }
        return [];
      } else {
        throw Exception('Failed');
      }
    } on Exception catch (e) {
      debugPrint("Ué $e");
    } finally {}
    return [];
  }

  Future<List<IndexProspect>> getIndexProspect() async {
    var uri = Uri.https(baseUrl, '/mvp/GetProspect');

    final response = await http.get(uri);

    if (response.statusCode == 200) {
      final List<dynamic> decodedJson = jsonDecode(response.body);

      return decodedJson
          .map((dynamic json) => IndexProspect.fromJson(json))
          .toList();
    } else {
      throw Exception('Failed');
    }
  }
}
