import 'package:json_annotation/json_annotation.dart';
import 'walk_game_projection_dto.dart';

part 'walk_game_state_dto.g.dart';

@JsonSerializable()
class WalkGameStateDto {
  @JsonKey(name: 'token')
  final String token;

  @JsonKey(name: 'piles')
  final WalkGamePilesDto piles;

  @JsonKey(name: 'hands')
  final List<WalkGameHandDto> hands;

  const WalkGameStateDto({
    required this.token,
    required this.piles,
    required this.hands,
  });

  factory WalkGameStateDto.fromJson(Map<String, dynamic> json) =>
      _$WalkGameStateDtoFromJson(json);

  Map<String, dynamic> toJson() => _$WalkGameStateDtoToJson(this);
}

@JsonSerializable()
class WalkGameHandDto {
  @JsonKey(name: 'index')
  final int index;

  @JsonKey(name: 'isDraft')
  final bool isDraft;

  @JsonKey(name: 'casino')
  final CasinoDto casino;

  @JsonKey(name: 'game')
  final GameItemDto game;

  @JsonKey(name: 'pilesBefore')
  final WalkGamePilesDto pilesBefore;

  @JsonKey(name: 'hasSeenSpike0p5')
  final bool hasSeenSpike0p5;

  @JsonKey(name: 'isOverSpike0p5')
  final bool isOverSpike0p5;

  @JsonKey(name: 'hasSeenSpike1')
  final bool hasSeenSpike1;

  @JsonKey(name: 'isOverSpike1')
  final bool isOverSpike1;

  @JsonKey(name: 'actions')
  final WalkGameActionsDto? actions;

  @JsonKey(name: 'action')
  final WalkGameAction? action;

  @JsonKey(name: 'probabilitySpace')
  final WalkGameProbabilitySpaceDto? probabilitySpace;

  @JsonKey(name: 'probabilitySpaceAvailableAt')
  final int probabilitySpaceAvailableAt;

  @JsonKey(name: 'needsAnswer')
  final bool needsAnswer;

  const WalkGameHandDto({
    required this.index,
    required this.isDraft,
    required this.casino,
    required this.game,
    required this.pilesBefore,
    required this.hasSeenSpike0p5,
    required this.isOverSpike0p5,
    required this.hasSeenSpike1,
    required this.isOverSpike1,
    this.actions,
    this.action,
    this.probabilitySpace,
    required this.probabilitySpaceAvailableAt,
    required this.needsAnswer,
  });

  bool get hasActions => actions != null;
  bool get hasProbabilitySpace => probabilitySpace != null;

  factory WalkGameHandDto.fromJson(Map<String, dynamic> json) =>
      _$WalkGameHandDtoFromJson(json);

  Map<String, dynamic> toJson() => _$WalkGameHandDtoToJson(this);
}

@JsonSerializable()
class WalkGamePilesDto {
  @JsonKey(name: 'investable')
  final int investable;

  @JsonKey(name: 'investableUnits')
  final int investableUnits;

  @JsonKey(name: 'inPlay')
  final int inPlay;

  @JsonKey(name: 'inPlayUnits')
  final int inPlayUnits;

  @JsonKey(name: 'banked')
  final int banked;

  @JsonKey(name: 'bankedUnits')
  final int bankedUnits;

  @JsonKey(name: 'cashInHand')
  final int cashInHand;

  @JsonKey(name: 'cashInHandUnits')
  final int cashInHandUnits;

  @JsonKey(name: 'profit')
  final int profit;

  @JsonKey(name: 'profitUnits')
  final int profitUnits;

  const WalkGamePilesDto({
    required this.investable,
    required this.investableUnits,
    required this.inPlay,
    required this.inPlayUnits,
    required this.banked,
    required this.bankedUnits,
    required this.cashInHand,
    required this.cashInHandUnits,
    required this.profit,
    required this.profitUnits,
  });

  factory WalkGamePilesDto.fromJson(Map<String, dynamic> json) =>
      _$WalkGamePilesDtoFromJson(json);

  Map<String, dynamic> toJson() => _$WalkGamePilesDtoToJson(this);
}

@JsonSerializable()
class GameItemDto {
  @JsonKey(name: 'name')
  final String name;

  @JsonKey(name: 'houseEdge')
  final double houseEdge;

  @JsonKey(name: 'notes')
  final String? notes;

  const GameItemDto({required this.name, required this.houseEdge, this.notes});

  factory GameItemDto.fromJson(Map<String, dynamic> json) =>
      _$GameItemDtoFromJson(json);

  Map<String, dynamic> toJson() => _$GameItemDtoToJson(this);
}

@JsonSerializable()
class CasinoDto {
  @JsonKey(name: 'name')
  final String name;

  @JsonKey(name: 'location')
  final LocationDto location;

  const CasinoDto({required this.name, required this.location});

  factory CasinoDto.fromJson(Map<String, dynamic> json) =>
      _$CasinoDtoFromJson(json);

  Map<String, dynamic> toJson() => _$CasinoDtoToJson(this);
}

@JsonSerializable()
class LocationDto {
  @JsonKey(name: 'name')
  final String name;

  const LocationDto({required this.name});

  factory LocationDto.fromJson(Map<String, dynamic> json) =>
      _$LocationDtoFromJson(json);

  Map<String, dynamic> toJson() => _$LocationDtoToJson(this);
}

@JsonSerializable()
class WalkGameActionsDto {
  @JsonKey(name: 'canPutInAndPlay')
  final bool? canPutInAndPlay;

  @JsonKey(name: 'canPutInUnits')
  final int? canPutInUnits;

  @JsonKey(name: 'canPlay')
  final bool? canPlay;

  @JsonKey(name: 'canHailMary')
  final bool? canHailMary;

  @JsonKey(name: 'canHailMaryUnits')
  final int? canHailMaryUnits;

  @JsonKey(name: 'canWalk')
  final bool? canWalk;

  const WalkGameActionsDto({
    this.canPutInAndPlay,
    this.canPutInUnits,
    this.canPlay,
    this.canHailMary,
    this.canHailMaryUnits,
    this.canWalk,
  });

  factory WalkGameActionsDto.fromJson(Map<String, dynamic> json) =>
      _$WalkGameActionsDtoFromJson(json);

  Map<String, dynamic> toJson() => _$WalkGameActionsDtoToJson(this);
}

enum WalkGameAction {
  @JsonValue('None')
  none,
  @JsonValue('Continue')
  continue_,
  @JsonValue('Bank')
  bank,
  @JsonValue('CashOut')
  cashOut,
  @JsonValue('HailMary')
  hailMary,
}

@JsonSerializable()
class WalkGameProbabilitySpaceDto extends WalkOutcomesBucketDto {
  const WalkGameProbabilitySpaceDto({
    required super.majorBustPercentage,
    required super.minorBustPercentage,
    required super.evensPercentage,
    required super.spike0p5Percentage,
    required super.spike1Percentage,
    required super.spike1PlusPercentage,
    required super.averageProfitWhenWon,
    required super.averageCoinIn,
  });

  factory WalkGameProbabilitySpaceDto.fromJson(Map<String, dynamic> json) =>
      _$WalkGameProbabilitySpaceDtoFromJson(json);

  @override
  Map<String, dynamic> toJson() => _$WalkGameProbabilitySpaceDtoToJson(this);
}
