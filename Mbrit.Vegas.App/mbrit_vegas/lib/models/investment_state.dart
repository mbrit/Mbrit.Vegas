import 'package:flutter/material.dart';

enum InvestmentState {
  available,
  consumed,
}

extension InvestmentStateExtension on InvestmentState {
  Color get color {
    switch (this) {
      case InvestmentState.available:
        return const Color(0xFF10B981); // Green
      case InvestmentState.consumed:
        return const Color(0xFFEF4444); // Red
    }
  }

  IconData get icon {
    switch (this) {
      case InvestmentState.available:
        return Icons.attach_money;
      case InvestmentState.consumed:
        return Icons.close;
    }
  }
} 