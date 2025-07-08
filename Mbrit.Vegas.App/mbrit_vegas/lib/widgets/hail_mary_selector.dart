import 'package:flutter/material.dart';

class HailMarySelector extends StatefulWidget {
  final int initialValue;
  final ValueChanged<int>? onChanged;
  final String label;

  const HailMarySelector({
    super.key,
    this.initialValue = 1,
    this.onChanged,
    this.label = 'Hail Mary',
  });

  @override
  State<HailMarySelector> createState() => _HailMarySelectorState();
}

class _HailMarySelectorState extends State<HailMarySelector> {
  late int _selectedValue;

  @override
  void initState() {
    super.initState();
    _selectedValue = widget.initialValue;
  }

  @override
  Widget build(BuildContext context) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Row(
          children: [
            Icon(Icons.favorite, color: Colors.grey[400], size: 16),
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
        const SizedBox(height: 8),
        Text(
          'An optional bet that increases your chances of hitting your goal. This is part of your planned investment, but done at a time of your choosing',
          style: TextStyle(
            color: Colors.grey[500],
            fontSize: 12,
          ),
        ),
        const SizedBox(height: 12),
        Row(
          mainAxisAlignment: MainAxisAlignment.spaceEvenly,
          children: [0, 1, 2].map((value) {
            final isSelected = value == _selectedValue;
            return Expanded(
              child: Padding(
                padding: const EdgeInsets.symmetric(horizontal: 4.0),
                child: InkWell(
                  onTap: () {
                    setState(() {
                      _selectedValue = value;
                    });
                    widget.onChanged?.call(value);
                  },
                  child: Container(
                    padding: const EdgeInsets.symmetric(vertical: 12, horizontal: 8),
                    decoration: BoxDecoration(
                      color: isSelected ? const Color(0xFF3B82F6) : const Color(0xFF2D3748),
                      borderRadius: BorderRadius.circular(8),
                      border: Border.all(
                        color: isSelected ? const Color(0xFF3B82F6) : Colors.grey[600]!,
                        width: isSelected ? 2 : 1,
                      ),
                    ),
                    child: Center(
                      child: Text(
                        value.toString(),
                        style: TextStyle(
                          color: isSelected ? Colors.white : Colors.grey[300],
                          fontSize: 16,
                          fontWeight: isSelected ? FontWeight.bold : FontWeight.normal,
                        ),
                      ),
                    ),
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