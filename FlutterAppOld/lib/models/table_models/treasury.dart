class Treasury {
  int? id;
  String treasuryBondName;
  double unitPrice;
  double quantity;
  DateTime date;
  int operation;


  Treasury({
    this.id,
    required this.treasuryBondName,
    required this.unitPrice,
    required this.quantity,
    required this.date,
    required this.operation,
  });

  Map<String, dynamic> toMap() {
    final treasuryMap = Map<String,dynamic>();
    if(id != null){
      treasuryMap['id'] = id;
    }
      treasuryMap['treasuryBondName'] = treasuryBondName;
      treasuryMap['unitPrice'] = unitPrice;
      treasuryMap['quantity'] = quantity;
      treasuryMap['date'] = date.millisecondsSinceEpoch;
      treasuryMap['operation'] = operation;
    return treasuryMap;
  }

  factory Treasury.fromMap(Map<String, dynamic> row) {
  return Treasury(
    id: row['id'],
    treasuryBondName: row['treasuryBondName'],
    unitPrice: row['unitPrice'],
    quantity: row['quantity'],
    operation: row['operation'],
    date: DateTime.fromMillisecondsSinceEpoch(row['date']),
    );
  }

}
