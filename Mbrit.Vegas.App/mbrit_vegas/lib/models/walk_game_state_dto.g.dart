// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'walk_game_state_dto.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

WalkGameStateDto _$WalkGameStateDtoFromJson(Map<String, dynamic> json) =>
    WalkGameStateDto(
      token: json['token'] as String,
      name: json['name'] as String?,
      piles: WalkGamePilesDto.fromJson(json['piles'] as Map<String, dynamic>),
      hands: (json['hands'] as List<dynamic>)
          .map((e) => WalkGameHandDto.fromJson(e as Map<String, dynamic>))
          .toList(),
      probabilitySpace: json['probabilitySpace'] == null
          ? null
          : WalkGameProbabilitySpaceDto.fromJson(
              json['probabilitySpace'] as Map<String, dynamic>,
            ),
      hasProbabilitySpace: json['hasProbabilitySpace'] as bool?,
      probabilitySpaceAvailableAt: (json['probabilitySpaceAvailableAt'] as num?)
          ?.toInt(),
      spike0p5: (json['spike0p5'] as num?)?.toInt(),
      spike0p5Units: (json['spike0p5Units'] as num?)?.toInt(),
      spike1: (json['spike1'] as num?)?.toInt(),
      spike1Units: (json['spike1Units'] as num?)?.toInt(),
    );

Map<String, dynamic> _$WalkGameStateDtoToJson(WalkGameStateDto instance) =>
    <String, dynamic>{
      'token': instance.token,
      'name': instance.name,
      'piles': instance.piles,
      'hands': instance.hands,
      'probabilitySpace': instance.probabilitySpace,
      'hasProbabilitySpace': instance.hasProbabilitySpace,
      'probabilitySpaceAvailableAt': instance.probabilitySpaceAvailableAt,
      'spike0p5': instance.spike0p5,
      'spike0p5Units': instance.spike0p5Units,
      'spike1': instance.spike1,
      'spike1Units': instance.spike1Units,
    };

WalkGameHandDto _$WalkGameHandDtoFromJson(Map<String, dynamic> json) =>
    WalkGameHandDto(
      index: (json['index'] as num).toInt(),
      isDraft: json['isDraft'] as bool,
      casino: CasinoDto.fromJson(json['casino'] as Map<String, dynamic>),
      game: GameItemDto.fromJson(json['game'] as Map<String, dynamic>),
      actions: json['actions'] == null
          ? null
          : WalkGameActionsDto.fromJson(
              json['actions'] as Map<String, dynamic>,
            ),
      action: $enumDecodeNullable(_$WalkGameActionEnumMap, json['action']),
      outcome: json['outcome'] as String?,
      needsAnswer: json['needsAnswer'] as bool,
    );

Map<String, dynamic> _$WalkGameHandDtoToJson(WalkGameHandDto instance) =>
    <String, dynamic>{
      'index': instance.index,
      'isDraft': instance.isDraft,
      'casino': instance.casino,
      'game': instance.game,
      'actions': instance.actions,
      'action': _$WalkGameActionEnumMap[instance.action],
      'outcome': instance.outcome,
      'needsAnswer': instance.needsAnswer,
    };

const _$WalkGameActionEnumMap = {
  WalkGameAction.none: 'None',
  WalkGameAction.play: 'Play',
  WalkGameAction.hailMary: 'HailMary',
  WalkGameAction.walk: 'Walk',
};

WalkGamePilesDto _$WalkGamePilesDtoFromJson(Map<String, dynamic> json) =>
    WalkGamePilesDto(
      investable: (json['investable'] as num).toInt(),
      investableUnits: (json['investableUnits'] as num).toInt(),
      inPlay: (json['inPlay'] as num).toInt(),
      inPlayUnits: (json['inPlayUnits'] as num).toInt(),
      banked: (json['banked'] as num).toInt(),
      bankedUnits: (json['bankedUnits'] as num).toInt(),
      cashInHand: (json['cashInHand'] as num).toInt(),
      cashInHandUnits: (json['cashInHandUnits'] as num).toInt(),
      profit: (json['profit'] as num).toInt(),
      profitUnits: (json['profitUnits'] as num).toInt(),
    );

Map<String, dynamic> _$WalkGamePilesDtoToJson(WalkGamePilesDto instance) =>
    <String, dynamic>{
      'investable': instance.investable,
      'investableUnits': instance.investableUnits,
      'inPlay': instance.inPlay,
      'inPlayUnits': instance.inPlayUnits,
      'banked': instance.banked,
      'bankedUnits': instance.bankedUnits,
      'cashInHand': instance.cashInHand,
      'cashInHandUnits': instance.cashInHandUnits,
      'profit': instance.profit,
      'profitUnits': instance.profitUnits,
    };

GameItemDto _$GameItemDtoFromJson(Map<String, dynamic> json) => GameItemDto(
  name: json['name'] as String,
  houseEdge: (json['houseEdge'] as num).toDouble(),
  notes: json['notes'] as String?,
);

Map<String, dynamic> _$GameItemDtoToJson(GameItemDto instance) =>
    <String, dynamic>{
      'name': instance.name,
      'houseEdge': instance.houseEdge,
      'notes': instance.notes,
    };

CasinoDto _$CasinoDtoFromJson(Map<String, dynamic> json) => CasinoDto(
  name: json['name'] as String,
  location: LocationDto.fromJson(json['location'] as Map<String, dynamic>),
);

Map<String, dynamic> _$CasinoDtoToJson(CasinoDto instance) => <String, dynamic>{
  'name': instance.name,
  'location': instance.location,
};

LocationDto _$LocationDtoFromJson(Map<String, dynamic> json) =>
    LocationDto(name: json['name'] as String);

Map<String, dynamic> _$LocationDtoToJson(LocationDto instance) =>
    <String, dynamic>{'name': instance.name};

WalkGameActionsDto _$WalkGameActionsDtoFromJson(Map<String, dynamic> json) =>
    WalkGameActionsDto(
      instructions: json['instructions'] as String?,
      canPlay: json['canPlay'] as bool?,
      play: (json['play'] as num?)?.toInt(),
      playUnits: (json['playUnits'] as num?)?.toInt(),
      canHailMary: json['canHailMary'] as bool?,
      hailMary: (json['hailMary'] as num?)?.toInt(),
      hailMaryUnits: (json['hailMaryUnits'] as num?)?.toInt(),
      canWalk: json['canWalk'] as bool?,
    );

Map<String, dynamic> _$WalkGameActionsDtoToJson(WalkGameActionsDto instance) =>
    <String, dynamic>{
      'instructions': instance.instructions,
      'canPlay': instance.canPlay,
      'play': instance.play,
      'playUnits': instance.playUnits,
      'canHailMary': instance.canHailMary,
      'hailMary': instance.hailMary,
      'hailMaryUnits': instance.hailMaryUnits,
      'canWalk': instance.canWalk,
    };

WalkGameProbabilitySpaceDto _$WalkGameProbabilitySpaceDtoFromJson(
  Map<String, dynamic> json,
) => WalkGameProbabilitySpaceDto(
  majorBustPercentage: (json['majorBustPercentage'] as num).toDouble(),
  minorBustPercentage: (json['minorBustPercentage'] as num).toDouble(),
  evensPercentage: (json['evensPercentage'] as num).toDouble(),
  spike0p5Percentage: (json['spike0p5Percentage'] as num).toDouble(),
  spike1Percentage: (json['spike1Percentage'] as num).toDouble(),
  spike1PlusPercentage: (json['spike1PlusPercentage'] as num).toDouble(),
  averageProfitWhenWon: (json['averageProfitWhenWon'] as num).toDouble(),
  averageCoinIn: (json['averageCoinIn'] as num).toDouble(),
);

Map<String, dynamic> _$WalkGameProbabilitySpaceDtoToJson(
  WalkGameProbabilitySpaceDto instance,
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
