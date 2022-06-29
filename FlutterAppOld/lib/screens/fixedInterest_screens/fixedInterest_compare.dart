import 'package:extended_masked_text/extended_masked_text.dart';
import 'package:financial_app/database/dao/holidays_dao.dart';
import 'package:financial_app/database/dao/indexDailyValue_dao.dart';
import 'package:financial_app/database/dao/indexLastValue.dart';
import 'package:financial_app/database/dao/workingDaysByYear_dao.dart';
import 'package:financial_app/functions/dateFunctions.dart';
import 'package:financial_app/functions/futureValuesFixedInterest.dart';
import 'package:financial_app/functions/lists/common_lists.dart';
import 'package:financial_app/functions/prospectValueDaily.dart';
import 'package:financial_app/models/models/IndexesAndDate.dart';
import 'package:financial_app/models/table_models/fixedInterest.dart';
import 'package:financial_app/models/table_models/holiday.dart';
import 'package:financial_app/models/table_models/index.dart';
import 'package:financial_app/models/table_models/indexDailyInterest.dart';
import 'package:financial_app/models/table_models/workingDaysByYear.dart';
import 'package:flutter/material.dart';
import 'package:intl/intl.dart';

class FixedInterestCompare extends StatefulWidget {
  FixedInterestCompare();

  @override
  _FixedInterestCompareState createState() => _FixedInterestCompareState();
}

class _FixedInterestCompareState extends State<FixedInterestCompare> {
  var _amountController = MoneyMaskedTextController(leftSymbol: 'R\$ ');
  var _futureLcaController = MoneyMaskedTextController(leftSymbol: 'R\$ ');
  var _futureCdbController = MoneyMaskedTextController(leftSymbol: 'R\$ ');
  var _indexPercentageController = MoneyMaskedTextController(leftSymbol: ' ');
  var _additionalFixedInterestController = MoneyMaskedTextController(initialValue: 0);
  // var _lcaByCdb = MoneyMaskedTextController(leftSymbol: 'R\$ ', initialValue: 0);
  var _cdbByLca = MoneyMaskedTextController(leftSymbol: 'R\$ ', initialValue: 0);



  Index? _indexController;
  List<Index> indexList  = CommonLists.indexList;
  final TextEditingController _finalDateController = TextEditingController();

  DateTime _dateInitial = getInitialDatePicker();
  final TextEditingController _initialDateController = TextEditingController(text: DateFormat('dd/MM/yyyy').format(getInitialDatePicker()));
  DateTime? _dateFinal;

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text("Comparar Investimentos"),
      ),
      body: SingleChildScrollView(
        child: Padding(
          padding: const EdgeInsets.fromLTRB(16.0, 0.0, 16.0, 0.0),
          child: Column(
            children: [
              Padding(
                padding: const EdgeInsets.only(top: 8.0),
                child: new TextFormField(
                  controller: _amountController,
                  autocorrect: true,
                  onChanged: (value) async {
                    await getFutureValue();
                    setState(() {});
                  },
                  decoration: InputDecoration(
                    labelText: 'Valor Investido',
                  ),
                  keyboardType: TextInputType.numberWithOptions(decimal: true),
                ),
              ),
              Padding(
                padding: const EdgeInsets.only(top: 8.0),
                child: new TextFormField(
                  enabled: false,
                  controller: _initialDateController,
                  autocorrect: true,
                  decoration: InputDecoration(
                    labelText: 'Data do Investimento',
                  ),
                ),
              ),
              Padding(
                padding: const EdgeInsets.only(top: 8.0),
                child: new TextFormField(
                  readOnly: true,
                  controller: _finalDateController,
                  onTap: () async {
                    FocusScope.of(context).requestFocus(new FocusNode());
                    final DateTime? picked = await showDatePicker(
                      context: (context),
                      initialDate: _dateFinal == null
                          ? getInitialDatePicker()
                          : _dateFinal!,
                      selectableDayPredicate: predicate,
                      firstDate: DateTime.now(),
                      lastDate: DateTime(DateTime.now().year + 100, 1, 1),
                    );
                    if (picked != null) {

                      setState(() {
                        _dateFinal = picked;
                      });
                      await getFutureValue();
                      setState((){});
                    }
                    _finalDateController.text =
                        DateFormat('dd/MM/yyyy').format(_dateFinal!);
                  },
                  decoration: InputDecoration(
                    labelText: 'Data de Vencimento',
                  ),
                ),
              ),

              Padding(
                padding: const EdgeInsets.only(top: 8.0, left: 0),
                child: DropdownButtonFormField<Index>(
                  value: _indexController,
                  decoration: InputDecoration(
                    labelText: 'Indice',
                  ),
                  items: indexList
                      .map<DropdownMenuItem<Index>>((_indexController) {
                    return DropdownMenuItem<Index>(
                      value: _indexController,
                      child: Text(
                        _indexController.name,
                      ),
                    );
                  }).toList(),
                  onChanged: (Index? newValue) async {
                    setState(
                          () {
                        _indexController = newValue!;
                      },
                    );
                    await getFutureValue();
                    setState(() {});
                  },
                ),
              ),
              Padding(
                padding: const EdgeInsets.only(top: 8.0),
                child: new TextField(
                  controller: _indexPercentageController,
                  autocorrect: true,
                  onChanged: (value) async {
                    await getFutureValue();
                    setState(() {});
                  },
                  decoration: InputDecoration(
                    labelText: _indexController != null
                        ? '% ' + _indexController!.name
                        : 'Indice (%)',
                  ),
                  keyboardType:
                  TextInputType.numberWithOptions(decimal: true),
                ),
              ),
              Padding(
                padding: const EdgeInsets.only(top: 8.0),
                child: new TextField(
                  controller: _additionalFixedInterestController,
                  autocorrect: true,
                  onChanged: (value) async {
                    await getFutureValue();
                    setState((){});
                  },
                  decoration: InputDecoration(
                    labelText: 'Rendimento Fixo (%)',
                  ),
                  keyboardType:
                  TextInputType.numberWithOptions(decimal: true),
                ),
              ),
              Padding(
                padding: const EdgeInsets.only(top: 8.0),
                child: Text("*Calculo feito utilizando a mediana das expectativas de mercado")
              ),
              Padding(
                  padding: const EdgeInsets.only(top: 8.0),
                  child: Text((_cdbByLca.numberValue*100).toString()+ "%"),
              ),
              Padding(
                padding: const EdgeInsets.only(top: 8.0),
                child: new TextFormField(
                  controller: _futureLcaController,
                  autocorrect: true,
                  enabled: false,
                  decoration: InputDecoration(
                    labelText: 'LCA/LCI',
                  ),
                  keyboardType: TextInputType.numberWithOptions(decimal: true),
                ),
              ),
              Padding(
                padding: const EdgeInsets.only(top: 8.0),
                child: new TextFormField(
                  controller: _futureCdbController,
                  autocorrect: true,
                  enabled: false,
                  decoration: InputDecoration(
                    labelText: 'CDB/LC',
                  ),
                  keyboardType: TextInputType.numberWithOptions(decimal: true),
                ),
              )
            ],
          ),
        ),
      ),
    );
  }

  Future<int> getFutureValue() async {
    var listFixedType = CommonLists.fixedInterestTypeList;

    if(_dateFinal == null || _amountController.numberValue == 0 || _indexController == null){
      return 1;
      }


    var element = FixedInterest(
        additionalFixedInterest: _additionalFixedInterestController.numberValue/100,
        indexPercentage: _indexPercentageController.numberValue/100,
        investmentDate: _dateInitial,
        name: '',
        indexName: _indexController!.name,
        amount: _amountController.numberValue,
        liquidityOnExpiration: 0,
        expirationDate: _dateFinal!,
        typeFixedInterestId: listFixedType.firstWhere((element) => element.name == "LCA").id,
        preFixedInvestment: 0);

    debugPrint("index percentage "+_indexPercentageController.toString());
    final HolidaysDao _daoHolidays = HolidaysDao();
    List<Holiday> holidaysList =
        await _daoHolidays.findAll(_dateInitial, _dateFinal!);

    List<DateTime> holidays = holidaysList.map((e) => e.date).toList();

    final IndexDailyValueDao _daoIndexDaily = IndexDailyValueDao();

    List<IndexDaily> listDaily = await _daoIndexDaily.findAll();

    final IndexLastValueDao _daoLastValueIndex = IndexLastValueDao();

    List<IndexDaily> lastValueIndex = await _daoLastValueIndex.findAll();
    final WorkingDaysByYearDao _dao = WorkingDaysByYearDao();
    List<WorkingDaysByYear> workingDaysList =
        await _dao.findAll(_dateInitial.year, _dateFinal!.year);

    List<IndexAndDate> listIndexAndDate = [];

    listIndexAndDate.add(IndexAndDate(
        indexName: _indexController!.name, minDate: DateTime.now(), maxDate: DateTime.now()),);

    listDaily = await addProspectValue(
        listIndexAndDate, listDaily, _dateFinal!, holidays, lastValueIndex);

    debugPrint("List Daily Len " + listDaily.length.toString());
    debugPrint("WorkinDaysList Len " + workingDaysList.length.toString());
    try{
    listDaily = List.unmodifiable(listDaily);
    double todayValue = getTodayValueFixedInterest(
        element, _dateFinal!, listDaily, holidays, workingDaysList);

    debugPrint(todayValue.toString());
    _futureLcaController =
        MoneyMaskedTextController(leftSymbol: 'R\$ ', initialValue: todayValue);


    element.typeFixedInterestId = listFixedType.firstWhere((element) => element.name == "CDB").id;

    todayValue = getTodayValueFixedInterest(
        element, _dateFinal!, listDaily, holidays, workingDaysList);

    _futureCdbController =
        MoneyMaskedTextController(leftSymbol: 'R\$ ', initialValue: todayValue);


    double cdbByLca = (_futureCdbController.numberValue-_amountController.numberValue)/(_futureLcaController.numberValue-_amountController.numberValue);
    double lcaByCdb = (_futureLcaController.numberValue-_amountController.numberValue)/(_futureCdbController.numberValue-_amountController.numberValue);





    // _lcaByCdb =
    //     MoneyMaskedTextController(leftSymbol: 'R\$ ', initialValue: lcaByCdb);

    _cdbByLca =
        MoneyMaskedTextController(leftSymbol: 'R\$ ', initialValue: cdbByLca);


    debugPrint("LCA/CDB " + lcaByCdb.toString());
    debugPrint("CDB/LCA " + cdbByLca.toString());
    }
    catch(e){
      _futureCdbController =
          MoneyMaskedTextController(leftSymbol: 'R\$ ', initialValue: 0);
      _futureLcaController =
          MoneyMaskedTextController(leftSymbol: 'R\$ ', initialValue: 0);

    }

    return 1;
  }
}
