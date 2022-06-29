import 'package:financial_app/functions/dateFunctions.dart';
import 'package:financial_app/functions/futureValuesFixedInterest.dart';
import 'package:financial_app/models/models/fixedInterestMonthly.dart';
import 'package:financial_app/providers/fixedInterestListProvider.dart';
import 'package:flutter/material.dart';
import 'package:provider/provider.dart';

Future<List<FixedInterestMonthly>> fixedInterestPosition(
    BuildContext context) async {

  DateTime date = await getLastWorkingDay();
  List<DateTime> dates = [date];
  List<FixedInterestMonthly> list = [];


  list = await getFixedInterestFutureValues(dates);
  Provider.of<FixedInterestListProvider>(context, listen: false)
      .refresh(list);

  return list;
}
