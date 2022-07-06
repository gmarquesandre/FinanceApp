class CurrentBalance {
  double? percentageCdi;
  bool updateValueWithCdiIndex;
  double value;
  DateTime creationDateTime;
  DateTime updateDateTime;

  CurrentBalance({
    required this.percentageCdi,
    required this.updateDateTime,
    required this.value,
    required this.creationDateTime,
    required this.updateValueWithCdiIndex,
  });

  factory CurrentBalance.copyWith(CurrentBalance element) {
    return CurrentBalance(
      percentageCdi: element.percentageCdi,
      updateDateTime: element.updateDateTime,
      value: element.value,
      creationDateTime: element.creationDateTime,
      updateValueWithCdiIndex: element.updateValueWithCdiIndex,
    );
  }

  CurrentBalance.fromJson(Map<String, dynamic> json)
      : creationDateTime =
            DateTime.tryParse(json['creationDateTime'].toString())!,
        updateDateTime = DateTime.tryParse(json['updateDateTime'].toString())!,
        percentageCdi = json['percentageCdi']?.toDouble(),
        value = json['value'].toDouble(),
        updateValueWithCdiIndex = json['updateValueWithCdiIndex'] == '1';
}
