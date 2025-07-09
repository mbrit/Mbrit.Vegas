// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'walk_game_projection_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

WalkGameProjectionDto _$WalkGameProjectionDtoFromJson(
  Map<String, dynamic> json,
) => WalkGameProjectionDto(
  outcomes: (json['outcomes'] as List<dynamic>)
      .map((e) => WalkGameProjectionItemDto.fromJson(e as Map<String, dynamic>))
      .toList(),
);

Map<String, dynamic> _$WalkGameProjectionDtoToJson(
  WalkGameProjectionDto instance,
) => <String, dynamic>{'outcomes': instance.outcomes};

WalkGameProjectionItemDto _$WalkGameProjectionItemDtoFromJson(
  Map<String, dynamic> json,
) => WalkGameProjectionItemDto(
  mode: $enumDecode(_$WalkGameModeEnumMap, json['mode']),
  outcomes: WalkOutcomesBucketDto.fromJson(
    json['outcomes'] as Map<String, dynamic>,
  ),
);

Map<String, dynamic> _$WalkGameProjectionItemDtoToJson(
  WalkGameProjectionItemDto instance,
) => <String, dynamic>{
  'mode': _$WalkGameModeEnumMap[instance.mode]!,
  'outcomes': instance.outcomes,
};

const _$WalkGameModeEnumMap = {
  WalkGameMode.unrestricted: 'Unrestricted',
  WalkGameMode.reachSpike0p5: 'ReachSpike0p5',
  WalkGameMode.reachSpike1: 'ReachSpike1',
  WalkGameMode.stretchToSpike1: 'StretchToSpike1',
};

WalkOutcomesBucketDto _$WalkOutcomesBucketDtoFromJson(
  Map<String, dynamic> json,
) => WalkOutcomesBucketDto(
  majorBustPercentage: (json['majorBustPercentage'] as num).toDouble(),
  minorBustPercentage: (json['minorBustPercentage'] as num).toDouble(),
  evensPercentage: (json['evensPercentage'] as num).toDouble(),
  spike0p5Percentage: (json['spike0p5Percentage'] as num).toDouble(),
  spike1Percentage: (json['spike1Percentage'] as num).toDouble(),
  spike1PlusPercentage: (json['spike1PlusPercentage'] as num).toDouble(),
  averageProfitWhenWon: (json['averageProfitWhenWon'] as num).toDouble(),
  averageCoinIn: (json['averageCoinIn'] as num).toDouble(),
);

Map<String, dynamic> _$WalkOutcomesBucketDtoToJson(
  WalkOutcomesBucketDto instance,
) => <String, dynamic>{
  'majorBustPercentage': instance.majorBustPercentage,
  'minorBustPercentage': instance.minorBustPercentage,
  'evensPercentage': instance.evensPercentage,
  'spike0p5Percentage': instance.spike0p5Percentage,
  'spike1Percentage': instance.spike1Percentage,
  'spike1PlusPercentage': instance.spike1PlusPercentage,
  'averageProfitWhenWon': instance.averageProfitWhenWon,
  'averageCoinIn': instance.averageCoinIn,
};
