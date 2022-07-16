class Loan {
  int id;
  String name;
  DateTime initialDate;
  String typeDisplayValue;
  int monthsPayment;
  double loanValue;
  double interestRate;
  int type;

  Loan({
    required this.id,
    required this.name,
    required this.initialDate,
    required this.typeDisplayValue,
    required this.monthsPayment,
    required this.loanValue,
    required this.interestRate,
    required this.type,
  });

  factory Loan.copyWith(Loan element) {
    return Loan(
      id: element.id,
      name: element.name,
      initialDate: element.initialDate,
      typeDisplayValue: element.typeDisplayValue,
      monthsPayment: element.monthsPayment,
      loanValue: element.loanValue,
      interestRate: element.interestRate,
      type: element.type,
    );
  }

  Loan.fromJson(Map<String, dynamic> json)
      : id = json['id'],
        name = json['name'].toString(),
        initialDate = DateTime.parse(json['initialDate'].toString()),
        typeDisplayValue = json['typeDisplayValue'],
        monthsPayment = json['monthsPayment'],
        loanValue = json['loanValue'],
        interestRate = json['interestRate'],
        type = json['type'];
}
