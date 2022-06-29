class Holiday {
  String countryCode;
  DateTime date;
  DateTime dateLastUpdate;


  Holiday({
    required this.countryCode,
    required this.date,
    required this.dateLastUpdate
  });

  Map<String, dynamic> toMap() {
    final assetMap = Map<String, dynamic>();
    assetMap['countryCode'] = countryCode;
    assetMap['date'] = date.millisecondsSinceEpoch;
    assetMap['dateLastUpdate'] = dateLastUpdate.millisecondsSinceEpoch;
    return assetMap;
  }

  factory Holiday.fromMap(Map<String, dynamic> map) {
    return Holiday(
      countryCode: map['countryCode'],
      date: DateTime.fromMillisecondsSinceEpoch(map['date']),
      dateLastUpdate: DateTime.fromMillisecondsSinceEpoch
        (map['dateLastUpdate']),
    );
  }


  Map<String, dynamic> toJson() =>
      {
        'countryCode' : countryCode,
        'date': date,
        'dateLastUpdate': dateLastUpdate,
      };


  Holiday.fromJson(Map<String, dynamic> json) :
        countryCode = json['countryCode'],
        dateLastUpdate = DateTime.tryParse(json['dateLastUpdate'].toString())!,
        date = DateTime.tryParse(json['date'].toString())!;

}
