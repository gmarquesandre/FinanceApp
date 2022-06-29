class IndexDaily {
  int? id;
  DateTime date;
  String indexName;
  double value;



  IndexDaily({
    this.id,
    required this.date,
    required this.indexName,
    required this.value
  });


  Map<String, dynamic> toMap() {
    final map = Map<String, dynamic>();
    if (id != null) {
      map['id'] = id;
    }
    map['date'] = date.millisecondsSinceEpoch;
    map['indexName'] = indexName;
    map['value'] = value;
    map['uniqueKey'] = indexName+date.millisecondsSinceEpoch.toString();
    return map;
  }


  Map<String, dynamic> toMapLastValue() {
    final map = Map<String, dynamic>();
    if (id != null) {
      map['id'] = id;
    }
    map['date'] = date.millisecondsSinceEpoch;
    map['indexName'] = indexName;
    map['value'] = value;
    return map;
  }



  factory IndexDaily.fromMap(Map<String, dynamic> map) {
    return IndexDaily(
      id: map['id'],
      date: DateTime.fromMillisecondsSinceEpoch(map['date']),
      indexName: map['indexName'],
      value: map['value'],
    );
  }


  IndexDaily.fromJson(Map<String, dynamic> json) :
        date = DateTime.tryParse(json['date'].toString())!,
        indexName = json['indexName'],
        value = double.tryParse(json['value'].toString())!;

  Map<String, dynamic> toJson() =>
      {
        'date' : date,
        'indexName': indexName,
        'value': value,
      };

}
