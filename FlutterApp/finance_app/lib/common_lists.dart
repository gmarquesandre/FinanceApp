import 'package:finance_app/models/payment_type.dart';
import 'package:finance_app/models/recurrence.dart';

class CommonLists {
  static List<Recurrence> recurrenceList = [
    Recurrence(id: 1, name: "Uma Vez"),
    Recurrence(id: 2, name: "Diário"),
    Recurrence(id: 3, name: "Semanal"),
    Recurrence(id: 4, name: "Mensal"),
    Recurrence(id: 5, name: "Anual"),
  ];

  static List<LoanPaymentType> loanPaymentType = [
    LoanPaymentType(
        id: 0,
        type: "SAC",
        name: "Amortização Constante (Sac)",
        description:
            "*A parcela é descrescente com o tempo\nNo inicio a parcela é maior, porém o valor total é menor do que o Price"),
    LoanPaymentType(
        id: 1,
        type: "PRICE",
        name: "Parcela Constante (Price)",
        description:
            "*Todos pagamentos são iguais do inicio ao fim\nO valor da parcela é menor, porém o pagamento total é maior do que o Sac"),
  ];

  // static List<Operations> operationType = [
  //   Operations(code: 1, nameCode: "Compra", shortNameCode: "C"),
  //   Operations(code: 2, nameCode: "Venda", shortNameCode: "V"),
  // ];

  // static List<Index> indexList = [
  //   Index(id: 99, name: "", type: ""),
  //   //IPCA E IGPM estão errados no calculo do valor atual, precisa arruimar.
  //   Index(id: 1, name: "IPCA", type: "Monthly"),
  //   Index(id: 2, name: "IGPM", type: "Monthly"),
  //   Index(id: 3, name: "SELIC", type: "Daily"),
  //   Index(id: 4, name: "CDI", type: "Daily"),
  //   // Index(id: 5, name: "Poupança", type: "Monthly"),
  // ];

  // static List<IOF> iofValue = [
  //   IOF(day: 0, tax: 1),
  //   IOF(day: 1, tax: 0.96),
  //   IOF(day: 2, tax: 0.93),
  //   IOF(day: 3, tax: 0.9),
  //   IOF(day: 4, tax: 0.86),
  //   IOF(day: 5, tax: 0.83),
  //   IOF(day: 6, tax: 0.8),
  //   IOF(day: 7, tax: 0.76),
  //   IOF(day: 8, tax: 0.73),
  //   IOF(day: 9, tax: 0.7),
  //   IOF(day: 10, tax: 0.66),
  //   IOF(day: 11, tax: 0.63),
  //   IOF(day: 12, tax: 0.6),
  //   IOF(day: 13, tax: 0.56),
  //   IOF(day: 14, tax: 0.53),
  //   IOF(day: 15, tax: 0.5),
  //   IOF(day: 16, tax: 0.46),
  //   IOF(day: 17, tax: 0.43),
  //   IOF(day: 18, tax: 0.4),
  //   IOF(day: 19, tax: 0.36),
  //   IOF(day: 20, tax: 0.33),
  //   IOF(day: 21, tax: 0.3),
  //   IOF(day: 22, tax: 0.26),
  //   IOF(day: 23, tax: 0.23),
  //   IOF(day: 24, tax: 0.2),
  //   IOF(day: 25, tax: 0.16),
  //   IOF(day: 26, tax: 0.13),
  //   IOF(day: 27, tax: 0.1),
  //   IOF(day: 28, tax: 0.06),
  //   IOF(day: 29, tax: 0.03),
  //   IOF(day: 30, tax: 0),
  // ];

  // static List<FixedInterestType> fixedInterestTypeList = [
  //   // FixedInterestType(id: 1, name: "Poupança", incomeTax: 0),
  //   FixedInterestType(id: 2, name: "CDB", incomeTax: 1),
  //   FixedInterestType(id: 3, name: "LCA", incomeTax: 0),
  //   FixedInterestType(id: 4 ,name: "LCI", incomeTax: 0),
  //   FixedInterestType(id: 5, name: "LC", incomeTax: 1),
  //   // FixedInterestType(id: 6, name: "LF", incomeTax: 1),
  //   // FixedInterestType(id: 7, name: "DPGE", incomeTax: 1),
  //   // FixedInterestType(id: 8, name: "FIDC", incomeTax: 1),
  //   // FixedInterestType(id: 9, name: "CRI", incomeTax: 0),
  //   // FixedInterestType(id: 10, name: "CRA", incomeTax: 0),
  //   // FixedInterestType(id: 11, name: "Debêntures", incomeTax: 0),
  // ];

  // static List<FGTSAnniversaryWithdraw> fgtsAnniversaryWithdrawList = [
  //   FGTSAnniversaryWithdraw(minvalue: 0.00, maxValue: 500.00, withDrawPercentage: 0.50, additionalAmount: 0.00),
  //   FGTSAnniversaryWithdraw(minvalue: 500.01, maxValue: 1000.00, withDrawPercentage: 0.40, additionalAmount: 50.00),
  //   FGTSAnniversaryWithdraw(minvalue: 1000.01, maxValue: 5000.00, withDrawPercentage: 0.30, additionalAmount: 150.00),
  //   FGTSAnniversaryWithdraw(minvalue: 5000.01, maxValue: 10000.00, withDrawPercentage: 0.20, additionalAmount: 650.00),
  //   FGTSAnniversaryWithdraw(minvalue: 10000.01, maxValue: 15000.00, withDrawPercentage: 0.15, additionalAmount: 1150.00),
  //   FGTSAnniversaryWithdraw(minvalue: 15000.01, maxValue: 20000.00, withDrawPercentage: 0.10, additionalAmount: 1900.00),
  //   FGTSAnniversaryWithdraw(minvalue: 20000.01, maxValue: double.maxFinite, withDrawPercentage: 0.05, additionalAmount: 2900.00),
  // ];

  // static List<LoanPaymentType> loanPaymentType = [
  //   LoanPaymentType(id: 1, type: "PRICE", name: "Parcela Constante (Price)", description: "*Todos pagamentos são iguais do inicio ao fim\nO valor da parcela é menor, porém o pagamento total é maior do que o Sac"),
  //   LoanPaymentType(id: 2, type: "SAC", name: "Amortização Constante (Sac)", description: "*A parcela é descrescente com o tempo\nNo inicio a parcela é maior, porém o valor total é menor do que o Price"),
  // ];

}
