import 'dart:convert';
import 'package:json_annotation/json_annotation.dart';

part 'walk_game_setup_dto.g.dart';

enum WalkGameMode {
  @JsonValue('Unrestricted')
  unrestricted,
  @JsonValue('ReachSpike0p5')
  reachSpike0p5,
  @JsonValue('ReachSpike1')
  reachSpike1,
  @JsonValue('StretchToSpike1')
  stretchToSpike1,
}

@JsonSerializable()
class WalkGameSetupDto {
  @JsonKey(name: 'unit')
  final int unit;
  
  @JsonKey(name: 'mode')
  final WalkGameMode mode;
  
  @JsonKey(name: 'hailMaryCount')
  final int hailMaryCount;

  const WalkGameSetupDto({
    required this.unit,
    required this.mode,
    required this.hailMaryCount,
  });

  factory WalkGameSetupDto.fromJson(Map<String, dynamic> json) => 
      _$WalkGameSetupDtoFromJson(json);
  
  Map<String, dynamic> toJson() => _$WalkGameSetupDtoToJson(this);

  String toJsonString() => jsonEncode(toJson());
} 