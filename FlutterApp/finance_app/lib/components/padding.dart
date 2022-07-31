import 'package:flutter/material.dart';

Padding defaultInputPadding(Widget form, {double padding = 8.0}) {
  return Padding(
    padding: EdgeInsets.only(top: padding),
    child: form,
  );
}

Padding defaultBodyPadding(Widget form, {double padding = 8.0}) {
  return Padding(
    padding: EdgeInsets.all(padding),
    child: form,
  );
}

Padding defaultButtonPadding(Widget form, {double padding = 16.0}) {
  return Padding(
    padding: EdgeInsets.only(top: padding),
    child: form,
  );
}
