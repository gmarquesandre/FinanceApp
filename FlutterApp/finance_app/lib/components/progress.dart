import 'package:flutter/material.dart';

class Progress extends StatelessWidget {
  final String message;

  const Progress({
    Key? key,
    this.message = 'Carregando',
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return SizedBox(
      width: MediaQuery.of(context).size.width,
      height: MediaQuery.of(context).size.height / 1.3,
      child: Column(
        mainAxisAlignment: MainAxisAlignment.center,
        crossAxisAlignment: CrossAxisAlignment.center,
        children: <Widget>[
          const CircularProgressIndicator(),
          Padding(
            padding: const EdgeInsets.only(top: 8.0),
            child: Text(
              message,
            ),
          ),
        ],
      ),
    );
  }
}
