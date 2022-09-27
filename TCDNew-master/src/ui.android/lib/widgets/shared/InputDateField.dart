import 'dart:async';

import 'package:flutter/material.dart';
import 'package:intl/intl.dart';

class InputDateField extends StatelessWidget {
  const InputDateField(
      {Key key,
      this.labelText,
      this.dateFormat,
      this.firstDate,
      this.lastDate,
      this.defaultDate,
      this.selectedDate,
      this.selectDate})
      : super(key: key);

  final String labelText;
  final DateFormat dateFormat;
  final DateTime firstDate;
  final DateTime lastDate;
  final DateTime defaultDate;
  final DateTime selectedDate;
  final ValueChanged<DateTime> selectDate;

  Future<void> _selectDate(BuildContext context) async {
    final DateTime picked = await showDatePicker(
      context: context,
      initialDate: selectedDate ?? defaultDate,
      firstDate: firstDate,
      lastDate: lastDate,
      locale: const Locale('ru', 'RU')
    );
    if (picked != null && picked != selectedDate) {
      if (selectDate != null) {
        selectDate(picked);
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    final TextStyle valueStyle = Theme.of(context).textTheme.bodyText2;
    return new Row(
      crossAxisAlignment: CrossAxisAlignment.end,
      children: <Widget>[
        new Expanded(
          child: new _InputDropdown(
            labelText: labelText,
            valueText: selectedDate != null ? dateFormat.format(selectedDate) : '',
            valueStyle: valueStyle,
            onPressed: () {
              _selectDate(context);
            },
          ),
        ),
      ],
    );
  }
}

class _InputDropdown extends StatelessWidget {
  const _InputDropdown(
      {Key key,
      this.child,
      this.labelText,
      this.valueText,
      this.valueStyle,
      this.onPressed})
      : super(key: key);

  final String labelText;
  final String valueText;
  final TextStyle valueStyle;
  final VoidCallback onPressed;
  final Widget child;

  @override
  Widget build(BuildContext context) {
    return new InkWell(
      onTap: onPressed,
      child: new InputDecorator(
        decoration: new InputDecoration(
          labelText: labelText,
        ),
        baseStyle: valueStyle,
        child: Row(
          mainAxisAlignment: MainAxisAlignment.spaceBetween,
          mainAxisSize: MainAxisSize.min,
          children: <Widget>[
            new Text(valueText, style: valueStyle),
            new Icon(Icons.arrow_drop_down,
                color: Theme.of(context).brightness == Brightness.light
                    ? Colors.grey.shade700
                    : Colors.white70),
          ],
        ),
      ),
    );
  }
}
