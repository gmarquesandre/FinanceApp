class CreateOrUpdateCurrentBalance {
  double? percentageCdi;
  bool updateValueWithCdiIndex;
  double value;

  CreateOrUpdateCurrentBalance({
    required this.percentageCdi,
    required this.value,
    required this.updateValueWithCdiIndex,
  });

  Map<String, dynamic> toJson() => {
        'percentageCdi': percentageCdi,
        'value': value,
        'updateValueWithCdiIndex': updateValueWithCdiIndex.toString(),
      };
}
