class Recurrence {
  int? id;
  String name;

  Recurrence({
    this.id,
    required this.name,
  });

  Map<String, dynamic> toMap() {
    final map = <String, dynamic>{};
    if (id != null) {
      map['id'] = id;
    }
    map['name'] = name;
    return map;
  }

  factory Recurrence.fromMap(Map<String, dynamic> map) {
    return Recurrence(
      id: map['id'],
      name: map['name'],
    );
  }
}
