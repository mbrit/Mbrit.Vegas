import 'dart:convert';
import 'package:http/http.dart' as http;
import '../models/walk_game_setup_dto.dart';
import '../models/walk_game_projection_dto.dart';

class WalkGameService {
  static const String _baseUrl = 'http://192.168.1.248:61655'; // Replace with your actual API URL
  
  final http.Client _httpClient;

  WalkGameService({http.Client? httpClient}) : _httpClient = httpClient ?? http.Client();

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
        throw Exception('Failed to setup walk game: ${response.statusCode} - ${response.body}');
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
        headers: {
          'Accept': 'application/json',
        },
      );

      if (response.statusCode == 200) {
        return jsonDecode(response.body) as Map<String, dynamic>;
      } else {
        throw Exception('Failed to get walk game results: ${response.statusCode} - ${response.body}');
      }
    } catch (e) {
      throw Exception('Network error: $e');
    }
  }

  /// Starts a walk game simulation
  Future<WalkGameProjectionDto> startWalkGameSimulation(WalkGameSetupDto setupDto) async {
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
        throw Exception('Failed to start walk game simulation: ${response.statusCode} - ${response.body}');
      }
    } catch (e) {
      throw Exception('Network error: $e');
    }
  }

  void dispose() {
    _httpClient.close();
  }
} 