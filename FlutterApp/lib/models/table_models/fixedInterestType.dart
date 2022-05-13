class FixedInterestType {
  int id;
  String name;
  int incomeTax;


  FixedInterestType({
    required this.id,
    required this.name,
    required this.incomeTax,
  });



  Map<String, dynamic> toMap() {
    final map = Map<String,dynamic>();
    map['id'] = id;
    map['name'] = name;
    map['incomeTax'] = incomeTax;

    return map;
  }

  factory FixedInterestType.fromMap(Map<String, dynamic> row){
    return FixedInterestType(
      id: row['id'],
      name: row['name'],
      incomeTax: row['incomeTax']
    );
  }

  @override
  String toString() {
    return
      'name: $name,'
      'incomeTax: ${incomeTax.toString()}';

  }
}
