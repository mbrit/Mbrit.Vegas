import 'package:flutter/material.dart';
import 'models/run_state.dart';
import 'widgets/cash_progress_widget.dart';
import 'models/investment_state.dart';
import 'services/walk_game_service.dart';
import 'models/walk_game_setup_dto.dart';
import 'models/walk_game_state_dto.dart';
import 'widgets/vegas_action_button.dart';

class RunPage extends StatefulWidget {
  final RunState? runState;
  const RunPage({super.key, this.runState});

  @override
  State<RunPage> createState() => _RunPageState();
}

class _RunPageState extends State<RunPage> {
  late RunState _runState;
  late String _runName;
  int _selectedTab = 0;
  WalkGameStateDto? _walkGameState;

  @override
  void initState() {
    super.initState();
    _runState = widget.runState ?? RunState.defaultRun();
    _runName = _runState.name.isNotEmpty
        ? _runState.name
        : RunState.generateDefaultName(_runState.location, DateTime.now());
    _startGameOnServer();
  }

  Future<void> _startGameOnServer() async {
    print('=== REFRESH STARTED ===');
    try {
      final setup = WalkGameSetupDto(
        unit: _runState.unitSize,
        mode: WalkGameMode
            .unrestricted, // TODO: map from _runState.playMode if needed
        hailMaryCount: 0, // TODO: map from state if needed
      );
      final result = await WalkGameService.startGame(setup);
      setState(() {
        _walkGameState = result;
      });
      print('=== SERVER RESULT: $result ===');
    } catch (e) {
      print('=== REFRESH ERROR: $e ===');
    }
    print('=== REFRESH DONE ===');
  }

  void _onTabTapped(int index) {
    // TODO: Implement navigation for each tab
    setState(() {
      _selectedTab = index;
    });
  }

  Widget _buildDraftHandCard(hand) {
    final actions = hand.actions;
    final piles = hand.pilesBefore;
    final currency = _runState.currencySymbol;
    return Container(
      width: double.infinity,
      margin: const EdgeInsets.symmetric(vertical: 8),
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: const Color(0xFF2D3748),
        borderRadius: BorderRadius.circular(12),
        border: Border.all(color: Colors.grey[600]!),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            'Hand ${hand.index + 1}',
            style: const TextStyle(
              color: Colors.white,
              fontWeight: FontWeight.bold,
              fontSize: 16,
            ),
          ),
          const SizedBox(height: 8),
          Text(
            'Casino: ${hand.casino.name}',
            style: const TextStyle(color: Colors.white),
          ),
          Text(
            'Location: ${hand.casino.location.name}',
            style: const TextStyle(color: Colors.white),
          ),
          Text(
            'Game: ${hand.game.name}',
            style: const TextStyle(color: Colors.white),
          ),
          const SizedBox(height: 16),
          // In play amount
          Container(
            width: double.infinity,
            padding: const EdgeInsets.symmetric(vertical: 8),
            child: Text(
              'In play: ${piles?.inPlayUnits ?? 0} / $currency${piles?.inPlay ?? 0}',
              textAlign: TextAlign.center,
              style: const TextStyle(
                color: Colors.white,
                fontSize: 18,
                fontWeight: FontWeight.bold,
              ),
            ),
          ),
          const SizedBox(height: 16),
          // Show win/loss buttons if needsAnswer is true, otherwise show action buttons
          if (hand.needsAnswer) ...[
            // Dotted separator line
            Container(
              width: double.infinity,
              height: 1,
              decoration: BoxDecoration(
                border: Border(
                  top: BorderSide(
                    color: Colors.grey[600]!,
                    width: 1,
                    style: BorderStyle.solid,
                  ),
                ),
              ),
            ),
            const SizedBox(height: 16),
            // What happened? label
            Container(
              width: double.infinity,
              padding: const EdgeInsets.symmetric(vertical: 8),
              child: const Text(
                '"What happened?"',
                textAlign: TextAlign.center,
                style: TextStyle(
                  color: Colors.white,
                  fontSize: 18,
                  fontWeight: FontWeight.bold,
                ),
              ),
            ),
            const SizedBox(height: 16),
            // Win/Loss buttons
            Row(
              children: [
                Expanded(
                  child: VegasActionButton(
                    label: "Won",
                    icon: Icons.check_circle,
                    backgroundColor: const Color(0xFF10B981),
                    onPressed: () {
                      // TODO: Send win result to server
                      print('Player won hand ${hand.index + 1}');
                    },
                  ),
                ),
                const SizedBox(width: 12),
                Expanded(
                  child: VegasActionButton(
                    label: "Lost",
                    icon: Icons.cancel,
                    backgroundColor: const Color(0xFFEF4444),
                    onPressed: () {
                      // TODO: Send loss result to server
                      print('Player lost hand ${hand.index + 1}');
                    },
                  ),
                ),
              ],
            ),
          ] else ...[
            // Put in button - only show if canPutInAndPlay is true
            if (actions?.canPutInAndPlay == true) ...[
              VegasActionButton(
                label:
                    "Put in ${actions?.canPutInUnits ?? 0} ${(actions?.canPutInUnits ?? 0) == 1 ? 'Unit' : 'Units'} / $currency${(actions?.canPutInUnits ?? 0) * (piles?.investable != null && piles?.investableUnits != 0 ? piles!.investable ~/ piles.investableUnits : 0)}",
                icon: Icons.add_circle_outline,
                backgroundColor: const Color(0xFF10B981),
                onPressed: () {},
              ),
              const SizedBox(height: 12),
            ],
            // Hail Mary button - only show if canHailMary is true
            if (actions?.canHailMary == true) ...[
              VegasActionButton(
                label:
                    "'Hail Mary' bet ${actions?.canHailMaryUnits ?? 0} ${(actions?.canHailMaryUnits ?? 0) == 1 ? 'Unit' : 'Units'} / $currency${(actions?.canHailMaryUnits ?? 0) * (piles?.investable != null && piles?.investableUnits != 0 ? piles!.investable ~/ piles.investableUnits : 0)}",
                icon: Icons.flash_on,
                backgroundColor: const Color(0xFFF59E0B),
                onPressed: () {},
              ),
              const SizedBox(height: 12),
            ],
            // Play On button - only show if canPlay is true
            if (actions?.canPlay == true) ...[
              VegasActionButton(
                label: "Play On",
                icon: Icons.play_arrow,
                backgroundColor: const Color(0xFF10B981),
                onPressed: () {},
              ),
              const SizedBox(height: 12),
            ],
            // Walk button - only show if canWalk is true
            if (actions?.canWalk == true) ...[
              VegasActionButton(
                label: "Walk with $currency${piles?.profit ?? 0}",
                icon: Icons.emoji_events,
                backgroundColor: const Color(0xFFFF9800),
                onPressed: () {},
              ),
            ],
          ],
        ],
      ),
    );
  }

  Widget _buildFinalHandCard(hand) {
    final actions = hand.actions;
    final piles = hand.pilesBefore;
    final currency = _runState.currencySymbol;
    return Container(
      width: double.infinity,
      margin: const EdgeInsets.symmetric(vertical: 8),
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: const Color(0xFF2D3748),
        borderRadius: BorderRadius.circular(12),
        border: Border.all(color: Colors.grey[600]!),
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: [
          Text(
            'Hand ${hand.index + 1}',
            style: const TextStyle(
              color: Colors.white,
              fontWeight: FontWeight.bold,
              fontSize: 16,
            ),
          ),
          const SizedBox(height: 8),
          Text(
            'Casino: ${hand.casino.name}',
            style: const TextStyle(color: Colors.white),
          ),
          Text(
            'Location: ${hand.casino.location.name}',
            style: const TextStyle(color: Colors.white),
          ),
          Text(
            'Game: ${hand.game.name}',
            style: const TextStyle(color: Colors.white),
          ),
          const SizedBox(height: 16),
          // In play amount
          Container(
            width: double.infinity,
            padding: const EdgeInsets.symmetric(vertical: 8),
            child: Text(
              'In play: ${piles?.inPlayUnits ?? 0} / $currency${piles?.inPlay ?? 0}',
              textAlign: TextAlign.center,
              style: const TextStyle(
                color: Colors.white,
                fontSize: 18,
                fontWeight: FontWeight.bold,
              ),
            ),
          ),
          const SizedBox(height: 16),
          // Show win/loss buttons if needsAnswer is true, otherwise show action buttons
          if (hand.needsAnswer) ...[
            // Dotted separator line
            Container(
              width: double.infinity,
              height: 1,
              decoration: BoxDecoration(
                border: Border(
                  top: BorderSide(
                    color: Colors.grey[600]!,
                    width: 1,
                    style: BorderStyle.solid,
                  ),
                ),
              ),
            ),
            const SizedBox(height: 16),
            // What happened? label
            Container(
              width: double.infinity,
              padding: const EdgeInsets.symmetric(vertical: 8),
              child: const Text(
                '"What happened?"',
                textAlign: TextAlign.center,
                style: TextStyle(
                  color: Colors.white,
                  fontSize: 18,
                  fontWeight: FontWeight.bold,
                ),
              ),
            ),
            const SizedBox(height: 16),
            // Win/Loss buttons
            Row(
              children: [
                Expanded(
                  child: VegasActionButton(
                    label: "Won",
                    icon: Icons.check_circle,
                    backgroundColor: const Color(0xFF10B981),
                    onPressed: () {
                      // TODO: Send win result to server
                      print('Player won hand ${hand.index + 1}');
                    },
                  ),
                ),
                const SizedBox(width: 12),
                Expanded(
                  child: VegasActionButton(
                    label: "Lost",
                    icon: Icons.cancel,
                    backgroundColor: const Color(0xFFEF4444),
                    onPressed: () {
                      // TODO: Send loss result to server
                      print('Player lost hand ${hand.index + 1}');
                    },
                  ),
                ),
              ],
            ),
          ] else ...[
            // Put in button - only show if canPutInAndPlay is true
            if (actions?.canPutInAndPlay == true) ...[
              VegasActionButton(
                label:
                    "Put in ${actions?.canPutInUnits ?? 0} ${(actions?.canPutInUnits ?? 0) == 1 ? 'Unit' : 'Units'} / $currency${(actions?.canPutInUnits ?? 0) * (piles?.investable != null && piles?.investableUnits != 0 ? piles!.investable ~/ piles.investableUnits : 0)}",
                icon: Icons.add_circle_outline,
                backgroundColor: const Color(0xFF10B981),
                onPressed: () {},
              ),
              const SizedBox(height: 12),
            ],
            // Hail Mary button - only show if canHailMary is true
            if (actions?.canHailMary == true) ...[
              VegasActionButton(
                label:
                    "'Hail Mary' bet ${actions?.canHailMaryUnits ?? 0} ${(actions?.canHailMaryUnits ?? 0) == 1 ? 'Unit' : 'Units'} / $currency${(actions?.canHailMaryUnits ?? 0) * (piles?.investable != null && piles?.investableUnits != 0 ? piles!.investable ~/ piles.investableUnits : 0)}",
                icon: Icons.flash_on,
                backgroundColor: const Color(0xFFF59E0B),
                onPressed: () {},
              ),
              const SizedBox(height: 12),
            ],
            // Play On button - only show if canPlay is true
            if (actions?.canPlay == true) ...[
              VegasActionButton(
                label: "Play On",
                icon: Icons.play_arrow,
                backgroundColor: const Color(0xFF10B981),
                onPressed: () {},
              ),
              const SizedBox(height: 12),
            ],
            // Walk button - only show if canWalk is true
            if (actions?.canWalk == true) ...[
              VegasActionButton(
                label: "Walk with $currency${piles?.profit ?? 0}",
                icon: Icons.emoji_events,
                backgroundColor: const Color(0xFFFF9800),
                onPressed: () {},
              ),
            ],
          ],
        ],
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    final piles = _walkGameState?.piles;
    final profit = piles?.profit ?? 0;
    final profitMin = -600;
    final profitMax = 600;
    final profitProgress = ((profit - profitMin) / (profitMax - profitMin))
        .clamp(0.0, 1.0);
    Color profitColor;
    if (profit > 0) {
      profitColor = Colors.green;
    } else if (profit < 0) {
      profitColor = Colors.orange;
    } else {
      profitColor = Colors.white;
    }
    return Scaffold(
      backgroundColor: const Color(0xFF0F1419),
      appBar: AppBar(
        title: const Text('Vegas Walk'),
        backgroundColor: const Color(0xFF1E3A8A),

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
          child: RefreshIndicator(
            onRefresh: () async {
              await _startGameOnServer();
            },
            child: SingleChildScrollView(
              physics: const AlwaysScrollableScrollPhysics(),
              child: Column(
                children: [
                  // Cover image/title container
                  Container(
                    height: 180,
                    margin: const EdgeInsets.only(bottom: 16.0),
                    decoration: BoxDecoration(
                      borderRadius: BorderRadius.circular(10),
                      image: const DecorationImage(
                        image: AssetImage(
                          'assets/casino-lasvegas-strip-flamingo.png',
                        ),
                        fit: BoxFit.cover,
                      ),
                    ),
                    alignment: Alignment.bottomLeft,
                    child: Container(
                      width: double.infinity,
                      padding: const EdgeInsets.symmetric(
                        vertical: 16.0,
                        horizontal: 20.0,
                      ),
                      decoration: BoxDecoration(
                        color: Colors.black.withOpacity(0.45),
                        borderRadius: const BorderRadius.only(
                          bottomLeft: Radius.circular(10),
                          bottomRight: Radius.circular(10),
                        ),
                      ),
                      child: Text(
                        _runName,
                        style: const TextStyle(
                          fontSize: 22,
                          fontWeight: FontWeight.bold,
                          color: Colors.white,
                        ),
                        textAlign: TextAlign.left,
                      ),
                    ),
                  ),
                  // All content below the cover image/title container has been removed
                  // Progress Section
                  Container(
                    padding: const EdgeInsets.all(16.0),
                    margin: const EdgeInsets.only(bottom: 16.0),
                    decoration: BoxDecoration(
                      color: const Color(0xFF2D3748),
                      borderRadius: BorderRadius.circular(12),
                      border: Border.all(color: Colors.grey[600]!),
                    ),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Row(
                          children: [
                            Icon(
                              Icons.timeline,
                              color: Colors.grey[400],
                              size: 16,
                            ),
                            const SizedBox(width: 8),
                            Text(
                              'Progress',
                              style: TextStyle(
                                color: Colors.grey[400],
                                fontWeight: FontWeight.w500,
                                fontSize: 14,
                              ),
                            ),
                          ],
                        ),
                        const SizedBox(height: 12),
                        Row(
                          children: [
                            Expanded(
                              child: Column(
                                crossAxisAlignment: CrossAxisAlignment.start,
                                children: [
                                  Text(
                                    'Current Hand',
                                    style: TextStyle(
                                      color: Colors.grey[400],
                                      fontSize: 12,
                                    ),
                                  ),
                                  Text(
                                    '${_runState.currentHand + 1}',
                                    style: const TextStyle(
                                      color: Colors.white,
                                      fontSize: 24,
                                      fontWeight: FontWeight.bold,
                                    ),
                                  ),
                                ],
                              ),
                            ),
                            Expanded(
                              child: Column(
                                crossAxisAlignment: CrossAxisAlignment.end,
                                children: [
                                  Text(
                                    'Total Hands',
                                    style: TextStyle(
                                      color: Colors.grey[400],
                                      fontSize: 12,
                                    ),
                                  ),
                                  Text(
                                    '${_runState.numHands}',
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
                        ),
                        const SizedBox(height: 16),
                        LinearProgressIndicator(
                          value:
                              (_runState.currentHand + 1) / _runState.numHands,
                          backgroundColor: Colors.grey[700],
                          valueColor: const AlwaysStoppedAnimation<Color>(
                            Color(0xFF10B981),
                          ),
                        ),
                        const SizedBox(height: 20),
                        // Investables block (hidden for now)
                        Visibility(
                          visible: false,
                          child: Column(
                            crossAxisAlignment: CrossAxisAlignment.stretch,
                            children: [
                              Padding(
                                padding: const EdgeInsets.only(bottom: 4.0),
                                child: Align(
                                  alignment: Alignment.centerLeft,
                                  child: Text(
                                    'Investables',
                                    style: TextStyle(
                                      color: Colors.grey[400],
                                      fontSize: 12,
                                    ),
                                  ),
                                ),
                              ),
                              Center(
                                child: Text(
                                  '12 units / \$${_runState.maxInvestment}',
                                  style: const TextStyle(
                                    color: Colors.white,
                                    fontSize: 24,
                                    fontWeight: FontWeight.bold,
                                  ),
                                  textAlign: TextAlign.center,
                                ),
                              ),
                              const SizedBox(height: 16),
                              Directionality(
                                textDirection: TextDirection.rtl,
                                child: LinearProgressIndicator(
                                  value:
                                      _runState.investments
                                          .where(
                                            (i) =>
                                                i == InvestmentState.available,
                                          )
                                          .length /
                                      12.0,
                                  backgroundColor: Colors.grey[700],
                                  valueColor:
                                      const AlwaysStoppedAnimation<Color>(
                                        Color(0xFF10B981),
                                      ),
                                  minHeight: 4,
                                  semanticsLabel: 'Investables',
                                ),
                              ),
                            ],
                          ),
                        ),
                        // After the progress bar, reduce space before Piles
                        const SizedBox(height: 8),
                        Align(
                          alignment: Alignment.centerLeft,
                          child: Text(
                            'Piles',
                            style: TextStyle(
                              color: Colors.grey[400],
                              fontSize: 12,
                            ),
                          ),
                        ),
                        const SizedBox(height: 4),
                        Table(
                          border: TableBorder.all(color: Colors.grey, width: 1),
                          defaultVerticalAlignment:
                              TableCellVerticalAlignment.middle,
                          children: [
                            TableRow(
                              children: [
                                Padding(
                                  padding: EdgeInsets.symmetric(
                                    horizontal: 8.0,
                                  ),
                                  child: Text(
                                    'Investables',
                                    style: TextStyle(color: Colors.white),
                                  ),
                                ),
                                Padding(
                                  padding: EdgeInsets.symmetric(
                                    horizontal: 8.0,
                                  ),
                                  child: Align(
                                    alignment: Alignment.centerRight,
                                    child: Text(
                                      '${piles?.investableUnits ?? '-'} units',
                                      style: TextStyle(color: Colors.white),
                                    ),
                                  ),
                                ),
                                Padding(
                                  padding: EdgeInsets.symmetric(
                                    horizontal: 8.0,
                                  ),
                                  child: Align(
                                    alignment: Alignment.centerRight,
                                    child: Text(
                                      '${_runState.currencySymbol}${piles?.investable ?? '-'}',
                                      style: TextStyle(color: Colors.white),
                                    ),
                                  ),
                                ),
                              ],
                            ),
                            TableRow(
                              children: [
                                Padding(
                                  padding: EdgeInsets.symmetric(
                                    horizontal: 8.0,
                                  ),
                                  child: Text(
                                    'In play',
                                    style: TextStyle(color: Colors.white),
                                  ),
                                ),
                                Padding(
                                  padding: EdgeInsets.symmetric(
                                    horizontal: 8.0,
                                  ),
                                  child: Align(
                                    alignment: Alignment.centerRight,
                                    child: Text(
                                      '${piles?.inPlayUnits ?? '-'} units',
                                      style: TextStyle(color: Colors.white),
                                    ),
                                  ),
                                ),
                                Padding(
                                  padding: EdgeInsets.symmetric(
                                    horizontal: 8.0,
                                  ),
                                  child: Align(
                                    alignment: Alignment.centerRight,
                                    child: Text(
                                      '${_runState.currencySymbol}${piles?.inPlay ?? '-'}',
                                      style: TextStyle(color: Colors.white),
                                    ),
                                  ),
                                ),
                              ],
                            ),
                            TableRow(
                              children: [
                                Padding(
                                  padding: EdgeInsets.symmetric(
                                    horizontal: 8.0,
                                  ),
                                  child: Text(
                                    'Banked',
                                    style: TextStyle(color: Colors.white),
                                  ),
                                ),
                                Padding(
                                  padding: EdgeInsets.symmetric(
                                    horizontal: 8.0,
                                  ),
                                  child: Align(
                                    alignment: Alignment.centerRight,
                                    child: Text(
                                      '${piles?.bankedUnits ?? '-'} units',
                                      style: TextStyle(color: Colors.white),
                                    ),
                                  ),
                                ),
                                Padding(
                                  padding: EdgeInsets.symmetric(
                                    horizontal: 8.0,
                                  ),
                                  child: Align(
                                    alignment: Alignment.centerRight,
                                    child: Text(
                                      '${_runState.currencySymbol}${piles?.banked ?? '-'}',
                                      style: TextStyle(color: Colors.white),
                                    ),
                                  ),
                                ),
                              ],
                            ),
                            TableRow(
                              children: [
                                Padding(
                                  padding: EdgeInsets.symmetric(
                                    horizontal: 8.0,
                                  ),
                                  child: Text(
                                    'Cash in hand',
                                    style: TextStyle(
                                      color: Colors.white,
                                      fontWeight: FontWeight.bold,
                                    ),
                                  ),
                                ),
                                Padding(
                                  padding: EdgeInsets.symmetric(
                                    horizontal: 8.0,
                                  ),
                                  child: Align(
                                    alignment: Alignment.centerRight,
                                    child: Text(
                                      '${piles?.cashInHandUnits ?? '-'} units',
                                      style: TextStyle(
                                        color: Colors.white,
                                        fontWeight: FontWeight.bold,
                                      ),
                                    ),
                                  ),
                                ),
                                Padding(
                                  padding: EdgeInsets.symmetric(
                                    horizontal: 8.0,
                                  ),
                                  child: Align(
                                    alignment: Alignment.centerRight,
                                    child: Text(
                                      '${_runState.currencySymbol}${piles?.cashInHand ?? '-'}',
                                      style: TextStyle(
                                        color: Colors.white,
                                        fontWeight: FontWeight.bold,
                                      ),
                                    ),
                                  ),
                                ),
                              ],
                            ),
                          ],
                        ),
                        // Profit block
                        const SizedBox(height: 24),
                        Align(
                          alignment: Alignment.centerLeft,
                          child: Text(
                            'Profit',
                            style: TextStyle(
                              color: Colors.grey[400],
                              fontSize: 12,
                            ),
                          ),
                        ),
                        Center(
                          child: Text(
                            '${_runState.currencySymbol}${profit}',
                            style: TextStyle(
                              color: profitColor,
                              fontSize: 24,
                              fontWeight: FontWeight.bold,
                            ),
                            textAlign: TextAlign.center,
                          ),
                        ),
                        const SizedBox(height: 16),
                        LinearProgressIndicator(
                          value: profitProgress,
                          backgroundColor: Colors.grey[700],
                          valueColor: AlwaysStoppedAnimation<Color>(
                            profitColor,
                          ),
                          minHeight: 4,
                          semanticsLabel: 'Profit',
                        ),
                      ],
                    ),
                  ),
                  // Hands Section
                  if (_walkGameState?.hands != null)
                    ..._walkGameState!.hands.reversed.map((hand) {
                      if (hand.isDraft) {
                        return _buildDraftHandCard(hand);
                      } else {
                        return _buildFinalHandCard(hand);
                      }
                    }),
                ],
              ),
            ),
          ),
        ),
      ),
      // No bottomNavigationBar here
    );
  }
}
