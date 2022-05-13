import 'dart:math';
import 'package:financial_app/functions/dateFunctions.dart';
import 'package:financial_app/functions/investmentTaxes.dart';
import 'package:financial_app/functions/lists/common_lists.dart';
import 'package:financial_app/models/dto_models/treasuryToBalance.dart';
import 'package:financial_app/models/models/BalanceDeposits.dart';
import 'package:financial_app/models/models/interestAndDays.dart';
import 'package:financial_app/models/models/interestAndPeriod.dart';
import 'package:financial_app/models/models/treasurySI.dart';
import 'package:financial_app/models/table_models/fixedInterest.dart';
import 'package:financial_app/models/table_models/workingDaysByYear.dart';

List<InterestAndNDays> listPreFixedInterest(
    DateTime initialDate,
    DateTime finalDate,
    double interestYearRate,
    List<DateTime> holidays,
    List<WorkingDaysByYear> workingDaysList) {
  int nWorkingDays = 0;
  List<InterestAndNDays> returnList = [];

  for (int year = initialDate.year; year <= finalDate.year; year++) {
    var obj = workingDaysList.where((element) => element.year == year);

    int nWorkingDaysYear = obj.length == 0 ? 252 : obj.first.workingDays;

    if (initialDate.year == finalDate.year) {
      nWorkingDays = returnWorkingDays(initialDate, finalDate, holidays);
    } else if (year == initialDate.year) {
      nWorkingDays = returnWorkingDays(
          initialDate, DateTime(initialDate.year, 12, 31), holidays);
    } else if (year == finalDate.year) {
      nWorkingDays = returnWorkingDays(
              DateTime(finalDate.year, 1, 1), finalDate, holidays) +
          1;
    } else {
      nWorkingDays = returnWorkingDays(
              DateTime(year, 1, 1), DateTime(year, 12, 31), holidays) +
          1;
    }

    //Calcula rendimento diario
    num interestDaily =
        pow((1.00 + interestYearRate), (1.00 / nWorkingDaysYear)) - 1;
    returnList.add(InterestAndNDays(
        workingDays: nWorkingDays,
        interestRate: interestDaily));
  }

  return returnList;
}

double returnFinalValueFixedInterest(DateTime date, FixedInterest value,
    List<InterestAndNDays> listInterestDaily) {
  double incomeTaxDeflation = 1.00;
  double iofDeflation = 1.00;

  double finalValue = value.amount;
  var indexInfo = CommonLists.fixedInterestTypeList
      .firstWhere((element) => element.id == value.typeFixedInterestId);

  if (indexInfo.incomeTax == 1) {
    DateTime lastDate =
        date.compareTo(value.expirationDate) > 0 ? value.expirationDate : date;

    double iofValue = getIofValue(value.investmentDate, lastDate);
    iofDeflation -= iofValue;
    double incomeTaxValue = getIncomeTax(value.investmentDate, lastDate);
    incomeTaxDeflation -= incomeTaxValue;
  }

  listInterestDaily.forEach((element) {
    double interestRate = 1.00 + element.interestRate;
    finalValue = finalValue * pow((interestRate), element.workingDays);
  });

  //É necessário verificar se o valor é maior por causa do Tesouro Direto
  double finalValueAfterTax = value.amount +
      (finalValue - value.amount) * incomeTaxDeflation * (iofDeflation);

  return finalValueAfterTax;
}

double returnFinalValueTreasuryBond(DateTime date, TreasuryToBalance value,
    List<InterestAndNDays> listInterestDaily) {
  double incomeTaxDeflation = 1.00;
  double iofDeflation = 1.00;

  double finalValue = value.quantity * value.unitPriceSell;

  DateTime lastDate =
      date.compareTo(value.expirationDate) > 0 ? value.expirationDate : date;

  double iofValue = getIofValue(value.investmentDate, lastDate);
  iofDeflation -= iofValue;
  double incomeTaxValue = getIncomeTax(value.investmentDate, lastDate);
  incomeTaxDeflation -= incomeTaxValue;

  listInterestDaily.forEach((element) {
    double interestRate = 1.00 + element.interestRate;
    finalValue = finalValue * pow((interestRate), element.workingDays);
  });

  double valueAfterIncomeTax = value.quantity * value.unitPricePurchase +
      (finalValue - value.quantity * value.unitPricePurchase) *
          incomeTaxDeflation *
          (iofDeflation);
  //É necessário verificar se o valor é maior por causa do Tesouro Direto
  double finalValueAfterTax =
      valueAfterIncomeTax > value.quantity * value.unitPricePurchase
          ? valueAfterIncomeTax
          : valueAfterIncomeTax;

  return finalValueAfterTax;
}

TreasurySI returnFinalValueTreasuryBondHalfYearPayment(DateTime date, TreasuryToBalance value,
    List<InterestAndPeriod> listInterestPeriod, List<double> cupomArray, List<DateTime> cupomDates) {
  double incomeTaxDeflation = 1.00;
  double iofDeflation = 1.00;

  double finalValue = value.quantity * value.unitPriceSell;

  DateTime lastDate =
  date.compareTo(value.expirationDate) > 0 ? value.expirationDate : date;

  double iofValue = getIofValue(value.investmentDate, lastDate);
  iofDeflation -= iofValue;
  double incomeTaxValue = getIncomeTax(value.investmentDate, lastDate);
  incomeTaxDeflation -= incomeTaxValue;
  double cupomPayment = 0;
  int i = 0;
  listInterestPeriod.forEach((element) {
    if(cupomDates.contains(element.initialDate)){
      finalValue -= cupomArray[i];
      if(lastDate.month == element.initialDate.month && lastDate.year == element.initialDate.year)
      {
        cupomPayment = cupomArray[i]*(1-getIncomeTax(value.investmentDate, element.initialDate))*(1-getIofValue(value.investmentDate, element.initialDate));
      }
      else {
        cupomPayment = 0;
      }
        i++;
    }
    else{
      cupomPayment = 0 ;
    }

    finalValue = finalValue * element.interestRate;
  });


  double valueAfterIncomeTax = value.quantity * value.unitPricePurchase +
      (finalValue - value.quantity * value.unitPricePurchase) *
          incomeTaxDeflation *
          (iofDeflation);
  //É necessário verificar se o valor é maior por causa do Tesouro Direto
  double finalValueAfterTax =
  valueAfterIncomeTax > value.quantity * value.unitPricePurchase
      ? valueAfterIncomeTax
      : valueAfterIncomeTax;

  return TreasurySI(cupomPayment: cupomPayment, unitValue: finalValueAfterTax,);
}


double returnFinalValueBalance(DateTime lastDate, BalanceDeposits value,
    List<InterestAndNDays> listInterestDaily) {
  double incomeTaxDeflation = 1.00;
  double iofDeflation = 1.00;

  double finalValue = value.value;


  double iofValue = getIofValue(value.initialDate, lastDate);
  iofDeflation -= iofValue;
  double incomeTaxValue = 0.225;
  incomeTaxDeflation -= incomeTaxValue;


  listInterestDaily.forEach((element) {
    double interestRate = 1.00 + element.interestRate;
    finalValue = finalValue * pow((interestRate), element.workingDays);
  });

  //É necessário verificar se o valor é maior por causa do Tesouro Direto
  double finalValueAfterTax = value.value +
      (finalValue - value.value) * incomeTaxDeflation * (iofDeflation);

  return finalValueAfterTax;
}

