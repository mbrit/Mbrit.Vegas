import 'package:flutter/material.dart';
import 'main.dart';
import 'widgets/gambling_help.dart';
import 'setup_run_page.dart';
import 'tutorial_page.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'utils/string_helper.dart';
import 'utils/app_version.dart';

class SplashPage extends StatefulWidget {
  const SplashPage({Key? key}) : super(key: key);

  @override
  State<SplashPage> createState() => _SplashPageState();
}

class _SplashPageState extends State<SplashPage> {
  bool _isLoading = true;
  bool _invitationAccepted = false;
  final TextEditingController _invitationController = TextEditingController();
  static const String _correctInvitationCode = 'ILOVEVEGAS';

  @override
  void initState() {
    super.initState();
    _initializeAsync();
  }

  @override
  void dispose() {
    _invitationController.dispose();
    super.dispose();
  }

  Future<void> _initializeAsync() async {
    await StringHelper.initialize();

    // Check if invitation code has been accepted
    final prefs = await SharedPreferences.getInstance();
    _invitationAccepted = prefs.getBool('invitationCodeAccepted') ?? false;

    if (mounted) {
      setState(() {
        _isLoading = false;
      });

      // Show invitation popup if not accepted
      if (!_invitationAccepted) {
        WidgetsBinding.instance.addPostFrameCallback((_) {
          _showInvitationDialog();
        });
      }
    }
  }

  void _showInvitationDialog() {
    showDialog(
      context: context,
      barrierDismissible: false,
      builder: (BuildContext context) {
        return AlertDialog(
          backgroundColor: Colors.grey[900],
          shape: RoundedRectangleBorder(
            borderRadius: BorderRadius.circular(16),
          ),
          title: const Text(
            'THE VEGAS WALK METHOD IS IN PRIVATE BETA',
            style: TextStyle(
              color: Colors.white,
              fontSize: 18,
              fontWeight: FontWeight.bold,
            ),
            textAlign: TextAlign.center,
          ),
          content: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              const SizedBox(height: 16),
              const Text(
                'Enter your invitation code to access the app',
                style: TextStyle(color: Colors.grey, fontSize: 16),
                textAlign: TextAlign.center,
              ),
              const SizedBox(height: 20),
              TextField(
                controller: _invitationController,
                style: const TextStyle(color: Colors.white, fontSize: 16),
                decoration: InputDecoration(
                  hintText: 'Enter invitation code',
                  hintStyle: TextStyle(color: Colors.grey[400]),
                  filled: true,
                  fillColor: Colors.grey[800],
                  border: OutlineInputBorder(
                    borderRadius: BorderRadius.circular(8),
                    borderSide: BorderSide(color: Colors.grey[600]!),
                  ),
                  enabledBorder: OutlineInputBorder(
                    borderRadius: BorderRadius.circular(8),
                    borderSide: BorderSide(color: Colors.grey[600]!),
                  ),
                  focusedBorder: OutlineInputBorder(
                    borderRadius: BorderRadius.circular(8),
                    borderSide: const BorderSide(color: Color(0xFF10B981)),
                  ),
                ),
                textInputAction: TextInputAction.done,
                onSubmitted: (_) => _validateInvitationCode(),
              ),
            ],
          ),
          actions: [
            SizedBox(
              width: double.infinity,
              child: Padding(
                padding: const EdgeInsets.symmetric(horizontal: 16),
                child: ElevatedButton(
                  onPressed: _validateInvitationCode,
                  style: ElevatedButton.styleFrom(
                    backgroundColor: const Color(0xFF10B981),
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(8),
                    ),
                    padding: const EdgeInsets.symmetric(vertical: 12),
                  ),
                  child: const Text(
                    'Submit',
                    style: TextStyle(
                      color: Colors.white,
                      fontSize: 16,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                ),
              ),
            ),
          ],
        );
      },
    );
  }

  void _validateInvitationCode() async {
    final enteredCode = _invitationController.text.trim();

    if (enteredCode.toUpperCase() == _correctInvitationCode) {
      // Store acceptance
      final prefs = await SharedPreferences.getInstance();
      await prefs.setBool('invitationCodeAccepted', true);

      if (mounted) {
        setState(() {
          _invitationAccepted = true;
        });
        Navigator.of(context).pop(); // Close dialog
      }
    } else {
      // Show error
      if (mounted) {
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(
            content: const Text(
              'Invalid invitation code. Please try again.',
              style: TextStyle(color: Colors.white),
            ),
            backgroundColor: Colors.red[700],
            behavior: SnackBarBehavior.floating,
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(8),
            ),
          ),
        );
      }
    }
  }

  void _goToMain(BuildContext context) async {
    if (!_invitationAccepted) {
      _showInvitationDialog();
      return;
    }

    final prefs = await SharedPreferences.getInstance();
    final hasSeenTutorial = prefs.getBool('hasSeenTutorial') ?? false;
    if (!hasSeenTutorial) {
      Navigator.push(
        context,
        MaterialPageRoute(
          builder: (context) => const TutorialPage(fromStartWalk: true),
        ),
      );
    } else {
      Navigator.push(
        context,
        MaterialPageRoute(
          builder: (context) => const SetupRunPage(fromSplash: true),
        ),
      );
    }
  }

  void _goToLearn(BuildContext context) {
    if (!_invitationAccepted) {
      _showInvitationDialog();
      return;
    }

    Navigator.push(
      context,
      MaterialPageRoute(builder: (context) => const TutorialPage()),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: Container(
        width: double.infinity,
        height: double.infinity,
        decoration: const BoxDecoration(
          image: DecorationImage(
            image: AssetImage('assets/splash.png'),
            fit: BoxFit.cover,
          ),
        ),
        child: Container(
          width: double.infinity,
          height: double.infinity,
          decoration: BoxDecoration(
            gradient: LinearGradient(
              colors: [
                Colors.black.withOpacity(0.7),
                Colors.black.withOpacity(0.5),
                Colors.black.withOpacity(0.7),
              ],
              begin: Alignment.topCenter,
              end: Alignment.bottomCenter,
            ),
          ),
          child: _isLoading
              ? const Center(child: CircularProgressIndicator())
              : Column(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    Expanded(
                      child: Center(
                        child: Column(
                          mainAxisSize: MainAxisSize.min,
                          children: [
                            Text(
                              StringHelper.get('splash/main-title'),
                              style: const TextStyle(
                                color: Colors.white,
                                fontSize: 28,
                                fontWeight: FontWeight.bold,
                                letterSpacing: 1.2,
                              ),
                            ),
                            const SizedBox(height: 16),
                            Text(
                              StringHelper.get('splash/sub-title'),
                              style: const TextStyle(
                                color: Colors.grey,
                                fontSize: 16,
                                fontWeight: FontWeight.w500,
                                letterSpacing: 0.5,
                              ),
                              textAlign: TextAlign.center,
                            ),
                            const SizedBox(height: 40),
                            SizedBox(
                              width: 300,
                              child: OutlinedButton(
                                onPressed: () => _goToLearn(context),
                                style: OutlinedButton.styleFrom(
                                  side: BorderSide(
                                    color: Colors.grey[600]!,
                                    width: 1.5,
                                  ),
                                  padding: const EdgeInsets.symmetric(
                                    vertical: 18,
                                  ),
                                  shape: RoundedRectangleBorder(
                                    borderRadius: BorderRadius.circular(12),
                                  ),
                                ),
                                child: const Text(
                                  'Learn How it Works',
                                  style: TextStyle(
                                    color: Colors.grey,
                                    fontSize: 16,
                                    fontWeight: FontWeight.bold,
                                    letterSpacing: 1.0,
                                  ),
                                ),
                              ),
                            ),
                            const SizedBox(height: 20),
                            SizedBox(
                              width: 300,
                              height: 60,
                              child: ElevatedButton.icon(
                                onPressed: () => _goToMain(context),
                                style: ElevatedButton.styleFrom(
                                  backgroundColor: const Color(0xFF10B981),
                                  shape: RoundedRectangleBorder(
                                    borderRadius: BorderRadius.circular(12),
                                  ),
                                ),
                                icon: const Icon(
                                  Icons.play_arrow_rounded,
                                  color: Colors.white,
                                  size: 28,
                                ),
                                label: const Text(
                                  'Start Walk',
                                  style: TextStyle(
                                    color: Colors.white,
                                    fontSize: 18,
                                    fontWeight: FontWeight.bold,
                                    letterSpacing: 1.0,
                                  ),
                                ),
                              ),
                            ),
                          ],
                        ),
                      ),
                    ),
                    Padding(
                      padding: const EdgeInsets.all(16.0),
                      child: Column(
                        children: [
                          const GamblingHelp(),
                          const SizedBox(height: 8),
                          Align(
                            alignment: Alignment.centerRight,
                            child: FutureBuilder<String>(
                              future: AppVersion.getFullVersion(),
                              builder: (context, snapshot) {
                                return Text(
                                  'v${snapshot.data ?? '1.0.0'}',
                                  style: TextStyle(
                                    color: Colors.grey[400],
                                    fontSize: 12,
                                    fontWeight: FontWeight.w500,
                                  ),
                                );
                              },
                            ),
                          ),
                        ],
                      ),
                    ),
                  ],
                ),
        ),
      ),
    );
  }
}
