import 'package:package_info_plus/package_info_plus.dart';

class AppVersion {
  static String? _version;
  static String? _buildNumber;
  static String? _fullVersion;

  static Future<String> getVersion() async {
    if (_version == null) {
      final packageInfo = await PackageInfo.fromPlatform();
      _version = packageInfo.version;
    }
    return _version!;
  }

  static Future<String> getBuildNumber() async {
    if (_buildNumber == null) {
      final packageInfo = await PackageInfo.fromPlatform();
      _buildNumber = packageInfo.buildNumber;
    }
    return _buildNumber!;
  }

  static Future<String> getFullVersion() async {
    if (_fullVersion == null) {
      final packageInfo = await PackageInfo.fromPlatform();
      _fullVersion = '${packageInfo.version}+${packageInfo.buildNumber}';
    }
    return _fullVersion!;
  }
}
