import 'dart:async';
import 'package:flutter/material.dart';
import 'models/setup_run_state.dart';
import 'models/location.dart';
import 'models/hand_result.dart';
import 'models/investment_state.dart';
import 'models/outcomes.dart';
import 'models/walk_game_setup_dto.dart';
import 'models/walk_game_projection_dto.dart';
import 'services/walk_game_service.dart';
import 'utils/mode_mapper.dart';
import 'widgets/unit_size_selector.dart';
import 'widgets/play_mode_selector.dart';
import 'widgets/investment_display.dart';
import 'widgets/hail_mary_selector.dart';
import 'widgets/configurable_table.dart';
import 'models/run_state.dart';
import 'run_page.dart';
import 'widgets/gambling_help.dart';
import 'main.dart';

class SetupRunPage extends StatefulWidget {
  final bool fromSplash;
  const SetupRunPage({super.key, this.fromSplash = false});

  @override
  State<SetupRunPage> createState() => _SetupRunPageState();
}

class _SetupRunPageState extends State<SetupRunPage> {
  late SetupRunState _runState;
  late TextEditingController _nameController;
  bool _isRunSetupExpanded = true;

  // Play mode state
  PlayMode _selectedPlayMode = PlayMode.balanced;

  // Hail Mary state
  int _selectedHailMary = 1;

  // Simulate & Test state
  bool _isSimulateTestExpanded = false;

  // Currency symbol state
  String _currencySymbol = '\$';

  // API state
  bool _isLoading = false;
  bool _isDebouncedLoading = false;
  WalkGameProjectionDto? _projection;
  final WalkGameService _walkGameService = WalkGameService();

  // Debounce timers
  Timer? _unitSizeDebounceTimer;
  Timer? _hailMaryDebounceTimer;
  Timer? _refreshDebounceTimer;
  int _selectedTab = 0;

  void _onTabTapped(int index) {
    // TODO: Implement navigation for each tab
    setState(() {
      _selectedTab = index;
    });
  }

  @override
  void initState() {
    super.initState();
    _runState = SetupRunState.defaultRun();
    _nameController = TextEditingController(text: _runState.name);

    _selectedPlayMode = _runState.playMode;
    _currencySymbol = _runState.currencySymbol;

    // Fetch initial projection from server
    _fetchWalkGameProjection();
  }

  @override
  void dispose() {
    _nameController.dispose();
    _walkGameService.dispose();
    _unitSizeDebounceTimer?.cancel();
    _hailMaryDebounceTimer?.cancel();
    _refreshDebounceTimer?.cancel();
    super.dispose();
  }

  Future<void> _fetchWalkGameProjection() async {
    setState(() => _isLoading = true);

    try {
      final setupDto = WalkGameSetupDto(
        unit: _runState.unitSize,
        mode: ModeMapper.playModeToWalkGameMode(_selectedPlayMode),
        hailMaryCount: _selectedHailMary,
      );
      final projection = await _walkGameService.setupWalkGame(setupDto);

      setState(() {
        _projection = projection;
        _isLoading = false;
      });
    } catch (e) {
      setState(() => _isLoading = false);
      print('Error fetching projection: $e');
    }
  }

  Future<void> _fetchWalkGameProjectionDebounced() async {
    setState(() => _isDebouncedLoading = true);

    try {
      final setupDto = WalkGameSetupDto(
        unit: _runState.unitSize,
        mode: ModeMapper.playModeToWalkGameMode(_selectedPlayMode),
        hailMaryCount: _selectedHailMary,
      );
      final projection = await _walkGameService.setupWalkGame(setupDto);

      setState(() {
        _projection = projection;
        _isDebouncedLoading = false;
      });
    } catch (e) {
      setState(() => _isDebouncedLoading = false);
      print('Error fetching projection: $e');
    }
  }

  WalkOutcomesBucketDto? getCurrentOutcomes() {
    if (_projection == null) return null;

    return _projection!.outcomes
        .firstWhere(
          (item) =>
              item.mode == ModeMapper.playModeToWalkGameMode(_selectedPlayMode),
          orElse: () => _projection!.outcomes.first,
        )
        .outcomes;
  }

  @override
  Widget build(BuildContext context) {
    Widget page = Scaffold(
      backgroundColor: const Color(0xFF0F1419),
      appBar: AppBar(
        title: Row(
          children: const [
            Icon(Icons.casino, color: Colors.white, size: 24),
            SizedBox(width: 8),
            Text('Setup Walk'),
          ],
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
            colors: [Color(0xFF1E3A8A), Color(0xFF0F1419)],
            stops: [0.0, 0.3],
          ),
        ),
        child: Padding(
          padding: const EdgeInsets.all(16.0),
          child: SingleChildScrollView(
            child: Column(
              children: [
                _buildRunSetupCard(),
                const SizedBox(height: 24),
                const GamblingHelp(),
                const SizedBox(height: 20),
              ],
            ),
          ),
        ),
      ),
      // No bottomNavigationBar here
    );
    if (widget.fromSplash) {
      return WillPopScope(
        onWillPop: () async {
          Navigator.of(context).pushAndRemoveUntil(
            MaterialPageRoute(
              builder: (context) => const MainScaffold(initialTab: 0),
            ),
            (route) => false,
          );
          return false;
        },
        child: page,
      );
    } else {
      return page;
    }
  }

  Widget _buildRunSetupCard() {
    return Container(
      decoration: BoxDecoration(
        gradient: const LinearGradient(
          colors: [Color(0xFF2D3748), Color(0xFF1A202C)],
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
      child: Padding(
        padding: const EdgeInsets.fromLTRB(20.0, 20.0, 20.0, 20.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // All the setup controls and summary go here, no header or collapse logic
            // Unit size
            UnitSizeSelector(
              initialIndex: 9, // Default to $50
              onChanged: (value) {
                setState(() {
                  _runState = SetupRunState(
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
                _fetchWalkGameProjectionDebounced();
              },
            ),
            const SizedBox(height: 16),
            // Investments
            InvestmentDisplay(
              unitSize: _runState.unitSize,
              maxInvestment: _runState.maxInvestment,
              currencySymbol: _currencySymbol,
            ),
            const SizedBox(height: 16),
            // Play mode
            PlayModeSelector(
              unitSize: _runState.unitSize,
              currencySymbol: _currencySymbol,
              initialMode: _runState.playMode,
              onChanged: (mode) {
                setState(() {
                  _runState = SetupRunState(
                    name: _runState.name,
                    startTime: _runState.startTime,
                    location: _runState.location,
                    numHands: _runState.numHands,
                    currentHand: _runState.currentHand,
                    handResults: _runState.handResults,
                    investments: _runState.investments,
                    unitSize: _runState.unitSize,
                    playMode: mode,
                    currencySymbol: _runState.currencySymbol,
                  );
                  _selectedPlayMode = mode;
                });
                // Do NOT call _fetchWalkGameProjectionDebounced() here
              },
            ),
            const SizedBox(height: 16),
            // Hail Mary
            HailMarySelector(
              initialValue: _selectedHailMary,
              onChanged: (value) {
                setState(() {
                  _selectedHailMary = value;
                });
                _fetchWalkGameProjectionDebounced();
              },
            ),
            const SizedBox(height: 24),
            // Simulate & Test section header
            Row(
              children: [
                const Text(
                  'Simulate & Test',
                  style: TextStyle(
                    fontSize: 20,
                    fontWeight: FontWeight.bold,
                    color: Colors.white,
                  ),
                ),
                const Spacer(),
                IconButton(
                  icon: Icon(Icons.refresh, color: Colors.white),
                  tooltip: 'Refresh projections',
                  onPressed: _isLoading ? null : _fetchWalkGameProjection,
                ),
              ],
            ),
            const SizedBox(height: 12),
            // Average profit display inside Simulate & Test section
            _buildAverageProfitDisplay(),
            const SizedBox(height: 16),
            // Simulate & Test table (classic grid)
            _buildSimulateTestTable(),
            const SizedBox(height: 24),
            // Start Run button
            _buildStartRunButton(),
            const SizedBox(height: 24),
          ],
        ),
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

    // Use server data if available
    final outcomes = getCurrentOutcomes();

    // Show loading indicator if no data yet (only for immediate loading, not debounced)
    if (_isLoading || outcomes == null) {
      return Container(
        padding: const EdgeInsets.all(20),
        decoration: BoxDecoration(
          color: const Color(0xFF2D3748),
          borderRadius: BorderRadius.circular(8),
          border: Border.all(color: Colors.grey[700]!),
        ),
        child: Row(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            SizedBox(
              width: 20,
              height: 20,
              child: CircularProgressIndicator(
                strokeWidth: 2,
                valueColor: AlwaysStoppedAnimation<Color>(Colors.grey[400]!),
              ),
            ),
            const SizedBox(width: 12),
            Text(
              'Loading projections...',
              style: TextStyle(color: Colors.grey[400], fontSize: 16),
            ),
          ],
        ),
      );
    }
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
      return playMode == PlayMode.make50PercentProfit ||
              playMode == PlayMode.balanced
          ? Colors.green
          : Colors.grey;
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
      return playMode == PlayMode.doubleYourMoney ||
              playMode == PlayMode.balanced
          ? Colors.green
          : Colors.grey;
    }

    String getMoreThan100PercentBlurb() {
      if (playMode == PlayMode.goForBroke)
        return 'A chance to more profit between $currency$spike1 and $currency$spike3';
      else
        return '$currency$spike1 to $currency$spike3';
    }

    Color getMoreThan100PercentColor() {
      return playMode == PlayMode.goForBroke ? Colors.green : Colors.grey;
    }

    // Table data: label, explanation (empty), currency, value, color
    final rows = [
      [
        'Big Loss',
        '',
        '${negCurrency(maxInvestment)} to ${negCurrency(maxInvestment ~/ 1.5)}',
        fmtPct(outcomes.majorBustPercentage),
        Colors.red,
      ],
      [
        'Small Loss',
        '',
        '${negCurrency(maxInvestment ~/ 1.5)} to ${negCurrency(0)}',
        fmtPct(outcomes.minorBustPercentage),
        Colors.orange,
      ],
      [
        'Evens',
        '',
        '${currency}0 to ${currency}$spike0p5',
        fmtPct(outcomes.evensPercentage),
        Colors.grey,
      ],
      [
        get50PercentLabel(),
        '',
        get50PercentBlurb(),
        fmtPct(outcomes.spike0p5Percentage),
        get50PercentColor(),
      ],
      [
        get100PercentLabel(),
        '',
        get100PercentBlurb(),
        fmtPct(outcomes.spike1Percentage),
        get100PercentColor(),
      ],
      [
        'More Than 100% Profit',
        '',
        getMoreThan100PercentBlurb(),
        fmtPct(outcomes.spike1PlusPercentage),
        getMoreThan100PercentColor(),
      ],
    ];
    return Table(
      border: TableBorder(
        top: BorderSide(color: Colors.grey[700]!, width: 1),
        bottom: BorderSide(color: Colors.grey[700]!, width: 1),
        left: BorderSide(color: Colors.grey[700]!, width: 1),
        right: BorderSide(color: Colors.grey[700]!, width: 1),
        verticalInside: BorderSide.none,
      ),
      columnWidths: const {0: FlexColumnWidth(2), 1: IntrinsicColumnWidth()},
      defaultVerticalAlignment: TableCellVerticalAlignment.middle,
      children: [
        for (int i = 0; i < rows.length; i++)
          TableRow(
            decoration: highlightedRows.contains(i)
                ? BoxDecoration(
                    color: const Color(0xFF3B82F6).withOpacity(0.25),
                  )
                : null,
            children: [
              Padding(
                padding: const EdgeInsets.symmetric(vertical: 8, horizontal: 8),
                child: Column(
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Text(
                      rows[i][0] as String,
                      style: const TextStyle(
                        color: Colors.white,
                        fontWeight: FontWeight.bold,
                        fontSize: 16,
                      ),
                    ),
                    if ((rows[i][2] as String).isNotEmpty)
                      Padding(
                        padding: const EdgeInsets.only(top: 2.0),
                        child: Text(
                          rows[i][2] as String,
                          style: TextStyle(
                            color: rows[i].length > 4
                                ? rows[i][4] as Color
                                : Colors.green,
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

  Widget _buildAverageProfitDisplay() {
    final currency = _currencySymbol;
    final outcomes = getCurrentOutcomes();

    // Don't show anything if loading or no data yet (only for immediate loading, not debounced)
    if (_isLoading || outcomes == null) {
      return const SizedBox.shrink();
    }

    return Container(
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: const Color(0xFF2D3748),
        borderRadius: BorderRadius.circular(8),
        border: Border.all(color: Colors.grey[600]!),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Row(
            children: [
              Icon(Icons.trending_up, color: Colors.grey[400], size: 16),
              const SizedBox(width: 8),
              Text(
                'Average profit (when won): ',
                style: TextStyle(
                  color: Colors.grey[400],
                  fontWeight: FontWeight.w500,
                  fontSize: 14,
                ),
              ),
              Text(
                '$currency${outcomes.averageProfitWhenWon.toStringAsFixed(0)}',
                style: TextStyle(
                  color: Colors.green,
                  fontWeight: FontWeight.bold,
                  fontSize: 18,
                ),
              ),
            ],
          ),
          const SizedBox(height: 8),
          Row(
            children: [
              Icon(Icons.monetization_on, color: Colors.grey[400], size: 16),
              const SizedBox(width: 8),
              Text(
                'Average coin-in: ',
                style: TextStyle(
                  color: Colors.grey[400],
                  fontWeight: FontWeight.w500,
                  fontSize: 14, // Match 'Average profit (when won):' label
                ),
              ),
              Text(
                '$currency${outcomes.averageCoinIn.toStringAsFixed(0)}',
                style: TextStyle(
                  color: const Color(0xFF059669), // Darker shade of green
                  fontWeight: FontWeight.bold,
                  fontSize: 18, // Match value size
                ),
              ),
            ],
          ),
        ],
      ),
    );
  }

  Widget _buildStartRunButton() {
    return Container(
      width: double.infinity,
      height: 60,
      decoration: BoxDecoration(
        gradient: const LinearGradient(
          colors: [Color(0xFF10B981), Color(0xFF059669)],
          begin: Alignment.topLeft,
          end: Alignment.bottomRight,
        ),
        borderRadius: BorderRadius.circular(12),
        boxShadow: [
          BoxShadow(
            color: const Color(0xFF10B981).withOpacity(0.3),
            blurRadius: 12,
            offset: const Offset(0, 4),
          ),
        ],
      ),
      child: Material(
        color: Colors.transparent,
        child: InkWell(
          borderRadius: BorderRadius.circular(12),
          onTap: () {
            final runState = RunState.fromSetup(_runState);
            Navigator.push(
              context,
              MaterialPageRoute(
                builder: (context) => RunPage(runState: runState),
              ),
            );
          },
          child: Container(
            padding: const EdgeInsets.symmetric(horizontal: 24),
            child: Row(
              mainAxisAlignment: MainAxisAlignment.center,
              children: [
                Container(
                  padding: const EdgeInsets.all(8),
                  decoration: BoxDecoration(
                    color: Colors.white.withOpacity(0.2),
                    borderRadius: BorderRadius.circular(8),
                  ),
                  child: const Icon(
                    Icons.play_arrow_rounded,
                    color: Colors.white,
                    size: 24,
                  ),
                ),
                const SizedBox(width: 12),
                const Text(
                  'START WALK',
                  style: TextStyle(
                    color: Colors.white,
                    fontSize: 18,
                    fontWeight: FontWeight.bold,
                    letterSpacing: 1.2,
                  ),
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
