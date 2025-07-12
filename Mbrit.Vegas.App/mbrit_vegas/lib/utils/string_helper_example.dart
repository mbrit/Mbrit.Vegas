import 'string_helper.dart';

/// Example usage of StringHelper class
class StringHelperExample {
  
  /// Example of basic string lookup
  static void basicExample() {
    // Get a string by path
    String title = StringHelper.get('splash/main-title');
    print(title); // Output: "The Vegas Walk Method"
    
    // Get a string that doesn't exist
    String missing = StringHelper.get('nonexistent/path');
    print(missing); // Output: "nonexistent/path"
  }
  
  /// Example of string with parameters
  static void parameterExample() {
    // Get a string with parameters
    String welcome = StringHelper.getWithParams(
      'messages/welcome', 
      {'appName': 'The Vegas Walk Method'}
    );
    print(welcome); // Output: "Welcome to The Vegas Walk Method"
  }
  
  /// Example of checking if a string exists
  static void existenceExample() {
    bool exists = StringHelper.has('home/title');
    print('Home title exists: $exists'); // Output: true
    
    bool missing = StringHelper.has('nonexistent/path');
    print('Nonexistent path exists: $missing'); // Output: false
  }
  
  /// Example of getting all available paths (for debugging)
  static void debugExample() {
    List<String> allPaths = StringHelper.getAllPaths();
    print('Available string paths:');
    for (String path in allPaths) {
      print('  - $path');
    }
  }
  
  /// Example of common usage patterns in a Flutter app
  static void flutterUsageExample() {
    // In a Text widget
    // Text(StringHelper.get('home/setup-walk-button'))
    
    // In an AppBar title
    // AppBar(title: Text(StringHelper.get('home/title')))
    
    // In a dialog
    // AlertDialog(
    //   title: Text(StringHelper.get('common/confirm')),
    //   content: Text(StringHelper.get('messages/data-saved')),
    // )
    
    // With parameters
    // Text(StringHelper.getWithParams('messages/welcome', {'appName': 'Vegas App'}))
  }
} 