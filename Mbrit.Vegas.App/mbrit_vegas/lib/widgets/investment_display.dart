import 'package:flutter/material.dart';

class InvestmentDisplay extends StatelessWidget {
  final int unitSize;
  final int maxInvestment;
  final String label;
  final String currencySymbol;

  const InvestmentDisplay({
    super.key,
    required this.unitSize,
    required this.maxInvestment,
    this.label = 'Investment',
    this.currencySymbol = '\$',
  });

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Row(
          children: [
            Icon(Icons.account_balance_wallet, color: Colors.grey[400], size: 16),
            const SizedBox(width: 8),
            Text(
              label,
              style: TextStyle(
                color: Colors.grey[400],
                fontWeight: FontWeight.w500,
                fontSize: 14,
              ),
            ),
          ],
        ),
        const SizedBox(height: 12),
        Container(
          padding: const EdgeInsets.all(16),
          decoration: BoxDecoration(
            color: const Color(0xFF2D3748),
            borderRadius: BorderRadius.circular(12),
            border: Border.all(color: Colors.grey[600]!),
          ),
          child: Row(
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              Text(
                '12 units × ${currencySymbol}${unitSize} = ${currencySymbol}${maxInvestment}',
                style: const TextStyle(
                  color: Colors.white,
                  fontSize: 24,
                  fontWeight: FontWeight.bold,
                ),
              ),
            ],
          ),
        ),
      ],
    );
  }
} 