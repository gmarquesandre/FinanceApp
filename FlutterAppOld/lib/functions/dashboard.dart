import 'dart:math';
import 'package:financial_app/components/globalVariables.dart';
import 'package:financial_app/database/dao/holidays_dao.dart';
import 'package:financial_app/database/dao/indexDailyValue_dao.dart';
import 'package:financial_app/database/dao/indexLastValue.dart';
import 'package:financial_app/database/dao/loanDao.dart';
import 'package:financial_app/database/dto/balance_dto.dart';
import 'package:financial_app/functions/assetFutureValue.dart';
import 'package:financial_app/functions/fundFutureValue.dart';
import 'package:financial_app/functions/futureValues.dart';
import 'package:financial_app/functions/futureValuesFixedInterest.dart';
import 'package:financial_app/functions/futureValuesTreasuryBond.dart';
import 'package:financial_app/functions/lists/common_lists.dart';
import 'package:financial_app/functions/prospectValueDaily.dart';
import 'package:financial_app/functions/updateData.dart';
import 'package:financial_app/models/models/IndexesAndDate.dart';
import 'package:financial_app/models/models/fgtsBalance.dart';
import 'package:financial_app/models/models/fundMonthly.dart';
import 'package:financial_app/models/models/interestAndDays.dart';
import 'package:financial_app/models/models/loanMonthly.dart';
import 'package:financial_app/models/models/BalanceDeposits.dart';
import 'package:financial_app/models/table_models/holiday.dart';
import 'package:financial_app/models/table_models/indexDailyInterest.dart';
import 'package:financial_app/models/table_models/loan.dart';
import 'package:financial_app/models/dto_models/spendingAndIncome.dart';
import 'package:financial_app/models/dto_models/balanceMonth.dart';
import 'package:financial_app/models/models/fixedInterestMonthly.dart';
import 'package:financial_app/models/models/treasuryBondMonthly.dart';
import 'package:flutter/material.dart';
import 'package:shared_preferences/shared_preferences.dart';

final LoanDao _daoLoan = LoanDao();

final HolidaysDao _daoHolidays = HolidaysDao();

Future<List<BalanceMonth>> getDashboardData(BuildContext context) async {
  try {
    await updateData();
  } catch (error) {
    debugPrint("Erro ao capturar dados");
  }
  int nMonths = 14;

  DateTime dateInit = DateTime(DateTime.now().year, DateTime.now().month + 1, 1)
      .subtract(Duration(days: 1));

  DateTime dateFinal =
      DateTime(DateTime.now().year, DateTime.now().month + nMonths, 1);
  List<BalanceMonth> resultMonthly = [];
  List<DateTime> dates = [];

  for (DateTime i = dateInit;
      i.compareTo(dateFinal) < 0;
      i = DateTime(i.year, i.month + 2, 1).subtract(Duration(days: 1))) {
    dates.add(i);
  }

  //Tesouro direto
  List<TreasuryBondMonthly> listTreasury = [];
  await getTreasuryBondFutureValues(context, dates)
      .then((value) => listTreasury = value);

  //Renda Fixa
  List<FixedInterestMonthly> listFixedInterestCurrent = [];

  await getFixedInterestFutureValues(dates)
      .then((value) => listFixedInterestCurrent = value);

  final SharedPreferences prefs = await SharedPreferences.getInstance();

  List<FundMonthly> listFundMonthly = await getFundFutureValue(context, dates);

  var listAssetMonthly = await getAssetFutureValue(context, dates);

  List<LoanMonthly> loan = await getLoanMonthly(dates);

  //Gastos e Receitas
  List<SpendingAndIncome> resultOpenByRecurrence =
      await getIncomeAndSpendingMonthly(nMonths);

  double spendingTotal = 0;
  double incomeTotal = 0;

  //Saldo Corrente
  List<BalanceDeposits> currentBalance = [];
  final double currentBalanceValue =
      prefs.getDouble(GlobalVariables.currentBalanceValue) ?? 0;

  currentBalance.add(
    BalanceDeposits(
      initialDate: DateTime.fromMillisecondsSinceEpoch(
          prefs.getInt(GlobalVariables.dateEpochLastUpdateBalanceValue) ?? 0),
      value: currentBalanceValue,
    ),
  );

  //FGTS

  List<FgtsBalance> fgtsList = await getFgts(dates);

  double loanTotal = 0;

  for (var date in dates) {
    //Ações
    double totalStockValue = listAssetMonthly
        .where((item) => item.date == date)
        .map((e) => e.totalValue)
        .fold(0, (previous, current) => previous + current);

    //Fundos
    double totalFundValue = listFundMonthly
        .where((item) => item.date == date)
        .map((e) => e.totalValue)
        .fold(0, (previous, current) => previous + current);

    //Empréstimo e Financiamento
    double loanMonth = loan
        .where((element) => element.date == date)
        .map((e) => e.value)
        .fold(0, (previous, current) => previous + current);

    //Gastos e Receitas
    var monthValues = resultOpenByRecurrence.where((element) =>
        element.date!.month == date.month && element.date!.year == date.year);

    double spendingMonth = monthValues
        .where((element) => element.isSpending == 1)
        .map((e) => e.amount)
        .fold(0, (previous, current) => previous + current);

    double spendingMonthRequired = monthValues
        .where((a) => a.isRequiredSpending == 1)
        .map((e) => e.amount)
        .fold(0, (previous, current) => previous + current);

    double incomeMonth = monthValues
        .where((element) => element.isSpending == 0)
        .map((e) => e.amount)
        .fold(0, (previous, current) => previous + current);

    double result = (incomeMonth - spendingMonth);
    spendingTotal += spendingMonth;
    incomeTotal += incomeMonth;

    //FGTS
    double fgtsValue = fgtsList.length == 0
        ? 0
        : fgtsList.firstWhere((element) => element.date == date).value;

    double fgtsWithdrawTotal = fgtsList.length == 0
        ? 0
        : fgtsList
            .firstWhere((element) => element.date == date)
            .withdrawValueTotal;

    //Renda Fixa
    double fixedInterestNotLiquidityMonth = listFixedInterestCurrent
        .where((element) =>
            element.liquidityOnExpiration == 1 &&
            element.date == date &&
            element.expirationDate.compareTo(date) > 0)
        .map((e) => e.todayValue)
        .fold(0, (previous, current) => previous + current);

    double fixedInterestLiquidityMonth = listFixedInterestCurrent
        .where((element) =>
            element.liquidityOnExpiration == 0 &&
            element.date == date &&
            date.difference(element.expirationDate).inDays < 0)
        .map((e) => e.todayValue)
        .fold(0, (previous, current) => previous + current);

    double fixedInterestExpiredMonth = 0;

    double treasuryBondValueExpired = listTreasury
        .where((element) =>
            element.date == date && element.expirationDate.compareTo(date) < 0)
        .map((e) => e.todayValue)
        .fold(0, (previous, current) => previous + current);

    double listTreasuryMonth = listTreasury
        .where((element) =>
            element.date == date && element.expirationDate.compareTo(date) >= 0)
        .map((e) => e.todayValue)
        .fold(0, (previous, current) => previous + current);

    //Pagamento de Cupom
    double cupomPayments = listTreasury
        .where((element) => element.date == date && element.cupomPayment >= 0)
        .map((e) => e.cupomPayment)
        .fold(0, (previous, current) => previous + current);

    //Conta Corrente
    List<BalanceDeposits> balanceList = [];
    balanceList.addAll(currentBalance);

    //Conta Corrente
    listFixedInterestCurrent
        .where((element) =>
            element.date == date &&
            date.difference(element.expirationDate).inDays > 0 &&
            element.expirationDate.month == date.month &&
            element.expirationDate.year == date.year)
        .forEach(
      (element) {
        balanceList.add(
          BalanceDeposits(
              initialDate: element.expirationDate, value: element.todayValue),
        );
      },
    );

    if (fgtsList.length > 0) {
      debugPrint("Entrou");
      double fgtsWithdrawMonth = fgtsList
          .firstWhere((element) => element.date == date)
          .withdrawValueMonth;
      debugPrint("Valor FGTS Saque ${fgtsWithdrawMonth.toString()}");

      debugPrint(balanceList.length.toString());

      balanceList.add(
        BalanceDeposits(
            initialDate: DateTime(date.year, date.month, 10),
            value: fgtsWithdrawMonth),
      );

      debugPrint(balanceList.length.toString());
    }

    listTreasury
        .where((element) =>
            element.date == date &&
            element.expirationDate.compareTo(date) <= 0 &&
            element.expirationDate.month == date.month &&
            element.expirationDate.year == date.year)
        .forEach(
      (element) {
        balanceList.add(
          BalanceDeposits(
            initialDate: element.expirationDate,
            value: element.todayValue,
          ),
        );
      },
    );

    debugPrint(date.toString());
    debugPrint("Antes");
    balanceList.forEach((b) {
      debugPrint(b.value.toString());
    });

    debugPrint("result " + result.toString());
    //Provisório. Caso o resultado seja negativo terá que subtrair de outros depósitos.
    if (result > 0) {
      balanceList.add(
        BalanceDeposits(
          initialDate: date,
          value: result,
        ),
      );
    } else if (result < 0) {
      double resultRemaining = result;

      int lengthBalance = balanceList.length;

      for (int i = 0; i <= lengthBalance - 1; i++) {
        if (i == lengthBalance - 1) {
          balanceList[i].value += resultRemaining;
        } else {
          if (balanceList[i].value < resultRemaining.abs()) {
            resultRemaining += balanceList[i].value;
            balanceList[i].value = 0;
          } else if (balanceList[i].value >= resultRemaining.abs()) {
            balanceList[i].value += resultRemaining;
            resultRemaining = 0;
            break;
          }
        }
      }
    }
    debugPrint("Depois");
    balanceList.forEach((b) {
      debugPrint(b.value.toString());
    });

    double currentBalanceValue = 0;

    //Está na tela de conta corrente
    bool updateValueWithCdi =
        prefs.getBool(GlobalVariables.updateCurrentValueWithCdi) ?? false;

    balanceList.add(
      BalanceDeposits(
        initialDate: date,
        value: cupomPayments,
      ),
    );

    if (updateValueWithCdi) {
      //Adicionar variavel de juros quando há dividas
      List<BalanceDepositsFutureValue> balanceCurrentValue =
          await updateBalanceValues(date, balanceList.toList());

      currentBalanceValue = balanceCurrentValue
          .map((e) => e.endValue)
          .fold(0, (previous, current) => previous + current);

      //Atualiza para reduzir os calculos e calcular corretamente em caso de divida
      currentBalance = [];
      currentBalance
          .add(BalanceDeposits(initialDate: date, value: currentBalanceValue));
    } else {
      currentBalanceValue = balanceList
          .map((e) => e.value)
          .fold(0, (previous, current) => previous + current);

      debugPrint("Tamanho agora " + balanceList.length.toString());

      balanceList.forEach((element) {
        debugPrint(element.value.toString());
      });

      debugPrint("Valor " + currentBalanceValue.toString());

      currentBalance = [];
      currentBalance
          .add(BalanceDeposits(initialDate: date, value: currentBalanceValue));
    }

    loanTotal = loanMonth + loanTotal;

    debugPrint("Uai " + currentBalanceValue.toString());

    double totalPatrimony = loanTotal +
        totalStockValue +
        currentBalanceValue +
        fixedInterestLiquidityMonth +
        fixedInterestNotLiquidityMonth +
        fixedInterestExpiredMonth +
        totalFundValue +
        fgtsValue +
        listTreasuryMonth;

    resultMonthly.add(
      BalanceMonth(
          date: date,
          spendingCumulated: spendingTotal,
          incomeCumulated: incomeTotal,
          income: incomeMonth,
          loan: loanMonth,
          spending: spendingMonth,
          spendingRequired: spendingMonthRequired,
          incomeLessSpending: result,
          stocksValue: totalStockValue,
          currentBalance: currentBalanceValue,
          fixedIncomeValueFreeLiquidity: fixedInterestLiquidityMonth,
          fixedIncomeValueLiquidityOnExpire: fixedInterestNotLiquidityMonth,
          fundsValue: totalFundValue,
          fgtsValue: fgtsValue,
          fgtsWithdraw: fgtsWithdrawTotal,
          treasuryBondValue: listTreasuryMonth,
          cupomPayments: cupomPayments,
          treasuryBondValueExpired: treasuryBondValueExpired,
          totalAvailable: -spendingTotal -
              loanTotal +
              currentBalanceValue +
              fixedInterestLiquidityMonth +
              fixedInterestExpiredMonth,
          totalPatrimonyLiquid: loanTotal +
              totalStockValue +
              listTreasuryMonth +
              currentBalanceValue +
              fixedInterestLiquidityMonth +
              totalFundValue +
              fixedInterestExpiredMonth,
          totalPatrimony: totalPatrimony),
    );
  }

  return resultMonthly;
}

Future<List<BalanceDepositsFutureValue>> updateBalanceValues(
    DateTime date, List<BalanceDeposits> balanceList) async {
  //Transforma Indices mensais em diarios

  if (balanceList.length == 0) return [];

  final IndexLastValueDao _daoLastValueIndex = IndexLastValueDao();

  List<IndexDaily> lastValueIndex = await _daoLastValueIndex.findAll();

  DateTime minDateInvestment = balanceList
      .map((e) => e.initialDate)
      .reduce((a, b) => a.isBefore(b) ? a : b);

  List<Holiday> holidaysList = await _daoHolidays.findAll(
      minDateInvestment.subtract(const Duration(days: 1)),
      date.add(const Duration(days: 1)));

  List<DateTime> holidays = holidaysList.map((e) => e.date).toList();

  final IndexDailyValueDao _daoIndexDaily = IndexDailyValueDao();
  List<IndexAndDate> listIndexAndDate = await _daoIndexDaily.getIndexesList();
  List<IndexDaily> listDaily = await _daoIndexDaily.findAll();

  listDaily = listDaily.where((element) => element.indexName == "CDI").toList();

  listIndexAndDate.add(IndexAndDate(indexName: "CDI", maxDate: date));

  //Adiona datas faltantes entre a previsão e o real com o ultimo valor do real.
  listDaily = await addProspectValue(
      listIndexAndDate, listDaily, date, holidays, lastValueIndex);

  List<BalanceDepositsFutureValue> balanceReturn = [];

  balanceList.forEach(
    (element) {
      List<IndexDaily> listDailyThisElement = [];
      List<InterestAndNDays> listDailyThisElementGrouped = [];

      listDailyThisElement.addAll(listDaily.where((item) =>
          item.indexName == "CDI" &&
          item.date.compareTo(element.initialDate) >= 0 &&
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
          listDailyThisElementGrouped.add(
            InterestAndNDays(
              interestRate: element,
              workingDays: thisWorkingDays,
            ),
          );
        },
      );

      double endValue =
          returnFinalValueBalance(date, element, listDailyThisElementGrouped);

      balanceReturn.add(
        BalanceDepositsFutureValue(
          initialDate: element.initialDate,
          initialValue: element.value,
          endDate: date,
          endValue: endValue,
        ),
      );
    },
  );

  return balanceReturn;
}

Future<List<FgtsBalance>> getFgts(List<DateTime> dates) async {
  SharedPreferences prefs = await SharedPreferences.getInstance();

  final double totalAccountBalanceFGTS =
      prefs.getDouble(GlobalVariables.totalAccountBalanceFGTS) ?? 0;

  final double valueGrossIncome =
      prefs.getDouble(GlobalVariables.valueGrossIncome) ?? 0;

  if (totalAccountBalanceFGTS == 0 && valueGrossIncome == 0) return [];

  final bool birthdayWithDrawFGTS =
      prefs.getBool(GlobalVariables.birthdayWithDrawFGTS) ?? false;

  final DateTime fgtsDateLastUpdate = DateTime.fromMillisecondsSinceEpoch(
      prefs.getInt(GlobalVariables.dateLastUpdateFGTS) ?? 0);

  final int monthWithdrawFGTS =
      prefs.getInt(GlobalVariables.monthWithdrawFGTS) ?? 0;

  List<FgtsBalance> list = [];

  double interestFgtsYear = 0.03;

  num interestFgtsMonth = roundDown(pow(1 + interestFgtsYear, 1 / 12) - 1, 6);

  double monthFgtsDeposit = roundDown(0.08 * valueGrossIncome, 2);

  dates.forEach(
    (date) {
      double withdrawValueTotal = 0;
      double withdrawValueMonth = 0;
      double value = totalAccountBalanceFGTS;

      for (DateTime iterDate = DateTime(
        fgtsDateLastUpdate.year,
        fgtsDateLastUpdate.month + 1,
        1,
      );
          iterDate.compareTo(date) < 0;
          iterDate =
              DateTime(iterDate.year, iterDate.month + 1, iterDate.day),) {
        if (iterDate.month == monthWithdrawFGTS && birthdayWithDrawFGTS) {
          var withdrawInfo = CommonLists.fgtsAnniversaryWithdrawList.firstWhere(
              (item) => value >= item.minvalue && value <= item.maxValue);

          double withdrawValue = withdrawInfo.withDrawPercentage * value +
              withdrawInfo.additionalAmount;
          withdrawValueMonth = withdrawValue;
          value = value - withdrawValue;

          withdrawValueTotal = withdrawValueTotal + withdrawValue;
        } else {
          withdrawValueMonth = 0;
        }

        value = value * (1 + interestFgtsMonth) +
            (iterDate.month != 12 ? monthFgtsDeposit : 2 * monthFgtsDeposit);
      }
      list.add(
        FgtsBalance(
          date: date,
          value: value,
          withdrawValueMonth: withdrawValueMonth,
          withdrawValueTotal: withdrawValueTotal,
        ),
      );
    },
  );

  return list;
}

Future<List<SpendingAndIncome>> getIncomeAndSpendingMonthly(int nMonths) async {
  final SpendingAndIncomeDto _daoBalance = SpendingAndIncomeDto();

  List<SpendingAndIncome> listIncomeAndSpending = await _daoBalance.findAll();

  List<SpendingAndIncome> resultOpenByRecurrence = [];

  listIncomeAndSpending.forEach((element) {
    //Uma vez
    if (element.recurrenceId == 1) {
      element.date = element.initialDate;
      resultOpenByRecurrence.add(SpendingAndIncome.copyWith(element));
    }
    //Diaria e Semanal
    else if ([2, 3, 4, 5].contains(element.recurrenceId)) {
      int daysSpanTime = 0;
      int monthsSpanTime = 0;
      int yearSpanTime = 0;
      //Diario
      if (element.recurrenceId == 2)
        daysSpanTime = 1;
      //Semanal
      else if (element.recurrenceId == 3)
        daysSpanTime = 7;
      //Mensal
      else if (element.recurrenceId == 4) {
        monthsSpanTime = 1;
        element.initialDate =
            DateTime(element.initialDate.year, element.initialDate.month, 1);
      }
      //Anual
      else if (element.recurrenceId == 5) yearSpanTime = 1;

      DateTime endDate;
      if (element.timesRecurrence > 0)
        endDate = DateTime(
            element.initialDate.year +
                (yearSpanTime > 0
                    ? yearSpanTime * (element.timesRecurrence - 1)
                    : 0),
            element.initialDate.month +
                (monthsSpanTime > 0
                    ? monthsSpanTime * (element.timesRecurrence - 1)
                    : 0),
            element.initialDate.day +
                (daysSpanTime > 0
                    ? daysSpanTime * (element.timesRecurrence - 1)
                    : 0));
      else if (element.isEndless == 1)
        endDate = DateTime(element.initialDate.year,
            element.initialDate.month + nMonths + 1, 1);
      else
        endDate = element.endDate;

      for (DateTime date = element.initialDate;
          date.compareTo(endDate) <= 0;
          date = DateTime(date.year + yearSpanTime, date.month + monthsSpanTime,
              date.day + daysSpanTime)) {
        element.date = date;
        resultOpenByRecurrence.add(SpendingAndIncome.copyWith(element));
      }
    }
  });

  return resultOpenByRecurrence;
}

double roundDown(double value, int precision) {
  final isNegative = value.isNegative;
  final mod = pow(10.0, precision);
  final roundDown = (((value.abs() * mod).floor()) / mod);
  return isNegative ? -roundDown : roundDown;
}

Future<List<LoanMonthly>> getLoanMonthly(List<DateTime> dates) async {
  List<Loan> loanList = await _daoLoan.findAll();

  if (loanList.length == 0) return [];

  List<LoanMonthly> list = [];

  loanList.forEach(
    (element) {
      //Tabela Price
      DateTime dateFinal =
          DateTime(element.date.year, element.date.month + element.months, 1);
      double interestRateMonth =
          pow(1 + element.interestRate / 100, 1 / 12) - 1;

      if (element.paymentType == 1) {
        double value = (element.totalValue *
            (((pow((1 + interestRateMonth), element.months) *
                    interestRateMonth)) /
                (pow((1 + interestRateMonth), element.months) - 1)));

        dates.forEach(
          (date) {
            if (dateFinal.compareTo(date) >= 0 &&
                date.compareTo(element.date) >= 0) {
              list.add(
                LoanMonthly(
                  name: element.name,
                  value: value,
                  date: date,
                ),
              );
            }
          },
        );
      }
      //Tabela SAC
      else if (element.paymentType == 2) {
        dates.forEach(
          (date) {
            if (dateFinal.compareTo(date) >= 0 &&
                date.compareTo(element.date) >= 0) {
              int monthsDifference = (date.year * 12 + date.month) -
                  (element.date.year * 12 + element.date.month);

              double amortization = element.totalValue / element.months;

              double interestPayment = interestRateMonth *
                  element.totalValue *
                  (1 - monthsDifference / element.months);

              double value = amortization + interestPayment;

              list.add(
                LoanMonthly(
                  name: element.name,
                  value: value,
                  date: date,
                ),
              );
            }
          },
        );
      }
    },
  );

  return list;
}
