namespace Mbrit.Vegas.Simulator
{
    public class WalkEvaluationResults
    {
        public decimal ImprovesToSpike1Percentage { get; }
        public decimal ImprovesToSpike0p5Percentage { get; }
        public decimal DeclinesToMinorLoss { get; }
        public decimal DeclinesToMajorLoss { get; }
        public decimal UniverseScore { get; }
        public decimal EvPer100Dollars { get; }

        internal WalkEvaluationResults(decimal improvesToSpike1Percentage, decimal improvesToSpike0p5Percentage, decimal declinesToMinorLoss,
            decimal declinesToMajorLoss, decimal universeScore, decimal evPer100Dollars)
        {
            this.ImprovesToSpike1Percentage = improvesToSpike1Percentage;
            this.ImprovesToSpike0p5Percentage = improvesToSpike0p5Percentage;
            this.DeclinesToMinorLoss = declinesToMinorLoss;
            this.DeclinesToMajorLoss = declinesToMajorLoss;
            this.UniverseScore = universeScore;
            this.EvPer100Dollars = evPer100Dollars;
        }

        public decimal DeclinesToEitherLoss => this.DeclinesToMajorLoss + this.DeclinesToMinorLoss;
    }
}