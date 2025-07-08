using BootFX.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    internal class WalkOutcomesBucket
    {
        internal decimal HouseEdge { get; }
        private WalkArgs Args { get; }

        internal int Total { get; }

        internal int MajorBustCount { get; }
        internal int MinorBustCount { get; }
        internal int BustCount { get; }
        internal int NotBustCount { get; }
        internal int EvensCount { get; }
        //internal int MinorCount { get; }
        internal int SpikeCount { get; }

        internal decimal MajorBustPercentage { get; }
        internal decimal MinorBustPercentage { get; }
        internal decimal BustPercentage { get; }
        internal decimal EvensPercentage { get; }
        //internal decimal MinorPercentage { get; }
        //internal decimal SpikePercentage { get; }

        internal int MaxBankroll { get; }
        internal int MinBankroll { get; }
        internal int AveragePositiveBankroll { get; }
        internal int AverageNegativeBankroll { get; }
        internal int AverageWagered { get; }
        internal int AverageHandsPlayed { get; }
        internal decimal ExpectedValuePerHundredCurrency { get; }

        internal int Spike0p5Count { get; }
        internal int Spike1Count { get; }
        internal int Spike1p5Count { get; }
        internal int Spike2Count { get; }
        internal int Spike3PlusCount { get; }
        internal int AnySpike1Count { get; }
        internal int MissedSpike1Count { get; }

        internal decimal Spike0p5Percentage { get; }
        internal decimal Spike1Percentage { get; }
        internal decimal Spike1p5Percentage { get; }
        internal decimal Spike2Percentage { get; }
        internal decimal Spike3PlusPercentage { get; }
        internal decimal AnySpike1Percentage { get; }
        internal decimal MissedSpike1Percentage { get; }

        internal WalkOutcomesBucket(WalkRun run, int numRounds, WalkGameSetup setup)
        {
            this.HouseEdge = setup.HouseEdge;

            this.Args = run.Args;

            var results = run.Results;
            if (results.Any())
            {
                this.Total = results.Count();

                this.MajorBustCount = results.Count(v => v.Outcome == WalkOutcome.MajorBust);
                this.MinorBustCount = results.Count(v => v.Outcome == WalkOutcome.MinorBust);
                this.BustCount = results.Count(v => v.DidBust);
                this.NotBustCount = results.Count(v => !(v.DidBust));
                this.EvensCount = results.Count(v => v.Outcome == WalkOutcome.Evens);
                //this.MinorCount = results.Count(v => v.Outcome == WalkOutcome.Minor);

                this.MajorBustPercentage = this.GetPercentage(this.MajorBustCount, numRounds);
                this.MinorBustPercentage = this.GetPercentage(this.MinorBustCount, numRounds);
                this.BustPercentage = this.GetPercentage(this.BustCount, numRounds);
                this.EvensPercentage = this.GetPercentage(this.EvensCount, numRounds);
                //this.MinorPercentage = this.GetPercentage(this.MinorCount, numRounds);

                var args = run.Args;

                this.Spike0p5Count = results.Count(v => v.IsSpike0p5);
                this.Spike1Count = results.Count(v => v.IsSpike1);
                this.Spike1p5Count = results.Count(v => v.IsSpike1p5);
                this.Spike2Count = results.Count(v => v.IsSpike2);
                this.Spike3PlusCount = results.Count(v => v.IsSpike3Plus);

                this.Spike0p5Percentage = this.GetPercentage(this.Spike0p5Count, numRounds);
                this.Spike1Percentage = this.GetPercentage(this.Spike1Count, numRounds);
                this.Spike1p5Percentage = this.GetPercentage(this.Spike1p5Count, numRounds);
                this.Spike2Percentage = this.GetPercentage(this.Spike2Count, numRounds);
                this.Spike3PlusPercentage = this.GetPercentage(this.Spike3PlusCount, numRounds);

                this.AnySpike1Count = this.Spike1Count + this.Spike1p5Count + this.Spike2Count + this.Spike3PlusCount;
                this.AnySpike1Percentage = this.GetPercentage(this.AnySpike1Count, numRounds);

                this.MissedSpike1Count = this.NotBustCount - this.AnySpike1Count;
                this.MissedSpike1Percentage = this.GetPercentage(this.MissedSpike1Count, numRounds);

                this.AverageWagered = (int)results.AverageSafe(v => v.TotalWagered);
                this.AverageHandsPlayed = (int)results.AverageSafe(v => v.Vectors.Count());

                var bankrolls = results.Select(v => v.Bankroll);
                this.MaxBankroll = bankrolls.Max();
                this.MinBankroll = bankrolls.Min();

                var positives = bankrolls.Where(v => v > 0);
                if(positives.Any())
                    this.AveragePositiveBankroll = (int)Math.Floor(positives.Average());

                var negatives = bankrolls.Where(v => v <= 0);
                if (negatives.Any())
                    this.AverageNegativeBankroll = (int)Math.Floor(negatives.Average());

                var outcomeCounts = new Dictionary<int, int>();
                foreach(var bankroll in bankrolls)
                {
                    if (!(outcomeCounts.ContainsKey(bankroll)))
                        outcomeCounts[bankroll] = 0;
                    outcomeCounts[bankroll]++;
                }

                var total = outcomeCounts.Values.Sum();
                var baseEv = outcomeCounts.Sum(kvp => (kvp.Key * (decimal)kvp.Value) / total);
                this.ExpectedValuePerHundredCurrency = (baseEv / (decimal)this.AverageWagered) * 100M; //  (decimal)this.Args.InitialUnit;
            }
        }

        private decimal GetPercentage(int count, int numRounds)
        {
            if (numRounds > 0)
                return (decimal)count / (decimal)numRounds;
            else
                return 0M;
        }

        internal void Dump()
        {
            var table = new ConsoleTable();
            table.AddHeaderRow("Name", "Value", "%age");

            table.AddRow("Major bust", this.MajorBustCount, this.MajorBustPercentage);
            table.AddRow("Minor bust", this.MinorBustCount, this.MinorBustPercentage);
            table.AddRow("Evens", this.EvensCount, this.EvensPercentage);
            table.AddRow("Spike 0.5x", this.Spike0p5Count, this.Spike0p5Percentage);
            table.AddRow("Any spike 1+", this.AnySpike1Count, this.AnySpike1Percentage);

            table.Render();
        }
    }
}
