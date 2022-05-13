class IndexAndDate{
  String indexName;
  DateTime? minDate;
  DateTime? maxDate;

  IndexAndDate({
    required this.indexName,
    this.minDate,
    this.maxDate,
  });


  factory IndexAndDate.fromMap(Map<String, dynamic> row){
    return IndexAndDate(
      indexName: row['indexName'],
      minDate: DateTime.fromMillisecondsSinceEpoch(row['minDate']),
      maxDate: DateTime.fromMillisecondsSinceEpoch(row['maxDate']),
    );
  }

}
