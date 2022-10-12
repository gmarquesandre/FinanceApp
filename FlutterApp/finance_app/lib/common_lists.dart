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
        id: 1,
        type: "SAC",
        name: "Amortização Constante (Sac)",
        description:
            "*A parcela é descrescente com o tempo\nNo inicio a parcela é maior, porém o valor total é menor do que o Price"),
    LoanPaymentType(
        id: 2,
        type: "PRICE",
        name: "Parcela Constante (Price)",
        description:
            "*Todos pagamentos são iguais do inicio ao fim\nO valor da parcela é menor, porém o pagamento total é maior do que o Sac"),
  ];
}
