import 'dart:convert';
import 'dart:io';
import 'package:flutter/services.dart';
import 'package:http/http.dart' as http;
import 'package:path_provider/path_provider.dart';
import 'app_config.dart';

class StringHelper {
  static Map<String, String> _strings = {};
  static bool _isInitialized = false;
  static void Function()? _onLoadedCallback;
  static const String _locale = AppConfig.defaultLocale;

  /// Initialize the string table by checking server version and loading appropriate strings
  static Future<void> initialize({void Function()? onLoaded}) async {
    if (_isInitialized) return;
    _onLoadedCallback = onLoaded;

    if (AppConfig.enableDebugLogging) {
      print('StringHelper: Initializing (flat dictionary mode)...');
    }

    try {
      await _loadStringsFromServer();
      _isInitialized = true;
      if (AppConfig.enableDebugLogging) {
        print('StringHelper: Initialized from server');
      }
    } catch (e) {
      if (AppConfig.enableDebugLogging) {
        print(
          'StringHelper: Server unavailable, loading from cache/assets: $e',
        );
      }
      await _loadStringsFromCacheOrAssets();
      _isInitialized = true;
      if (AppConfig.enableDebugLogging) {
        print('StringHelper: Initialized from cache/assets');
      }
    }

    // Dump all string paths and values
    if (AppConfig.enableDebugLogging) {
      print(
        "StringHelper: Dumping ' [33m${_strings.length} [0m' string paths and values:",
      );
      _strings.forEach((k, v) => print('  $k: $v'));
      print("Finished dumping strings.");
    }

    // Fire callback if set
    _onLoadedCallback?.call();
  }

  static Future<void> _loadStringsFromServer() async {
    if (AppConfig.enableDebugLogging) {
      print(
        'StringHelper: Checking server version at ${AppConfig.stringsVersionEndpoint}',
      );
    }
    final versionResponse = await http
        .get(
          Uri.parse(AppConfig.stringsVersionEndpoint),
          headers: {'Content-Type': 'application/json'},
        )
        .timeout(AppConfig.networkTimeout);
    if (versionResponse.statusCode == 404) {
      throw Exception('Strings endpoints not implemented on server yet');
    } else if (versionResponse.statusCode != 200) {
      throw Exception(
        'Failed to get version from server: ${versionResponse.statusCode}',
      );
    }
    final versionData =
        json.decode(versionResponse.body) as Map<String, dynamic>;
    final serverVersion = versionData['value'] as int;
    if (AppConfig.enableDebugLogging) {
      print('StringHelper: Server version: $serverVersion');
    }
    final localVersion = await _getLocalVersion();
    if (AppConfig.enableDebugLogging) {
      print('StringHelper: Local version: $localVersion');
    }
    if (localVersion != serverVersion) {
      if (AppConfig.enableDebugLogging) {
        print('StringHelper: Downloading new strings from server...');
      }
      final stringsResponse = await http
          .get(
            Uri.parse('${AppConfig.stringsEndpoint}/$serverVersion'),
            headers: {'Content-Type': 'application/json'},
          )
          .timeout(AppConfig.networkTimeout);
      if (stringsResponse.statusCode == 404) {
        throw Exception('Strings endpoints not implemented on server yet');
      } else if (stringsResponse.statusCode != 200) {
        throw Exception(
          'Failed to get strings from server: ${stringsResponse.statusCode}',
        );
      }
      final stringsData =
          json.decode(stringsResponse.body) as Map<String, dynamic>;
      _strings = stringsData.map((k, v) => MapEntry(k, v.toString()));
      await _saveStringsToCache(_strings, serverVersion);
      if (AppConfig.enableDebugLogging) {
        print('StringHelper: Downloaded and cached new strings');
      }
    } else {
      if (AppConfig.enableDebugLogging) {
        print('StringHelper: Versions match, loading from cache');
      }
      await _loadStringsFromCacheOrAssets();
    }
  }

  static Future<void> _loadStringsFromCacheOrAssets() async {
    try {
      final directory = await getApplicationDocumentsDirectory();
      final file = File('${directory.path}/strings_$_locale.json');
      if (await file.exists()) {
        if (AppConfig.enableDebugLogging) {
          print('StringHelper: Loading from cache file');
        }
        final jsonString = await file.readAsString();
        final loaded = json.decode(jsonString) as Map<String, dynamic>;
        _strings = loaded.map((k, v) => MapEntry(k, v.toString()));
        return;
      }
    } catch (e) {
      if (AppConfig.enableDebugLogging) {
        print('StringHelper: Cache error: $e, loading from assets');
      }
    }
    // Fallback to assets
    await _loadStringsFromAssets();
  }

  static Future<void> _loadStringsFromAssets() async {
    try {
      if (AppConfig.enableDebugLogging) {
        print('StringHelper: Loading from assets/strings.json');
      }
      final jsonString = await rootBundle.loadString('assets/strings.json');
      final loaded = json.decode(jsonString) as Map<String, dynamic>;
      _strings = loaded.map((k, v) => MapEntry(k, v.toString()));
      if (AppConfig.enableDebugLogging) {
        print(
          'StringHelper: Successfully loaded ${_strings.length} string pairs from assets',
        );
      }
    } catch (e) {
      if (AppConfig.enableDebugLogging) {
        print('StringHelper: Assets loading error: $e, using empty strings');
      }
      _strings = {};
    }
  }

  static Future<void> _saveStringsToCache(
    Map<String, String> strings,
    int version,
  ) async {
    try {
      final directory = await getApplicationDocumentsDirectory();
      final stringsFile = File('${directory.path}/strings_$_locale.json');
      final versionFile = File('${directory.path}/strings_$_locale.version');
      await stringsFile.writeAsString(json.encode(strings));
      await versionFile.writeAsString(version.toString());
    } catch (e) {
      // If we can't save to cache, that's okay - we'll just use the strings in memory
    }
  }

  static Future<int> _getLocalVersion() async {
    try {
      final directory = await getApplicationDocumentsDirectory();
      final versionFile = File('${directory.path}/strings_$_locale.version');
      if (await versionFile.exists()) {
        final versionString = await versionFile.readAsString();
        return int.parse(versionString);
      }
    } catch (e) {
      // If version file is corrupted or doesn't exist, return 0
    }
    return 0;
  }

  static Future<void> refreshFromServer() async {
    _isInitialized = false;
    await initialize();
  }

  /// Get a string by key (e.g., "foo/bar"). Returns the key itself if not found.
  static String get(String key) {
    if (!_isInitialized) {
      if (AppConfig.enableDebugLogging) {
        print('StringHelper: Not initialized, returning key: $key');
      }
      return key;
    }
    final value = _strings[key];
    if (AppConfig.enableDebugLogging) {
      print('StringHelper: Lookup "$key" -> "${value ?? key}"');
    }
    return value ?? key;
  }

  /// Check if a string exists
  static bool has(String key) {
    return _isInitialized && _strings.containsKey(key);
  }

  /// Get all available string keys
  static List<String> getAllKeys() {
    return _strings.keys.toList();
  }

  /// For debugging: get the raw map
  static Map<String, String> getRawMap() => _strings;

  /// For debugging: is loaded
  static bool isLoaded() => _isInitialized && _strings.isNotEmpty;
}
