import 'package:financial_app/functions/lists/common_lists.dart';

double getIofValue(DateTime initialDate, DateTime finalDate){

  var daysSinceInvestment =
      finalDate
          .difference(initialDate)
          .inDays;

  if(daysSinceInvestment >= 30)
    return 0.00;
  else
    return CommonLists.iofValue.firstWhere((element) => element.day ==
        daysSinceInvestment).tax;
}
double getIncomeTax(DateTime initialDate, DateTime finalDate){

  // Tabela IR
  //   0 - 180 dias = 22.5%
  // 181 - 360 dias = 20.0%
  // 361 - 720 dias = 17.5%
  // 720 -  ∞  dias = 15.0%

  double incomeTaxValue = 0;

  var daysSinceInvestment =
      finalDate
          .difference(initialDate)
          .inDays;

  if (daysSinceInvestment <= 180) {
    incomeTaxValue = 0.225;
  } else if (daysSinceInvestment <= 360) {
    incomeTaxValue = 0.20;
  } else if (daysSinceInvestment <= 720) {
    incomeTaxValue = 0.175;
  } else {
    incomeTaxValue = 0.15;
  }
  return incomeTaxValue;
}
