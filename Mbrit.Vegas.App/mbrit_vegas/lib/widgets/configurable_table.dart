import 'package:flutter/material.dart';

class CustomTableRow {
  final String label;
  final String explanation;
  final String? currencyAmount;
  final Color? currencyColor;
  final bool isHighlighted;

  const CustomTableRow({
    required this.label,
    required this.explanation,
    this.currencyAmount,
    this.currencyColor,
    this.isHighlighted = false,
  });
}

class ConfigurableTable extends StatelessWidget {
  final List<String> columnHeaders;
  final List<CustomTableRow> rows;
  final List<double> columnWeights;
  final String currencySymbol;

  const ConfigurableTable({
    super.key,
    required this.columnHeaders,
    required this.rows,
    required this.columnWeights,
    this.currencySymbol = '\$',
  });

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        // Headers
        Container(
          padding: const EdgeInsets.symmetric(vertical: 12, horizontal: 16),
          decoration: BoxDecoration(
            color: const Color(0xFF1A202C),
            borderRadius: const BorderRadius.only(
              topLeft: Radius.circular(12),
              topRight: Radius.circular(12),
            ),
          ),
          child: Row(
            children: List.generate(columnHeaders.length, (index) {
              return Expanded(
                flex: (columnWeights[index] * 100).round(),
                child: Text(
                  columnHeaders[index],
                  style: const TextStyle(
                    color: Colors.white,
                    fontWeight: FontWeight.bold,
                    fontSize: 14,
                  ),
                ),
              );
            }),
          ),
        ),
        // Rows
        ...rows.map((row) => _buildTableRow(row)).toList(),
      ],
    );
  }

  Widget _buildTableRow(CustomTableRow row) {
    return Container(
      margin: const EdgeInsets.only(bottom: 1),
      decoration: BoxDecoration(
        color: row.isHighlighted 
            ? const Color(0xFF3B82F6).withOpacity(0.1)
            : const Color(0xFF2D3748),
        borderRadius: BorderRadius.circular(8),
      ),
      child: Padding(
        padding: const EdgeInsets.all(16),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // Label
            Text(
              row.label,
              style: const TextStyle(
                color: Colors.white,
                fontSize: 16,
                fontWeight: FontWeight.w600,
              ),
            ),
            const SizedBox(height: 8),
            // Explanation
            Text(
              row.explanation,
              style: TextStyle(
                color: Colors.grey[500],
                fontSize: 12,
              ),
            ),
            if (row.currencyAmount != null) ...[
              const SizedBox(height: 8),
              // Currency amount
              Container(
                height: 20, // Same height as button labels
                child: Text(
                  row.currencyAmount!,
                  style: TextStyle(
                    color: row.currencyColor ?? const Color(0xFF10B981),
                    fontSize: 16,
                    fontWeight: FontWeight.bold,
                  ),
                ),
              ),
            ],
          ],
        ),
      ),
    );
  }
} 