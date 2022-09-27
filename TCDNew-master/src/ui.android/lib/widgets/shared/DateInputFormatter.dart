import 'package:flutter/foundation.dart';
import 'package:flutter/services.dart';
import 'package:flutter/material.dart' show TextField;
import 'dart:math';

///
/// An implementation of [TextInputFormatter] provides a way to input date form
/// with [TextField], such as dd.MM.yyyy. In order to guide user about input form,
/// the formatter will provide [TextField] a placeholder --/--/---- as soon as
/// user start editing. During editing session, the formatter will replace appropriate
/// placeholder characters by user's input.
///
class DateInputFormatter extends TextInputFormatter {
  String _placeholder = '  .  .    ';

  @override
  TextEditingValue formatEditUpdate(TextEditingValue oldValue, TextEditingValue newValue) {
    /// provides placeholder text when user start editing
    if (oldValue.text.isEmpty) {
      oldValue = oldValue.copyWith(
        text: _placeholder,
      );
      newValue = newValue.copyWith(
        text: _fillInputToPlaceholder(newValue.text),
      );
      return newValue;
    }

    int offset = newValue.selection.baseOffset;

    /// restrict user's input within the length of date form
    if (offset > 10) {
      return oldValue;
    }

    if ((newValue.text.length - oldValue.text.length).abs() != 1) {
      return oldValue;
    }

    if (oldValue.text == newValue.text && oldValue.text.length > 0) {
      return newValue;
    }

    final String oldText = oldValue.text;
    final String newText = newValue.text;
    String resultText;

    /// handle user editing, there're two cases:
    /// 1. user add new digit: replace '-' at cursor's position by user's input.
    /// 2. user delete digit: replace digit at cursor's position by '-'
    int index = oldValue.selection.start;
    if (oldText.length < newText.length) {
      /// add new digit
      String newChar = newText[index];
      if (index == 2 || index == 5) {
        index++;
        offset++;
      }
      resultText = oldText.replaceRange(index, index + 1, newChar);
      if (offset == 2 || offset == 5) {
        offset++;
      }
    } else if (oldText.length > newText.length) {
      /// delete digit
      if (oldText[index] != '.') {
        resultText = oldText.replaceRange(index, index + 1, ' ');
        if (offset == 3 || offset == 6) {
          offset--;
        }
      } else {
        resultText = oldText;
      }
    }

    /// verify the number and position of splash character
    final splashes = resultText.replaceAll(RegExp(r'[^.]'), '');
    int count = splashes.length;
    if (resultText.length > 10 ||
        count != 2 ||
        resultText[2].toString() != '.' ||
        resultText[5].toString() != '.') {
      return oldValue;
    }

    resultText = _checkResultText(resultText);

    return oldValue.copyWith(
      text: resultText,
      selection: TextSelection.collapsed(offset: offset),
      composing: defaultTargetPlatform == TargetPlatform.iOS
          ? TextRange(start: 0, end: 0)
          : TextRange.empty,
    );
  }

  String _fillInputToPlaceholder(String input) {
    if (input == null || input.isEmpty) {
      return _placeholder;
    }
    String result = _placeholder;
    final index = [0, 1, 3, 4, 6, 7, 8, 9];
    final length = min(index.length, input.length);
    for (int i = 0; i < length; i++) {
      result = result.replaceRange(index[i], index[i] + 1, input[i]);
    }
    return result;
  }

  String _checkResultText(String s) {
    String dayRaw = '  ';
    int day = 0;
    String monthRaw = '  ';
    int month = 0;
    String yearRaw = '    ';
    int year = 0;

    if (s.length > 2) { // days
      dayRaw = s.substring(0, 2);
      day = int.tryParse(dayRaw);
    }

    if (s.length > 5) { // month
      monthRaw = s.substring(3, 5);
      month = int.tryParse(monthRaw);
    }

    if (s.length == 10) { // year
      yearRaw  = s.substring(6, 10);
      year = int.tryParse(yearRaw);
    }

    if (year != null) {
      if (year < 2016) {
        year = 2016;
        yearRaw = '2016';
      } else if (year > 2030) {
        year = 2030;
        yearRaw = '2030';
      }
    }

    if (month != null) {
      if (month == 0) {
        month = 1;
        monthRaw = '01';
      } else if (month > 12) {
        month = 12;
        monthRaw = '12';
      }
    }

    if (day != null) {
      int daysInMonth = 31;
      bool isLeapYear = true;
      if (year != null) {
        isLeapYear = _isLeapYear(year);
      }
      if (month != null) {
        daysInMonth = _daysInMonth(month, isLeapYear);
      }

      if (day == 0) {
        day = 1;
        dayRaw = '01';
      } else if (day > daysInMonth) {
        day = daysInMonth;
        dayRaw = daysInMonth.toString().padLeft(2, '0');
      }
    }

    return '$dayRaw.$monthRaw.$yearRaw';
  }

  bool _isLeapYear(int year) {
    // If a year is multiple of 400,
    // then it is a leap year
    if (year % 400 == 0)
      return true;

    // Else If a year is multiple of 100,
    // then it is not a leap year
    if (year % 100 == 0)
      return false;

    // Else If a year is multiple of 4,
    // then it is a leap year
    if (year % 4 == 0)
      return true;

    return false;
  }

  int _daysInMonth(int month, bool isLeapYear) {
    if ([1, 3, 5, 7, 8, 10, 12].contains(month))
      return 31;

    if ([4, 6, 9, 11].contains(month))
      return 30;

    if (month == 2 && isLeapYear)
      return 29;

    return 28;
  }
}