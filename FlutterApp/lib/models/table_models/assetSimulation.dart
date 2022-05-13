class AssetSimulation {
  String assetCode;
  String indexName;
  double fixedYearGain;

  AssetSimulation({
    required this.assetCode,
    required this.indexName,
    required this.fixedYearGain,
  });

  Map<String, dynamic> toMap() {
    final assetMap = Map<String, dynamic>();

    assetMap['indexName'] = indexName;
    assetMap['assetCode'] = assetCode;
    assetMap['fixedYearGain'] = fixedYearGain;
    return assetMap;
  }

  factory AssetSimulation.fromMap(Map<String, dynamic> map) {
    return AssetSimulation(
      assetCode: map['assetCode'],
      indexName: map['indexName'],
      fixedYearGain: map['fixedYearGain'],
    );
  }
}
