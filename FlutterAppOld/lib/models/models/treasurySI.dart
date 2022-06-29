class TreasurySI {

  double cupomPayment;
  double unitValue;

  TreasurySI({
    required this.cupomPayment,
    required this.unitValue,
  });

  factory TreasurySI.fromMap(Map<String, dynamic> map) {
    return TreasurySI(
      unitValue: map['unitValue'],
      cupomPayment: map['cupomPayment'],
    );
  }
}
