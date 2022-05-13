

import 'dart:math';

import 'package:financial_app/database/dao/fund_simulation.dart';
import 'package:financial_app/database/dao/holidays_dao.dart';
import 'package:financial_app/database/dao/indexLastValue.dart';
import 'package:financial_app/database/dao/workingDaysByYear_dao.dart';
import 'package:financial_app/functions/fundCurrentPositionToBalance.dart';
import 'package:financial_app/functions/futureValues.dart';
import 'package:financial_app/functions/prospectValueDaily.dart';
import 'package:financial_app/models/dto_models/fundToBalance.dart';
import 'package:financial_app/models/models/IndexesAndDate.dart';
import 'package:financial_app/models/models/fundMonthly.dart';
import 'package:financial_app/models/models/interestAndDays.dart';
import 'package:financial_app/models/table_models/fundSimulation.dart';
import 'package:financial_app/models/table_models/holiday.dart';
import 'package:financial_app/models/table_models/indexDailyInterest.dart';
import 'package:financial_app/models/table_models/workingDaysByYear.dart';
import 'package:flutter/material.dart';

Future<List<FundMonthly>> getFundFutureValue(
    BuildContext context, List<DateTime> dates) async
  {


  List<FundToBalance> listFund =
    await getFundCurrentPositionToBalance(context);

  if (listFund.length == 0) return [];

  FundSimulationDao _dao = FundSimulationDao();

  List<FundSimulation> listFundSimulation = await _dao.findAll();

  List<FundMonthly> listReturn = [];

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

  listFundSimulation.map((e) => e.indexName).toSet().forEach((element) {
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

  listFund.forEach(
        (element) {
      var checkFundSimulation = listFundSimulation
          .where((fund) => fund.cnpj == element.cnpj);

      if (checkFundSimulation.length > 0) {
        FundSimulation fundSimulation = checkFundSimulation.first;

        dates.forEach(
              (date) {
            List<IndexDaily> listDailyThisElement = [];
            List<InterestAndNDays> listDailyThisElementGrouped = [];

            if (fundSimulation.indexName != '') {
              listDailyThisElement.addAll(listDaily.where((item) =>
              item.indexName == fundSimulation.indexName &&
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

            if (fundSimulation.fixedYearGain > 0.00) {
              var returnListPreFixedInterest = listPreFixedInterest(
                  firstDate,
                  date,
                  fundSimulation.fixedYearGain,
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
              FundMonthly(
                  date: date,
                  nameShort: fundSimulation.nameShort,
                  cnpj: element.cnpj,
                  totalValue: element.quantity * unitValueFinal,
                  unitValue: unitValueFinal),
            );
          },
        );
      } else {
        dates.forEach(
              (date) {
            listReturn.add(
              FundMonthly(
                  date: date,
                  cnpj: element.cnpj,
                  totalValue: element.quantity * element.unitPrice,
                  unitValue: element.unitPrice,
                  nameShort: element.nameShort ),
            );
          },
        );
      }
    },
  );

  return listReturn;
}