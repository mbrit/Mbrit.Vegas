import 'package:flutter/material.dart';
import 'models/run_state.dart';
import 'models/location.dart';
import 'models/hand_result.dart';
import 'models/investment_state.dart';
import 'widgets/unit_size_selector.dart';
import 'widgets/play_mode_selector.dart';
import 'widgets/investment_display.dart';
import 'widgets/hail_mary_selector.dart';
import 'widgets/configurable_table.dart';
import 'models/outcomes.dart';

class RunPage extends StatefulWidget {
  const RunPage({super.key});

  @override
  State<RunPage> createState() => _RunPageState();
}

class _RunPageState extends State<RunPage> {
  late RunState _runState;
  late TextEditingController _nameController;
  bool _isRunSetupExpanded = true;
  
  // Unit size state
  int _selectedUnitSize = 100;
  
  // Play mode state
  PlayMode _selectedPlayMode = PlayMode.balanced;
  
  // Hail Mary state
  int _selectedHailMary = 1;
  
  // Simulate & Test state
  bool _isSimulateTestExpanded = false;
  
  // Currency symbol state
  String _currencySymbol = '\$';

  @override
  void initState() {
    super.initState();
    _runState = RunState.defaultRun();
    _nameController = TextEditingController(text: _runState.name);
    _selectedUnitSize = _runState.unitSize;
    _selectedPlayMode = _runState.playMode;
    _currencySymbol = _runState.currencySymbol;
  }

  @override
  void dispose() {
    _nameController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFF0F1419),
      appBar: AppBar(
        title: const Text(
          'Vegas Walk',
          style: TextStyle(
            color: Colors.white,
            fontWeight: FontWeight.bold,
          ),
        ),
        backgroundColor: const Color(0xFF1E3A8A),
        elevation: 0,
        iconTheme: const IconThemeData(color: Colors.white),
      ),
      body: Container(
        decoration: const BoxDecoration(
          gradient: LinearGradient(
            begin: Alignment.topCenter,
            end: Alignment.bottomCenter,
            colors: [
              Color(0xFF1E3A8A),
              Color(0xFF0F1419),
            ],
            stops: [0.0, 0.3],
          ),
        ),
        child: Padding(
          padding: const EdgeInsets.all(16.0),
          child: SingleChildScrollView(
            child: Column(
              children: [
                _buildRunSetupCard(),
                const SizedBox(height: 20),
              ],
            ),
          ),
        ),
      ),
    );
  }

  Widget _buildRunSetupCard() {
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
          // Header with dice icon, title, and expand/collapse button
          InkWell(
            onTap: () {
              setState(() {
                _isRunSetupExpanded = !_isRunSetupExpanded;
              });
            },
            child: Padding(
              padding: const EdgeInsets.all(20.0),
              child: Row(
                children: [
                  Container(
                    padding: const EdgeInsets.all(8),
                    decoration: BoxDecoration(
                      color: const Color(0xFF3B82F6),
                      borderRadius: BorderRadius.circular(8),
                    ),
                    child: const Icon(
                      Icons.casino,
                      color: Colors.white,
                      size: 20,
                    ),
                  ),
                  const SizedBox(width: 12),
                  const Text(
                    'Run Setup',
                    style: TextStyle(
                      fontSize: 20,
                      fontWeight: FontWeight.bold,
                      color: Colors.white,
                    ),
                  ),
                  const Spacer(),
                  Icon(
                    _isRunSetupExpanded ? Icons.expand_less : Icons.expand_more,
                    color: Colors.white,
                    size: 24,
                  ),
                ],
              ),
            ),
          ),
          // Collapsible content
          if (_isRunSetupExpanded)
            Padding(
              padding: const EdgeInsets.fromLTRB(20.0, 0.0, 20.0, 20.0),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  // Unit size
                  UnitSizeSelector(
                    initialIndex: 10, // Default to $100
                    onChanged: (value) {
                      setState(() {
                        _selectedUnitSize = value;
                        _runState = RunState(
                          name: _runState.name,
                          startTime: _runState.startTime,
                          location: _runState.location,
                          numHands: _runState.numHands,
                          currentHand: _runState.currentHand,
                          handResults: _runState.handResults,
                          investments: _runState.investments,
                          unitSize: value,
                          playMode: _runState.playMode,
                          currencySymbol: _runState.currencySymbol,
                        );
                      });
                    },
                  ),
                  
                  const SizedBox(height: 16),
                  
                  // Investments
                  InvestmentDisplay(
                    unitSize: _selectedUnitSize,
                    maxInvestment: _selectedUnitSize * 12, // Simple multiplication
                    currencySymbol: _currencySymbol,
                  ),
                  
                  const SizedBox(height: 16),
                  
                  // Play mode
                  PlayModeSelector(
                    unitSize: _selectedUnitSize,
                    currencySymbol: _currencySymbol,
                    initialMode: PlayMode.balanced,
                    onChanged: (mode) {
                      setState(() {
                        _selectedPlayMode = mode;
                      });
                    },
                  ),
                  
                  const SizedBox(height: 16),
                  
                  // Hail Mary
                  HailMarySelector(
                    initialValue: 1,
                    onChanged: (value) {
                      setState(() {
                        _selectedHailMary = value;
                      });
                    },
                  ),
                  const SizedBox(height: 24),
                  // Simulate & Test section header
                  const Text(
                    'Simulate & Test',
                    style: TextStyle(
                      fontSize: 20,
                      fontWeight: FontWeight.bold,
                      color: Colors.white,
                    ),
                  ),
                  const SizedBox(height: 12),
                  // Simulate & Test table (classic grid)
                  _buildSimulateTestTable(),
                ],
              ),
            ),
        ],
      ),
    );
  }

  Widget _buildInputField({
    required String label,
    required IconData icon,
    required Widget child,
  }) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Row(
          children: [
            Icon(icon, color: Colors.grey[400], size: 16),
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
        const SizedBox(height: 8),
        child,
      ],
    );
  }

  Widget _buildInfoRow({
    required String label,
    required IconData icon,
    required String value,
    required Color valueColor,
  }) {
    return Row(
      children: [
        Icon(icon, color: Colors.grey[400], size: 16),
        const SizedBox(width: 8),
        Text(
          '$label: ',
          style: TextStyle(
            color: Colors.grey[400],
            fontWeight: FontWeight.w500,
            fontSize: 14,
          ),
        ),
        Text(
          value,
          style: TextStyle(
            color: valueColor,
            fontWeight: FontWeight.bold,
            fontSize: 14,
          ),
        ),
      ],
    );
  }

  Widget _buildDropdownField({
    required String label,
    required IconData icon,
    required Location value,
    required List<Location> items,
    required ValueChanged<Location?> onChanged,
  }) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Row(
          children: [
            Icon(icon, color: Colors.grey[400], size: 16),
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
        const SizedBox(height: 8),
        Container(
          padding: const EdgeInsets.symmetric(horizontal: 12),
          decoration: BoxDecoration(
            color: const Color(0xFF2D3748),
            borderRadius: BorderRadius.circular(8),
            border: Border.all(color: Colors.grey[600]!),
          ),
          child: DropdownButtonHideUnderline(
            child: DropdownButton<Location>(
              value: value,
              dropdownColor: const Color(0xFF2D3748),
              style: const TextStyle(color: Colors.white),
              items: items.map((location) {
                return DropdownMenuItem(
                  value: location,
                  child: Text(location.displayName),
                );
              }).toList(),
              onChanged: onChanged,
            ),
          ),
        ),
      ],
    );
  }

  Widget _buildSimulateTestTable() {
    final currency = _currencySymbol;
    final maxInvestment = _runState.maxInvestment;
    final spike0p5 = _runState.spike0p5;
    final spike1 = _runState.spike1;
    final spike3 = _runState.spike3;
    final outcomes = Outcomes.defaultOutcomes;
    // Helper to format percentage (always positive)
    String fmtPct(double pct) {
      final val = (pct * 100).toStringAsFixed(2);
      return '$val%';
    }
    // Helper to format negative currency
    String negCurrency(num value) => '-$currency${value.abs()}';
    // Determine which rows to highlight based on play mode
    final playMode = _selectedPlayMode;
    Set<int> highlightedRows = {};
    if (playMode == PlayMode.goForBroke) {
      highlightedRows = {5};
    } else if (playMode == PlayMode.make50PercentProfit) {
      highlightedRows = {3};
    } else if (playMode == PlayMode.doubleYourMoney) {
      highlightedRows = {4};
    } else if (playMode == PlayMode.balanced) {
      highlightedRows = {3, 4};
    }
    // Helper functions for different play modes
    String get50PercentLabel() {
      return "50% Profit";
    }
    
    String get50PercentBlurb() {
      if (playMode == PlayMode.balanced) 
        return 'Can walk at $currency$spike0p5 profit, try to get to $currency$spike1';
      else if (playMode == PlayMode.make50PercentProfit) 
        return 'Walk at $currency$spike0p5 profit';
      else
        return '$currency$spike0p5';
    }
    
    Color get50PercentColor() {
      return playMode == PlayMode.make50PercentProfit || playMode == PlayMode.balanced ? Colors.green : Colors.grey;
    }
    
    String get100PercentLabel() {
      return "100% Profit";
    }
    
    String get100PercentBlurb() {
      if (playMode == PlayMode.doubleYourMoney || playMode == PlayMode.balanced) 
        return 'Walk at $currency$spike1 profit';
      else
        return '$currency$spike1';
    }
    
    Color get100PercentColor() {
      return playMode == PlayMode.doubleYourMoney || playMode == PlayMode.balanced ? Colors.green : Colors.grey;
    }
    
    String getMoreThan100PercentBlurb() {
      if (playMode == PlayMode.goForBroke) 
        return 'A chance to get from $currency$spike1 profit to $currency$spike3 profit';
      else
        return '$currency$spike1 to $currency$spike3';
    }
    
    Color getMoreThan100PercentColor() {
      return playMode == PlayMode.goForBroke ? Colors.green : Colors.grey;
    }
    
    // Table data: label, explanation (empty), currency, value, color
    final rows = [
      ['Big Loss', '', '${negCurrency(maxInvestment)} to ${negCurrency(maxInvestment ~/ 1.5)}', fmtPct(outcomes.majorBustPercentage), Colors.red],
      ['Small Loss', '', '${negCurrency(maxInvestment ~/ 1.5)} to ${negCurrency(0)}', fmtPct(outcomes.minorBustPercentage), Colors.orange],
      ['Evens', '', '${currency}0 to ${currency}$spike0p5', fmtPct(outcomes.evensPercentage), Colors.grey],
      [get50PercentLabel(), '', get50PercentBlurb(), fmtPct(outcomes.spike0p5Percentage), get50PercentColor()],
      [get100PercentLabel(), '', get100PercentBlurb(), fmtPct(outcomes.spike1Percentage), get100PercentColor()],
      ['More Than 100% Profit', '', getMoreThan100PercentBlurb(), fmtPct(outcomes.spike1PlusPercentage), getMoreThan100PercentColor()],
    ];
    return Table(
      border: TableBorder(
        top: BorderSide(color: Colors.grey[700]!, width: 1),
        bottom: BorderSide(color: Colors.grey[700]!, width: 1),
        left: BorderSide(color: Colors.grey[700]!, width: 1),
        right: BorderSide(color: Colors.grey[700]!, width: 1),
        verticalInside: BorderSide.none,
      ),
      columnWidths: const {
        0: FlexColumnWidth(2),
        1: IntrinsicColumnWidth(),
      },
      defaultVerticalAlignment: TableCellVerticalAlignment.middle,
      children: [
        for (int i = 0; i < rows.length; i++)
          TableRow(
            decoration: highlightedRows.contains(i)
                ? BoxDecoration(color: const Color(0xFF3B82F6).withOpacity(0.25))
                : null,
            children: [
              Padding(
                padding: const EdgeInsets.symmetric(vertical: 8, horizontal: 8),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(rows[i][0] as String, style: const TextStyle(color: Colors.white, fontWeight: FontWeight.bold, fontSize: 16)),
                    if ((rows[i][2] as String).isNotEmpty)
                      Padding(
                        padding: const EdgeInsets.only(top: 2.0),
                        child: Text(
                          rows[i][2] as String,
                          style: TextStyle(
                            color: rows[i].length > 4 ? rows[i][4] as Color : Colors.green,
                            fontSize: 18,
                            fontWeight: FontWeight.bold,
                          ),
                        ),
                      ),
                  ],
                ),
              ),
              Padding(
                padding: const EdgeInsets.symmetric(vertical: 8, horizontal: 8),
                child: Align(
                  alignment: Alignment.centerRight,
                  child: Text(
                    rows[i][3] as String,
                    style: const TextStyle(
                      color: Colors.white,
                      fontWeight: FontWeight.bold,
                      fontSize: 24, // Match investment box
                    ),
                  ),
                ),
              ),
            ],
          ),
      ],
    );
  }
} 