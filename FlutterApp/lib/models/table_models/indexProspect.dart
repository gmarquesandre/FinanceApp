class IndexProspect {
  int? id;
  String indexName;
  DateTime dateResearch;
  DateTime dateStart;
  DateTime dateEnd;
  double average;
  double median;
  double min;
  double max;
  int researchAnswers;
  int baseCalculo;
  DateTime dateLastUpdate;
  DateTime? dateEndIndex;

  IndexProspect({
    this.id,
    required this.indexName,
    required this.dateResearch,
    required this.dateStart,
    required this.dateEnd,
    required this.average,
    required this.median,
    required this.min,
    required this.max,
    required this.researchAnswers,
    required this.baseCalculo,
    required this.dateLastUpdate,
  });

  Map<String, dynamic> toMap() {
    final map = Map<String,dynamic>();

    if(id != null){
      map['id'] = id;
    }
    map['indexName'] = indexName;
    map['dateResearch'] = dateResearch.millisecondsSinceEpoch;
    map['dateStart'] = dateStart.millisecondsSinceEpoch;
    map['dateEnd'] = dateEnd.millisecondsSinceEpoch;
    map['average'] = average;
    map['median'] = median;
    map['min'] = min;
    map['max'] = max;
    map['researchAnswers'] = researchAnswers;
    map['baseCalculo'] = baseCalculo;
    map['dateLastUpdate'] = dateLastUpdate.millisecondsSinceEpoch;
    map['uniqueKey'] = indexName+dateStart.toString();
    return map;
  }

  factory IndexProspect.fromMap(Map<String, dynamic> row) {
    return IndexProspect(
      id: row['id'],
      indexName: row['indexName'],
      dateResearch: DateTime.fromMillisecondsSinceEpoch(row['dateResearch']),
      dateStart: DateTime.fromMillisecondsSinceEpoch(row['dateStart']),
      dateEnd: DateTime.fromMillisecondsSinceEpoch(row['dateEnd']),
      average: row['average'],
      median: row['median'],
      min: row['min'],
      max: row['max'],
      researchAnswers: row['researchAnswers'],
      baseCalculo: row['baseCalculo'],
      dateLastUpdate: DateTime.fromMillisecondsSinceEpoch(row['dateLastUpdate']),
    );
  }

  IndexProspect.fromJson(Map<String, dynamic> json) :
        indexName = json['indexName'],
        dateResearch = DateTime.tryParse(json['dateResearch'].toString())!,
        dateStart = DateTime.tryParse(json['dateStart'].toString())!,
        dateEnd = DateTime.tryParse(json['dateEnd'].toString())!,
        average = json['average'].toDouble(),
        median = json['median'].toDouble(),
        min = json['min'].toDouble(),
        max = json['max'].toDouble(),
        researchAnswers = json['researchAnswers'],
        baseCalculo = json['baseCalculo'],
        dateLastUpdate = DateTime.tryParse(json['dateLastUpdate'].toString())!;


  Map<String, dynamic> toJson() =>
      {
        'indexName': indexName,
        'dateResearch': dateResearch,
        'dateStart': dateStart,
        'dateEnd': dateEnd,
        'average': average,
        'median': median,
        'min': min,
        'max': max,
        'researchAnswers': researchAnswers,
        'baseCalculo': baseCalculo,
        'dateLastUpdate': dateLastUpdate,
      };



}
