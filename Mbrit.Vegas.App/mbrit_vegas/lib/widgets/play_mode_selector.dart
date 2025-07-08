import 'package:flutter/material.dart';

enum PlayMode {
  goForBroke('Go For Broke'),
  make50PercentProfit('Make 50% Profit'),
  balanced('Balanced'),
  doubleYourMoney('Double Your Money');

  const PlayMode(this.displayName);
  final String displayName;
}

class PlayModeSelector extends StatefulWidget {
  final PlayMode initialMode;
  final ValueChanged<PlayMode>? onChanged;
  final String label;
  final int unitSize;
  final String currencySymbol;

  const PlayModeSelector({
    super.key,
    this.initialMode = PlayMode.balanced,
    this.onChanged,
    this.label = 'Play Mode',
    required this.unitSize,
    this.currencySymbol = '\$',
  });

  @override
  State<PlayModeSelector> createState() => _PlayModeSelectorState();
}

class _PlayModeSelectorState extends State<PlayModeSelector> {
  late PlayMode _selectedMode;

  @override
  void initState() {
    super.initState();
    _selectedMode = widget.initialMode;
  }

  // Helper methods to calculate amounts
  int get maxPutIns => 12;
  int get maxInvestment => widget.unitSize * maxPutIns * 3;
  int get spike0p5 => (widget.unitSize * maxPutIns) ~/ 2;
  int get spike1 => widget.unitSize * maxPutIns;

  String _getAmountForMode(PlayMode mode) {
    switch (mode) {
      case PlayMode.goForBroke:
        return '~${widget.currencySymbol}${maxInvestment}';
      case PlayMode.make50PercentProfit:
        return '+${widget.currencySymbol}${spike0p5}';
      case PlayMode.balanced:
        return '+${widget.currencySymbol}${spike0p5} to +${widget.currencySymbol}${spike1}';
      case PlayMode.doubleYourMoney:
        return '+${widget.currencySymbol}${spike1}';
    }
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Row(
          children: [
            Icon(Icons.sports_esports, color: Colors.grey[400], size: 16),
            const SizedBox(width: 8),
            Text(
              widget.label,
              style: TextStyle(
                color: Colors.grey[400],
                fontWeight: FontWeight.w500,
                fontSize: 14,
              ),
            ),
          ],
        ),
        const SizedBox(height: 12),
        Column(
          children: PlayMode.values.map((mode) {
            final isSelected = mode == _selectedMode;
            return Padding(
              padding: const EdgeInsets.only(bottom: 8.0),
              child: InkWell(
                onTap: () {
                  setState(() {
                    _selectedMode = mode;
                  });
                  widget.onChanged?.call(mode);
                },
                child: Container(
                  padding: const EdgeInsets.all(16),
                  decoration: BoxDecoration(
                    color: const Color(0xFF2D3748),
                    borderRadius: BorderRadius.circular(12),
                    border: Border.all(
                      color: isSelected ? const Color(0xFF3B82F6) : Colors.grey[600]!,
                      width: isSelected ? 2 : 1,
                    ),
                  ),
                  child: Row(
                    children: [
                      Container(
                        width: 20,
                        height: 20,
                        decoration: BoxDecoration(
                          color: isSelected ? const Color(0xFF3B82F6) : Colors.transparent,
                          borderRadius: BorderRadius.circular(4),
                          border: Border.all(
                            color: isSelected ? const Color(0xFF3B82F6) : Colors.grey[600]!,
                            width: 2,
                          ),
                        ),
                        child: isSelected
                            ? const Icon(
                                Icons.check,
                                color: Colors.white,
                                size: 14,
                              )
                            : null,
                      ),
                                             const SizedBox(width: 12),
                       Expanded(
                         child: Row(
                           mainAxisAlignment: MainAxisAlignment.spaceBetween,
                           children: [
                             Text(
                               mode.displayName,
                               style: TextStyle(
                                 color: isSelected ? Colors.white : Colors.grey[300],
                                 fontSize: 16,
                                 fontWeight: isSelected ? FontWeight.w600 : FontWeight.normal,
                               ),
                             ),
                             Text(
                               _getAmountForMode(mode),
                               style: TextStyle(
                                 color: const Color(0xFF10B981), // Green color
                                 fontSize: 16,
                                 fontWeight: FontWeight.bold,
                               ),
                             ),
                           ],
                         ),
                       ),
                    ],
                  ),
                ),
              ),
            );
          }).toList(),
        ),
      ],
    );
  }
} 