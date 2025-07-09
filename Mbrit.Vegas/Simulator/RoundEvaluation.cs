namespace Mbrit.Vegas.Simulator
{
    internal class RoundEvaluation
    {
        internal Dictionary<int, WalkOutcomesBucket> Evaluations { get; } = new Dictionary<int, WalkOutcomesBucket>();
        internal int Stop { get; private set; } = -1;

        internal bool HasBucket(int hand) => this.Evaluations.ContainsKey(hand);

        internal void SetOutcome(int hand, WalkOutcomesBucket bucket)
        {
            if (this.Evaluations.ContainsKey(hand))
                throw new InvalidOperationException("Evaluation already set.");

            this.Evaluations[hand] = bucket;
        }

        internal void SetStop(int hand)
        {
            if (this.Stop != -1)
                throw new InvalidOperationException("Stop already set.");

            this.Stop = hand;
        }

        internal WalkOutcomesBucket GetBucket(int hand) => this.Evaluations[hand];

        internal decimal? GetSlope(int startHand, int endHand)
        {
            var points = new List<decimal>();
            foreach(var hand in this.Evaluations.Keys)
            {
                if (hand >= startHand && hand <= endHand)
                    points.Add(this.Evaluations[hand].ExpectedValuePerHundredCurrency);

                if (hand > endHand)
                    break;
            }

            if (points.Count >= 2)
            {
                var slope = CalculateLinearRegressionSlope(points);
                return slope;
            }
            else
                return null;
        }

        private static decimal CalculateLinearRegressionSlope(List<decimal> yValues)
        {
            if (yValues == null || yValues.Count < 2)
                throw new ArgumentException("At least two data points are required.");

            int n = yValues.Count;
            decimal sumX = 0;
            decimal sumY = 0;
            decimal sumXY = 0;
            decimal sumX2 = 0;

            for (int i = 0; i < n; i++)
            {
                decimal x = i + 1; // 1-based x axis for clarity
                decimal y = yValues[i];

                sumX += x;
                sumY += y;
                sumXY += x * y;
                sumX2 += x * x;
            }

            decimal numerator = (n * sumXY) - (sumX * sumY);
            decimal denominator = (n * sumX2) - (sumX * sumX);

            if (denominator == 0)
                throw new InvalidOperationException("Cannot compute slope with zero variance in X.");

            return numerator / denominator;
        }
    }
}