import 'package:financial_app/components/globalVariables.dart';
import 'package:financial_app/database/dao/assetChanges_dao.dart';
import 'package:financial_app/database/dao/assetCurrentValue_dao.dart';
import 'package:financial_app/database/dao/assetEarnings_dao.dart';
import 'package:financial_app/database/dao/asset_dao.dart';
import 'package:financial_app/database/dao/fixedInterest_dao.dart';
import 'package:financial_app/database/dao/holidays_dao.dart';
import 'package:financial_app/database/dao/indexDailyValue_dao.dart';
import 'package:financial_app/database/dao/indexLastValue.dart';
import 'package:financial_app/database/dao/indexProspect.dart';
import 'package:financial_app/database/dao/investmentFundCurrentValue_dao.dart';
import 'package:financial_app/database/dao/investmentFund_dao.dart';
import 'package:financial_app/database/dao/treasuryCurrentValue_dao.dart';
import 'package:financial_app/database/dao/workingDaysByYear_dao.dart';
import 'package:financial_app/functions/lists/common_lists.dart';
import 'package:financial_app/http/webclients/asset_webclient.dart';
import 'package:financial_app/models/models/IndexesAndDate.dart';
import 'package:financial_app/models/table_models/asset.dart';
import 'package:financial_app/models/table_models/assetChangeInput.dart';
import 'package:financial_app/models/table_models/assetChanges.dart';
import 'package:financial_app/models/table_models/assetCurrentValue.dart';
import 'package:financial_app/models/table_models/assetEarnings.dart';
import 'package:financial_app/models/table_models/holiday.dart';
import 'package:financial_app/models/table_models/index.dart';
import 'package:financial_app/models/table_models/indexDailyInterest.dart';
import 'package:financial_app/models/table_models/indexProspect.dart';
import 'package:financial_app/models/table_models/investmentFundCurrentValue.dart';
import 'package:financial_app/models/table_models/treasuryCurrentValue.dart';
import 'package:financial_app/models/table_models/workingDaysByYear.dart';
import 'package:collection/collection.dart';
import 'package:shared_preferences/shared_preferences.dart';

Function deepEq = const DeepCollectionEquality().equals;

Future<int> updateData() async {
  try {
    await updateFunds();
  } catch (e) {}
  try {
    await updateAsset();
  } catch (e) {}
  try {
    await updateIndexLastValue();
  } catch (e) {}
  try {
    await updateIndexes();
  } catch (e) {}
  try {
    await updateTreasuryBond();
  } catch (e) {}
  try {
    await updateHolidays();
  } catch (e) {}
  try {
    await updateIndexesProspect();
  } catch (e) {}
  try {
    await updateWorkingDaysByYear();
  } catch (e) {}

  return 1;
}

Future<int> updateIndexLastValue() async {
  final IndexLastValueDao _dao = IndexLastValueDao();

  List<IndexDaily> list = await TransactionWebClient().getIndexLastValues();

  if (list.length > 0) {
    await _dao.deleteAll().then((value) async => await _dao.saveList(list));
  }
  return 1;
}

Future<int> updateWorkingDaysByYear() async {
  final WorkingDaysByYearDao _dao = WorkingDaysByYearDao();

  DateTime lastUpdateDate = await _dao.getLastUpdateDate();

  if (lastUpdateDate.compareTo(DateTime.now().add(Duration(days: -30))) < 0) {
    List<WorkingDaysByYear> list =
        await TransactionWebClient().getWorkingDaysByYear(lastUpdateDate);

    if (list.length != 0) {
      await _dao.saveList(list);
    }
  }
  return 1;
}

Future<int> updateHolidays() async {
  final HolidaysDao _daoHolidays = HolidaysDao();

  DateTime lastUpdateDate = await _daoHolidays.getLastUpdateDate();

  if (lastUpdateDate.compareTo(DateTime.now().add(Duration(days: -30))) < 0) {
    List<Holiday> listHolidays =
        await TransactionWebClient().getHolidays(lastUpdateDate);

    if (listHolidays.length != 0) {
      await _daoHolidays.deleteAll();
      await _daoHolidays.saveList(listHolidays);
    }
  }
  return 1;
}

Future<int> updateIndexesProspect() async {

  final IndexProspectDao _daoIndexProspect = IndexProspectDao();

  DateTime lastUpdate = await _daoIndexProspect.getLastUpdateDate();

  // Atualiza todo dia util
  if (lastUpdate.compareTo(DateTime.now()) < 0) {
    if (lastUpdate.difference(DateTime.now()).inDays < 0) {
      //Atualiza

      List<IndexProspect> list =
          await TransactionWebClient().getIndexProspect();
      if (list.length > 0) {
        await _daoIndexProspect.deleteAll();
        await _daoIndexProspect.saveList(list);
      }
    }
  }

  return 1;
}

Future<int> updateIndexes() async {
  final FixedInterestDao _daoFixedInterest = FixedInterestDao();
  final IndexDailyValueDao _daoIndexDailyValue = IndexDailyValueDao();

  List<IndexAndDate> listIndexesId = await _daoFixedInterest.getIndexesList();
  List<IndexAndDate> listValue = await _daoIndexDailyValue.getIndexesList();

  final SharedPreferences prefs = await SharedPreferences.getInstance();

  //Adiciona para calculo de rendimento da conta corrente
  if((prefs.getDouble(GlobalVariables.currentBalanceValue) ?? 0 )> 0.00){

    DateTime dateCurrentBalance = DateTime.fromMillisecondsSinceEpoch(
        prefs.getInt(GlobalVariables.dateEpochLastUpdateBalanceValue) ?? 0);

    var cdiRow = listIndexesId.firstWhereIndexedOrNull((index, element) => element.indexName == "CDI");


    if(cdiRow == null)
    {
      listIndexesId.add(IndexAndDate(indexName: "CDI", minDate: dateCurrentBalance, maxDate: DateTime.now()),);
    }
    else{
      if(cdiRow.minDate!.difference(dateCurrentBalance).inDays > 0){
        listIndexesId.firstWhere((element) => element.indexName == "CDI").minDate = dateCurrentBalance;
        listIndexesId.firstWhere((element) => element.indexName == "CDI").maxDate = DateTime.now();
      }
    }



    // listIndexesId.add(IndexAndDate(indexName: indexName, minDate: ))

  }

  listValue.forEach((element) async {
    if (listIndexesId
            .firstWhereOrNull((item) => item.indexName == element.indexName) ==
        null) {
      await _daoIndexDailyValue.deleteWithIndexName(element);
    }
  });

  List<Index> listIndex = CommonLists.indexList;

  //Se a lista estiver vazia, nada a fazer
  if (listIndexesId.isEmpty) {
    //Deleta todos valores desta tabela
    await _daoIndexDailyValue.deleteAll();
  } else {
    listIndexesId.forEach((element) async {
      Index thisIndexInfo =
          listIndex.firstWhere((item) => item.name == element.indexName);

      if (thisIndexInfo.type == "Monthly") {
        element.minDate =
            DateTime(element.minDate!.year, element.minDate!.month, 1);
        element.maxDate =
            DateTime(element.maxDate!.year, element.maxDate!.month - 1, 1);
      }

      var indexLocalValues = listValue
          .firstWhereOrNull((item) => item.indexName == element.indexName);

      if (indexLocalValues != null) {
        //Local
        if (element.minDate!.compareTo(indexLocalValues.minDate!) < 0) {
          List<IndexDaily> list = await TransactionWebClient().getIndexValues(
              IndexAndDate(
                  indexName: element.indexName,
                  minDate: element.minDate,
                  maxDate: indexLocalValues.minDate));

          await _daoIndexDailyValue.saveList(list);
        }

        List<IndexDaily> list = await TransactionWebClient().getIndexValues(
            IndexAndDate(
                indexName: element.indexName,
                minDate: indexLocalValues.maxDate!,
                maxDate: element.maxDate));
        _daoIndexDailyValue.saveList(list);
      } else {
        List<IndexDaily> list =
            await TransactionWebClient().getIndexValues(element);
        await _daoIndexDailyValue.saveList(list);
      }

      //Deleta valores abaixo do minimo
      await _daoIndexDailyValue.deleteOutofRangeDate(element);
    });
  }
  return 1;
}

Future<int> updateFunds() async {
  final InvestmentFundDao _daoFund = InvestmentFundDao();
  final InvestmentFundCurrentValueDao _daoFundCurrentValue =
      InvestmentFundCurrentValueDao();

  List<String> list = await _daoFund.getFundList();
  List<String> listValue = await _daoFundCurrentValue.getFundList();
  //Se a lista estiver vazia, nada a fazer
  if (list.isEmpty) {
    //Se a lista de valores não estiver vazia, deletar todos valores

    listValue.forEach((element) async {
      if (!list.contains(element))
        await _daoFundCurrentValue.deleteRowWithCnpj(element);
    });
  } else {
    DateTime minDate = await _daoFundCurrentValue.minDate();

    if (minDate.compareTo(DateTime.now()) < 0) {
      //Pega valores na API
      List<InvestmentFundCurrentValue> listFunds =
          await TransactionWebClient().getFundsWithList(list);
      //Deleta açõse que não estão mais na lista do cliente
      if (listFunds.isNotEmpty) {
        listValue.forEach((element) async {
          if (!list.contains(element))
            await _daoFundCurrentValue.deleteRowWithCnpj(element);
        });
        //Atualiza
        await _daoFundCurrentValue.saveList(listFunds);
      }
    }
  }
  return 1;
}

Future<int> updateTreasuryBond() async {
  final TreasuryCurrentValueDao _daoTreasury = TreasuryCurrentValueDao();

  DateTime minDate = await _daoTreasury.minDate();
  if (minDate.compareTo(DateTime(
          DateTime.now().year, DateTime.now().month, DateTime.now().day + 1)) <
      0) {
    List<TreasuryCurrentValue> list =
        await TransactionWebClient().getTreasury();
    if (list.length > 0)
      await _daoTreasury
          .deleteAll()
          .then((value) async => await _daoTreasury.saveList(list));
  }
  return 1;
}

Future<int> updateAsset() async {
  final AssetDao _daoAsset = AssetDao();
  final AssetChangesDao _assetChangesDao = AssetChangesDao();
  final AssetEarningsDao _assetEarningsDao = AssetEarningsDao();
  final AssetCurrentValueDao _daoAssetCurrentValue = AssetCurrentValueDao();

  List<Asset> list = await _daoAsset.getAssetAndDateList();
  List<String> listValue = list.map((e) => e.assetCode).toList();

  var listCurrentValue = await _daoAssetCurrentValue.findAll();
  var listCurrentValueAssets = listCurrentValue.map((e) => e.assetCode).toList();

  //Se a lista estiver vazia, nada a fazer
  if (list.isEmpty) {
    //Se a lista de valores não estiver vazia, deletar todos valores

    listValue.forEach((element) async {
      await _daoAssetCurrentValue.deleteRowWithName(element);
    });
  } else {

    try {
      List<AssetCurrentValue> listAsset =
      await TransactionWebClient().getAssetWithList(
          list.map((e) => e.assetCode).toList());

      List<AssetChangeInput> listChangeInput = list.map((e) =>
          AssetChangeInput(assetCode: e.assetCode, dateStart: e.date,),)
          .toList();

      List<AssetEarnings> listEarnings = await TransactionWebClient()
          .postAssetEarnings(listChangeInput);

      List<AssetChanges> listChanges = await TransactionWebClient()
          .postAssetChanges(listChangeInput);

      //Deleta ações que não estão mais na lista do cliente
      listCurrentValueAssets.forEach((element) async {
        if (!list.contains(element))
          await _daoAssetCurrentValue.deleteRowWithName(element);
      });

      await _assetEarningsDao.deleteAll();
      await _assetEarningsDao.saveList(listEarnings);

      await _assetChangesDao.deleteAll();
      await _assetChangesDao.saveList(listChanges);

      //Atualiza
      await _daoAssetCurrentValue.saveList(listAsset);

    }
    catch(e) {

    }


  }
  return 1;
}
