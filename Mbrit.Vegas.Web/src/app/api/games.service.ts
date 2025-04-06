export class Game {
  name: string;
  houseEdge: number;
  betPerHand: number;
  handsPerHour: number;
  dollarsPerTierPoint: number;
  relativeVolatility: number;

  constructor(name: string, houseEdge: number, betPerHand: number, handsPerHour: number, relativeVolatility: number, dollarsPerTierPoint?: number) {
    this.name = name;
    this.houseEdge = houseEdge;
    this.betPerHand = betPerHand;
    this.handsPerHour = handsPerHour;
    this.relativeVolatility = relativeVolatility;
    this.dollarsPerTierPoint = dollarsPerTierPoint ?? 0;
  }
}

export class GamesRepo {
  private games = [
    new Game("Blackjack (3:2)", 0.005, 25, 80, 1.15),
    new Game("Blackjack (5:6)", 0.02, 25, 80, 1.15),
    new Game("UTH", 0.022, 25, 60, 2.9),
    new Game("3 Card", 0.037, 25, 95, 1.8),
    new Game("4 Card", 0.034, 25, 90, 2.1),
    new Game("Pai Gow", 0.015, 25, 35, 0.75),
    new Game("Baccarat", 0.011, 25, 35, 1),
    new Game("Roulette (00)", 0.053, 25, 70, 1.4),
    new Game("Craps", 0.015, 25, 35, 1.1),
    new Game("Slots", 0.05, 3, 350, 5, 5),
    new Game("Video Poker (9/6)", 0.005, 5, 450, 1.2, 15),
    new Game("Video Poker (9/5)", 0.016, 5, 450, 1.3, 10),
  ];

  getGames() {
    return this.games;  
  }
}
