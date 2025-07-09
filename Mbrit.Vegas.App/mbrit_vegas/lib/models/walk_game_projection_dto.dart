import 'package:json_annotation/json_annotation.dart';
import 'walk_game_setup_dto.dart';

part 'walk_game_projection_dto.g.dart';

@JsonSerializable()
class WalkGameProjectionDto {
  @JsonKey(name: 'outcomes')
  final List<WalkGameProjectionItemDto> outcomes;

  const WalkGameProjectionDto({
    required this.outcomes,
  });

  factory WalkGameProjectionDto.fromJson(Map<String, dynamic> json) => 
      _$WalkGameProjectionDtoFromJson(json);
  
  Map<String, dynamic> toJson() => _$WalkGameProjectionDtoToJson(this);
}

@JsonSerializable()
class WalkGameProjectionItemDto {
  @JsonKey(name: 'mode')
  final WalkGameMode mode;
  
  @JsonKey(name: 'outcomes')
  final WalkOutcomesBucketDto outcomes;

  const WalkGameProjectionItemDto({
    required this.mode,
    required this.outcomes,
  });

  factory WalkGameProjectionItemDto.fromJson(Map<String, dynamic> json) => 
      _$WalkGameProjectionItemDtoFromJson(json);
  
  Map<String, dynamic> toJson() => _$WalkGameProjectionItemDtoToJson(this);
}

@JsonSerializable()
class WalkOutcomesBucketDto {
  @JsonKey(name: 'majorBustPercentage')
  final double majorBustPercentage;
  
  @JsonKey(name: 'minorBustPercentage')
  final double minorBustPercentage;
  
  @JsonKey(name: 'evensPercentage')
  final double evensPercentage;
  
  @JsonKey(name: 'spike0p5Percentage')
  final double spike0p5Percentage;
  
  @JsonKey(name: 'spike1Percentage')
  final double spike1Percentage;
  
  @JsonKey(name: 'spike1PlusPercentage')
  final double spike1PlusPercentage;
  
  @JsonKey(name: 'averageProfitWhenWon')
  final double averageProfitWhenWon;

  @JsonKey(name: 'averageCoinIn')
  final double averageCoinIn;

  const WalkOutcomesBucketDto({
    required this.majorBustPercentage,
    required this.minorBustPercentage,
    required this.evensPercentage,
    required this.spike0p5Percentage,
    required this.spike1Percentage,
    required this.spike1PlusPercentage,
    required this.averageProfitWhenWon,
    required this.averageCoinIn,
  });

  factory WalkOutcomesBucketDto.fromJson(Map<String, dynamic> json) => 
      _$WalkOutcomesBucketDtoFromJson(json);
  
  Map<String, dynamic> toJson() => _$WalkOutcomesBucketDtoToJson(this);
} 