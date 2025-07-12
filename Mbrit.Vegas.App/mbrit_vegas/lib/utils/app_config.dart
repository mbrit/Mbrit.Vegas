class AppConfig {
  // Server configuration
  static const String baseUrl = 'https://your-api-server.com'; // Update with your actual server URL
  
  // API endpoints
  static String get stringsVersionEndpoint => '$baseUrl/application/strings/en-us/version';
  static String get stringsEndpoint => '$baseUrl/application/strings/en-us';
  
  // App configuration
  static const String defaultLocale = 'en-us';
  static const String appName = 'The Vegas Walk Method';
  
  // Timeout settings
  static const Duration networkTimeout = Duration(seconds: 10);
  static const Duration cacheTimeout = Duration(days: 1);
  
  // Debug settings
  static const bool enableDebugLogging = true;
} 