class InterestAndPeriod {

  DateTime initialDate;
  DateTime endDate;
  num interestRate;

  InterestAndPeriod({
    required this.initialDate,
    required this.endDate,
    required this.interestRate
  });

  factory InterestAndPeriod.fromMap(Map<String, dynamic> map) {
    return InterestAndPeriod(
      endDate: map['endDate'],
      initialDate: map['initialDate'],
      interestRate: map['interestRate'],
    );
  }
}
