class AssetChanges {
  int id;
  String type;
  DateTime declarationDate;
  DateTime exDate;
  double groupingFactor;
  String assetCode;
  String assetCodeISIN;
  String toAssetISIN;
  // String notes;
  String hash;

  AssetChanges({
    required this.id,
    required this.type,
    required this.assetCode,
    required this.assetCodeISIN,
    required this.declarationDate,
    required this.exDate,
    required this.groupingFactor,
    required this.toAssetISIN,
    // required this.notes,
    required this.hash,
  });


  AssetChanges.fromJson(Map<String, dynamic> json) :
        id = json['id'],
        type = json['type'],
        groupingFactor = json['groupingFactor'].toDouble(),
        // notes = json['notes'],
        hash = json['hash'],
        toAssetISIN = json['toAssetISIN'],
        assetCode = json['assetCode'],
        assetCodeISIN = json['assetCodeISIN'],
        exDate = DateTime.tryParse(json['exDate'].toString())!,
        declarationDate = DateTime.tryParse(json['declarationDate'].toString())!;

  Map<String, dynamic> toJson() =>
      {
        'id' : id,
        'type' : type,
        'groupingFactor': groupingFactor,
        // 'notes': notes,
        'hash': hash,
        'assetCode': assetCode,
        'assetCodeISIN': assetCodeISIN,
        'toAssetISIN': toAssetISIN,
        'exDate': exDate,
        'declarationDate': declarationDate,
      };

  Map<String, dynamic> toMap() {
    final map = Map<String, dynamic>();
    map['id'] = id;
    map['type'] = type;
    map['assetCode'] = assetCode;
    map['assetCodeISIN'] = assetCodeISIN;
    map['groupingFactor'] = groupingFactor;
    // map['notes'] = notes;
    map['hash'] = hash;
    map['declarationDate'] = declarationDate.millisecondsSinceEpoch;
    map['assetCodeISIN'] = assetCodeISIN;
    map['toAssetISIN'] = toAssetISIN;
    map['exDate'] = exDate.millisecondsSinceEpoch;
    return map;
  }

  factory AssetChanges.fromMap(Map<String, dynamic> map) {
    return AssetChanges(
      id: map['id'],
      type: map['type'],
      assetCode: map['assetCode'],
      assetCodeISIN: map['assetCodeISIN'],
      exDate: DateTime.fromMillisecondsSinceEpoch(map['exDate']),
      declarationDate: DateTime.fromMillisecondsSinceEpoch(map['dateLastUpdate']),
      groupingFactor: map['groupingFactor'],
      // notes: map['notes'],
      toAssetISIN: map['toAssetISIN'],
      hash: map['hash']
    );
  }
}
