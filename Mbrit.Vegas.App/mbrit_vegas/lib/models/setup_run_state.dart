import 'location.dart';
import 'hand_result.dart';
import 'investment_state.dart';
import '../widgets/play_mode_selector.dart';

class SetupRunState {
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

  SetupRunState({
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

  // Default constructor with Las Vegas Strip
  SetupRunState.defaultRun()
    : name = "",
      startTime = DateTime.now(),
      location = Location.lasVegasStrip,
      numHands = 25,
      currentHand = 0,
      handResults = List.generate(25, (_) => HandResult.pending),
      investments = List.generate(12, (_) => InvestmentState.available),
      unitSize = 50,
      playMode = PlayMode.balanced,
      currencySymbol = '\$';

  String get formattedStartTime {
    return '${startTime.month}/${startTime.day}/${startTime.year} ${startTime.hour}:${startTime.minute.toString().padLeft(2, '0')}';
  }

  // Read-only property for MaxPutIns
  int get maxPutIns => 12;

  // Calculated properties for investment amounts
  int get maxInvestment => unitSize * maxPutIns;
  int get spike0p5 => (unitSize * maxPutIns) ~/ 2;
  int get spike1 => unitSize * maxPutIns;
  int get spike3 => unitSize * maxPutIns * 3;
}
