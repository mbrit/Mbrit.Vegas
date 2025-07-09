import 'package:flutter/material.dart';

class GamblingHelp extends StatelessWidget {
  const GamblingHelp({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    return Container(
      padding: const EdgeInsets.all(16),
      decoration: BoxDecoration(
        color: const Color(0xFF232946), // dark background
        borderRadius: BorderRadius.circular(8),
        border: Border.all(color: Colors.grey[800]!), // subtle border
      ),
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: const [
          Text(
            'This app is intended for educational and entertainment purposes only. No real money is used or won. All outcomes are based on simulations, estimates, and heuristics and will differ from real-world play. Gambling involves risk and can be addictive. If you or someone you know is struggling, please contact a responsible gambling support service in your area. No real money gaming or prizes offered. 18+ only.',
            style: TextStyle(fontSize: 14, color: Color(0xFFEEEEEE)),
          ),
          SizedBox(height: 12),
          Text('US: Call 1-800-GAMBLER', style: TextStyle(fontWeight: FontWeight.bold, color: Color(0xFFCCCCCC))),
          Text('UK: BeGambleAware.org', style: TextStyle(fontWeight: FontWeight.bold, color: Color(0xFFCCCCCC))),
          Text('Elsewhere: Search online for "gambling help"', style: TextStyle(fontWeight: FontWeight.bold, color: Color(0xFFCCCCCC))),
        ],
      ),
    );
  }
} 