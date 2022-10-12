class UpdateLoan {
  String id;
  String name;
  DateTime initialDate;
  int monthsPayment;
  double loanValue;
  double interestRate;
  int type;

  UpdateLoan({
    required this.id,
    required this.name,
    required this.initialDate,
    required this.monthsPayment,
    required this.loanValue,
    required this.interestRate,
    required this.type,
  });

  Map<String, dynamic> toJson() => {
        'id': id,
        'name': name,
        'initialDate': initialDate.toString(),
        'monthsPayment': monthsPayment,
        'loanValue': loanValue,
        'interestRate': interestRate,
        'type': type,
      };
}
