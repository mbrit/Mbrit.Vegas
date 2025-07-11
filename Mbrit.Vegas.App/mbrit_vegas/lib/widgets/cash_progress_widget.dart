import 'package:flutter/material.dart';

class CashProgressWidget extends StatelessWidget {
  final double min;
  final double max;
  final double value;
  final Color color;
  final int tickCount;
  final String currencySymbol;

  const CashProgressWidget({
    Key? key,
    required this.min,
    required this.max,
    required this.value,
    this.color = const Color(0xFF10B981), // Green default
    this.tickCount = 9, // min, max, midpoint, and 6 more
    this.currencySymbol = ' 24',
  }) : super(key: key);

  @override
  Widget build(BuildContext context) {
    // Clamp value
    final double clampedValue = value.clamp(min, max);
    // Bar dimensions
    const double barHeight = 6;
    const double barHorizontalPadding = 24;
    const double barVerticalPadding = 12;
    const double labelFontSize = 24;
    const double tickHeight = 18;
    const double tickWidth = 2;
    const double barWidth = 260; // fixed width for layout

    // Calculate value position (0.0 to 1.0)
    final double valueFraction = (clampedValue - min) / (max - min);
    final double valueX =
        barHorizontalPadding +
        valueFraction * (barWidth - 2 * barHorizontalPadding);

    // Tick positions
    List<double> tickFractions = List.generate(
      tickCount,
      (i) => i / (tickCount - 1),
    );
    List<double> tickXs = tickFractions
        .map(
          (f) =>
              barHorizontalPadding + f * (barWidth - 2 * barHorizontalPadding),
        )
        .toList();
    List<double> tickValues = tickFractions
        .map((f) => min + f * (max - min))
        .toList();

    // Find midpoint index
    int midIdx = (tickCount / 2).floor();

    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Row(
          children: [
            Icon(Icons.trending_up, color: color, size: 20),
            const SizedBox(width: 8),
            Text(
              'Profit',
              style: TextStyle(
                color: color,
                fontWeight: FontWeight.w600,
                fontSize: 15,
                letterSpacing: 0.2,
              ),
            ),
          ],
        ),
        const SizedBox(height: 14),
        SizedBox(
          width: barWidth,
          height: 70,
          child: Stack(
            clipBehavior: Clip.none,
            children: [
              // Bar
              Positioned(
                left: barHorizontalPadding,
                right: barHorizontalPadding,
                top: barVerticalPadding,
                child: Container(
                  height: barHeight,
                  decoration: BoxDecoration(
                    color: Colors.grey[700],
                    borderRadius: BorderRadius.circular(3),
                  ),
                ),
              ),
              // Ticks
              ...List.generate(tickCount, (i) {
                return Positioned(
                  left: tickXs[i] - tickWidth / 2,
                  top:
                      barVerticalPadding -
                      (i == 0 || i == tickCount - 1 ? 2 : 0),
                  child: Container(
                    width: tickWidth,
                    height: i == 0 || i == tickCount - 1
                        ? tickHeight
                        : tickHeight * 0.7,
                    color: i == 0 || i == tickCount - 1
                        ? Colors.white
                        : (i == midIdx ? color : Colors.grey[400]),
                  ),
                );
              }),
              // Value label (transparent, no border, text color = color)
              Positioned(
                left: valueX - 40,
                top: barVerticalPadding - labelFontSize - 8,
                child: Text(
                  '$currencySymbol${clampedValue.toStringAsFixed(0)}',
                  style: TextStyle(
                    color: color,
                    fontSize: labelFontSize,
                    fontWeight: FontWeight.bold,
                  ),
                ),
              ),
              // Min label
              Positioned(
                left: barHorizontalPadding - 8,
                top: barVerticalPadding + barHeight + 8,
                child: Text(
                  min.toStringAsFixed(0),
                  style: TextStyle(
                    color: Colors.grey[400],
                    fontSize: 13,
                    fontWeight: FontWeight.w500,
                  ),
                ),
              ),
              // Max label
              Positioned(
                left: barWidth - barHorizontalPadding - 8,
                top: barVerticalPadding + barHeight + 8,
                child: Text(
                  max.toStringAsFixed(0),
                  style: TextStyle(
                    color: Colors.grey[400],
                    fontSize: 13,
                    fontWeight: FontWeight.w500,
                  ),
                ),
              ),
              // Midpoint label
              Positioned(
                left: tickXs[midIdx] - 12,
                top: barVerticalPadding + barHeight + 8,
                child: Text(
                  ((min + max) / 2).toStringAsFixed(0),
                  style: TextStyle(
                    color: color,
                    fontSize: 13,
                    fontWeight: FontWeight.w600,
                  ),
                ),
              ),
            ],
          ),
        ),
      ],
    );
  }
}
