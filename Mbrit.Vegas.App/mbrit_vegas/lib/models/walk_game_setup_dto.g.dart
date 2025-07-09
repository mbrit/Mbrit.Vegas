// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'walk_game_setup_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

WalkGameSetupDto _$WalkGameSetupDtoFromJson(Map<String, dynamic> json) =>
    WalkGameSetupDto(
      unit: (json['unit'] as num).toInt(),
      mode: $enumDecode(_$WalkGameModeEnumMap, json['mode']),
      hailMaryCount: (json['hailMaryCount'] as num).toInt(),
    );

Map<String, dynamic> _$WalkGameSetupDtoToJson(WalkGameSetupDto instance) =>
    <String, dynamic>{
      'unit': instance.unit,
      'mode': _$WalkGameModeEnumMap[instance.mode]!,
      'hailMaryCount': instance.hailMaryCount,
    };

const _$WalkGameModeEnumMap = {
  WalkGameMode.unrestricted: 'Unrestricted',
  WalkGameMode.reachSpike0p5: 'ReachSpike0p5',
  WalkGameMode.reachSpike1: 'ReachSpike1',
  WalkGameMode.stretchToSpike1: 'StretchToSpike1',
};
