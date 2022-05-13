import 'package:financial_app/models/table_models/asset.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';

class AssetTransaction with ChangeNotifier {
  int quantityCumulated;
  double avgUnitPrice;
  double exValue;
  double value;
  String operation;
  Asset asset;


  AssetTransaction({
    required this.quantityCumulated,
    required this.avgUnitPrice,
    required this.operation,
    required this.exValue,
    required this.value,
    required this.asset
  });


  String quantityCumulatedLabel(){


    if(operation == "C"){
      return "$quantityCumulated ( + ${asset.quantity} )";
    }
    else if(operation == "V"){
      return "$quantityCumulated ( - ${asset.quantity} )";
    }
    else if(operation == "Bonif"){
      return "$quantityCumulated ( + ${(value*100).toStringAsFixed(0)} %)";
    }
    else if(operation == "Desd" || operation == "Grup"){
      return "$quantityCumulated ( * ${(value+1).toStringAsFixed(0)} )";
    }

    return quantityCumulated.toString();

  }



  //

  String avgUnitPriceLabel(){

    final currencyFormat = NumberFormat.currency(
        locale: "pt_BR",
        symbol: "R"
            "\$",
        decimalDigits: 2);

    if(operation == "JCP" || operation == "Rend" || operation == "Div"){
      return "${currencyFormat.format(avgUnitPrice)} ( - ${exValue.toStringAsFixed(2)} )";

    }

    else if(operation == "Desd" || operation == "Grup"){
      return "${currencyFormat.format(avgUnitPrice)} ( ÷ ${(1+value).toStringAsFixed(0)} )";
    }


    return currencyFormat
        .format(avgUnitPrice)
        .toString();

  }


}
