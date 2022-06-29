import 'dart:math';
import 'package:financial_app/database/dao/indexProspect.dart';
import 'package:financial_app/database/dao/workingDaysByYear_dao.dart';
import 'package:financial_app/models/models/IndexesAndDate.dart';
import 'package:financial_app/models/table_models/indexDailyInterest.dart';
import 'package:financial_app/models/table_models/indexProspect.dart';
import 'package:financial_app/models/table_models/workingDaysByYear.dart';

Future<List<IndexDaily>> addProspectValue(
    List<IndexAndDate> listIndexAndDate,
    List<IndexDaily> listDaily,
    DateTime maxDateReturn,
    List<DateTime> nationalHolidays,
    List<IndexDaily> lastValueIndex) async {


  if (listIndexAndDate.length == 0) return [];

  if (lastValueIndex.length == 0) return [];


  final IndexProspectDao _daoIndexProspect = IndexProspectDao();

  List<IndexProspect> listProspect = await _daoIndexProspect.findAll();

  final WorkingDaysByYearDao _dao = WorkingDaysByYearDao();


  List<WorkingDaysByYear> workingDaysList =
      await _dao.findAll(DateTime.now().year, maxDateReturn.year);

  listIndexAndDate.forEach(
    (element) {
      //Pega maior data do indice
      DateTime maxDateReal = element.maxDate!;

      if (['IPCA', 'IGPM'].contains(element.indexName)) {
        List<IndexProspect> thisListProspect = listProspect
            .where((item) =>
                item.indexName == element.indexName)
            .toSet()
            .toList();

        thisListProspect.forEach((item) async {
          for (DateTime i = item.dateStart;
              i.difference(item.dateEnd).inDays <= 0;
              i = DateTime(i.year, i.month , i.day+1)) {
            if (i.compareTo(maxDateReturn) > 0) {
              //Só vai funcionar até a data maxima q sera analisada
              break;
            } else {
              // if (![6, 7].contains(i.weekday) &&
              //     !nationalHolidays.contains(i)) {
              
              
                int nDays = (DateTime(i.year, i.month + 1, 1 ).subtract(const Duration(days: 1)).difference(DateTime(i.year, i.month, 1))).inDays;
                // int nWorkingDaysYear = workingDaysList
                //     .firstWhere((element) => element.year == i.year)
                //     .workingDays;

                //Calcula rendimento diario
                num interestDaily =
                    pow((1.00 + item.median / 100), (1.00 / nDays)) -
                        1;

                listDaily.add(IndexDaily(
                    date: i,
                    indexName: item.indexName,
                    value: interestDaily.toDouble()));
              }
            }
          // }
        });
      } else if (['SELIC', 'CDI'].contains(element.indexName)) {
        //CDI, SELIC
        //Adiciona datas até a proxima alteração do COPOM
        DateTime minDateProspect = listProspect
            .where((item) => item.indexName == element.indexName)
            .map((e) => e.dateStart)
            .reduce((a, b) => a.isBefore(b) ? a : b);

        double value = lastValueIndex
            .firstWhere((item) => item.indexName == element.indexName)
            .value;

        for (DateTime i = maxDateReal;
            i.compareTo(minDateProspect) < 0;
            i = i.add(Duration(days: 1))) {
          //Remove fins de semana e feriados nacionais
          if (![6, 7].contains(i.weekday) && !nationalHolidays.contains(i)) {
            listDaily.add(IndexDaily(
                date: i, indexName: element.indexName, value: value*0.98));
          }
        }

        //Adiciona datas futuras
        List<IndexProspect> thisListProspect = listProspect
            .where((item) => item.indexName == element.indexName)
            .toSet()
            .toList();

        thisListProspect.forEach(
          (item) async {
            for (DateTime i = item.dateStart;
                i.difference(item.dateEnd).inDays < 0;
                i = DateTime(i.year, i.month, i.day + 1)) {
              if (i.compareTo(maxDateReturn) > 0) {
                //Só vai funcionar até a data maxima q sera analisada
                break;
              } else {
                if (![6, 7].contains(i.weekday) &&
                    !nationalHolidays.contains(i)) {
                  int nWorkingDaysYear = workingDaysList
                      .firstWhere((element) => element.year == i.year)
                      .workingDays;

                  //Calcula rendimento diario
                  num interestDaily = pow((1.00 + item.median / 100),
                          (1.00 / nWorkingDaysYear)) -
                      1;

                  listDaily.add(IndexDaily(
                      date: i,
                      indexName: item.indexName,
                      value: interestDaily.toDouble()));
                }
              }
            }
          },
        );
      }
    },
  );

  return listDaily;
}

// Future<List<IndexDaily>> addProspectValue(List<IndexAndDate>
// listIndexAndDate, List<IndexDaily> listDaily, DateTime maxDateReturn ) async {
//
//
//   final IndexProspectDao _daoIndexProspect = IndexProspectDao();
//
//   List<IndexProspect> listProspect = await _daoIndexProspect.findAll();
//
//   List<DateTime> nationalHolidays = await getHolidays(DateTime(1900,1,
//       1), DateTime(DateTime.now().year+100,1,1));
//
//   listIndexAndDate.forEach((element) {
//
//     //Pega maior data do indice
//     DateTime maxDateReal = listDaily.where((item) =>
//     item.indexName == element
//         .indexName).map(
//             (e) =>
//         e.date).reduce((a, b) => a.isAfter(b) ? a : b);
//
//     if (['IPCA', 'IGPM'].contains(element.indexName)) {
//       List<IndexProspect> thisListProspect = listProspect.where((item) => item
//           .indexName
//           == element
//               .indexName && item.dateStart.compareTo(maxDateReturn) <= 0
//           && item.dateStart.compareTo(maxDateReal) > 0
//       ).toSet
//         ().toList();
//
//       thisListProspect.forEach((item) {
//
//         listDaily.add(IndexDaily(date: item.dateStart,
//             indexName: item.indexName,
//             value: item.median/100));
//       });
//     }
//     else {
//
//       //CDI, SELIC
//       //Adiciona datas até a proxima alteração do COPOM
//       DateTime minDateProspect = listProspect.where((item) =>
//       item.indexName ==
//           element.indexName).map(
//               (e) =>
//           e.dateStart).reduce((a, b) => a.isBefore(b) ? a : b);
//       double value = listDaily.firstWhere((item) =>
//       item.indexName == element
//           .indexName && item.date == maxDateReal).value;
//
//       for (DateTime i = maxDateReal; i.compareTo(minDateProspect) < 0;
//       i = i.add(Duration(days: 1))) {
//
//         //Remove fins de semana e feriados nacionais
//         if(![6,7].contains(i.weekday) && !nationalHolidays.contains(i)) {
//           listDaily.add(IndexDaily(date: i,
//               indexName: element.indexName,
//               value: value));
//         }
//       }
//
//       //Adiciona datas futuras
//       List<IndexProspect> thisListProspect = listProspect.where((item) => item
//           .indexName
//           == element
//               .indexName).toSet().toList();
//
//       thisListProspect.forEach((item) async {
//         for(DateTime i = item.dateStart; i.compareTo(item.dateEnd) < 0; i =
//             DateTime(i.year, i.month, i.day+1)){
//           if(i.compareTo(maxDateReturn) > 0)
//           {
//             //Só vai funcionar até a data maxima q sera analisada
//             break;
//           }
//           else{
//             if(![6,7].contains(i.weekday) && !nationalHolidays.contains(i) ){
//
//               int nWorkingDaysYear = await returnWorkingDays(DateTime(i.year,
//                   1,1),
//                   DateTime(i.year+1,1));
//
//               //Calcula rendimento diario
//               num interestDaily = pow(
//                   (1.00 + item.median/100), (1.00 / nWorkingDaysYear))-1;
//
//
//               listDaily.add(IndexDaily(date: i,
//                   indexName: item.indexName,
//                   value: interestDaily.toDouble()));
//             }
//           }
//         }
//
//       });
//     }
//   });
//
//   return listDaily;
// }
