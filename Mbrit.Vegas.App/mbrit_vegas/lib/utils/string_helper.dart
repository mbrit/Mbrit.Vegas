import 'dart:convert';
import 'dart:io';
import 'package:flutter/services.dart';
import 'package:http/http.dart' as http;
import 'package:path_provider/path_provider.dart';
import 'app_config.dart';

class StringHelper {
  static Map<String, dynamic>? _strings;
  static bool _isInitialized = false;
  static const String _locale = AppConfig.defaultLocale;
  
  /// Initialize the string table by checking server version and loading appropriate strings
  static Future<void> initialize() async {
    if (_isInitialized) return;
    
    try {
      await _loadStringsFromServer();
      _isInitialized = true;
    } catch (e) {
      // If server is unavailable, try to load from local cache
      await _loadStringsFromCache();
      _isInitialized = true;
    }
  }

  /// Check server for latest version and download if needed
  static Future<void> _loadStringsFromServer() async {
    try {
      // Get current version from server
      final versionResponse = await http.get(
        Uri.parse(AppConfig.stringsVersionEndpoint),
        headers: {'Content-Type': 'application/json'},
      ).timeout(AppConfig.networkTimeout);

      if (versionResponse.statusCode != 200) {
        throw Exception('Failed to get version from server');
      }

      final versionData = json.decode(versionResponse.body) as Map<String, dynamic>;
      final serverVersion = versionData['value'] as int;

      // Check local version
      final localVersion = await _getLocalVersion();
      
      if (localVersion != serverVersion) {
        // Download new strings
        final stringsResponse = await http.get(
          Uri.parse('${AppConfig.stringsEndpoint}/$serverVersion'),
          headers: {'Content-Type': 'application/json'},
        ).timeout(AppConfig.networkTimeout);

        if (stringsResponse.statusCode != 200) {
          throw Exception('Failed to get strings from server');
        }

        final stringsData = json.decode(stringsResponse.body) as Map<String, dynamic>;
        
        // Save to local cache
        await _saveStringsToCache(stringsData, serverVersion);
        
        _strings = stringsData;
      } else {
        // Version is the same, load from cache
        await _loadStringsFromCache();
      }
    } catch (e) {
      // If server is unavailable, try to load from cache
      await _loadStringsFromCache();
    }
  }

  /// Load strings from local cache
  static Future<void> _loadStringsFromCache() async {
    try {
      final directory = await getApplicationDocumentsDirectory();
      final file = File('${directory.path}/strings_$_locale.json');
      
      if (await file.exists()) {
        final jsonString = await file.readAsString();
        _strings = json.decode(jsonString) as Map<String, dynamic>;
      } else {
        // No cache exists, use empty map
        _strings = {};
      }
    } catch (e) {
      // If cache is corrupted, use empty map
      _strings = {};
    }
  }

  /// Save strings to local cache
  static Future<void> _saveStringsToCache(Map<String, dynamic> strings, int version) async {
    try {
      final directory = await getApplicationDocumentsDirectory();
      final stringsFile = File('${directory.path}/strings_$_locale.json');
      final versionFile = File('${directory.path}/strings_$_locale.version');
      
      // Save strings
      await stringsFile.writeAsString(json.encode(strings));
      
      // Save version
      await versionFile.writeAsString(version.toString());
    } catch (e) {
      // If we can't save to cache, that's okay - we'll just use the strings in memory
    }
  }

  /// Get local version number
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

  /// Force refresh strings from server (useful for testing or manual refresh)
  static Future<void> refreshFromServer() async {
    _isInitialized = false;
    await initialize();
  }

  /// Get a string by path (e.g., "splash/main-title")
  /// Returns the path itself if the string is not found
  static String get(String path) {
    if (!_isInitialized) {
      // If not initialized, return the path
      return path;
    }

    if (_strings == null) {
      return path;
    }

    return _getNestedValue(_strings!, path) ?? path;
  }

  /// Get a string with parameters (e.g., "welcome/user" with {"name": "John"})
  /// Returns the path itself if the string is not found
  static String getWithParams(String path, Map<String, String> params) {
    String value = get(path);
    
    // Replace parameters in the string
    params.forEach((key, replacement) {
      value = value.replaceAll('{$key}', replacement);
    });
    
    return value;
  }

  /// Get a nested value from a map using dot notation
  static String? _getNestedValue(Map<String, dynamic> map, String path) {
    List<String> keys = path.split('/');
    dynamic current = map;

    for (String key in keys) {
      if (current is Map<String, dynamic> && current.containsKey(key)) {
        current = current[key];
      } else {
        return null;
      }
    }

    return current?.toString();
  }

  /// Check if a string exists in the table
  static bool has(String path) {
    if (!_isInitialized || _strings == null) {
      return false;
    }
    return _getNestedValue(_strings!, path) != null;
  }

  /// Get all available string paths (for debugging)
  static List<String> getAllPaths() {
    if (!_isInitialized || _strings == null) {
      return [];
    }
    return _getAllPathsRecursive(_strings!, '');
  }

  static List<String> _getAllPathsRecursive(Map<String, dynamic> map, String currentPath) {
    List<String> paths = [];
    
    map.forEach((key, value) {
      String newPath = currentPath.isEmpty ? key : '$currentPath/$key';
      
      if (value is Map<String, dynamic>) {
        paths.addAll(_getAllPathsRecursive(value, newPath));
      } else {
        paths.add(newPath);
      }
    });
    
    return paths;
  }

  /// Get current locale
  static String get locale => _locale;

  /// Get current version (for debugging)
  static Future<int> getCurrentVersion() async {
    return await _getLocalVersion();
  }
} 