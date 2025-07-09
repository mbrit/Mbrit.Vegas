import 'package:flutter/material.dart';
import 'models/run_state.dart';

class RunPage extends StatefulWidget {
  final RunState? runState;
  const RunPage({super.key, this.runState});

  @override
  State<RunPage> createState() => _RunPageState();
}

class _RunPageState extends State<RunPage> {
  late RunState _runState;
  late String _runName;
  int _selectedTab = 0;

  @override
  void initState() {
    super.initState();
    _runState = widget.runState ?? RunState.defaultRun();
    _runName = _runState.name.isNotEmpty
      ? _runState.name
      : RunState.generateDefaultName(_runState.location, DateTime.now());
  }

  void _onTabTapped(int index) {
    // TODO: Implement navigation for each tab
    setState(() {
      _selectedTab = index;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFF0F1419),
      appBar: AppBar(
        title: Text(_runState.name.isNotEmpty ? _runState.name : RunState.generateDefaultName(_runState.location, DateTime.now())),
        backgroundColor: const Color(0xFF1E3A8A),
        elevation: 0,
        iconTheme: const IconThemeData(color: Colors.white),
      ),
      body: Container(
        decoration: const BoxDecoration(
          gradient: LinearGradient(
            begin: Alignment.topCenter,
            end: Alignment.bottomCenter,
            colors: [
              Color(0xFF1E3A8A),
              Color(0xFF0F1419),
            ],
            stops: [0.0, 0.3],
          ),
        ),
        child: Padding(
          padding: const EdgeInsets.all(16.0),
          child: SingleChildScrollView(
            child: Column(
              children: [
                Container(
                  decoration: BoxDecoration(
                    gradient: const LinearGradient(
                      colors: [
                        Color(0xFF2D3748),
                        Color(0xFF1A202C),
                      ],
                    ),
                    borderRadius: BorderRadius.circular(16),
                    boxShadow: [
                      BoxShadow(
                        color: Colors.black.withOpacity(0.3),
                        blurRadius: 10,
                        offset: const Offset(0, 4),
                      ),
                    ],
                  ),
                  child: Padding(
                    padding: const EdgeInsets.fromLTRB(20.0, 20.0, 20.0, 20.0),
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text(_runName, style: const TextStyle(fontSize: 20, fontWeight: FontWeight.bold, color: Colors.white)),
                        const SizedBox(height: 16),
                        // Plan section
                        Text('Plan', style: Theme.of(context).textTheme.headlineSmall),
                        const SizedBox(height: 8),
                        Container(
                          padding: const EdgeInsets.all(16),
                          decoration: BoxDecoration(
                            color: Colors.grey[200],
                            borderRadius: BorderRadius.circular(8),
                          ),
                          child: const Text('Run summary goes here.'),
                        ),
                        const SizedBox(height: 24),
                        // How to Vegas Walk (collapsible)
                        ExpansionTile(
                          title: const Text('How to Vegas Walk'),
                          children: [
                            Padding(
                              padding: const EdgeInsets.all(8.0),
                              child: Text(
                                'Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed euismod, nunc ut laoreet dictum, massa erat cursus enim, nec dictum ex nulla eu urna. Vivamus euismod, massa eget dictum dictum, nunc urna dictum nunc, eget dictum nunc urna eget nunc.',
                                style: Theme.of(context).textTheme.bodyMedium,
                              ),
                            ),
                          ],
                        ),
                      ],
                    ),
                  ),
                ),
              ],
            ),
          ),
        ),
      ),
      // No bottomNavigationBar here
    );
  }
} 