import 'package:financial_app/database/dao/assetChanges_dao.dart';
import 'package:financial_app/database/dao/assetEarnings_dao.dart';
import 'package:financial_app/database/dao/asset_dao.dart';
import 'package:financial_app/database/getDatabase.dart';
import 'package:sqflite/sqflite.dart';

class AssetDto{
  String query = '''
  select t1.* from (
  select  
    ${AssetDao.id} as id,
    ${AssetDao.assetCode} as assetCode, 
    ${AssetDao.date} as date, 
    ${AssetDao.quantity} as quantity,
    ${AssetDao.operation} as operation,
    case ${AssetDao.operation} when 1 then "C" when 2 then "V" end as operationName,
    ${AssetDao.unitPrice} as value 
  from ${AssetDao.tableName}
  UNION  ALL 
    select ${AssetEarningsDao.id} as id,
        ${AssetEarningsDao.assetCode} as assetCode,
        ${AssetEarningsDao.exDate} as date,
        0 as quantity,
        case ${AssetEarningsDao.type}
          when "RENDIMENTO" then 3
          when "JRS CAP PROPRIO" then 4
          when "DIVIDENDO" then 5
          when "REST CAP DIN" then 6
          else 999 end as operation,
          case ${AssetEarningsDao.type}
          when "RENDIMENTO" then "Rend"
          when "JRS CAP PROPRIO" then "JCP"
          when "DIVIDENDO" then "Div"
          when "REST CAP DIN" then "Am"
          else 999 end as operatioName,
        ${AssetEarningsDao.cashAmount} as value
    from ${AssetEarningsDao.tableName}
      UNION  ALL 
    select ${AssetChangesDao.id} as id,
        ${AssetChangesDao.assetCode} as assetCode,
        ${AssetChangesDao.exDate} as date,
        0 as quantity,
        case ${AssetChangesDao.type}
          when "BONIFICACAO" then 7
          when "INCORPORACAO" then 8
          when "DESDOBRAMENTO" then 9
          when "GRUPAMENTO" then 10
          when "CIS RED CAP" then 11
          when "REG TOTAL RV" then 12
          when "REST CAP ACOES" then 13
          else 999 end as operation,
        case ${AssetChangesDao.type}
          when "BONIFICACAO" then "Bonif"
          when "DESDOBRAMENTO" then "Desd"
          when "GRUPAMENTO" then "Grup"
          when "INCORPORACAO" then "Incp"        
          when "CIS RED CAP" then "CRC"
          when "REG TOTAL RV" then "REG T"
          when "REST CAP ACOES" then "REST C."
          else 999 end as operatioName,
        ${AssetChangesDao.groupingFactor} as value
    from ${AssetChangesDao.tableName}
    ) as t1
    left join
    (select min(${AssetDao.date}) as minDate, assetCode from ${AssetDao.tableName} group by assetCode) as t2
    on t1.assetCode = t2.${AssetDao.assetCode} 
    where t1.date >= t2.minDate 
    order by t1.date, t1.id asc
  ''';

  Future<List<AssetObject>> findAll() async {
    final Database db = await getDatabase();

    final List<Map<String, dynamic>> result = await db.rawQuery(query);
    final List<AssetObject> assets = [];

    result.forEach((element) {
      assets.add(AssetObject.fromMap(element));
    });
    return assets;
  }
}

class AssetObject {
  int id;
  String assetCode;
  String operationName;
  DateTime date;
  int operation;
  int quantity;
  double value;

  AssetObject({
      required this.id,
      required this.assetCode,
      required this.operationName,
      required this.quantity,
      required this.value,
      required this.operation,
      required this.date
    });


  factory AssetObject.fromMap(Map<String, dynamic> map) {
    return AssetObject(
      operation: map['operation'],
      operationName: map['operationName'],
      id: map['id'],
      assetCode: map['assetCode'],
      quantity: map['quantity'],
      value: map['value'],
      date: DateTime.fromMillisecondsSinceEpoch(map['date']),
    );
  }

}