class Outcomes {
  final double majorBustPercentage;
  final double minorBustPercentage;
  final double evensPercentage;
  final double spike0p5Percentage;
  final double spike1Percentage;
  final double spike1PlusPercentage;
  final double averageCoinIn;

  const Outcomes({
    required this.majorBustPercentage,
    required this.minorBustPercentage,
    required this.evensPercentage,
    required this.spike0p5Percentage,
    required this.spike1Percentage,
    required this.spike1PlusPercentage,
    this.averageCoinIn = 0.0,
  });

  static const defaultOutcomes = Outcomes(
    majorBustPercentage: 0.3548,
    minorBustPercentage: 0.2010,
    evensPercentage: 0.1784,
    spike0p5Percentage: 0.0592,
    spike1Percentage: 0.2066,
    spike1PlusPercentage: 0.0,
  );
} 