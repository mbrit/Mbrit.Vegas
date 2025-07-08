import 'package:flutter/material.dart';

class UnitSizeSelector extends StatefulWidget {
  final List<int> unitSizes;
  final int initialIndex;
  final ValueChanged<int>? onChanged;
  final String label;

  const UnitSizeSelector({
    super.key,
    this.unitSizes = const [5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 75, 100, 125, 150, 175, 200, 250, 300, 350, 400, 450, 500],
    this.initialIndex = 4, // Default to $25
    this.onChanged,
    this.label = 'Unit Size',
  });

  @override
  State<UnitSizeSelector> createState() => _UnitSizeSelectorState();
}

class _UnitSizeSelectorState extends State<UnitSizeSelector> {
  late int _currentIndex;

  @override
  void initState() {
    super.initState();
    _currentIndex = widget.initialIndex;
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Row(
          children: [
            Icon(Icons.casino, color: Colors.grey[400], size: 16),
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
        Container(
          padding: const EdgeInsets.all(16),
          decoration: BoxDecoration(
            color: const Color(0xFF2D3748),
            borderRadius: BorderRadius.circular(12),
            border: Border.all(color: Colors.grey[600]!),
          ),
          child: Row(
            mainAxisAlignment: MainAxisAlignment.spaceBetween,
            children: [
              // Left arrow button
              IconButton(
                onPressed: _currentIndex > 0
                    ? () {
                        setState(() {
                          _currentIndex--;
                        });
                        widget.onChanged?.call(widget.unitSizes[_currentIndex]);
                      }
                    : null,
                icon: Icon(
                  Icons.chevron_left,
                  color: _currentIndex > 0 ? Colors.white : Colors.grey[600],
                  size: 28,
                ),
              ),
              
              // Center display
              Expanded(
                child: Center(
                                       child: Text(
                       '\$${widget.unitSizes[_currentIndex]}',
                       style: const TextStyle(
                         color: Color(0xFFF59E0B), // Orange color
                         fontSize: 24,
                         fontWeight: FontWeight.bold,
                       ),
                     ),
                ),
              ),
              
              // Right arrow button
              IconButton(
                onPressed: _currentIndex < widget.unitSizes.length - 1
                    ? () {
                        setState(() {
                          _currentIndex++;
                        });
                        widget.onChanged?.call(widget.unitSizes[_currentIndex]);
                      }
                    : null,
                icon: Icon(
                  Icons.chevron_right,
                  color: _currentIndex < widget.unitSizes.length - 1 ? Colors.white : Colors.grey[600],
                  size: 28,
                ),
              ),
            ],
          ),
        ),
      ],
    );
  }
} 