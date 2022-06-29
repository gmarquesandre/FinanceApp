class InvestmentFundCurrentValue {
  String cnpj;
  String name;
  String nameShort;
  double unitPrice;
  String fundTypeName;
  String situation;
  bool taxLongTerm;
  double administrationFee;
  DateTime? dateLastUpdate;

  InvestmentFundCurrentValue({
    required this.cnpj,
    required this.name,
    required this.nameShort,
    required this.unitPrice,
    required this.fundTypeName,
    required this.situation,
    required this.taxLongTerm,
    required this.administrationFee,
    required this.dateLastUpdate
  });

  Map<String, dynamic> toMap() {
    final map = Map<String, dynamic>();
    map['cnpj'] = cnpj;
    map['name'] = name;
    map['nameShort'] = nameShort;
    map['fundTypeName'] = fundTypeName;
    map['situation'] = situation;
    map['taxLongTerm'] = taxLongTerm ? 1: 0;
    map['administrationFee'] = administrationFee;
    map['unitPrice'] = unitPrice;
    map['dateLastUpdate'] = dateLastUpdate!.millisecondsSinceEpoch;
    return map;
  }

  factory InvestmentFundCurrentValue.fromMap(Map<String, dynamic> map) {
    return InvestmentFundCurrentValue(
      cnpj: map['cnpj'],
      name: map['name'],
      nameShort: map['nameShort'],
      unitPrice: map['unitPrice'],
      dateLastUpdate: DateTime.fromMillisecondsSinceEpoch(map['dateLastUpdate']),
      fundTypeName: map['fundTypeName'],
      situation: map['situation'],
      administrationFee: map['administrationFee'],
      taxLongTerm: map['taxLongTerm'] == 1? true: false,


    );
  }

  InvestmentFundCurrentValue.fromJson(Map<String, dynamic> json) :
        cnpj = json['cnpj'],
        unitPrice = json['unitPrice'],
        name = json['name'],
        nameShort = json['nameShort'],
        fundTypeName = json['fundTypeName'],
        situation = json['situation'],
        taxLongTerm = json['taxLongTerm'],
        administrationFee = json['administrationFee'],
        dateLastUpdate = DateTime.tryParse(json['dateLastUpdate'].toString());

  Map<String, dynamic> toJson() =>
    {
      'cnpj' : cnpj,
      'unitPrice': unitPrice,
      'name': name,
      'nameShort': nameShort,
      'dateLastUpdate': dateLastUpdate,
      'fundTypeName': fundTypeName,
      'situation': situation,
      'administrationFee': administrationFee,
      'taxLongTerm': taxLongTerm,

    };

}
