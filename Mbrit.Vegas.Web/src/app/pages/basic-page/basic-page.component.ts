import { Component, SimpleChanges } from '@angular/core';
import { SharedModule } from '../../shared.module';
import { Game, GamesRepo } from '../../api/games.service';
import { FormatHelper } from '../../api/format.helper';

class GamePlan {
  owner: BasicPageComponent;
  game: Game;

  isSelected: boolean = false;
  isPlanned: boolean = true;

  _betPerHand = 0;
  _handsPerHour = 0;
  _hoursPlayed = 0;

  constructor(game: Game, owner: BasicPageComponent) {
    this.owner = owner;
    this.game = game;

    this.betPerHand = game.betPerHand;
    this.handsPerHour = game.handsPerHour;
    this.hoursPlayed = owner.defaultHoursPlayed;
  }

  get betPerHand(): number {
    return this._betPerHand;
  }

  set betPerHand(value: number) {
    this._betPerHand = Number(value);
  }

  get handsPerHour(): number {
    return this._handsPerHour;
  }

  set handsPerHour(value: number) {
    this._handsPerHour = Number(value);
  }

  get hoursPlayed(): number {
    return this._hoursPlayed;
  }

  set hoursPlayed(value: number) {
    this._hoursPlayed = Number(value);
  }

  get name(): string {
    return this.game.name;
  }

  get houseEdge(): number {
    return this.game.houseEdge;
  }

  get houseEdgeAsString(): string {
    return (this.game.houseEdge * 100).toFixed(2).toString();
  }

  get totalDecisions(): number {
    return this.handsPerHour * this.hoursPlayed;
  }

  get totalWager(): number {
    return this.betPerHand * this.totalDecisions;
  }

  get theo(): number {
    return this.totalWager * this.houseEdge;
  }

  get theoPerDecision(): number {
    if (this.totalDecisions > 0)
      return this.theo / this.totalDecisions;
    else
      return 0;
  }

  get comp(): number {
    return this.theo * this.owner.compRate;
  }

  get relativeVolatility(): number {
    return this.game.relativeVolatility;
  }

  get sessionStandardDeviation(): number {
    return Math.sqrt(this.totalDecisions) * this.relativeVolatility * this.betPerHand;
  }

  get stopLoss(): number {
    return this.sessionStandardDeviation * this.owner.stopLossRatio;
  }

  get stopLossUnits(): number {
    return Math.round(this.stopLoss / this.betPerHand);
  }

  get quit(): number {
    return this.sessionStandardDeviation * this.owner.quitRatio;
  }

  get quitUnits(): number {
    return Math.round(this.quit / this.betPerHand);
  }

  get bank(): number {
    return this.sessionStandardDeviation * this.owner.bankRatio;
  }

  get bankUnits(): number {
    return Math.round(this.bank / this.betPerHand);
  }

  get compPerHour(): number {
    if (this.hoursPlayed != 0)
      return this.comp / this.hoursPlayed;
    else
      return 0;
  }

  get dollarsPerTierPoint(): number {
    return this.game.dollarsPerTierPoint;
  }

  get tierPoints(): number {
    if (this.hasTierPoints)
      return this.totalWager / this.game.dollarsPerTierPoint;
    else
      return this.theo * this.owner.theoToTierPoints;
  }

  get hasTierPoints(): boolean {
    return this.dollarsPerTierPoint > 0;
  }
}

class ValueAndName {
  value: number;
  name0: string;
  name1: string;
  name2: string;

  constructor(value: number) {
    this.value = value;
    this.name0 = value.toFixed(0).toString();
    this.name1 = value.toFixed(1).toString();
    this.name2 = value.toFixed(2).toString();
  }
}

@Component({
  selector: 'app-basic-page',
  templateUrl: './basic-page.component.html',
  styleUrl: './basic-page.component.css',
  standalone: true,
  imports: [SharedModule]
})
export class BasicPageComponent {
  compRate = .3;
  defaultHoursPlayed = 1.5;
  stopLossRatio = -.75;
  quitRatio = -.5;
  bankRatio = .35;
  theoToTierPoints = .1;

  firstColumn = 12.5;

  betPerHandRange: ValueAndName[] = [];

  hoursPlayedRange: ValueAndName[] = [];

  plans: GamePlan[] = [];

  FormatHelper = FormatHelper;

  ngOnInit() {
    const games = new GamesRepo().getGames();

    for (let index = 5; index <= 100; index += 5)
      this.betPerHandRange.push(new ValueAndName(index));

    for (let index = .5; index <= 6; index += .5)
      this.hoursPlayedRange.push(new ValueAndName(index));

    this.plans = [];
    for (const game of games)
      this.plans.push(new GamePlan(game, this));

    if (this.plans.length > 0)
      this.selectedPlan = this.plans[0];

    this.recalculate();
  }

  recalculate() {
  }

  get selectedGameName(): string | null {
    if (this.selectedPlan != null)
      return this.selectedPlan.name;
    else
      return null;
  }

  set selectedGameName(value: string) {
    this.selectedPlan = this.plans.find(v => v.name == value) ?? null;
  }

  get selectedPlan(): GamePlan | null {
    return this.plans.find(v => v.isSelected) ?? null;
  }

  set selectedPlan(game: Game | null) {
    console.log("setting selected plan --> ", game!.name, game);
    for (const plan of this.plans)
      plan.isSelected = plan.game.name == game?.name;
  }

  get firstColumnWidth(): string {
    return this.firstColumn.toString() + "%";
  }

  get columnWidth(): string {
    return ((100 - this.firstColumn) / (this.plans.length + 1)) + "%";
  }

  get plannedPlans(): GamePlan[] {
    const planned = new Array<GamePlan>();
    for (const plan of this.plans) {
      if (plan.isPlanned)
        planned.push(plan);
    }
    return planned;
  }

  get totalWager(): number {
    return this.getTotal(v => v.totalWager);
  }

  get totalTheo(): number {
    return this.getTotal(v => v.theo);
  }

  get totalComp(): number {
    return this.getTotal(v => v.comp);
  }

  get totalHoursPlayed(): number {
    return this.getTotal(v => v.hoursPlayed);
  }

  get totalTierPoints(): number {
    return this.getTotal(v => v.tierPoints);
  }

  private getTotal(callback: (plan: GamePlan) => number): number {
    var planneds = this.plannedPlans;

    let total = 0;
    for (const plan of planneds)
      total += callback(plan);
    return total;
  }

  get averageCompPerHour(): number {
    return this.getAverage(v => v.compPerHour);
  }

  get averageHouseEdge(): number {
    return this.getAverage(v => v.houseEdge);
  }

  private getAverage(callback: (plan: GamePlan) => number): number {
    var planneds = this.plannedPlans;

    if (planneds.length > 0) {
      let total = 0;
      for (const plan of planneds)
        total += callback(plan);
      return total / planneds.length;
    } else
      return 0;
  }

  get areTierPointsEstimated(): boolean {
    var planneds = this.plannedPlans;
    for (const plan of planneds) {
      if (!(plan.hasTierPoints))
        return true;
    }
    return false;
  }
}
