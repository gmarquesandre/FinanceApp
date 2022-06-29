import 'dart:math';
import 'package:financial_app/database/dao/fixedInterest_dao.dart';
import 'package:financial_app/database/dao/holidays_dao.dart';
import 'package:financial_app/database/dao/indexDailyValue_dao.dart';
import 'package:financial_app/database/dao/indexLastValue.dart';
import 'package:financial_app/database/dao/workingDaysByYear_dao.dart';
import 'package:financial_app/functions/futureValues.dart';
import 'package:financial_app/functions/prospectValueDaily.dart';
import 'package:financial_app/functions/updateData.dart';
import 'package:financial_app/models/models/IndexesAndDate.dart';
import 'package:financial_app/models/models/fixedInterestMonthly.dart';
import 'package:financial_app/models/models/interestAndDays.dart';
import 'package:financial_app/models/table_models/fixedInterest.dart';
import 'package:financial_app/models/table_models/holiday.dart';
import 'package:financial_app/models/table_models/indexDailyInterest.dart';
import 'package:financial_app/models/table_models/workingDaysByYear.dart';
import 'package:flutter/material.dart';

Future<List<FixedInterestMonthly>> getFixedInterestFutureValues(
    List<DateTime> dates) async {

  if (dates.length == 0) return [];
  //Importa dados
  final FixedInterestDao _daoFixedInterest = FixedInterestDao();
  List<FixedInterest> list = (await _daoFixedInterest.findAll())
      .where((a) => a.expirationDate.compareTo(DateTime.now()) >= 0)
      .toList();

  if (list.length == 0) return [];

  await updateIndexLastValue();
  await updateIndexes();

  DateTime maxDateExpirationInvestments =
      list.map((e) => e.expirationDate).reduce((a, b) => a.isAfter(b) ? a : b);

  DateTime minDateInvestment =
      list.map((e) => e.investmentDate).reduce((a, b) => a.isBefore(b) ? a : b);

  DateTime maxDate = dates.map((e) => e).reduce((a, b) => a.isAfter(b) ? a : b);

  final IndexDailyValueDao _daoIndexDaily = IndexDailyValueDao();

  final HolidaysDao _daoHolidays = HolidaysDao();

  final WorkingDaysByYearDao _dao = WorkingDaysByYearDao();

  final IndexLastValueDao _daoLastValueIndex = IndexLastValueDao();

  List<IndexDaily> lastValueIndex = await _daoLastValueIndex.findAll();

  List<WorkingDaysByYear> workingDaysList =
      await _dao.findAll(minDateInvestment.year, dates.last.year);

  List<Holiday> holidaysList = await _daoHolidays.findAll(
      minDateInvestment.subtract(const Duration(days: 1)),
      maxDate.add(const Duration(days: 1)));

  List<DateTime> holidays = holidaysList.map((e) => e.date).toList();

  List<IndexAndDate> listIndexAndDate = await _daoIndexDaily.getIndexesList();

  if (listIndexAndDate.length == 0) {
    list.map((e) => e.indexName).toSet().forEach((element) {
      listIndexAndDate.add(IndexAndDate(
          indexName: element,
          maxDate: list
              .where((e) => e.indexName == element)
              .map((e) => e.investmentDate)
              .reduce((a, b) => a.isBefore(b) ? a : b)));
    });
  } else {
    list.map((e) => e.indexName).toSet().forEach((element) {
      if (!listIndexAndDate.map((e) => e.indexName).contains(element)) {
        listIndexAndDate.add(IndexAndDate(
            indexName: element,
            maxDate: list
                .where((e) => e.indexName == element)
                .map((e) => e.investmentDate)
                .reduce((a, b) => a.isBefore(b) ? a : b)));
      }
    });
  }

  List<IndexDaily> listDaily = await _daoIndexDaily.findAll();

  //Transforma Indices mensais em diarios
  if (listDaily.any((element) =>
      element.indexName == "IPCA" || element.indexName == "IGPM")) {

      listDaily = convertMonthlyToDaily(listDaily);

  }

  //Adiciona valores futuros com o prospecto de mercado do FOCUS
  List<FixedInterestMonthly> listReturn = [];

  DateTime maxDateChart = dates.reduce((a, b) => a.isAfter(b) ? a : b);

  DateTime maxDateReturn =
      maxDateExpirationInvestments.compareTo(maxDateChart) > 0
          ? maxDateChart
          : maxDateExpirationInvestments;

  //Adiona datas faltantes entre a previsão e o real com o ultimo valor do real.
  listDaily = await addProspectValue(
      listIndexAndDate, listDaily, maxDateReturn, holidays, lastValueIndex);

  list.forEach((element) {
    dates.forEach((date) {
      double todayValue = getTodayValueFixedInterest(
          element, date, listDaily, holidays, workingDaysList);


      try {
        listReturn.add(
          FixedInterestMonthly(
              id: element.id!,
              name: element.name,
              date: date,
              typeFixedInterestId: element.typeFixedInterestId,
              amount: element.amount,
              preFixedInvestment: element.preFixedInvestment,
              indexName: element.indexName,
              indexPercentage: element.indexPercentage,
              additionalFixedInterest: element.additionalFixedInterest,
              investmentDate: element.investmentDate,
              expirationDate: element.expirationDate,
              liquidityOnExpiration: element.liquidityOnExpiration,
              fixedInterestElement: element,
              todayValue: todayValue),
        );
      } on Exception catch (e) {
        debugPrint("Erro " + e.toString());
      }
    });
  });
  debugPrint("Chegou");
  return listReturn;
}

List<IndexDaily> convertMonthlyToDaily(List<IndexDaily> listDaily) {
  var listTeste = listDaily
      .where((element) => ["IPCA", "IGPM"].contains(element.indexName))
      .toList();

  listDaily
      .removeWhere((element) => ["IPCA", "IGPM"].contains(element.indexName));

  listTeste.forEach(
        (element) {
      int nDays = (DateTime(element.date.year, element.date.month + 1, 1)
          .subtract(const Duration(days: 1))
          .difference(DateTime(element.date.year, element.date.month, 1)))
          .inDays;

      for (DateTime date = element.date;
      date
          .difference(
          DateTime(element.date.year, element.date.month + 1, 1)
              .subtract(const Duration(days: 1)))
          .inDays <=
          0;
      date = date.add(Duration(days: 1)))
        listDaily.add(
          IndexDaily(
              date: date,
              indexName: element.indexName,
              value: (pow(1 + element.value, 1.00 / nDays) - 1.00),
              id: element.id),
        );
    },
  );
  return listDaily;
}

double getTodayValueFixedInterest(
    FixedInterest element,
    DateTime date,
    List<IndexDaily> listDaily,
    List<DateTime> holidays,
    List<WorkingDaysByYear> workingDaysList) {
  List<IndexDaily> listDailyThisElement = [];

  List<InterestAndNDays> listDailyThisElementGrouped = [];

  if (element.preFixedInvestment != 1 && element.indexPercentage > 0) {

      DateTime thisDate = date.compareTo(element.expirationDate) <= 0
          ? date
          : element.expirationDate;

      listDailyThisElement.addAll(listDaily
          .where((item) =>
              item.indexName == element.indexName &&
              item.date.compareTo(element.investmentDate) >= 0 &&
              item.date.compareTo(thisDate) < 0));

    List<double> values =
        listDailyThisElement.map((e) => e.value).toSet().toList();

    listDailyThisElementGrouped = [];

    int thisWorkingDays;
    values.forEach((element) {
      thisWorkingDays = listDailyThisElement
          .where((elementDaily) => elementDaily.value == element)
          .length;
      listDailyThisElementGrouped.add(InterestAndNDays(
          interestRate: element, workingDays: thisWorkingDays));
    });

    //Atualiza para valores diferentes de 100%
    if (element.indexPercentage != 1) {
      listDailyThisElementGrouped.forEach((listThis) {
        //Recalcula para indices diferentes de 100% ou com adicional
        listThis.interestRate = listThis.interestRate * element.indexPercentage;
      });
    }
  }

  if (element.additionalFixedInterest > 0.00) {
    DateTime thisDate = date.compareTo(element.expirationDate) < 0
        ? date
        : element.expirationDate;

    var returnListPreFixedInterest = listPreFixedInterest(
        element.investmentDate,
        thisDate,
        element.additionalFixedInterest,
        holidays,
        workingDaysList);

    returnListPreFixedInterest.forEach((element) {
      listDailyThisElementGrouped.add(InterestAndNDays(
          workingDays: element.workingDays,
          interestRate: element.interestRate));
    });
  }
  return returnFinalValueFixedInterest(
      date, element, listDailyThisElementGrouped);
}
