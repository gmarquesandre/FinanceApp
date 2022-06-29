class Loan {
  int? id;
  String name;
  DateTime date;
  double totalValue;
  int months;
  double interestRate;
  int paymentType;


  Loan({
    this.id,
    required this.name,
    required this.date,
    required this.totalValue,
    required this.months,
    required this.interestRate,
    required this.paymentType,
  });

  Map<String, dynamic> toMap() {
    final map = Map<String, dynamic>();
    if (id != null) {
      map['id'] = id;
    }
    map['name'] = name;
    map['date'] = date.millisecondsSinceEpoch;
    map['totalValue'] = totalValue;
    map['months'] = months;
    map['interestRate'] = interestRate;
    map['paymentType'] = paymentType;

    return map;
  }

  factory Loan.fromMap(Map<String, dynamic> map) {
    return Loan(
      id: map['id'],
      name: map['name'],
      date: DateTime.fromMillisecondsSinceEpoch(map['date']),
      totalValue: map['totalValue'],
      months: map['months'],
      interestRate: map['interestRate'],
      paymentType: map['paymentType'],
    );
  }
}
