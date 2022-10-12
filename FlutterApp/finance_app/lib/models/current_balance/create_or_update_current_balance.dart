class CreateOrUpdateCurrentBalance {
  // String id;
  double? percentageCdi;
  bool updateValueWithCdiIndex;
  double value;

  CreateOrUpdateCurrentBalance({
    // required this.id,
    required this.percentageCdi,
    required this.value,
    required this.updateValueWithCdiIndex,
  });

  Map<String, dynamic> toJson() => {
        // 'id': id,
        'percentageCdi': percentageCdi,
        'value': value,
        'updateValueWithCdiIndex': updateValueWithCdiIndex.toString(),
      };
}
