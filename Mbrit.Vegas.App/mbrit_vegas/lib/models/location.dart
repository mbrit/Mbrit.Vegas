enum Location {
  lasVegasStrip,
  lasVegasDowntown,
  lasVegasValley,
  atlanticCity,
}

extension LocationExtension on Location {
  String get displayName {
    switch (this) {
      case Location.lasVegasStrip:
        return 'Las Vegas Strip';
      case Location.lasVegasDowntown:
        return 'Las Vegas Downtown';
      case Location.lasVegasValley:
        return 'Las Vegas Valley';
      case Location.atlanticCity:
        return 'Atlantic City';
    }
  }
} 