import 'package:flutter/material.dart';
import 'home_page.dart';
import 'learn_page.dart';
import 'profile_page.dart';
import 'analytics_page.dart';
import 'widgets/vegas_bottom_nav_bar.dart';

/// MainScaffold manages the tab bar and page switching. Set initialTab to choose the starting tab (0=Home, 1=Analytics, 2=Learn, 3=Profile).
class MainScaffold extends StatefulWidget {
  final int initialTab;
  const MainScaffold({Key? key, this.initialTab = 0}) : super(key: key);
  @override
  State<MainScaffold> createState() => _MainScaffoldState();
}

class _MainScaffoldState extends State<MainScaffold> {
  late int _selectedTab;

  List<Widget> get _pages => [
    HomePage(),
    AnalyticsPage(),
    LearnPage(),
    ProfilePage(),
  ];

  @override
  void initState() {
    super.initState();
    _selectedTab = widget.initialTab;
  }

  void _onTabTapped(int index) {
    setState(() {
      _selectedTab = index;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: _pages[_selectedTab],
      bottomNavigationBar: VegasBottomNavBar(
        currentIndex: _selectedTab,
        onTap: _onTabTapped,
      ),
    );
  }
}
