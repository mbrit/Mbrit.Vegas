class Casino {
  final String name;
  final String owner;

  Casino({required this.name, required this.owner});
}

class Location {
  final String name;
  final List<Casino> casinos;

  Location({required this.name, required this.casinos});

  String get displayName => name;

  static final lasVegasStrip = Location(
    name: 'Las Vegas Strip',
    casinos: [
      Casino(name: 'Bellagio', owner: 'MGM Resorts'),
      Casino(name: 'Caesars Palace', owner: 'Caesars Entertainment'),
      Casino(name: 'The Venetian', owner: 'Las Vegas Sands'),
      Casino(name: 'MGM Grand', owner: 'MGM Resorts'),
      Casino(name: 'Wynn', owner: 'Wynn Resorts'),
    ],
  );
} 