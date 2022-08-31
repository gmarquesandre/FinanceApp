class CreateCreditCard {
  String name;
  int invoiceClosingDay;
  int invoicePaymentDay;

  CreateCreditCard(
      {required this.name,
      required this.invoiceClosingDay,
      required this.invoicePaymentDay});

  CreateCreditCard.fromJson(Map<String, dynamic> json)
      : name = json['name'],
        invoiceClosingDay = json['invoiceClosingDay'],
        invoicePaymentDay = json['invoicePaymentDay'];

  Map<String, dynamic> toJson() => {
        'name': name,
        'invoiceClosingDay': invoiceClosingDay,
        'invoicePaymentDay': invoicePaymentDay,
      };
}
