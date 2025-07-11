import 'dart:convert';
import 'package:http/http.dart' as http;
import '../models/walk_game_setup_dto.dart';
import '../models/walk_game_projection_dto.dart';
import '../models/walk_game_state_dto.dart'; // Add this import

class WalkGameService {
  static const String _baseUrl =
      'http://192.168.1.248:61655'; // Replace with your actual API URL

  final http.Client _httpClient;

  WalkGameService({http.Client? httpClient})
    : _httpClient = httpClient ?? http.Client();

  /// Sends a walk game setup request to the server
  Future<WalkGameProjectionDto> setupWalkGame(WalkGameSetupDto setupDto) async {
    try {
      final response = await _httpClient.post(
        Uri.parse('$_baseUrl/walk-game/projection'),
        headers: {
          'Content-Type': 'application/json',
          'Accept': 'application/json',
        },
        body: setupDto.toJsonString(),
      );

      if (response.statusCode == 200) {
        final jsonData = jsonDecode(response.body) as Map<String, dynamic>;
        return WalkGameProjectionDto.fromJson(jsonData);
      } else {
        throw Exception(
          'Failed to setup walk game: ${response.statusCode} - ${response.body}',
        );
      }
    } catch (e) {
      throw Exception('Network error: $e');
    }
  }

  /// Gets walk game statistics or results
  Future<Map<String, dynamic>> getWalkGameResults(String gameId) async {
    try {
      final response = await _httpClient.get(
        Uri.parse('$_baseUrl/walk-game/results/$gameId'),
        headers: {'Accept': 'application/json'},
      );

      if (response.statusCode == 200) {
        return jsonDecode(response.body) as Map<String, dynamic>;
      } else {
        throw Exception(
          'Failed to get walk game results: ${response.statusCode} - ${response.body}',
        );
      }
    } catch (e) {
      throw Exception('Network error: $e');
    }
  }

  /// Starts a walk game simulation
  Future<WalkGameProjectionDto> startWalkGameSimulation(
    WalkGameSetupDto setupDto,
  ) async {
    try {
      final response = await _httpClient.post(
        Uri.parse('$_baseUrl/walk-game/simulate'),
        headers: {
          'Content-Type': 'application/json',
          'Accept': 'application/json',
        },
        body: setupDto.toJsonString(),
      );

      if (response.statusCode == 200) {
        final jsonData = jsonDecode(response.body) as Map<String, dynamic>;
        return WalkGameProjectionDto.fromJson(jsonData);
      } else {
        throw Exception(
          'Failed to start walk game simulation: ${response.statusCode} - ${response.body}',
        );
      }
    } catch (e) {
      throw Exception('Network error: $e');
    }
  }

  static Future<WalkGameStateDto> startGame(WalkGameSetupDto setup) async {
    final response = await http.post(
      Uri.parse('$_baseUrl/walk-game/start'),
      headers: {'Content-Type': 'application/json'},
      body: jsonEncode(setup.toJson()),
    );
    if (response.statusCode == 200) {
      print('=== RAW RESPONSE: ${response.body} ===');
      return WalkGameStateDto.fromJson(jsonDecode(response.body));
    } else {
      throw Exception('Failed to start game');
    }
  }

  /// Posts an action for a specific hand
  Future<WalkGameStateDto> postAction({
    required String token,
    required int handIndex,
    required String action, // Should match the server enum
  }) async {
    final url = '$_baseUrl/walk-game/$token/hands/$handIndex/actions/$action';
    final response = await _httpClient.post(
      Uri.parse(url),
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
      },
    );
    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body) as Map<String, dynamic>;
      return WalkGameStateDto.fromJson(jsonData);
    } else {
      throw Exception(
        'Failed to post action: ${response.statusCode} - ${response.body}',
      );
    }
  }

  /// Posts an outcome for a specific hand
  Future<WalkGameStateDto> postOutcome({
    required String token,
    required int handIndex,
    required String outcome, // e.g. "won" or "lost"
  }) async {
    final url = '$_baseUrl/walk-game/$token/hands/$handIndex/outcomes/$outcome';
    final response = await _httpClient.post(
      Uri.parse(url),
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
      },
    );
    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body) as Map<String, dynamic>;
      return WalkGameStateDto.fromJson(jsonData);
    } else {
      throw Exception(
        'Failed to post outcome: ${response.statusCode} - ${response.body}',
      );
    }
  }

  /// Gets the current state for a token
  Future<WalkGameStateDto> getState({required String token}) async {
    final url = '$_baseUrl/walk-game/$token/state';
    final response = await _httpClient.get(
      Uri.parse(url),
      headers: {'Accept': 'application/json'},
    );
    if (response.statusCode == 200) {
      final jsonData = jsonDecode(response.body) as Map<String, dynamic>;
      return WalkGameStateDto.fromJson(jsonData);
    } else {
      throw Exception(
        'Failed to get state: ${response.statusCode} - ${response.body}',
      );
    }
  }

  void dispose() {
    _httpClient.close();
  }
}
