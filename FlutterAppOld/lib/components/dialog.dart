
import 'package:flutter/material.dart';

Future<bool?> confirmDialog(BuildContext context) async {
  return showDialog<bool>(
    context: context,
    builder: (BuildContext context) => AlertDialog(
      title: const Text('Confirme a Exclusão'),
      content: const Text('Deseja confirmar a exclusão?'),
      actions: <Widget>[
        TextButton(
          onPressed: () => Navigator.pop(context, false),
          child: const Text('Cancelar'),
        ),
        TextButton(
          onPressed: () => Navigator.pop(context, true),
          child: const Text('Confirmar'),
        ),
      ],
    ),
  );
}