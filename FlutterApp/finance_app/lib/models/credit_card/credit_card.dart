class CreditCard {
  int id;
  String name;
  int invoiceClosingDay;
  int invoicePaymentDay;

  CreditCard(
      {required this.id,
      required this.name,
      required this.invoiceClosingDay,
      required this.invoicePaymentDay});

  CreditCard.fromJson(Map<String, dynamic> json)
      : id = json['id'],
        name = json['name'],
        invoiceClosingDay = json['invoiceClosingDay'],
        invoicePaymentDay = json['invoicePaymentDay'];
}
