import 'package:flutter/material.dart';
import 'string_helper.dart';

/// Simple examples of how to use getString() in your pages
class SimpleStringUsage {
  
  /// Example of using StringHelper.get in a Text widget
  static Widget exampleTextWidget() {
    return Text(StringHelper.get('splash/main-title'));
  }
  
  /// Example of using StringHelper.get in an AppBar
  static AppBar exampleAppBar() {
    return AppBar(
      title: Text(StringHelper.get('home/title')),
    );
  }
  
  /// Example of using StringHelper.get in a button
  static ElevatedButton exampleButton() {
    return ElevatedButton(
      onPressed: () {},
      child: Text(StringHelper.get('home/setup-walk-button')),
    );
  }
  
  /// Example of using StringHelper.getWithParams
  static Widget exampleWithParams() {
    return Text(StringHelper.getWithParams('messages/welcome', {'appName': 'Vegas App'}));
  }
  
  /// Example of using StringHelper.get in a dialog
  static AlertDialog exampleDialog() {
    return AlertDialog(
      title: Text(StringHelper.get('common/confirm')),
      content: Text(StringHelper.get('messages/data-saved')),
      actions: [
        TextButton(
          onPressed: () {},
          child: Text(StringHelper.get('common/cancel')),
        ),
        TextButton(
          onPressed: () {},
          child: Text(StringHelper.get('common/ok')),
        ),
      ],
    );
  }
  
  /// Example of using StringHelper.get in a complete page
  static Widget exampleCompletePage() {
    return Scaffold(
      appBar: AppBar(
        title: Text(StringHelper.get('home/title')),
      ),
      body: Center(
        child: Column(
          mainAxisAlignment: MainAxisAlignment.center,
          children: [
            Text(
              StringHelper.get('splash/main-title'),
              style: const TextStyle(fontSize: 24, fontWeight: FontWeight.bold),
            ),
            const SizedBox(height: 16),
            Text(StringHelper.get('home/description')),
            const SizedBox(height: 32),
            ElevatedButton(
              onPressed: () {},
              child: Text(StringHelper.get('home/setup-walk-button')),
            ),
          ],
        ),
      ),
    );
  }
}

/// Simple usage - just import and use directly in your pages:
/// 
/// ```dart
/// import 'package:your_app/utils/string_helper.dart';
/// 
/// class MyPage extends StatelessWidget {
///   @override
///   Widget build(BuildContext context) {
///     return Scaffold(
///       appBar: AppBar(
///         title: Text(StringHelper.get('my-page/title')),
///       ),
///       body: Text(StringHelper.get('my-page/content')),
///     );
///   }
/// }
/// ``` 