class CurrentBalance {
  double? percentageCdi;
  bool updateValueWithCdiIndex;
  double value;
  DateTime updateDateTime;

  CurrentBalance({
    required this.percentageCdi,
    required this.updateDateTime,
    required this.value,
    required this.updateValueWithCdiIndex,
  });

  factory CurrentBalance.copyWith(CurrentBalance element) {
    return CurrentBalance(
      // id: element.id,
      percentageCdi: element.percentageCdi,
      updateDateTime: element.updateDateTime,
      value: element.value,
      updateValueWithCdiIndex: element.updateValueWithCdiIndex,
    );
  }

  CurrentBalance.fromJson(Map<String, dynamic> json)
      : updateDateTime = DateTime.tryParse(json['updateDateTime'].toString())!,
        percentageCdi = json['percentageCdi']?.toDouble(),
        value = json['value'].toDouble(),
        updateValueWithCdiIndex = json['updateValueWithCdiIndex'];
}
