class InterestAndNDays {

  int workingDays;
  num interestRate;

  InterestAndNDays({
    required this.workingDays,
    required this.interestRate
  });

  factory InterestAndNDays.fromMap(Map<String, dynamic> map) {
    return InterestAndNDays(
      workingDays: map['workingDays'],
      interestRate: map['interestRate'],
    );
  }
}
