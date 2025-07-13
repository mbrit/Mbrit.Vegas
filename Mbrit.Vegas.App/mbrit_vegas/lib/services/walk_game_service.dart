import 'dart:convert';
import 'package:http/http.dart' as http;
import '../models/walk_game_setup_dto.dart';
import '../models/walk_game_projection_dto.dart';
import '../models/walk_game_state_dto.dart'; // Add this import
import '../utils/app_config.dart';

class NullResponse {
  const NullResponse();
  factory NullResponse.fromJson(Map<String, dynamic> json) =>
      const NullResponse();
  Map<String, dynamic> toJson() => {};
}

class WalkGameService {
  final http.Client _httpClient;

  WalkGameService({http.Client? httpClient})
    : _httpClient = httpClient ?? http.Client();

  /// Sends a walk game setup request to the server
  Future<WalkGameProjectionDto> setupWalkGame(WalkGameSetupDto setupDto) async {
    try {
      final response = await _httpClient.post(
        Uri.parse(AppConfig.walkGameProjectionEndpoint),
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
      print('Network error details: $e');
      if (e.toString().contains('SocketException')) {
        throw Exception(
          'DNS resolution failed. Please check your internet connection and try again.',
        );
      } else if (e.toString().contains('HandshakeException')) {
        throw Exception(
          'SSL certificate error. Please check the server configuration.',
        );
      } else {
        throw Exception('Network error: $e');
      }
    }
  }

  /// Gets walk game statistics or results
  Future<Map<String, dynamic>> getWalkGameResults(String gameId) async {
    try {
      final response = await _httpClient.get(
        Uri.parse('${AppConfig.walkGameResultsEndpoint}/$gameId'),
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
      print('Network error details: $e');
      if (e.toString().contains('SocketException')) {
        throw Exception(
          'DNS resolution failed. Please check your internet connection and try again.',
        );
      } else if (e.toString().contains('HandshakeException')) {
        throw Exception(
          'SSL certificate error. Please check the server configuration.',
        );
      } else {
        throw Exception('Network error: $e');
      }
    }
  }

  /// Starts a walk game simulation
  Future<WalkGameProjectionDto> startWalkGameSimulation(
    WalkGameSetupDto setupDto,
  ) async {
    try {
      final response = await _httpClient.post(
        Uri.parse(AppConfig.walkGameSimulateEndpoint),
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
      print('Network error details: $e');
      if (e.toString().contains('SocketException')) {
        throw Exception(
          'DNS resolution failed. Please check your internet connection and try again.',
        );
      } else if (e.toString().contains('HandshakeException')) {
        throw Exception(
          'SSL certificate error. Please check the server configuration.',
        );
      } else {
        throw Exception('Network error: $e');
      }
    }
  }

  static Future<WalkGameStateDto> startGame(WalkGameSetupDto setup) async {
    final response = await http.post(
      Uri.parse(AppConfig.walkGameStartEndpoint),
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
    final url =
        '${AppConfig.walkGameAbandonEndpoint}/$token/hands/$handIndex/actions/$action';
    final response = await _httpClient.get(
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
    required WinLoseDrawType outcome, // Use enum instead of string
  }) async {
    final url =
        '${AppConfig.walkGameAbandonEndpoint}/$token/hands/$handIndex/outcomes/${outcome.name}';
    final response = await _httpClient.get(
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
    final url = '${AppConfig.walkGameAbandonEndpoint}/$token';
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

  /// Abandon the run for a given token
  Future<NullResponse> abandonRun({required String token}) async {
    final url = '${AppConfig.walkGameAbandonEndpoint}/$token/abandon';
    final response = await _httpClient.get(
      Uri.parse(url),
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
      },
    );
    if (response.statusCode == 200) {
      return NullResponse();
    } else {
      throw Exception(
        'Failed to abandon run: ${response.statusCode} - ${response.body}',
      );
    }
  }

  void dispose() {
    _httpClient.close();
  }
}
