import 'package:flutter/material.dart';
import 'models/run_state.dart';
import 'widgets/cash_progress_widget.dart';
import 'models/investment_state.dart';
import 'services/walk_game_service.dart';
import 'models/walk_game_setup_dto.dart';
import 'models/walk_game_state_dto.dart';
import 'widgets/vegas_action_button.dart';
import 'utils/mode_mapper.dart';
import 'widgets/play_mode_selector.dart';
import 'dart:convert';

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
        mode: ModeMapper.playModeToWalkGameMode(_runState.playMode),
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

  // Feature flag for showing the 'In play:' label
  static const bool showInPlayLabel = false;

  Widget _buildDraftHandCard(hand) {
    final actions = hand.actions;
    final piles = _walkGameState?.piles;
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
          Row(
            children: [
              // Prominent hand index badge
              Container(
                width: 36,
                height: 36,
                decoration: BoxDecoration(
                  color: Colors.blue,
                  shape: BoxShape.circle,
                ),
                alignment: Alignment.center,
                child: Text(
                  '${hand.index + 1}',
                  style: const TextStyle(
                    color: Colors.white,
                    fontSize: 18,
                    fontWeight: FontWeight.bold,
                  ),
                ),
              ),
              const Spacer(),
              // Change badge from 'DRAFT' to 'Current Hand'
              Container(
                padding: const EdgeInsets.symmetric(
                  horizontal: 10,
                  vertical: 4,
                ),
                decoration: BoxDecoration(
                  color: Color(0xFF2563EB),
                  borderRadius: BorderRadius.circular(8),
                ),
                child: const Text(
                  'Current Hand',
                  style: TextStyle(
                    color: Colors.white,
                    fontWeight: FontWeight.bold,
                    fontSize: 12,
                  ),
                ),
              ),
              const Spacer(),
              // Win/Loss indicator badge
              if (hand.outcome != null)
                Container(
                  padding: const EdgeInsets.symmetric(
                    horizontal: 8,
                    vertical: 4,
                  ),
                  decoration: BoxDecoration(
                    color: hand.outcome.toString().toLowerCase().contains('win')
                        ? Color(0xFF10B981)
                        : Color(0xFFEF4444),
                    borderRadius: BorderRadius.circular(8),
                  ),
                  child: Row(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      Icon(
                        hand.outcome.toString().toLowerCase().contains('win')
                            ? Icons.check_circle
                            : Icons.cancel,
                        color: Colors.white,
                        size: 16,
                      ),
                      const SizedBox(width: 4),
                      Text(
                        hand.outcome.toString().toLowerCase().contains('win')
                            ? 'WON'
                            : 'LOST',
                        style: const TextStyle(
                          color: Colors.white,
                          fontWeight: FontWeight.bold,
                          fontSize: 12,
                        ),
                      ),
                    ],
                  ),
                ),
            ],
          ),
          const SizedBox(height: 8),
          Text(hand.casino.name, style: const TextStyle(color: Colors.white)),
          Text(
            hand.casino.location.name,
            style: const TextStyle(color: Colors.white),
          ),
          Text(hand.game.name, style: const TextStyle(color: Colors.white)),
          const SizedBox(height: 16),
          if (showInPlayLabel)
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
              child: Text(
                'What happened on that ${piles?.inPlayUnits ?? 0} Unit ($currency${piles?.inPlay ?? 0}) bet?',
                textAlign: TextAlign.center,
                style: const TextStyle(
                  color: Colors.white,
                  fontSize: 18,
                  fontWeight: FontWeight.bold,
                ),
              ),
            ),
            const SizedBox(height: 16),
            // Win/Loss buttons
            Container(
              margin: const EdgeInsets.only(bottom: 16),
              child: Row(
                children: [
                  Expanded(
                    child: VegasActionButton(
                      label: "Won",
                      icon: Icons.check_circle,
                      backgroundColor: const Color(0xFF10B981),
                      onPressed: () async {
                        if (_walkGameState?.token == null) return;
                        final token = _walkGameState!.token;
                        final handIndex = hand.index;
                        final result = await WalkGameService().postOutcome(
                          token: token,
                          handIndex: handIndex,
                          outcome: WinLoseDrawType.win,
                        );
                        setState(() {
                          _walkGameState = result;
                        });
                        print(
                          '=== RAW RESPONSE (WON): ${jsonEncode(result.toJson())} ===',
                        );
                      },
                    ),
                  ),
                  const SizedBox(width: 12),
                  Expanded(
                    child: VegasActionButton(
                      label: "Lost",
                      icon: Icons.cancel,
                      backgroundColor: const Color(0xFFEF4444),
                      onPressed: () async {
                        if (_walkGameState?.token == null) return;
                        final token = _walkGameState!.token;
                        final handIndex = hand.index;
                        final result = await WalkGameService().postOutcome(
                          token: token,
                          handIndex: handIndex,
                          outcome: WinLoseDrawType.lose,
                        );
                        setState(() {
                          _walkGameState = result;
                        });
                        print(
                          '=== RAW RESPONSE (LOST): ${jsonEncode(result.toJson())} ===',
                        );
                      },
                    ),
                  ),
                ],
              ),
            ),
          ] else ...[
            // Instructions (if present)
            if (actions?.instructions != null &&
                actions!.instructions!.isNotEmpty) ...[
              Padding(
                padding: const EdgeInsets.only(bottom: 16.0),
                child: Text(
                  actions.instructions!,
                  textAlign: TextAlign.center,
                  style: const TextStyle(
                    color: Colors.white,
                    fontSize: 16,
                    fontWeight: FontWeight.w500,
                  ),
                ),
              ),
            ],
            // Play button
            if (actions?.canPlay == true) ...[
              VegasActionButton(
                label:
                    "Play a Total of ${actions?.playUnits ?? 0} ${(actions?.playUnits ?? 0) == 1 ? 'Unit' : 'Units'} ($currency${actions?.play ?? 0})",
                icon: null,
                backgroundColor: const Color(0xFF10B981),
                onPressed: () async {
                  if (_walkGameState?.token == null) return;
                  final token = _walkGameState!.token;
                  final handIndex = hand.index;
                  final result = await WalkGameService().postAction(
                    token: token,
                    handIndex: handIndex,
                    action: 'play',
                  );
                  setState(() {
                    _walkGameState = result;
                  });
                  print(
                    '=== RAW RESPONSE (ACTION): ${jsonEncode(result.toJson())} ===',
                  );
                },
              ),
              const SizedBox(height: 12),
            ],
            // Hail Mary button
            if (actions?.canHailMary == true) ...[
              VegasActionButton(
                label: "Hail Mary",
                icon: Icons.flash_on,
                backgroundColor: const Color(0xFFF59E0B),
                onPressed: () {},
              ),
              const SizedBox(height: 12),
            ],
            // Walk button
            if (actions?.canWalk == true) ...[
              VegasActionButton(
                label:
                    "Walk with ${piles?.profitUnits ?? 0} ${(piles?.profitUnits ?? 0) == 1 ? 'Unit' : 'Units'} ($currency${piles?.profit ?? 0}) profit",
                icon: Icons.emoji_events,
                backgroundColor: const Color(0xFFFF9800),
                onPressed: () async {
                  if (_walkGameState?.token == null) return;
                  final token = _walkGameState!.token;
                  final handIndex = hand.index;
                  final result = await WalkGameService().postAction(
                    token: token,
                    handIndex: handIndex,
                    action: 'walk',
                  );
                  setState(() {
                    _walkGameState = result;
                  });
                  print(
                    '=== RAW RESPONSE (WALK): ${jsonEncode(result.toJson())} ===',
                  );
                },
              ),
              const SizedBox(height: 12),
            ],
          ],
          // Abandon this run link - always show
          Center(
            child: TextButton(
              onPressed: () async {
                final confirmed = await showDialog<bool>(
                  context: context,
                  builder: (context) => AlertDialog(
                    title: const Text('Are you sure?'),
                    content: const Text(
                      'Do you really want to abandon this run?',
                    ),
                    actions: [
                      TextButton(
                        onPressed: () => Navigator.of(context).pop(false),
                        child: const Text('No'),
                      ),
                      TextButton(
                        onPressed: () => Navigator.of(context).pop(true),
                        child: const Text('Yes'),
                      ),
                    ],
                  ),
                );
                if (confirmed == true) {
                  if (_walkGameState?.token != null) {
                    await WalkGameService().abandonRun(
                      token: _walkGameState!.token,
                    );
                  }
                  Navigator.of(context).popUntil((route) => route.isFirst);
                }
              },
              child: Text(
                'Abandon this run',
                style: TextStyle(
                  color: Colors.grey[400],
                  fontWeight: FontWeight.w500,
                ),
              ),
            ),
          ),
        ],
      ),
    );
  }

  Widget _buildFinalHandCard(hand) {
    final actions = hand.actions;
    final piles = _walkGameState?.piles;
    final currency = _runState.currencySymbol;
    // Determine win/loss indicator
    Widget? resultIndicator;
    if (hand.action != null) {
      // Check if the action indicates a win or loss
      final actionStr = hand.action.toString().toLowerCase();
      if (actionStr.contains('win') || actionStr.contains('won')) {
        resultIndicator = Row(
          children: const [
            Icon(Icons.check_circle, color: Color(0xFF10B981), size: 20),
            SizedBox(width: 6),
            Text(
              'WON',
              style: TextStyle(
                color: Color(0xFF10B981),
                fontWeight: FontWeight.bold,
              ),
            ),
          ],
        );
      } else if (actionStr.contains('lose') || actionStr.contains('lost')) {
        resultIndicator = Row(
          children: const [
            Icon(Icons.cancel, color: Color(0xFFEF4444), size: 20),
            SizedBox(width: 6),
            Text(
              'LOST',
              style: TextStyle(
                color: Color(0xFFEF4444),
                fontWeight: FontWeight.bold,
              ),
            ),
          ],
        );
      }
    }
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
          Row(
            children: [
              // Prominent hand index badge
              Container(
                width: 36,
                height: 36,
                decoration: BoxDecoration(
                  color: Colors.grey[600],
                  shape: BoxShape.circle,
                ),
                alignment: Alignment.center,
                child: Text(
                  '${hand.index + 1}',
                  style: const TextStyle(
                    color: Colors.white,
                    fontSize: 18,
                    fontWeight: FontWeight.bold,
                  ),
                ),
              ),
              const Spacer(),
              // Win/Loss indicator badge
              if (hand.outcome != null)
                Container(
                  padding: const EdgeInsets.symmetric(
                    horizontal: 8,
                    vertical: 4,
                  ),
                  decoration: BoxDecoration(
                    color:
                        hand.outcome.toString().toLowerCase().contains('win') ||
                            hand.outcome.toString().toLowerCase().contains(
                              'won',
                            )
                        ? Color(0xFF10B981)
                        : Color(0xFFEF4444),
                    borderRadius: BorderRadius.circular(8),
                  ),
                  child: Row(
                    mainAxisSize: MainAxisSize.min,
                    children: [
                      Icon(
                        hand.outcome.toString().toLowerCase().contains('win') ||
                                hand.outcome.toString().toLowerCase().contains(
                                  'won',
                                )
                            ? Icons.check_circle
                            : Icons.cancel,
                        color: Colors.white,
                        size: 16,
                      ),
                      const SizedBox(width: 4),
                      Text(
                        hand.outcome.toString().toLowerCase().contains('win') ||
                                hand.outcome.toString().toLowerCase().contains(
                                  'won',
                                )
                            ? 'WON'
                            : 'LOST',
                        style: const TextStyle(
                          color: Colors.white,
                          fontWeight: FontWeight.bold,
                          fontSize: 12,
                        ),
                      ),
                    ],
                  ),
                ),
            ],
          ),
          const SizedBox(height: 8),
          Text(hand.casino.name, style: const TextStyle(color: Colors.white)),
          Text(
            hand.casino.location.name,
            style: const TextStyle(color: Colors.white),
          ),
          Text(hand.game.name, style: const TextStyle(color: Colors.white)),
          const SizedBox(height: 16),
          // Show result indicator if available
          if (resultIndicator != null) ...[
            const SizedBox(height: 8),
            resultIndicator,
          ],
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
              child: Text(
                '"What happened?" (${piles?.inPlayUnits ?? 0} units / $currency${piles?.inPlay ?? 0})',
                textAlign: TextAlign.center,
                style: const TextStyle(
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

  Widget _buildDiceDisplay(int handNumber) {
    return Text(
      '$handNumber',
      style: const TextStyle(
        color: Colors.white,
        fontSize: 18,
        fontWeight: FontWeight.bold,
      ),
    );
  }

  int _getCurrentHandIndex() {
    if (_walkGameState?.hands == null || _walkGameState!.hands.isEmpty) {
      return 0; // Default to 0 if no hands data
    }
    // Find the draft hand index
    final draftIndex = _walkGameState!.hands.indexWhere((hand) => hand.isDraft);
    return draftIndex >= 0 ? draftIndex : 0;
  }

  String _buildWinLossString() {
    if (_walkGameState?.hands == null) return '–';

    final completedHands = _walkGameState!.hands
        .where((hand) => !hand.isDraft && hand.outcome != null)
        .toList();

    if (completedHands.isEmpty) {
      return '–';
    }

    return completedHands
        .map((hand) {
          return hand.outcome.toString().toLowerCase().contains('win')
              ? 'W'
              : 'L';
        })
        .join('');
  }

  Widget _buildWinLossCircles() {
    if (_walkGameState?.hands == null) {
      return const Text(
        '–',
        style: TextStyle(
          color: Colors.white,
          fontSize: 16,
          fontFamily: 'monospace',
        ),
        textAlign: TextAlign.center,
      );
    }

    final completedHands = _walkGameState!.hands
        .where((hand) => !hand.isDraft && hand.outcome != null)
        .toList();

    if (completedHands.isEmpty) {
      return const Text(
        '–',
        style: TextStyle(
          color: Colors.white,
          fontSize: 16,
          fontFamily: 'monospace',
        ),
        textAlign: TextAlign.center,
      );
    }

    return Row(
      mainAxisAlignment: MainAxisAlignment.start,
      children: completedHands.asMap().entries.map((entry) {
        final index = entry.key;
        final hand = entry.value;
        final isWin = hand.outcome.toString().toLowerCase().contains('win');
        return Row(
          mainAxisSize: MainAxisSize.min,
          children: [
            Padding(
              padding: const EdgeInsets.symmetric(horizontal: 1.5),
              child: Container(
                width: 6,
                height: 6,
                decoration: BoxDecoration(
                  color: isWin ? Colors.green : Colors.red,
                  shape: BoxShape.circle,
                ),
              ),
            ),
            // Add space after every 5th circle
            if ((index + 1) % 5 == 0 && index < completedHands.length - 1)
              const SizedBox(width: 5),
          ],
        );
      }).toList(),
    );
  }

  @override
  Widget build(BuildContext context) {
    final piles = _walkGameState?.piles;
    final profit = piles?.profit ?? 0;
    final profitMin = -600;
    final profitMax = _walkGameState?.spike1 ?? 600;
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
              if (_walkGameState?.token != null) {
                final token = _walkGameState!.token;
                final result = await WalkGameService().getState(token: token);
                print(
                  '=== RAW RESPONSE (REFRESH): ${jsonEncode(result.toJson())} ===',
                );
                setState(() {
                  _walkGameState = result;
                });
              } else {
                await _startGameOnServer();
              }
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
                            Icon(Icons.trending_up, color: Colors.white),
                            const SizedBox(width: 8),
                            const Text(
                              'Progress',
                              style: TextStyle(
                                color: Colors.white,
                                fontWeight: FontWeight.bold,
                                fontSize: 18,
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
                                    '${_getCurrentHandIndex() + 1}',
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
                              (_getCurrentHandIndex() + 1) / _runState.numHands,
                          backgroundColor: Colors.grey[700],
                          valueColor: const AlwaysStoppedAnimation<Color>(
                            Color(0xFF10B981),
                          ),
                        ),
                        // Piles section
                        const SizedBox(height: 24),
                        Align(
                          alignment: Alignment.centerLeft,
                          child: Text(
                            'Stacks',
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
                            '${piles?.profitUnits ?? 0} units (${_runState.currencySymbol}${profit})',
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
                        const SizedBox(height: 16),
                        // Profit targets table
                        Table(
                          border: TableBorder.all(color: Colors.grey, width: 1),
                          columnWidths: const {
                            0: FlexColumnWidth(1),
                            1: FlexColumnWidth(1),
                          },
                          children: [
                            TableRow(
                              children: [
                                Padding(
                                  padding: const EdgeInsets.all(4.0),
                                  child: Text(
                                    '50% Profit',
                                    style: TextStyle(
                                      color: Colors.grey[300],
                                      fontWeight: FontWeight.bold,
                                    ),
                                    textAlign: TextAlign.center,
                                  ),
                                ),
                                Padding(
                                  padding: const EdgeInsets.all(4.0),
                                  child: Text(
                                    '100% Profit',
                                    style: TextStyle(
                                      color: Colors.grey[300],
                                      fontWeight: FontWeight.bold,
                                    ),
                                    textAlign: TextAlign.center,
                                  ),
                                ),
                              ],
                            ),
                            TableRow(
                              children: [
                                Padding(
                                  padding: const EdgeInsets.all(4.0),
                                  child: Row(
                                    mainAxisAlignment: MainAxisAlignment.center,
                                    children: [
                                      Text(
                                        '${_walkGameState?.spike0p5Units ?? 0} units (${_runState.currencySymbol}${_walkGameState?.spike0p5 ?? 0})',
                                        style: TextStyle(
                                          color:
                                              profit >=
                                                  (_walkGameState?.spike0p5 ??
                                                      0)
                                              ? Colors.green
                                              : Colors.white,
                                        ),
                                      ),
                                      if (profit >=
                                          (_walkGameState?.spike0p5 ?? 0))
                                        Padding(
                                          padding: const EdgeInsets.only(
                                            left: 4,
                                          ),
                                          child: Icon(
                                            Icons.check_circle,
                                            color: Colors.green,
                                            size: 16,
                                          ),
                                        ),
                                    ],
                                  ),
                                ),
                                Padding(
                                  padding: const EdgeInsets.all(4.0),
                                  child: Row(
                                    mainAxisAlignment: MainAxisAlignment.center,
                                    children: [
                                      Text(
                                        '${_walkGameState?.spike1Units ?? 0} units (${_runState.currencySymbol}${_walkGameState?.spike1 ?? 0})',
                                        style: TextStyle(
                                          color:
                                              profit >=
                                                  (_walkGameState?.spike1 ?? 0)
                                              ? Colors.green
                                              : Colors.white,
                                        ),
                                      ),
                                      if (profit >=
                                          (_walkGameState?.spike1 ?? 0))
                                        Padding(
                                          padding: const EdgeInsets.only(
                                            left: 4,
                                          ),
                                          child: Icon(
                                            Icons.check_circle,
                                            color: Colors.green,
                                            size: 16,
                                          ),
                                        ),
                                    ],
                                  ),
                                ),
                              ],
                            ),
                          ],
                        ),
                      ],
                    ),
                  ),
                  // Probability Space Container
                  Container(
                    width: double.infinity,
                    margin: const EdgeInsets.only(bottom: 16.0),
                    padding: const EdgeInsets.all(16.0),
                    decoration: BoxDecoration(
                      color: const Color(0xFF2D3748),
                      borderRadius: BorderRadius.circular(12),
                      border: Border.all(color: Colors.grey[600]!),
                    ),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Row(
                          children: const [
                            Icon(Icons.bubble_chart, color: Colors.white),
                            SizedBox(width: 8),
                            Text(
                              'Probability space',
                              style: TextStyle(
                                color: Colors.white,
                                fontWeight: FontWeight.bold,
                                fontSize: 18,
                              ),
                            ),
                          ],
                        ),
                        SizedBox(height: 12),
                        const SizedBox(height: 8),
                        _buildWinLossCircles(),
                        // Show when probability space will be available
                        if (_walkGameState?.hasProbabilitySpace == false &&
                            _walkGameState?.probabilitySpaceAvailableAt != null)
                          Padding(
                            padding: const EdgeInsets.only(top: 12),
                            child: Text(
                              'Probability space analysis available at hand ${_walkGameState!.probabilitySpaceAvailableAt}',
                              style: TextStyle(
                                color: Colors.grey[400],
                                fontSize: 14,
                              ),
                              textAlign: TextAlign.center,
                            ),
                          ),
                        const SizedBox(height: 16),
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
