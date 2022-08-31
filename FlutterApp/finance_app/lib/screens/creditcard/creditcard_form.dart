import 'package:finance_app/components/app_bar.dart';
import 'package:finance_app/components/padding.dart';
import 'package:finance_app/models/credit_card/create_credit_card.dart';
import 'package:finance_app/models/credit_card/credit_card.dart';
import 'package:finance_app/clients/crud_clients/creditcard_client.dart';
import 'package:flutter/material.dart';
import 'package:flutter/services.dart';

class CreditCardForm extends StatefulWidget {
  final CreditCard? creditcard;

  CreditCardForm([this.creditcard]);

  @override
  CreditCardFormState createState() => CreditCardFormState();
}

class CreditCardFormState extends State<CreditCardForm> {
  bool _firstPress = true;
  final TextEditingController _nameController = TextEditingController();
  final TextEditingController _invoicePaymentDay = TextEditingController();
  final TextEditingController _invoiceClosingDay = TextEditingController();

  final CreditCardClient _daoCreditCard = CreditCardClient();

  final _formKey = GlobalKey<FormState>();

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: appBarLoggedInDefault(
          "${widget.creditcard == null ? "Adicionar" : "Atualizar"} Cartão"),
      body: Form(
        key: _formKey,
        child: defaultBodyPadding(
          SingleChildScrollView(
            child: Column(
              children: [
                defaultInputPadding(
                  nameTextFormField(),
                ),
                defaultInputPadding(
                  TextFormField(
                    validator: (value) {
                      if (value == '' || int.tryParse(value!)! < 1) {
                        return 'O Valor deve ser maior que zero';
                      } else if (int.tryParse(value)! > 31) {
                        return 'O Valor deve ser menor do que 31';
                      }
                      return null;
                    },
                    controller: _invoiceClosingDay,
                    inputFormatters: [FilteringTextInputFormatter.digitsOnly],
                    decoration: const InputDecoration(
                      //border: OutlineInputBorder(),
                      labelText: 'Dia Fechamento Fatura',
                    ),
                    keyboardType: TextInputType.number,
                  ),
                ),
                defaultInputPadding(
                  TextFormField(
                    validator: (value) {
                      if (value == '' || int.tryParse(value!)! < 1) {
                        return 'O Valor deve ser maior que zero';
                      } else if (int.tryParse(value)! > 31) {
                        return 'O Valor deve ser menor do que 31';
                      }
                      return null;
                    },
                    controller: _invoicePaymentDay,
                    inputFormatters: [FilteringTextInputFormatter.digitsOnly],
                    decoration: const InputDecoration(
                      //border: OutlineInputBorder(),
                      labelText: 'Dia Pagamento Fatura',
                    ),
                    keyboardType: TextInputType.number,
                  ),
                ),
                defaultInputPadding(
                  padding: 24.0,
                  SizedBox(
                    width: double.maxFinite,
                    child: ElevatedButton(
                      onPressed: () async {
                        if (_formKey.currentState!.validate() && _firstPress) {
                          await createOrUpdate(context);
                        }
                      },
                      child: Text(widget.creditcard == null
                          ? "Adicionar"
                          : "Atualizar"),
                    ),
                  ),
                )
              ],
            ),
          ),
        ),
      ),
    );
  }

  Future<void> createOrUpdate(BuildContext context) async {
    final String name = _nameController.text;

    if (widget.creditcard == null) {
      final CreateCreditCard newCreditCard = CreateCreditCard(
          name: name,
          invoiceClosingDay: int.tryParse(_invoiceClosingDay.text)!,
          invoicePaymentDay: int.tryParse(_invoicePaymentDay.text)!);

      _firstPress = false;

      await _daoCreditCard
          .create(newCreditCard)
          .then((created) => Navigator.pop(context, created));
    } else {
      // final UpdateCreditCard newCreditCard = UpdateCreditCard(
      //     id: widget.creditcard!.id,
      //     name: name,
      //     amount: creditcardValue,
      //     initialDate: initialDate,
      //     endDate: _dateTimeFinal,
      //     recurrence: recurrenceId!,
      //     isEndless: isEndless,
      //     timesRecurrence: timesRecurrence);

      // newCreditCard.id = widget.creditcard!.id;
      // await _daoCreditCard
      //     .update(newCreditCard)
      //     .then((id) => Navigator.pop(context, newCreditCard.toString()));
    }
  }

  @override
  void initState() {
    super.initState();

    if (widget.creditcard != null) {}
  }

  TextFormField nameTextFormField() {
    return TextFormField(
      validator: (value) {
        if (value!.isEmpty) {
          return 'É necessário um nome';
        }
        return null;
      },
      controller: _nameController,
      decoration: const InputDecoration(
        labelText: 'Nome do Cartão',
      ),
    );
  }
}
