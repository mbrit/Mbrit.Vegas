import 'location.dart';
import 'hand_result.dart';
import 'investment_state.dart';
import '../widgets/play_mode_selector.dart';
import 'setup_run_state.dart';

class RunState {
  final String name;
  final DateTime startTime;
  final Location location;
  final int numHands;
  final int currentHand;
  final List<HandResult> handResults;
  final List<InvestmentState> investments;
  final int unitSize;
  final PlayMode playMode;
  final String currencySymbol;

  RunState({
    required this.name,
    required this.startTime,
    required this.location,
    required this.numHands,
    required this.currentHand,
    required this.handResults,
    required this.investments,
    required this.unitSize,
    required this.playMode,
    required this.currencySymbol,
  });

  factory RunState.fromSetup(SetupRunState setup) {
    return RunState(
      name: setup.name,
      startTime: setup.startTime,
      location: setup.location,
      numHands: setup.numHands,
      currentHand: setup.currentHand,
      handResults: List<HandResult>.from(setup.handResults),
      investments: List<InvestmentState>.from(setup.investments),
      unitSize: setup.unitSize,
      playMode: setup.playMode,
      currencySymbol: setup.currencySymbol,
    );
  }

  // Default constructor with Las Vegas Strip
  RunState.defaultRun()
    : name = 'Evening Walk at Flamingo, 7th July 2025',
      startTime = DateTime.now(),
      location = Location.lasVegasStrip,
      numHands = 25,
      currentHand = 0,
      handResults = List.generate(25, (_) => HandResult.pending),
      investments = List.generate(12, (_) => InvestmentState.available),
      unitSize = 50,
      playMode = PlayMode.balanced,
      currencySymbol = '\$';

  static String generateDefaultName(Location location, DateTime dateTime) {
    final hour = dateTime.hour;
    String period;
    if (hour >= 5 && hour < 12) {
      period = 'Morning';
    } else if (hour >= 12 && hour < 17) {
      period = 'Afternoon';
    } else if (hour >= 17 && hour < 22) {
      period = 'Evening';
    } else {
      period = 'Late Night';
    }
    final months = [
      'Jan',
      'Feb',
      'Mar',
      'Apr',
      'May',
      'Jun',
      'Jul',
      'Aug',
      'Sep',
      'Oct',
      'Nov',
      'Dec',
    ];
    final dateStr =
        '${dateTime.day}/${months[dateTime.month - 1]}/${dateTime.year}';
    return '$period walk in ${location.name} on $dateStr';
  }

  String get formattedStartTime {
    return '${startTime.month}/${startTime.day}/${startTime.year} ${startTime.hour}:${startTime.minute.toString().padLeft(2, '0')}';
  }

  int get maxPutIns => 12;
  int get maxInvestment => unitSize * maxPutIns;
  int get spike0p5 => (unitSize * maxPutIns) ~/ 2;
  int get spike1 => unitSize * maxPutIns;
  int get spike3 => unitSize * maxPutIns * 3;
}
