class FundSimulation {
  String cnpj;
  String nameShort;
  String indexName;
  double fixedYearGain;

  FundSimulation({
    required this.cnpj,
    required this.nameShort,
    required this.indexName,
    required this.fixedYearGain,
  });

  Map<String, dynamic> toMap() {
    final assetMap = Map<String, dynamic>();

    assetMap['indexName'] = indexName;
    assetMap['nameShort'] = nameShort;
    assetMap['cnpj'] = cnpj;
    assetMap['fixedYearGain'] = fixedYearGain;
    return assetMap;
  }

  factory FundSimulation.fromMap(Map<String, dynamic> map) {
    return FundSimulation(
      cnpj: map['cnpj'],
      nameShort: map['nameShort'],
      indexName: map['indexName'],
      fixedYearGain: map['fixedYearGain'],
    );
  }
}
