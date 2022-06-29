import 'package:financial_app/database/dao/assetChanges_dao.dart';
import 'package:financial_app/database/dao/assetCurrentValue_dao.dart';
import 'package:financial_app/database/dao/assetEarnings_dao.dart';
import 'package:financial_app/database/dao/asset_dao.dart';
import 'package:financial_app/database/dao/asset_simulation.dart';
import 'package:financial_app/database/dao/category_dao.dart';
import 'package:financial_app/database/dao/fixedInterest_dao.dart';
import 'package:financial_app/database/dao/holidays_dao.dart';
import 'package:financial_app/database/dao/income_dao.dart';
import 'package:financial_app/database/dao/indexDailyValue_dao.dart';
import 'package:financial_app/database/dao/indexLastValue.dart';
import 'package:financial_app/database/dao/indexProspect.dart';
import 'package:financial_app/database/dao/investmentFundCurrentValue_dao.dart';
import 'package:financial_app/database/dao/investmentFund_dao.dart';
import 'package:financial_app/database/dao/loanDao.dart';
import 'package:financial_app/database/dao/spending_dao.dart';
import 'package:financial_app/database/dao/treasuryCurrentValue_dao.dart';
import 'package:financial_app/database/dao/treasury_dao.dart';
import 'package:financial_app/database/dao/workingDaysByYear_dao.dart';
import 'package:flutter/material.dart';
import 'package:path/path.dart';
import 'package:sqflite/sqflite.dart';

import 'dao/fund_simulation.dart';

Future<Database> getDatabase() async {
  final String path = join( await getDatabasesPath(), 'app.db');
  return openDatabase(
    path,
    onCreate: (db, version) {
      List<String> tables = getTables();

      //Cria Tabelas
      tables.forEach((element) {db.execute(element);},);
      debugPrint("Tabelas Criadas");

      //Popula tabelas com valores iniciais
      List<InitialValues> initialValues = populateTablesWithInitialValues();
      initialValues.forEach((element) {
        db.insert(element.tableName, element.values);});
      debugPrint("Tabelas Populadas");
    },
        // onOpen: (db) {
        //   deleteDatabase(path);
        //   debugPrint('DB deletado');
        // },
    // onUpgrade: (db, version) {
    //   dbUpgrade(db,db.getVersion(),version)
    // },
    version: 1,

  );
}

Future<int> resetDatabaseFunc() async {
  final String path = join( await getDatabasesPath(), 'app.db');
  await deleteDatabase(path);
  await getDatabase();

  return 1;
}

List<String> getTables() {
  List<String> tables = [];
  tables.add(CategoryDao.tableSql);
  tables.add(IncomeDao.tableSql);
  tables.add(AssetDao.tableSql);
  tables.add(FixedInterestDao.tableSql);
  tables.add(SpendingDao.tableSql);
  tables.add(TreasuryDao.tableSql);
  tables.add(IndexDailyValueDao.tableSql);
  tables.add(AssetCurrentValueDao.tableSql);
  tables.add(TreasuryCurrentValueDao.tableSql);
  tables.add(InvestmentFundDao.tableSql);
  tables.add(InvestmentFundCurrentValueDao.tableSql);
  tables.add(IndexProspectDao.tableSql);
  tables.add(HolidaysDao.tableSql);
  tables.add(WorkingDaysByYearDao.tableSql);
  tables.add(IndexLastValueDao.tableSql);
  tables.add(LoanDao.tableSql);
  tables.add(AssetChangesDao.tableSql);
  tables.add(AssetEarningsDao.tableSql);
  tables.add(AssetSimulationDao.tableSql);
  tables.add(FundSimulationDao.tableSql);
  return tables;
}


class InitialValues{
  dynamic values;
  String tableName;

  InitialValues({
    this.values,
    required this.tableName,
  });

}

List<InitialValues> populateTablesWithInitialValues(){

  List<InitialValues> values = [];

   CategoryDao.categoryList.forEach((element) => values.add(InitialValues(values: element.toMap(),tableName: CategoryDao.tableName)));

 return values;
}





void dbUpgrade(Database db,int oldVersion,int newVersion){

 if (oldVersion < newVersion) {
  //Recursive
    dbUpgrade(db,oldVersion,newVersion);
  }
}