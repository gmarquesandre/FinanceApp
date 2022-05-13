class Index {
  int id;
  String name;
  String type;

  Index({
    required this.id,
    required this.name,
    required this.type,
  });

  Map<String, dynamic> toMap() {
    final map = Map<String,dynamic>();
    map['id'] = id;
    map['name'] = name;
    map['type'] = type;
    return map;
  }

  factory Index.fromMap(Map<String, dynamic> row) {
    return Index(
      id: row['id'],
      name: row['name'],
      type: row['type'],
    );
  }

}
