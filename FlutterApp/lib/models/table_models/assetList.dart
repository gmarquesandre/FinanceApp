class AssetList {
  int? id;
  String code;
  String companyName;



  AssetList({
    this.id,
    required this.code,
    required this.companyName,
  });


  Map<String, dynamic> toMap() {
    final assetMap = Map<String, dynamic>();
    if (id != null) {
      assetMap['id'] = id;
    }
    assetMap['code'] = code;
    assetMap['companyName'] = companyName;
    return assetMap;
  }

  factory AssetList.fromMap(Map<String, dynamic> map) {
    return AssetList(
      id: map['id'],
      code: map['code'],
      companyName: map['companyName'],
    );
  }
}
