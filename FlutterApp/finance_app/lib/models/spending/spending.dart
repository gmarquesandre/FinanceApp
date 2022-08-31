import 'package:finance_app/models/category/category.dart';
import 'package:finance_app/models/credit_card/credit_card.dart';

class Spending {
  int id;
  String name;
  int recurrence;
  int payment;
  CreditCard? creditCard;
  String recurrenceDisplayName;
  String paymentName;
  Category? category;
  bool isRequired;
  double amount;
  DateTime initialDate;
  DateTime? endDate;
  bool isEndless;
  int timesRecurrence;

  Spending(
      {required this.id,
      required this.name,
      required this.recurrence,
      required this.recurrenceDisplayName,
      required this.creditCard,
      required this.paymentName,
      required this.payment,
      required this.isRequired,
      required this.category,
      required this.amount,
      required this.initialDate,
      required this.endDate,
      required this.isEndless,
      required this.timesRecurrence});

  Spending.fromJson(Map<String, dynamic> json)
      : id = json['id'],
        name = json['name'].toString(),
        recurrence = json['recurrence'],
        recurrenceDisplayName = json['recurrenceDisplayName'].toString(),
        amount = json['amount'].toDouble(),
        initialDate = DateTime.tryParse(json['initialDate'].toString())!,
        endDate = DateTime.tryParse(json['endDate'].toString()),
        isEndless = json['isEndless'],
        timesRecurrence = json['timesRecurrence'],
        category = json['category'] == null
            ? null
            : Category.fromJson(json['category']),
        creditCard = json['creditCard'] == null
            ? null
            : CreditCard.fromJson(json['creditCard']),
        isRequired = json['isRequired'],
        payment = json['payment'],
        paymentName = json['paymentName'];
}
