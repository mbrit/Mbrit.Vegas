import 'package:flutter/material.dart';
import 'configurable_table.dart';

class SimulateTestSection extends StatelessWidget {
  final int unitSize;
  final String currencySymbol;
  final bool isExpanded;
  final VoidCallback onToggle;

  const SimulateTestSection({
    super.key,
    required this.unitSize,
    required this.currencySymbol,
    required this.isExpanded,
    required this.onToggle,
  });

  @override
  Widget build(BuildContext context) {
    // Calculate values
    final maxInvestment = unitSize * 12 * 3; // 3x multiplier
    final spike0p5 = (unitSize * 12) ~/ 2;
    final spike1 = unitSize * 12;

    // Create table rows
    final rows = [
      CustomTableRow(
        label: 'Big Loss',
        explanation: null,
        currencyAmount: '${currencySymbol}${maxInvestment} to ${currencySymbol}${maxInvestment ~/ 2}',
        currencyColor: const Color(0xFFEF4444), // Red for loss
      ),
      CustomTableRow(
        label: 'Small Loss',
        explanation: null,
        currencyAmount: '${currencySymbol}${maxInvestment ~/ 2} to ${currencySymbol}0',
        currencyColor: const Color(0xFFF59E0B), // Orange for small loss
      ),
      CustomTableRow(
        label: 'Evens',
        explanation: null,
        currencyAmount: '${currencySymbol}0 to ${currencySymbol}${spike0p5}',
        currencyColor: Colors.grey[400], // Gray for break even
      ),
      CustomTableRow(
        label: '0.5x Up',
        explanation: null,
        currencyAmount: '${currencySymbol}0 to ${currencySymbol}${spike0p5}',
        currencyColor: const Color(0xFF10B981), // Green for profit
      ),
      CustomTableRow(
        label: '1x Up',
        explanation: null,
        currencyAmount: '${currencySymbol}${spike0p5} to ${currencySymbol}${spike1}',
        currencyColor: const Color(0xFF10B981), // Green for profit
      ),
      CustomTableRow(
        label: 'Better than 1x Up',
        explanation: null,
        currencyAmount: '~${currencySymbol}${spike1}',
        currencyColor: const Color(0xFF10B981), // Green for profit
      ),
    ];

    return Container(
      decoration: BoxDecoration(
        gradient: const LinearGradient(
          colors: [
            Color(0xFF2D3748),
            Color(0xFF1A202C),
          ],
        ),
        borderRadius: BorderRadius.circular(16),
        boxShadow: [
          BoxShadow(
            color: Colors.black.withOpacity(0.3),
            blurRadius: 10,
            offset: const Offset(0, 4),
          ),
        ],
      ),
      child: Column(
        children: [
          // Header with icon, title, and expand/collapse button
          InkWell(
            onTap: onToggle,
            child: Padding(
              padding: const EdgeInsets.all(20.0),
              child: Row(
                children: [
                  Container(
                    padding: const EdgeInsets.all(8),
                    decoration: BoxDecoration(
                      color: const Color(0xFF8B5CF6),
                      borderRadius: BorderRadius.circular(8),
                    ),
                    child: const Icon(
                      Icons.analytics,
                      color: Colors.white,
                      size: 20,
                    ),
                  ),
                  const SizedBox(width: 12),
                  const Text(
                    'Simulate & Test',
                    style: TextStyle(
                      fontSize: 20,
                      fontWeight: FontWeight.bold,
                      color: Colors.white,
                    ),
                  ),
                  const Spacer(),
                  Icon(
                    isExpanded ? Icons.expand_less : Icons.expand_more,
                    color: Colors.white,
                    size: 24,
                  ),
                ],
              ),
            ),
          ),
          // Collapsible content
          if (isExpanded)
            Padding(
              padding: const EdgeInsets.fromLTRB(20.0, 0.0, 20.0, 20.0),
              child: ConfigurableTable(
                columnHeaders: ['Outcome'],
                rows: rows,
                columnWeights: [1.0],
                currencySymbol: currencySymbol,
              ),
            ),
        ],
      ),
    );
  }
} 