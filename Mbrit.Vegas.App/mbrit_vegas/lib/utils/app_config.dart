class AppConfig {
  // Server configuration - shared across all services
  static const String baseUrl = 'http://192.168.1.248:61655';

  // API endpoints
  static String get stringsVersionEndpoint =>
      '$baseUrl/application/strings/en-us/version';
  static String get stringsEndpoint => '$baseUrl/application/strings/en-us';

  // Walk Game endpoints
  static String get walkGameProjectionEndpoint =>
      '$baseUrl/walk-game/projection';
  static String get walkGameSimulateEndpoint => '$baseUrl/walk-game/simulate';
  static String get walkGameStartEndpoint => '$baseUrl/walk-game/start';
  static String get walkGameResultsEndpoint => '$baseUrl/walk-game/results';
  static String get walkGameAbandonEndpoint => '$baseUrl/walk-game';

  // App configuration
  static const String defaultLocale = 'en-us';
  static const String appName = 'The Vegas Walk Method';

  // Timeout settings
  static const Duration networkTimeout = Duration(seconds: 10);
  static const Duration cacheTimeout = Duration(days: 1);

  // Debug settings
  static const bool enableDebugLogging = true;
}
