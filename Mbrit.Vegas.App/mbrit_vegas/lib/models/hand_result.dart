import 'package:flutter/material.dart';

enum HandResult {
  pending,
  won,
  lost,
}

extension HandResultExtension on HandResult {
  Color get color {
    switch (this) {
      case HandResult.pending:
        return Colors.grey;
      case HandResult.won:
        return Colors.green;
      case HandResult.lost:
        return Colors.red;
    }
  }

  bool get isFilled {
    switch (this) {
      case HandResult.pending:
        return false;
      case HandResult.won:
      case HandResult.lost:
        return true;
    }
  }
} 