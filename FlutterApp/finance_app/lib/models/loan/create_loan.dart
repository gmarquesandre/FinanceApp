class CreateLoan {
  String name;
  DateTime initialDate;
  int monthsPayment;
  double loanValue;
  double interestRate;
  int type;

  CreateLoan({
    required this.name,
    required this.initialDate,
    required this.monthsPayment,
    required this.loanValue,
    required this.interestRate,
    required this.type,
  });

  Map<String, dynamic> toJson() => {
        'name': name,
        'initialDate': initialDate.toString(),
        'monthsPayment': monthsPayment,
        'loanValue': loanValue,
        'interestRate': interestRate,
        'type': type,
      };
}
