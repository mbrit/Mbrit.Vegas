import '../models/walk_game_setup_dto.dart';
import '../models/play_mode.dart';

class ModeMapper {
  /// Converts local PlayMode to server WalkGameMode
  static WalkGameMode playModeToWalkGameMode(PlayMode playMode) {
    switch (playMode) {
      case PlayMode.goForBroke:
        return WalkGameMode.unrestricted;
      case PlayMode.make50PercentProfit:
        return WalkGameMode.reachSpike0p5;
      case PlayMode.doubleYourMoney:
        return WalkGameMode.reachSpike1;
      case PlayMode.balanced:
        return WalkGameMode.stretchToSpike1;
    }
  }

  /// Converts server WalkGameMode to local PlayMode
  static PlayMode walkGameModeToPlayMode(WalkGameMode walkGameMode) {
    switch (walkGameMode) {
      case WalkGameMode.unrestricted:
        return PlayMode.goForBroke;
      case WalkGameMode.reachSpike0p5:
        return PlayMode.make50PercentProfit;
      case WalkGameMode.reachSpike1:
        return PlayMode.doubleYourMoney;
      case WalkGameMode.stretchToSpike1:
        return PlayMode.balanced;
    }
  }
}
