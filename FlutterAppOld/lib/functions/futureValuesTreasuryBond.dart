import 'dart:math';
import 'package:financial_app/database/dao/holidays_dao.dart';
import 'package:financial_app/database/dao/indexLastValue.dart';
import 'package:financial_app/database/dao/workingDaysByYear_dao.dart';
import 'package:financial_app/functions/futureValues.dart';
import 'package:financial_app/functions/prospectValueDaily.dart';
import 'package:financial_app/functions/treasuryCurrentPositionToBalance.dart';
import 'package:financial_app/models/dto_models/treasuryToBalance.dart';
import 'package:financial_app/models/models/IndexesAndDate.dart';
import 'package:financial_app/models/models/interestAndDays.dart';
import 'package:financial_app/models/models/interestAndPeriod.dart';
import 'package:financial_app/models/models/treasuryBondMonthly.dart';
import 'package:financial_app/models/models/treasurySI.dart';
import 'package:financial_app/models/table_models/holiday.dart';
import 'package:financial_app/models/table_models/indexDailyInterest.dart';
import 'package:financial_app/models/table_models/workingDaysByYear.dart';
import 'package:flutter/material.dart';

Future<List<TreasuryBondMonthly>> getTreasuryBondFutureValues(
    BuildContext context, List<DateTime> dates) async {
  List<TreasuryBondMonthly> listReturn = [];
  //Importa dados

  DateTime today =
      DateTime(DateTime.now().year, DateTime.now().month, DateTime.now().day);

  final List<TreasuryToBalance> listTreasury =
      (await getTreasuryCurrentPositionToBalance(context))
          .where((a) => a.expirationDate.compareTo(today) >= 0)
          .toList();

  if (listTreasury.length == 0) return [];

  final HolidaysDao _daoHolidays = HolidaysDao();

  final IndexLastValueDao _daoLastValueIndex = IndexLastValueDao();

  final WorkingDaysByYearDao _dao = WorkingDaysByYearDao();

  List<IndexDaily> lastValueIndex = await _daoLastValueIndex.findAll();

  DateTime maxDate = dates.map((e) => e).reduce((a, b) => a.isAfter(b) ? a : b);

  List<Holiday> holidaysList =
      await _daoHolidays.findAll(today, maxDate.add(const Duration(days: 1)));

  List<DateTime> holidays = holidaysList.map((e) => e.date).toList();

  List<WorkingDaysByYear> workingDaysList =
      await _dao.findAll(today.year, maxDate.year);

  List<String> indexes = listTreasury.map((e) => e.indexName).toSet().toList();

  lastValueIndex = lastValueIndex
      .where((element) => indexes.contains(element.indexName) || element.indexName == "VNA IPCA" || element.indexName == "VNA IGPM")
      .toList();

  List<IndexDaily> listDaily = [];

  //Adiciona valores futuros com o prospecto de mercado do FOCUS
  DateTime maxDateExpirationInvestments = listTreasury
      .map((e) => e.expirationDate)
      .reduce((a, b) => a.isAfter(b) ? a : b);

  DateTime maxDateChart = dates.reduce((a, b) => a.isAfter(b) ? a : b);

  DateTime maxDateReturn =
      maxDateExpirationInvestments.compareTo(maxDateChart) > 0
          ? maxDateChart
          : maxDateExpirationInvestments;

  List<IndexAndDate> listIndexAndDate = [];

  listTreasury.map((e) => e.indexName).forEach((element) {
    listIndexAndDate.add(IndexAndDate(
        indexName: element, maxDate: listTreasury.first.dateLastUpdate));
  });

  //Adiona datas faltantes entre a previsão e o real com o ultimo valor do real.
  listDaily = await addProspectValue(
      listIndexAndDate, listDaily, maxDateReturn, holidays, lastValueIndex);

  listDaily = List.unmodifiable(listDaily);

  List<InterestAndNDays> listDailyThisElementGrouped = [];
  listTreasury.forEach(
    (element) {
      dates.forEach(
        (date) {
          listDailyThisElementGrouped = [];

          DateTime expirationDate;

          //Limpa dados
          List<IndexDaily> listDailyThisElement = [];

          if (element.treasuryBondName
              .toUpperCase()
              .contains("JUROS SEMESTRAIS")) {
            //Os cupons são considerados de acordo com a data do vencimento do titulo
            //Será considerado um cupom na data e 6 meses após.
            //Só serão utilizados o dia e mes do vencimento
            DateTime lastCupom = element.expirationDate;

            expirationDate = DateTime(
              element.expirationDate.year,
              element.expirationDate.month,
              element.expirationDate.day,
            );

            double yearInterest =
                element.treasuryBondName.contains("IPCA") ? 0.06 : 0.1;

            double cupomUnitValue = element.treasuryBondName.contains("IPCA") ?
                lastValueIndex.firstWhere((element) => element.indexName == "VNA IPCA").value * (pow(1 + yearInterest, 1 / 2) - 1) :
                (pow(1 + yearInterest, 1 / 2) - 1) * 1000;

            List<double> cupomArray = [];

            DateTime cupom =
                DateTime(lastCupom.year, lastCupom.month, lastCupom.day);

            List<DateTime> cupomDates = [];

            while (cupom.difference(today).inDays >= 0) {
              if (DateTime(cupom.year, cupom.month - 6, cupom.day)
                          .difference(date)
                          .inDays <=
                      0 &&
                  cupom.difference(today).inDays >= 0) {
                cupomDates.add(cupom);
              }

              cupom = DateTime(cupom.year, cupom.month - 6, cupom.day);
            }

            // int i = 1;

            List<InterestAndPeriod> interestAndPeriod = [];

            DateTime dateRef = today;

            cupomDates.reversed.forEach(
              (dateCupom) {
                // if (dateCupom.difference(date).inDays <= 0 || i == cupomDates.length) {
                //   i++;
                  DateTime dateEnd =
                      dateCupom.difference(date).inDays >= 0 ? date: dateCupom;

                  List<InterestAndNDays> interestAndDays = [];

                  interestAndDays.addAll(
                    listPreFixedInterest(
                      dateRef,
                      dateEnd,
                      element.fixedInterestValueSell / 100,
                      holidays,
                      workingDaysList,
                    ),
                  );

                  if (element.treasuryBondName.contains("IPCA")) {
                    expirationDate = DateTime(element.expirationDate.year,
                        element.expirationDate.month, 1);

                    DateTime thisDate = date.compareTo(expirationDate) <= 0
                        ? date
                        : expirationDate;
                    listDailyThisElement = [];
                    listDailyThisElement.addAll(listDaily.where((item) =>
                        item.indexName == element.indexName &&
                        item.date.compareTo(dateRef) >=
                            0 &&
                        item.date.compareTo(thisDate) < 0));


                    List<double> values = listDailyThisElement.map((e) => e.value).toSet().toList();
                    listDailyThisElementGrouped = [];
                    int thisWorkingDays;
                    values.forEach((element) {
                      thisWorkingDays = listDailyThisElement
                          .where((elementDaily) => elementDaily.value == element)
                          .length;

                      listDailyThisElementGrouped.add(InterestAndNDays(
                          interestRate: element, workingDays: thisWorkingDays));

                    });
                  }

                  double interestRate = 1;

                  interestAndDays.forEach(
                    (e) {
                      interestRate = interestRate *
                          (pow(
                            1 + e.interestRate,
                            e.workingDays,
                          ));
                    },
                  );
                  listDailyThisElementGrouped.forEach(
                        (item) {
                          interestRate = interestRate * (pow(1 + item.interestRate, item.workingDays));
                  });

                  interestAndPeriod.add(
                    InterestAndPeriod(
                      endDate: dateEnd,
                      initialDate: dateRef,
                      interestRate: interestRate,
                    ),
                  );

                  if (cupomDates.contains(dateEnd)) {
                    double cupomThisPeriod = cupomUnitValue;

                    listDailyThisElementGrouped.forEach(
                      (item) {
                        cupomThisPeriod = cupomThisPeriod * (pow(1 + item.interestRate, item.workingDays));
                      },
                    );
                    cupomArray.add(cupomThisPeriod);
                  }
                  dateRef = dateCupom;
                }
              // },
            );

            TreasurySI object = returnFinalValueTreasuryBondHalfYearPayment(
                date, element, interestAndPeriod, cupomArray, cupomDates);

            listReturn.add(
              TreasuryBondMonthly(
                  id: element.id,
                  date: date,
                  amount: element.quantity * element.unitPriceBuy,
                  indexName: element.indexName,
                  investmentDate: element.investmentDate,
                  expirationDate: element.expirationDate,
                  todayValue: object.unitValue * element.quantity,
                  cupomPayment: object.cupomPayment * element.quantity
              ),
            );
          } else {
            if (element.indexName != "PREFIXADO") {
              //Selic

              DateTime thisDate = date.compareTo(element.expirationDate) <= 0
                  ? date
                  : element.expirationDate;

              listDailyThisElement.addAll(
                listDaily.where((item) =>
                    item.indexName == element.indexName &&
                    item.date.compareTo(element.investmentDate) >= 0 &&
                    item.date.compareTo(thisDate) < 0),
              );
            }

            List<double> values =
                listDailyThisElement.map((e) => e.value).toSet().toList();

            int thisWorkingDays;
            values.forEach((element) {
              thisWorkingDays = listDailyThisElement
                  .where((elementDaily) => elementDaily.value == element)
                  .length;

              listDailyThisElementGrouped.add(InterestAndNDays(
                  interestRate: element, workingDays: thisWorkingDays));
            });

            if (element.fixedInterestValueSell != 0.00) {
              DateTime thisDate = date.compareTo(element.expirationDate) < 0
                  ? date
                  : element.expirationDate;

              List<InterestAndNDays> returnListPreFixedInterest =
                  listPreFixedInterest(
                      element.dateLastUpdate,
                      thisDate,
                      element.fixedInterestValueSell / 100,
                      holidays,
                      workingDaysList);

              listDailyThisElementGrouped.addAll(returnListPreFixedInterest);
            }

            double todayUnitValue = returnFinalValueTreasuryBond(
                date, element, listDailyThisElementGrouped);

            listDailyThisElementGrouped = [];
            listReturn.add(
              TreasuryBondMonthly(
                  id: element.id,
                  date: date,
                  amount: element.quantity * element.unitPriceBuy,
                  indexName: element.indexName,
                  investmentDate: element.investmentDate,
                  expirationDate: element.expirationDate,
                  todayValue: todayUnitValue,
                  cupomPayment: 0,
              ),
            );
          }
        },
      );
    },
  );

  return listReturn;
}
