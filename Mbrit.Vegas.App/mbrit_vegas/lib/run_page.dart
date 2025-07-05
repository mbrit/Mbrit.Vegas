import 'package:flutter/material.dart';
import 'models/run_state.dart';
import 'models/location.dart';
import 'models/hand_result.dart';
import 'models/investment_state.dart';

class RunPage extends StatefulWidget {
  const RunPage({super.key});

  @override
  State<RunPage> createState() => _RunPageState();
}

class _RunPageState extends State<RunPage> {
  late RunState _runState;
  late TextEditingController _nameController;
  bool _isRunDetailsExpanded = false;
  bool _isHandsExpanded = true;
  bool _isInvestmentExpanded = true;

  @override
  void initState() {
    super.initState();
    _runState = RunState.defaultRun();
    _nameController = TextEditingController(text: _runState.name);
  }

  @override
  void dispose() {
    _nameController.dispose();
    super.dispose();
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: const Color(0xFF0F1419),
      appBar: AppBar(
        title: const Text(
          'Vegas Walk',
          style: TextStyle(
            color: Colors.white,
            fontWeight: FontWeight.bold,
          ),
        ),
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
          child: Column(
            children: [
              // First component: Run name, date/time, and location
              _buildRunInfoCard(),
              
              // Second component: Progress tracking
              const SizedBox(height: 20),
              _buildProgressCard(),
              
              // Third component: Investment tracking
              const SizedBox(height: 20),
              _buildInvestmentCard(),
              
              // Placeholder for future components
              const SizedBox(height: 20),
              _buildPlaceholderCard(),
            ],
          ),
        ),
      ),
    );
  }

  Widget _buildRunInfoCard() {
    return Container(
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
      child: Column(
        children: [
          // Header with icon, title, and expand/collapse button
          InkWell(
            onTap: () {
              setState(() {
                _isRunDetailsExpanded = !_isRunDetailsExpanded;
              });
            },
            child: Padding(
              padding: const EdgeInsets.all(20.0),
              child: Row(
                children: [
                  Container(
                    padding: const EdgeInsets.all(8),
                    decoration: BoxDecoration(
                      color: const Color(0xFF3B82F6),
                      borderRadius: BorderRadius.circular(8),
                    ),
                    child: const Icon(
                      Icons.casino,
                      color: Colors.white,
                      size: 20,
                    ),
                  ),
                  const SizedBox(width: 12),
                  const Text(
                    'Run Details',
                    style: TextStyle(
                      fontSize: 20,
                      fontWeight: FontWeight.bold,
                      color: Colors.white,
                    ),
                  ),
                  const Spacer(),
                  Icon(
                    _isRunDetailsExpanded ? Icons.expand_less : Icons.expand_more,
                    color: Colors.white,
                    size: 24,
                  ),
                ],
              ),
            ),
          ),
          // Collapsible content
          if (_isRunDetailsExpanded)
            Padding(
              padding: const EdgeInsets.fromLTRB(20.0, 0.0, 20.0, 20.0),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  // Run name
                  _buildInputField(
                    label: 'Run Name',
                    icon: Icons.edit,
                    child: TextField(
                      style: const TextStyle(color: Colors.white),
                      decoration: InputDecoration(
                        hintText: 'Enter run name',
                        hintStyle: TextStyle(color: Colors.grey[400]),
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
                          borderSide: const BorderSide(color: Color(0xFF3B82F6), width: 2),
                        ),
                        filled: true,
                        fillColor: const Color(0xFF2D3748),
                      ),
                      controller: _nameController,
                      onChanged: (value) {
                        setState(() {
                          _runState = RunState(
                            name: value,
                            startTime: _runState.startTime,
                            location: _runState.location,
                            numHands: _runState.numHands,
                            currentHand: _runState.currentHand,
                            handResults: _runState.handResults,
                            investments: _runState.investments,
                          );
                        });
                      },
                    ),
                  ),
                  
                  const SizedBox(height: 16),
                  
                  // Date/time
                  _buildInfoRow(
                    label: 'Started',
                    icon: Icons.access_time,
                    value: _runState.formattedStartTime,
                    valueColor: const Color(0xFF10B981),
                  ),
                  
                  const SizedBox(height: 16),
                  
                  // Area
                  _buildDropdownField(
                    label: 'Area',
                    icon: Icons.location_on,
                    value: _runState.location,
                    items: Location.values,
                    onChanged: (Location? newLocation) {
                      if (newLocation != null) {
                        setState(() {
                          _runState = RunState(
                            name: _runState.name,
                            startTime: _runState.startTime,
                            location: newLocation,
                            numHands: _runState.numHands,
                            currentHand: _runState.currentHand,
                            handResults: _runState.handResults,
                            investments: _runState.investments,
                          );
                        });
                      }
                    },
                  ),
                ],
              ),
            ),
        ],
      ),
    );
  }

  Widget _buildInputField({
    required String label,
    required IconData icon,
    required Widget child,
  }) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Row(
          children: [
            Icon(icon, color: Colors.grey[400], size: 16),
            const SizedBox(width: 8),
            Text(
              label,
              style: TextStyle(
                color: Colors.grey[400],
                fontWeight: FontWeight.w500,
                fontSize: 14,
              ),
            ),
          ],
        ),
        const SizedBox(height: 8),
        child,
      ],
    );
  }

  Widget _buildInfoRow({
    required String label,
    required IconData icon,
    required String value,
    required Color valueColor,
  }) {
    return Row(
      children: [
        Icon(icon, color: Colors.grey[400], size: 16),
        const SizedBox(width: 8),
        Text(
          '$label: ',
          style: TextStyle(
            color: Colors.grey[400],
            fontWeight: FontWeight.w500,
            fontSize: 14,
          ),
        ),
        Text(
          value,
          style: TextStyle(
            color: valueColor,
            fontWeight: FontWeight.bold,
            fontSize: 14,
          ),
        ),
      ],
    );
  }

  Widget _buildDropdownField({
    required String label,
    required IconData icon,
    required Location value,
    required List<Location> items,
    required ValueChanged<Location?> onChanged,
  }) {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Row(
          children: [
            Icon(icon, color: Colors.grey[400], size: 16),
            const SizedBox(width: 8),
            Text(
              label,
              style: TextStyle(
                color: Colors.grey[400],
                fontWeight: FontWeight.w500,
                fontSize: 14,
              ),
            ),
          ],
        ),
        const SizedBox(height: 8),
        Container(
          padding: const EdgeInsets.symmetric(horizontal: 12),
          decoration: BoxDecoration(
            color: const Color(0xFF2D3748),
            borderRadius: BorderRadius.circular(8),
            border: Border.all(color: Colors.grey[600]!),
          ),
                      child: DropdownButtonHideUnderline(
              child: DropdownButton<Location>(
                value: value,
                dropdownColor: const Color(0xFF2D3748),
                style: const TextStyle(color: Colors.white),
                items: items.map((location) {
                  return DropdownMenuItem(
                    value: location,
                    child: Text(location.displayName),
                  );
                }).toList(),
                onChanged: onChanged,
              ),
            ),
        ),
      ],
    );
  }

  Widget _buildProgressCard() {
    return Container(
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
      child: Column(
        children: [
          // Header with icon, title, and expand/collapse button
          InkWell(
            onTap: () {
              setState(() {
                _isHandsExpanded = !_isHandsExpanded;
              });
            },
            child: Padding(
              padding: const EdgeInsets.all(20.0),
              child: Row(
                children: [
                  Container(
                    padding: const EdgeInsets.all(8),
                    decoration: BoxDecoration(
                      color: const Color(0xFF10B981),
                      borderRadius: BorderRadius.circular(8),
                    ),
                    child: const Icon(
                      Icons.timeline,
                      color: Colors.white,
                      size: 20,
                    ),
                  ),
                  const SizedBox(width: 12),
                  const Text(
                    'Hands',
                    style: TextStyle(
                      fontSize: 20,
                      fontWeight: FontWeight.bold,
                      color: Colors.white,
                    ),
                  ),
                  const Spacer(),
                  Icon(
                    _isHandsExpanded ? Icons.expand_less : Icons.expand_more,
                    color: Colors.white,
                    size: 24,
                  ),
                ],
              ),
            ),
          ),
          // Collapsible content
          if (_isHandsExpanded)
            Padding(
              padding: const EdgeInsets.fromLTRB(20.0, 0.0, 20.0, 20.0),
              child: _buildProgressDots(),
            ),
        ],
      ),
    );
  }

  Widget _buildProgressDots() {
    const int dotsPerRow = 14; // 14 dots per row
    final int numHands = _runState.numHands;
    final int rows = (numHands / dotsPerRow).ceil();
    
    return Column(
      children: List.generate(rows, (rowIndex) {
        return Padding(
          padding: const EdgeInsets.only(bottom: 4.0),
          child: Row(
            mainAxisAlignment: MainAxisAlignment.spaceEvenly,
            children: List.generate(dotsPerRow, (colIndex) {
              final handIndex = rowIndex * dotsPerRow + colIndex;
              if (handIndex >= numHands) {
                return const SizedBox(width: 12, height: 12);
              }
              
              final handResult = _runState.handResults[handIndex];
              final isCurrentHand = handIndex == _runState.currentHand;
              
              return Container(
                width: 12,
                height: 12,
                decoration: BoxDecoration(
                  shape: BoxShape.circle,
                  color: handResult.isFilled ? handResult.color : Colors.transparent,
                  border: Border.all(
                    color: isCurrentHand 
                        ? const Color(0xFF3B82F6) 
                        : handResult.color,
                    width: isCurrentHand ? 2 : 1,
                  ),
                  boxShadow: isCurrentHand ? [
                    BoxShadow(
                      color: const Color(0xFF3B82F6).withOpacity(0.5),
                      blurRadius: 4,
                      spreadRadius: 1,
                    ),
                  ] : null,
                ),
                child: isCurrentHand
                    ? const Icon(
                        Icons.play_arrow,
                        color: Colors.white,
                        size: 6,
                      )
                    : null,
              );
            }),
          ),
        );
      }),
    );
  }

  Widget _buildInvestmentCard() {
    return Container(
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
      child: Column(
        children: [
          // Header with icon, title, and expand/collapse button
          InkWell(
            onTap: () {
              setState(() {
                _isInvestmentExpanded = !_isInvestmentExpanded;
              });
            },
            child: Padding(
              padding: const EdgeInsets.all(20.0),
              child: Row(
                children: [
                  Container(
                    padding: const EdgeInsets.all(8),
                    decoration: BoxDecoration(
                      color: const Color(0xFFF59E0B),
                      borderRadius: BorderRadius.circular(8),
                    ),
                    child: const Icon(
                      Icons.account_balance_wallet,
                      color: Colors.white,
                      size: 20,
                    ),
                  ),
                  const SizedBox(width: 12),
                  const Text(
                    'Investment',
                    style: TextStyle(
                      fontSize: 20,
                      fontWeight: FontWeight.bold,
                      color: Colors.white,
                    ),
                  ),
                  const Spacer(),
                  Icon(
                    _isInvestmentExpanded ? Icons.expand_less : Icons.expand_more,
                    color: Colors.white,
                    size: 24,
                  ),
                ],
              ),
            ),
          ),
          // Collapsible content
          if (_isInvestmentExpanded)
            Padding(
              padding: const EdgeInsets.fromLTRB(20.0, 0.0, 20.0, 20.0),
              child: _buildBankNotesGrid(),
            ),
        ],
      ),
    );
  }

  Widget _buildBankNotesGrid() {
    const int notesPerRow = 8;
    final int numNotes = _runState.investments.length;
    final int rows = (numNotes / notesPerRow).ceil();
    
    return Column(
      children: List.generate(rows, (rowIndex) {
        return Padding(
          padding: const EdgeInsets.only(bottom: 8.0),
          child: Row(
            mainAxisAlignment: MainAxisAlignment.spaceEvenly,
            children: List.generate(notesPerRow, (colIndex) {
              final noteIndex = rowIndex * notesPerRow + colIndex;
              if (noteIndex >= numNotes) {
                return const SizedBox(width: 32, height: 20);
              }
              
              final investmentState = _runState.investments[noteIndex];
              
              return Container(
                width: 32,
                height: 20,
                decoration: BoxDecoration(
                  color: Colors.transparent,
                  border: Border.all(
                    color: investmentState == InvestmentState.available 
                        ? Colors.white 
                        : investmentState.color,
                    width: 1,
                  ),
                  borderRadius: BorderRadius.circular(4),
                ),
                child: Stack(
                  alignment: Alignment.center,
                  children: [
                    // Bank note icon
                    Icon(
                      Icons.attach_money,
                      color: investmentState == InvestmentState.available 
                          ? Colors.white 
                          : investmentState.color,
                      size: 12,
                    ),
                    // X overlay for consumed investments
                    if (investmentState == InvestmentState.consumed)
                      Container(
                        decoration: BoxDecoration(
                          color: Colors.black.withOpacity(0.7),
                          borderRadius: BorderRadius.circular(4),
                        ),
                        child: const Icon(
                          Icons.close,
                          color: Colors.white,
                          size: 10,
                        ),
                      ),
                  ],
                ),
              );
            }),
          ),
        );
      }),
    );
  }

  Widget _buildPlaceholderCard() {
    return Container(
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
        padding: const EdgeInsets.all(20.0),
        child: Row(
          children: [
            Container(
              padding: const EdgeInsets.all(8),
              decoration: BoxDecoration(
                color: Colors.grey[600],
                borderRadius: BorderRadius.circular(8),
              ),
              child: Icon(
                Icons.add,
                color: Colors.grey[400],
                size: 20,
              ),
            ),
            const SizedBox(width: 12),
            Text(
              'More components will go here...',
              style: TextStyle(
                fontSize: 16,
                fontStyle: FontStyle.italic,
                color: Colors.grey[400],
              ),
            ),
          ],
        ),
      ),
    );
  }
} 