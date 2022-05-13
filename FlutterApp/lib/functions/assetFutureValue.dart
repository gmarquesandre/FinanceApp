import 'dart:math';

import 'package:financial_app/database/dao/asset_simulation.dart';
import 'package:financial_app/database/dao/holidays_dao.dart';
import 'package:financial_app/database/dao/indexLastValue.dart';
import 'package:financial_app/database/dao/workingDaysByYear_dao.dart';
import 'package:financial_app/functions/assetCurrentPositionToBalance.dart';
import 'package:financial_app/functions/futureValues.dart';
import 'package:financial_app/functions/prospectValueDaily.dart';
import 'package:financial_app/models/dto_models/assetToBalance.dart';
import 'package:financial_app/models/models/IndexesAndDate.dart';
import 'package:financial_app/models/models/assetMonthly.dart';
import 'package:financial_app/models/models/interestAndDays.dart';
import 'package:financial_app/models/table_models/assetSimulation.dart';
import 'package:financial_app/models/table_models/holiday.dart';
import 'package:financial_app/models/table_models/indexDailyInterest.dart';
import 'package:financial_app/models/table_models/workingDaysByYear.dart';
import 'package:flutter/cupertino.dart';

Future<List<AssetMonthly>> getAssetFutureValue(
    BuildContext context, List<DateTime> dates) async
  {


  List<AssetToBalance> listAsset =
    await getAssetCurrentPositionToBalance(context);

  if (listAsset.length == 0) return [];

  AssetSimulationDao _dao = AssetSimulationDao();

  List<AssetSimulation> listAssetSimulation = await _dao.findAll();

  List<AssetMonthly> listReturn = [];

  DateTime firstDate =
  dates.map((e) => e).reduce((a, b) => a.isBefore(b) ? a : b);

  DateTime lastDate =
  dates.map((e) => e).reduce((a, b) => a.isAfter(b) ? a : b);

  final WorkingDaysByYearDao _daoWorkingDays = WorkingDaysByYearDao();
  List<WorkingDaysByYear> workingDaysList =
  await _daoWorkingDays.findAll(firstDate.year, lastDate.year);

  final HolidaysDao _daoHolidays = HolidaysDao();

  List<Holiday> holidaysList = await _daoHolidays.findAll(
      DateTime.now(), lastDate.add(const Duration(days: 1)));

  final IndexLastValueDao _daoLastValueIndex = IndexLastValueDao();

  List<DateTime> holidays = holidaysList.map((e) => e.date).toList();

  List<IndexAndDate> listIndexAndDate = [];

  List<IndexDaily> listDaily = [];

  listAssetSimulation.map((e) => e.indexName).toSet().forEach((element) {
    listIndexAndDate.add(
      IndexAndDate(
        indexName: element,
        maxDate: DateTime.now(),
      ),
    );
  });

  List<IndexDaily> lastValueIndex = await _daoLastValueIndex.findAll();

  listDaily = await addProspectValue(
      listIndexAndDate, listDaily, lastDate, holidays, lastValueIndex);

  listAsset.forEach(
        (element) {
      var checkAssetSimulation = listAssetSimulation
          .where((asset) => asset.assetCode == element.assetCode);

      if (checkAssetSimulation.length > 0) {
        AssetSimulation assetSimulation = checkAssetSimulation.first;

        dates.forEach(
              (date) {
            List<IndexDaily> listDailyThisElement = [];
            List<InterestAndNDays> listDailyThisElementGrouped = [];

            if (assetSimulation.indexName != '') {
              listDailyThisElement.addAll(listDaily.where((item) =>
              item.indexName == assetSimulation.indexName &&
                  item.date.compareTo(firstDate) >= 0 &&
                  item.date.compareTo(date) < 0));

              List<double> values =
              listDailyThisElement.map((e) => e.value).toSet().toList();

              listDailyThisElementGrouped = [];

              int thisWorkingDays;
              values.forEach(
                    (element) {
                  thisWorkingDays = listDailyThisElement
                      .where((elementDaily) => elementDaily.value == element)
                      .length;
                  listDailyThisElementGrouped.add(InterestAndNDays(
                      interestRate: element, workingDays: thisWorkingDays));
                },
              );
            }

            if (assetSimulation.fixedYearGain > 0.00) {
              var returnListPreFixedInterest = listPreFixedInterest(
                  firstDate,
                  date,
                  assetSimulation.fixedYearGain,
                  holidays,
                  workingDaysList);

              returnListPreFixedInterest.forEach(
                    (element) {
                  listDailyThisElementGrouped.add(InterestAndNDays(
                      workingDays: element.workingDays,
                      interestRate: element.interestRate));
                },
              );
            }
            double interestTotal = 1;
            listDailyThisElementGrouped.forEach((item) {
              interestTotal *= pow(item.interestRate + 1, item.workingDays);
            });

            double unitValueFinal = element.unitPrice * interestTotal;

            listReturn.add(
              AssetMonthly(
                  date: date,
                  assetCode: element.assetCode,
                  totalValue: element.quantity * unitValueFinal,
                  unitValue: unitValueFinal),
            );
          },
        );
      } else {
        dates.forEach(
              (date) {
            listReturn.add(
              AssetMonthly(
                  date: date,
                  assetCode: element.assetCode,
                  totalValue: element.quantity * element.unitPrice,
                  unitValue: element.unitPrice),
            );
          },
        );
      }
    },
  );

  return listReturn;
}