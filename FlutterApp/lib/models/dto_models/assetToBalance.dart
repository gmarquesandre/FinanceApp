class AssetToBalance {

  String assetCode;
  double avgBuyPrice;
  double unitPrice;
  int quantity;
  DateTime dateLastUpdate;

  AssetToBalance({
    required this.assetCode,
    required this.avgBuyPrice,
    required this.unitPrice,
    required this.quantity,
    required this.dateLastUpdate,
  });

  factory AssetToBalance.fromMap(Map<String, dynamic> map) {
    return AssetToBalance(
      assetCode: map['assetCode'] ,
      avgBuyPrice: map['avgBuyPrice'] ,
      unitPrice: map['unitPrice'],
      quantity: map['quantity'],
      dateLastUpdate: DateTime.fromMillisecondsSinceEpoch(map['dateLastUpdate']),
    );
  }
  //
  // factory AssetToBalance.copyWith(AssetToBalance element) {
  //   return AssetToBalance(
  //     date: element.date,
  //     endDate: element.endDate,
  //     initialDate: element.initialDate,
  //     name: element.name,
  //     isSpending: element.isSpending,
  //     amount: element.amount,
  //     recurrenceId: element.recurrenceId,
  //     timesRecurrence: element.timesRecurrence,
  //     isEndless: element.isEndless
  //   );
  // }

}