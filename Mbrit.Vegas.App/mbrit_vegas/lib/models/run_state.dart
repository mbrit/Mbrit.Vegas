import 'location.dart';
import 'hand_result.dart';
import 'investment_state.dart';

class RunState {
  final String name;
  final DateTime startTime;
  final Location location;
  final int numHands;
  final int currentHand;
  final List<HandResult> handResults;
  final List<InvestmentState> investments;

  RunState({
    required this.name,
    required this.startTime,
    required this.location,
    required this.numHands,
    required this.currentHand,
    required this.handResults,
    required this.investments,
  });

  // Default constructor with Las Vegas Strip
  RunState.defaultRun()
      : name = 'New Run',
        startTime = DateTime.now(),
        location = Location.lasVegasStrip,
        numHands = 25,
        currentHand = 0,
        handResults = List.generate(25, (_) => HandResult.pending),
        investments = List.generate(12, (_) => InvestmentState.available);

  String get formattedStartTime {
    return '${startTime.month}/${startTime.day}/${startTime.year} ${startTime.hour}:${startTime.minute.toString().padLeft(2, '0')}';
  }
} 