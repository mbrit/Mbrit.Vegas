using BootFX.Common;
using BootFX.Common.Entities;
using BootFX.Common.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Numerics;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Mbrit.Vegas.Simulator
{
    internal record WalkGameOutcomeAtHand(WalkGameOutcome Outcome, int Hand);

    public class WalkGameEvaluator : Loggable
    {
        public WalkEvaluationResults Evaluate(int hand, IWinLoseDrawRound round, WalkPointOutcome result, IPermutationSelector selector)
        {
            if (selector == null)
                throw new ArgumentNullException("selector");

            if (selector.Mode != WalkGameMode.Unrestricted)
                throw new InvalidOperationException($"The game mode must be unrestricted. (The game mode has to match the database, not the game mode being played.)");

            var outcome = result.Outcome;
            if (outcome == WalkGameOutcome.Spike1OrBetter)
                throw new InvalidOperationException($"The game result is already at '{result.Outcome}' and cannot be improved.");

            // the hand has to match the length of the round...
            if (round.Count != hand)
                throw new InvalidOperationException("The provided round must the round up to the point of play.");

            var key = round.GetKey();

            this.LogInfo(() => "Loading permutations...");
            var cache = PermutationCache.GetCache(selector);
            var permutations = cache.GetItemsUnder(key);

            var count = permutations.Count();
            this.LogInfo(() => $"Loaded '{count:n0}' permutaions...");

            // what's our outcome so far?

            // binning...
            var improvesToSpike1 = 0M;
            var improvesToSpike0p5 = 0M;
            var mid = 0M;
            var declinesToMinorBust = 0M;
            var declinesToMajorBust = 0M;

            var scores = new List<decimal>();

            var positiveEvs = new List<decimal>();
            var positiveEvScores = new List<decimal>();
            var negativeEvs = new List<decimal>();
            var negativeEvScores = new List<decimal>();
            var evs = new List<decimal>();
            
            // walk the permutations...
            var logAt = DateTime.UtcNow.AddSeconds(2);
            var index = 0;
            foreach (var permutation in permutations)
            {
                // only look into this future -- don't look at this hand...
                var sequence = new List<WalkGameOutcomeAtHand>();
                if (permutation.MajorBustSeen && permutation.MajorBustHand > hand)
                    sequence.Add(new WalkGameOutcomeAtHand(WalkGameOutcome.MajorBust, permutation.MajorBustHand));
                if (permutation.MinorBustSeen && permutation.MinorBustHand > hand)
                    sequence.Add(new WalkGameOutcomeAtHand(WalkGameOutcome.MinorBust, permutation.MinorBustHand));
                if (permutation.Spike0p5Seen && permutation.Spike0p5Hand > hand)
                    sequence.Add(new WalkGameOutcomeAtHand(WalkGameOutcome.Spike0p5, permutation.Spike0p5Hand));
                if (permutation.Spike1Seen && permutation.Spike1Hand > hand)
                    sequence.Add(new WalkGameOutcomeAtHand(WalkGameOutcome.Spike1OrBetter, permutation.Spike1Hand));

                // which of those is next...
                if(sequence.Count > 1)
                    sequence.Sort((a, b) => a.Hand.CompareTo(b.Hand));

                var score = permutation.ResolvedScore;
                
                scores.Add(score);

                // what position are we in?
                var sawMajorBust = false;
                var sawMinorBust = false;
                var sawSpike0p5 = false;
                var sawSpike1 = false;
                foreach (var step in sequence)
                {
                    // if we see a major bust -- stop playing...
                    if (step.Outcome == WalkGameOutcome.MajorBust)
                    {
                        // if we're not currently at bust, flag it, otherwise bin it as mid...
                        if (outcome != WalkGameOutcome.MajorBust)
                            declinesToMajorBust += score;
                        else
                            mid += score;

                        sawMajorBust = true;
                    }
                    else if(step.Outcome == WalkGameOutcome.Spike1OrBetter)     // if we see a major win, we shoudl stop playing...
                    {
                        improvesToSpike1 += score;
                        sawSpike1 = true;
                    }
                    else if (step.Outcome == WalkGameOutcome.MinorBust)
                        sawMinorBust = true;
                    else if (step.Outcome == WalkGameOutcome.Spike0p5)
                        sawSpike0p5 = true;
                }

                // we're behind, but we get to 50%...
                if (!(sawSpike1) && !(sawMajorBust))
                {
                    if ((outcome == WalkGameOutcome.MajorBust || outcome == WalkGameOutcome.MinorBust || outcome == WalkGameOutcome.Evens) &&
                        sawSpike0p5 && !(sawSpike1))
                    {
                        improvesToSpike0p5 += score;

                    }               // we're beind, but we get to 100%...
                    else if ((outcome == WalkGameOutcome.MajorBust || outcome == WalkGameOutcome.MinorBust || outcome == WalkGameOutcome.Evens ||
                        outcome == WalkGameOutcome.Spike0p5) && sawSpike1)
                    {
                        improvesToSpike1 += score;
                    }
                    // were at 50% or 100%, but we never see it again (but never see a major loss)...
                    else if ((outcome == WalkGameOutcome.Spike0p5 || !(sawSpike0p5)) || (outcome == WalkGameOutcome.Spike1OrBetter && !(sawSpike1)) &&
                        sawMinorBust)
                    {
                        declinesToMinorBust += score;
                    }
                    else
                        mid += score;
                }

                // we never hit mid, so we're just going to put it in minor loss...
                if (mid != 0)
                    declinesToMinorBust += mid;

                // what about the ev...
                var bestEv = 0M;
                if (sawSpike1)
                    bestEv = permutation.Spike1Ev;
                else if (sawSpike0p5)
                    bestEv = permutation.Spike0p5Ev;
                else if (sawMinorBust)
                    bestEv = permutation.MinorBustEv;
                else if (sawMajorBust)
                    bestEv = permutation.MajorBustEv;
                else
                    bestEv = 0;         // we're a mid result with no ev...

                // it's the sum of each ev * score divided by the sum of the scores

                if (bestEv > 1)
                {
                    positiveEvs.Add(bestEv * score);
                    evs.Add(bestEv * score);
                    positiveEvScores.Add(score);
                }
                else
                {
                    negativeEvs.Add(bestEv * score);
                    evs.Add(bestEv * score);
                    negativeEvScores.Add(score);
                }

                if (DateTime.UtcNow >= logAt)
                {
                    this.LogInfo(() => $"Done '{index + 1:n0} of {count:n0}' permutations...");
                    logAt = DateTime.UtcNow.AddSeconds(2);
                }

                index++;
            }

            Console.WriteLine("===========================");

            var table = new ConsoleTable();
            table.AddRow("Current position", outcome);
            table.AddRow("Investable", result.Investable);
            table.AddRow("Banked", result.Banked);
            table.AddRow("Bankroll", result.Bankroll);
            table.AddRow("Profit", result.Profit);
            table.Render();

            table = new ConsoleTable();
            table.AddRow("Improves to Spike 1", this.GetPercentage(improvesToSpike1, count));
            table.AddRow("Improves to Spike 0.5", this.GetPercentage(improvesToSpike0p5, count));
            table.AddRow("Mid", this.GetPercentage(mid, count));
            table.AddRow("Declines to minor bust", this.GetPercentage(declinesToMinorBust, count));
            table.AddRow("Declines to major bust", this.GetPercentage(declinesToMajorBust, count));
            table.Render();

            table = new ConsoleTable();
            var universeScore = scores.AverageSafe(v => v);
            table.AddRow("Universe score", universeScore);
            //table.AddRow("Positive EV", positiveEvs.Sum() / scores.Sum());
            //table.AddRow("Negative EV", negativeEvs.Sum() / scores.Sum());

            var evPer100Dollars = 0M;
            if(scores.Count != 0)
                evPer100Dollars = evs.Sum() / scores.Sum();

            table.AddRow("All EV", evPer100Dollars);
            //table.AddRow("Confidence", positiveEvScores.Sum() / scores.Sum());
            table.Render();


            var improvesToSpike1Percentage = this.GetPercentage(improvesToSpike1, count);
            var improvesToSpike0p5Percentage = this.GetPercentage(improvesToSpike0p5, count);
            var declinesToMinorBustPercentage = this.GetPercentage(declinesToMinorBust, count);
            var declinesToMajorBustPercentage = this.GetPercentage(declinesToMajorBust, count);

            return new WalkEvaluationResults(improvesToSpike1Percentage, improvesToSpike0p5Percentage, declinesToMinorBustPercentage, 
                declinesToMajorBustPercentage, universeScore, evPer100Dollars);
        }

        private string FormatPercentage(decimal v) => v.ToString("n2") + "%";

        private decimal GetPercentage(decimal value, int count)
        {
            if (count != 0)
                return value / (decimal)count;
            else
                return 0M;
        }
    }
}
