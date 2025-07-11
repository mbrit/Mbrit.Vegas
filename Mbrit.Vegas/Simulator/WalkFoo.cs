using BootFX.Common;
using BootFX.Common.Data.Text;
using BootFX.Common.Entities;
using BootFX.Common.Management;
using Mbrit.Vegas.Games;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Net.NetworkInformation;
using System.Net.WebSockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Mbrit.Vegas.Simulator
{
    internal record ValueWithColour(object Value, ConsoleColor colour = ConsoleColor.Gray, bool Flagged = false);

    internal record PercentageWithColour(decimal Value, ConsoleColor colour = ConsoleColor.Gray, bool Flagged = false);

    public record WalkRun(IEnumerable<WalkResult> Results, WalkArgs Args, int Rounds, IWalkGameSetup Setup)
    {
        public string Name => this.Args.Name;   

        public WalkOutcomesBucket GetOutcomes() => new WalkOutcomesBucket(this, this.Rounds, Setup);
    }

    internal record EvaluationResultAndBucket(WalkResult Result, RoundEvaluation Evaluation);

    public class WalkFoo : CliFoo
    {
        private static readonly ThreadLocal<Random> ThreadRandom = new(() => new Random(VegasRuntime.GetToken().GetHashCode()));

        //private int StartAdvisingAt = 10;
        //private int StopAdvisingAt = 24;

        private const int MaxEvaluationSampleSize = 8192;

        public void DoMagicN()
        {
            var runs = this.GetValueWithDefault<int>("Number of runs");
            this.DoMagic(runs);
        }

        public void DoMagic(int numRounds = 75000)
        {
            //IWinLoseDrawRoundsBucket newRounds = null;

            var rounds = WinLoseDrawRoundsBucket.GetWinLoseBucket(numRounds, WalkGameDefaults.HandsPerRound, WalkGameDefaults.HouseEdge, this.Random);

            var hailMaryCount = 1;

            var unitSize = 100;

            var player = new AutomaticWalkGamePlayer();

            var runs = this.DoMagic(rounds, 4, player, (index, rounds) =>
            {
                if (index == 0)
                    return WalkGameDefaults.GetSetup(WalkGameMode.ReachSpike1, rounds, unitSize, hailMaryCount);
                else if (index == 1)
                    return WalkGameDefaults.GetSetup(WalkGameMode.ReachSpike0p5, rounds, unitSize, hailMaryCount);
                else if (index == 2)
                    return WalkGameDefaults.GetSetup(WalkGameMode.StretchToSpike1, rounds, unitSize, hailMaryCount);
                else if (index == 3)
                    return WalkGameDefaults.GetSetup(WalkGameMode.Unrestricted, rounds, unitSize, hailMaryCount);
                else
                    throw new NotSupportedException($"Cannot handle '{index}'.");
            });

            //this.DumpCombinations(runs, combinations);
            //this.DumpChains(runs.First());
            this.DumpBankrolls(runs);
        }

        private void DumpBankrolls(IEnumerable<WalkRun> runs)
        {
            var asList = runs.ToList();

            using (var writer = this.GetWriter("Bankrolls"))
            {
                var csv = new CsvDataWriter(writer);
                csv.WriteValue("Index");
                for (var i = 0; i < asList.Count; i++)
                    csv.WriteValue("#" + i);
                csv.WriteLine();

                var numRounds = runs.First().Rounds;

                var results = new List<List<WalkResult>>();
                for (var i = 0; i < asList.Count; i++)
                    results.Add(asList[i].Results.ToList());

                for (var index = 0; index < numRounds; index++)
                {
                    csv.WriteValue(index);
                    for (var i = 0; i < asList.Count; i++)
                        csv.WriteValue(results[i][index].Bankroll);
                    csv.WriteLine();
                }
            }
        }

        private void DumpChains(WalkRun run)
        {
            using (var writer = this.GetWriter("Chains"))
            {
                var csv = new CsvDataWriter(writer);
                csv.WriteValue("Bankroll");
                csv.WriteValue("Outcome");
                csv.WriteValue("Win Chain");
                csv.WriteValue("Loss Chain");
                csv.WriteValue("Net Chain");
                csv.WriteLine();

                foreach(var result in run.Results)
                {
                    csv.WriteValue(result.Bankroll);
                    csv.WriteValue(result.Outcome);
                    csv.WriteValue(result.WinChainScore);
                    csv.WriteValue(result.LossChainScore);
                    csv.WriteValue(result.ChainScore);
                    csv.WriteLine();
                }
            }
        }

        private void DumpCombinations(IEnumerable<WalkRun> runs, List<(int, int)> combinations)
        {
            using (var writer = this.GetWriter("Combinations"))
            {
                var csv = new CsvDataWriter(writer);
                csv.WriteValue("Hands");
                csv.WriteValue("Investments");
                csv.WriteValue("EV");
                csv.WriteValue("Major bust");
                csv.WriteValue("Major bust");
                csv.WriteValue("Evens");
                csv.WriteValue("Spike 0.5x");
                csv.WriteValue("Spike 1x");
                csv.WriteValue("Spike 1.5x");
                csv.WriteLine();

                var asList = runs.ToList();
                for (var index = 0; index < asList.Count - 1; index++)
                {
                    var run = asList[index + 1];
                    var combination = combinations[index];

                    var outcomes = run.GetOutcomes();

                    csv.WriteValue(combination.Item1);
                    csv.WriteValue(combination.Item2);
                    csv.WriteValue(outcomes.ExpectedValuePerHundredCurrency.ToString("n3"));
                    csv.WriteValue(outcomes.MajorBustPercentage.ToString("n3"));
                    csv.WriteValue(outcomes.MinorBustPercentage.ToString("n3"));
                    csv.WriteValue(outcomes.EvensPercentage.ToString("n3"));
                    csv.WriteValue(outcomes.Spike0p5Percentage.ToString("n3"));
                    csv.WriteValue(outcomes.Spike1Percentage.ToString("n3"));
                    csv.WriteValue(outcomes.Spike1p5Percentage.ToString("n3"));
                    csv.WriteLine();
                }
            }
        }

        /*
        private void RenderEvSlopesForHeatmap()
        {
            var run = runs.Last();

            var pairs = new List<EvaluationResultAndBucket>();

            foreach (var result in run.Results)
            {
                foreach (var e in bucket.Evaluations)
                {
                    if (result.Index == e.Key)
                        pairs.Add(new EvaluationResultAndBucket(result, e.Value));
                }
            }

            pairs.Sort((a, b) => b.Result.Bankroll.CompareTo(a.Result.Bankroll));

            using (var writer = this.CreateWriter("Evals"))
            {
                var csv = new CsvDataWriter(writer);
                csv.WriteValue("Round");
                csv.WriteValue("Bankroll");
                csv.WriteValue("Stop");

                for (var hand = 0; hand < run.Setup.HandsPerRound; hand++)
                    csv.WriteValue("EV " + hand);

                csv.WriteLine();

                foreach (var pair in pairs)
                {
                    csv.WriteValue(pair.Result.Index);
                    csv.WriteValue(pair.Result.Bankroll);

                    var eval = pair.Evaluation;
                    csv.WriteValue(eval.Stop);

                    var first = -1;

                    for (var hand = 0; hand < run.Setup.HandsPerRound; hand++)
                    {
                        var didWrite = false;

                        if (eval.HasBucket(hand))
                        {
                            if (first == -1)
                                first = hand;
                            else
                            {
                                var slope = eval.GetSlope(first, hand);
                                if (slope != null)
                                {
                                    csv.WriteValue(slope.Value.ToString("n2"));
                                    didWrite = true;
                                }
                            }
                        }

                        if(!(didWrite))
                            csv.WriteValue(string.Empty);
                    }

                    csv.WriteLine();
                }
            }

            Console.WriteLine("Done.");
        }
            */

        private StreamWriter GetWriter(string name)
        {
            var path = $@"c:\Mbrit\Casino\{name}\{name}--{DateTime.UtcNow.ToString("yyyyMMdd-HHmmss")}.csv";
            Runtime.Current.EnsureFolderForFileCreated(path);
            return new StreamWriter(path);
        }

        //internal static WalkArgs GetDefaultArgs(int unit = 100, decimal takeProfitMultipler = 4, int maxPutIns = 12) => new WalkArgs(unit, takeProfitMultipler, maxPutIns);

        private string FormatPercentage(int count, int numRounds)
        {
            //return (((decimal)count / (decimal)numRounds) * 100).ToString("n2") + "%";
            return count.ToString();
        }

        public IEnumerable<WalkRun> DoMagic(IWinLoseDrawRoundsBucket rounds, int numVariants, IWalkGamePlayer player, Func<int, IWinLoseDrawRoundsBucket, WalkGameSetup> getSetup)
        {
            var numRounds = rounds.Count;
            this.LogInfo(() => $"Running '{numRounds}' simulation(s)...");

            // get the rounds...
            //const decimal houseEdge = Game.AverageHouseEdge;
            ////const decimal houseEdge = Game.Blackjack32HouseEdge;
            //var rounds = WinLoseDrawRoundsBucket.GetWinLoseBucket(numRounds, 50, houseEdge, this.Random);

            var runs = new List<WalkRun>();

            for (var variant = 0; variant < numVariants; variant++)
            {
                this.LogInfo(() => $"Running variant #{variant} of {numVariants}...");

                var setup = getSetup(variant, rounds);

                Preamble(setup);

                var run = this.DoRun(setup, player);
                runs.Add(run);
            }

            this.LogInfo(() => "Finished running variants...");

            this.DumpResults(runs);

            return runs;
        }

        private WalkRun DoRun(IWalkGameSetup setup, IWalkGamePlayer player, Action<WalkRunArgs> configureArgs = null, Func<WalkArgs> getArgs = null)
        {
            var results = new List<WalkResult>();

            var logAt = DateTime.UtcNow.AddSeconds(2);

            var rounds = setup.Rounds;
            var numRounds = rounds.Count;

            WalkArgs args = null;
            if (getArgs != null)
                args = getArgs();

            if(args == null)
                args = setup.GetArgs();

            for (var index = 0; index < numRounds; index++)
            {
                var runArgs = new WalkRunArgs()
                {
                    DoTrace = numRounds == 1,
                };

                if(configureArgs != null)
                    configureArgs(runArgs);

                var result = this.DoWalk(rounds[index], args, player, setup, runArgs);
                results.Add(result);

                if (DateTime.UtcNow >= logAt)
                {
                    this.LogInfo(() => $"Done {index + 1:n0} of {numRounds:n0}...");
                    logAt = DateTime.UtcNow.AddSeconds(2);
                }
            }

            return new WalkRun(results, args, numRounds, setup);
        }

        private void Preamble(WalkGameSetup setup)
        {
            Console.WriteLine("Hands: " + setup.HandsPerRound);
            Console.WriteLine("House edge: " + setup.HouseEdge);
            Console.WriteLine("Unit: " + setup.UnitSize);
            //Console.WriteLine($"Side split: {args.SideSplit} ({args.BaseUnit}/{args.SideUnit})");
            Console.WriteLine("Max investment: " + (setup.UnitSize * setup.MaxPutIns) + ", put-ins: " + setup.MaxPutIns);
            Console.WriteLine("Take profit: " + (setup.UnitSize * setup.TakeProfitMultiplier) + ", multipler: " + setup.TakeProfitMultiplier);
        }

        private void DumpResults(IEnumerable<WalkRun> runs)
        {
            var checkRounds = runs.SelectAndMerge(v => v.Rounds);
            if (checkRounds.Count() != 1)
                throw new InvalidOperationException("The number of runs have to have the same across the runs.");

            var numRounds = checkRounds.First();

            var table = new ConsoleTable();
            var header = table.AddHeaderRow();
            header.AddCell("");

            var index = -1;
            foreach(var run in runs)
            {
                if (index == -1)
                    header.AddCell("Baseline");
                else
                {
                    var name = "#" + index;
                    if (run.Args.HasName)
                        name += " " + run.Args.Name;

                    header.AddCell(name);
                }

                index++;
            }

            Action sep = () =>
            {
                const string sep = "········";
                this.RenderRow(table, sep, runs, (index, results, args) => new ValueWithColour(sep, ConsoleColor.Cyan));
            };


            var baseline = runs.First().Args;

            WalkOutcomesBucket baselineOutcomes = null;
            var otherOutcomes = new Dictionary<int, WalkOutcomesBucket>();

            index = 0;
            foreach (var run in runs)
            {
                if (index == 0)
                    baselineOutcomes = new WalkOutcomesBucket(run, numRounds, run.Setup);
                else
                    otherOutcomes[index] = new WalkOutcomesBucket(run, numRounds, run.Setup);

                index++;
            }

            // config...
            this.RenderRow(table, "Hands", runs, (index, results, args) => args.MaxHands);

            this.RenderRow(table, "House edge", runs, (index, results, args) =>
            {
                Func<decimal, ConsoleColor> getColour = (value) =>
                {
                    if (value >= Game.AverageHouseEdge)
                        return ConsoleColor.Yellow;
                    else
                        return ConsoleColor.Magenta;
                };

                if (index == 0)
                    return new PercentageWithColour(baselineOutcomes.HouseEdge, getColour(baselineOutcomes.HouseEdge));
                else
                    return new PercentageWithColour(otherOutcomes[index].HouseEdge, getColour(otherOutcomes[index].HouseEdge));
            });

            this.RenderRow(table, "Unit", runs, (index, results, args) => args.Unit);
            //this.RenderRow(table, "Side split", runs, (index, results, args) => args.SideSplit);
            //this.RenderRow(table, "Initial max investment", runs, (index, results, args) => args.InitialMaxInvestment + ", put-ins: " + args.MaxPutIns);
            //this.RenderRow(table, "Take profit", runs, (index, results, args) => args.InitialTakeProfit + ", multiplier: " + args.TakeProfitMultiplier);
            this.RenderRow(table, "Spike 0.5x", runs, (index, results, args) => args.Spike0p5Win);
            this.RenderRow(table, "Spike 1x", runs, (index, results, args) => args.Spike1Win);
            this.RenderRow(table, "Stop mode", runs, (index, results, args) => args.StopAtWinMode);
            this.RenderRow(table, "Limited stretch hands", runs, (index, results, args) => args.LimitedStretchHands);
            this.RenderRow(table, "Limited investments", runs, (index, results, args) => args.LimitedStretchInvestments);
            this.RenderRow(table, "Hail Mary", runs, (index, results, args) => args.HailMaryMode + ", " + args.HailMaryCount);

            sep();

            // metrics...
            this.RenderRow(table, "Major Busts", runs, (index, results, args) =>
            {
                if (index == 0)
                    return new PercentageWithColour(baselineOutcomes.MajorBustPercentage);
                else
                {
                    var value = otherOutcomes[index].MajorBustPercentage;
                    return new PercentageWithColour(value, this.CompareColours(baselineOutcomes.MajorBustPercentage, value, false));
                }
            });

            this.RenderRow(table, $"Minor Busts ({baseline.MinorBust})", runs, (index, results, args) =>
            {
                if (index == 0)
                    return new PercentageWithColour(baselineOutcomes.MinorBustPercentage);
                else
                {
                    var value = otherOutcomes[index].MinorBustPercentage;
                    return new PercentageWithColour(value, this.CompareColours(baselineOutcomes.MinorBustPercentage, value, false));
                }
            });

            this.RenderRow(table, "Busts", runs, (index, results, args) =>
            {
                if (index == 0)
                    return new PercentageWithColour(baselineOutcomes.BustPercentage, ConsoleColor.Gray, true);
                else
                {
                    var value = otherOutcomes[index].BustPercentage;
                    return new PercentageWithColour(value, this.CompareColours(baselineOutcomes.BustPercentage, value, false), true);
                }
            });

            sep();
            
            this.RenderRow(table, $"Evens", runs, (index, results, args) =>
            {
                if (index == 0)
                    return new PercentageWithColour(baselineOutcomes.EvensPercentage);
                else
                {
                    var value = otherOutcomes[index].EvensPercentage;
                    return new PercentageWithColour(value, this.CompareColours(baselineOutcomes.EvensPercentage, value, false));
                }
            });

            this.RenderRow(table, $"Spike 0.5x ({baseline.Spike0p5Win})", runs, (index, results, args) =>
            {
                if (index == 0)
                    return new PercentageWithColour(baselineOutcomes.Spike0p5Percentage);
                else
                {
                    var value = otherOutcomes[index].Spike0p5Percentage;
                    return new PercentageWithColour(value, this.CompareColours(baselineOutcomes.Spike0p5Percentage, value, true));
                }
            });

            /*
            this.RenderRow(table, $"Missed spikes", runs, (index, results, args) =>
            {
                if (index == 0)
                    return new PercentageWithColour(baselineOutcomes.MissedSpike1Percentage, ConsoleColor.Gray, false);
                else
                {
                    var value = otherOutcomes[index].MissedSpike1Percentage;
                    return new PercentageWithColour(value, this.CompareColours(baselineOutcomes.MissedSpike1Percentage, value, false), false);
                }
            });

            this.RenderRow(table, $"Spikes or better", runs, (index, results, args) =>
            {
                if (index == 0)
                    return new PercentageWithColour(baselineOutcomes.AnySpike1Percentage, ConsoleColor.Gray, true);
                else
                {
                    var value = otherOutcomes[index].AnySpike1Percentage;
                    return new PercentageWithColour(value, this.CompareColours(baselineOutcomes.AnySpike1Percentage, value, true), true);
                }
            });

            sep();
            */

            this.RenderRow(table, $"Spike 1x ({baseline.Spike1Win})", runs, (index, results, args) =>
            {
                if (index == 0)
                    return new PercentageWithColour(baselineOutcomes.Spike1Percentage);
                else
                {
                    var Percentage = otherOutcomes[index].Spike1Percentage;
                    return new PercentageWithColour(Percentage, this.CompareColours(baselineOutcomes.Spike1Percentage, Percentage, true));
                }
            });

            this.RenderRow(table, $"Spike 1.5x ({baseline.Spike1p5Win})", runs, (index, results, args) =>
            {
                if (index == 0)
                    return new PercentageWithColour(baselineOutcomes.Spike1p5Percentage);
                else
                {
                    var Percentage = otherOutcomes[index].Spike1p5Percentage;
                    return new PercentageWithColour(Percentage, this.CompareColours(baselineOutcomes.Spike1p5Percentage, Percentage, true));
                }
            });

            this.RenderRow(table, $"Spike 2x ({baseline.Spike2Win})", runs, (index, results, args) =>
            {
                if (index == 0)
                    return new PercentageWithColour(baselineOutcomes.Spike2Percentage);
                else
                {
                    var Percentage = otherOutcomes[index].Spike2Percentage;
                    return new PercentageWithColour(Percentage, this.CompareColours(baselineOutcomes.Spike2Percentage, Percentage, true));
                }
            });

            this.RenderRow(table, $"Spike 3x+ ({baseline.Spike3Win})", runs, (index, results, args) =>
            {
                if (index == 0)
                    return new ValueWithColour(baselineOutcomes.Spike3PlusPercentage);
                else
                {
                    var value = otherOutcomes[index].Spike3PlusPercentage;
                    return new ValueWithColour(value, this.CompareColours(baselineOutcomes.Spike3PlusPercentage, value, true));
                }
            });

            sep();

            this.RenderRow(table, $"EV per $100", runs, (index, results, args) =>
            {
                if (index == 0)
                    return new ValueWithColour(baselineOutcomes.ExpectedValuePerHundredCurrency, ConsoleColor.Gray, true);
                else
                {
                    var value = otherOutcomes[index].ExpectedValuePerHundredCurrency;
                    return new ValueWithColour(value, this.CompareColours(baselineOutcomes.ExpectedValuePerHundredCurrency, value, true), true);
                }
            });

            sep();

            this.RenderRow(table, $"Average coin-in", runs, (index, results, args) =>
            {
                if (index == 0)
                    return new ValueWithColour(baselineOutcomes.AverageWagered, ConsoleColor.Gray);
                else
                {
                    var value = otherOutcomes[index].AverageWagered;
                    return new ValueWithColour(value, this.CompareColours(baselineOutcomes.AverageWagered, value, true));
                }
            });

            this.RenderRow(table, $"Average hands played", runs, (index, results, args) =>
            {
                if (index == 0)
                    return new ValueWithColour(baselineOutcomes.AverageHandsPlayed);
                else
                {
                    var value = otherOutcomes[index].AverageHandsPlayed;
                    return new ValueWithColour(value, this.CompareColours(baselineOutcomes.AverageHandsPlayed, value, true));
                }
            });

            this.RenderRow(table, $"Average win (where won)", runs, (index, results, args) =>
            {
                if (index == 0)
                    return new ValueWithColour(baselineOutcomes.AveragePositiveBankroll);
                else
                {
                    var value = otherOutcomes[index].AveragePositiveBankroll;
                    return new ValueWithColour(value, this.CompareColours(baselineOutcomes.AveragePositiveBankroll, value, true));
                }
            });

            sep();

            this.RenderRow(table, $"Average spike stop", runs, (index, results, args) =>
            {
                return new ValueWithColour(results.Where(v => v.StopReason == WalkStopReason.HitSpike1).AverageSafe(v => v.Bankroll));
            });

            this.RenderRow(table, $"Min spike stop", runs, (index, results, args) =>
            {
                return new ValueWithColour(results.Where(v => v.StopReason == WalkStopReason.HitSpike1).MinSafe(v => v.Bankroll));
            });

            this.RenderRow(table, $"Max spike stop", runs, (index, results, args) =>
            {
                return new ValueWithColour(results.Where(v => v.StopReason == WalkStopReason.HitSpike1).MaxSafe(v => v.Bankroll));
            });

            sep();

            this.RenderRow(table, $"Average spike 0.5x stop", runs, (index, results, args) =>
            {
                return new ValueWithColour(results.Where(v => v.StopReason == WalkStopReason.HitSpike0p5).AverageSafe(v => v.Bankroll));
            });

            this.RenderRow(table, $"Min spike 0.5x stop", runs, (index, results, args) =>
            {
                return new ValueWithColour(results.Where(v => v.StopReason == WalkStopReason.HitSpike0p5).MinSafe(v => v.Bankroll));
            });

            this.RenderRow(table, $"Max spike 0.5x stop", runs, (index, results, args) =>
            {
                return new ValueWithColour(results.Where(v => v.StopReason == WalkStopReason.HitSpike0p5).MaxSafe(v => v.Bankroll));
            });

            sep();

            foreach(var reason in Enum.GetValues<WalkStopReason>())
            {
                if (reason != WalkStopReason.HitSpike1p5 && reason != WalkStopReason.HitSpike2 && reason != WalkStopReason.HitSpike3Plus)
                {
                    this.RenderRow(table, $"Stop: " + reason, runs, (index, results, args) =>
                    {
                        return new PercentageWithColour(results.Where(v => v.StopReason == reason).Percentage(numRounds));
                    });
                }
            }

            sep();

            this.RenderRow(table, "Check all", runs, (index, results, args) =>
            {
                WalkOutcomesBucket outcome = null;
                if (index == 0)
                    outcome = baselineOutcomes;
                else
                    outcome = otherOutcomes[index];

                var total = (int)(outcome.MajorBustPercentage + outcome.MinorBustPercentage + outcome.EvensPercentage + outcome.Spike0p5Percentage +
                    outcome.Spike1Percentage + outcome.Spike1p5Percentage + outcome.Spike2Percentage + outcome.Spike3PlusPercentage);
                return new PercentageWithColour(total, total == 1 ? ConsoleColor.Green : ConsoleColor.Red);
            });

            table.Render();

            this.WriteCsv(table);
        }

        private void WriteCsv(ConsoleTable table)
        {
            var path = @$"c:\Mbrit\Casino\Walks\Walk--{DateTime.UtcNow.ToString("yyyyMMdd-HHmmss")}.csv";
            Runtime.Current.EnsureFolderForFileCreated(path);
            table.WriteCsv(path);
        }

        private ConsoleColor CompareColours(decimal a, decimal b, bool higherIsBetter)
        {
            if (((a <= b) && !(higherIsBetter)) || ((a > b) && higherIsBetter))
                return ConsoleColor.Red;
            else
                return ConsoleColor.Green;
        }

        private static void DumpProbabilities(int min, int maxInvestment, int takeProfit, IEnumerable<WalkResult> results, int numHands)
        {
            var path = @$"c:\Mbrit\Casino\probabilities--{DateTime.UtcNow.ToString("yyyyMMdd-HHmmss")}--{results.Count()}.csv";
            Runtime.Current.EnsureFolderForFileCreated(path);

            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine("Bankroll,Probability");
                foreach (var result in results)
                {
                    writer.Write(result.Bankroll);
                    writer.Write(",");
                    writer.Write(result.WinLose);
                }
            }
        }

        private void DumpVectors(IEnumerable<WalkResult> results, int numHands)
        {
            var byOutcome = results.ToDictionaryList(v => v.Outcome);
            var min = byOutcome.Values.Select(v => v.Count()).Min();

            // dump all of them...
            var now = DateTime.UtcNow;
            var path = @$"c:\Mbrit\Casino\vectors--{now.ToString("yyyyMMdd-HHmmss")}--{results.Count()}--all.csv";
            this.DumpVectorsInternal(results, path, numHands, (result) =>
            {
                if (result.Outcome == WalkOutcome.MajorBust)
                    return 0;
                else if (result.Outcome == WalkOutcome.MinorBust)
                    return 1;
                else if (result.Outcome == WalkOutcome.Evens)
                    return 2;
                else if (result.Outcome == WalkOutcome.Spike0p5)
                    return 3;
                else if (result.Outcome == WalkOutcome.Spike1OrBetter)
                    return 4;
                else
                    throw new NotSupportedException($"Cannot handle '{result.Outcome}'.");
            });

            // go through the set...
            var rand = ThreadRandom.Value;

            foreach (var outcome in byOutcome.Keys)
            {
                this.LogInfo(() => $"Running '{outcome}' and 'Not {outcome}'...");

                var isSet = byOutcome[outcome].Sample(min, rand);

                var candidates = new List<WalkResult>();
                foreach (var walk in byOutcome.Keys)
                {
                    if (walk != outcome)
                    {
                        var candidatesForOutcome = byOutcome[walk].Sample(min, rand);
                        candidates.AddRange(candidatesForOutcome);
                    }
                }

                var isNotSet = candidates.Sample(min, rand);

                var forDump = new List<WalkResult>(isSet);
                forDump.AddRange(isNotSet);

                path = @$"c:\Mbrit\Casino\vectors--{now.ToString("yyyyMMdd-HHmmss")}--{results.Count()}--{outcome}.csv";
                this.DumpVectorsInternal(forDump, path, numHands, (result) =>
                {
                    if (result.Outcome == outcome)
                        return 1;
                    else
                        return 0;
                });
            }

            this.LogInfo(() => "Finished dumping vectors.");
        }

        private Random Random => ThreadRandom.Value;

        private void DumpVectorsInternal(IEnumerable<WalkResult> results, string path, int numHands, Func<WalkResult, int> getLabel)
        {
            Runtime.Current.EnsureFolderForFileCreated(path);

            using (var writerx = new StreamWriter(path))
            {
                var csv = new CsvDataWriter(writerx);
                csv.WriteValue("label");
                csv.WriteValue("outcome");
                csv.WriteValue("bankroll");
                csv.WriteValue("count");

                for (var index = 0; index < numHands; index++)
                    csv.WriteValue("r" + index);

                csv.WriteLine();

                foreach (var result in results)
                {
                    var label = getLabel(result);
                    csv.WriteValue(label);

                    csv.WriteValue(result.Outcome);
                    csv.WriteValue(result.Bankroll);

                    csv.WriteValue(result.Vectors.Count());

                    var asList = result.Vectors.ToList();

                    for (var index = 0; index < numHands; index++)
                    {
                        if (index < asList.Count)
                        {
                            if (asList[index] == WinLoseDrawType.Win)
                                csv.WriteValue(1);
                            else if (asList[index] == WinLoseDrawType.Lose)
                                csv.WriteValue(0);
                            else
                                throw new NotSupportedException($"Cannot handle '{asList[index]}'.");
                        }
                    }

                    csv.WriteLine();
                }
            }

            this.LogInfo(() => "Finished writing vectors --> " + path);
        }

        //public WalkResult Walk(int unit, int maxInvestment, int takeProfit, decimal sideSplit, int numHands, Random rand, bool trace = true)
        public WalkResult DoWalk(IWinLoseDrawRound round, WalkArgs args, IWalkGamePlayer player, IWalkGameSetup setup, WalkRunArgs runArgs = null)
        {
            runArgs ??= new WalkRunArgs();

            //var allCasinos = Casino.GetCasinos();

            var rand = this.Random;         // seeded per thread...

            //var casinos = new List<Casino>();
            //for (var index = 0; index < args.MaxHands; index++)
            //{
            //    //casinos.Add(allCasinos.PickRandom(rand));
            //    casinos.Add(allCasinos.First());
            //}

            var games = new List<GameType>()
            {
                GameType.Blackjack,
                GameType.Baccarat,
                GameType.Roulette,
                GameType.Craps,
            };

            //const decimal crapsRatio = 0.5M;

            //var bankroll = 0;
            //var banked = 0;
            //var invested = 0;
            //var totalWagered = 0;
            //var numInvestments = 0;

            var doSideBetsOverall = args.SideSplit > 0;

            GameType? lastGame = null;

            var vectors = new List<WinLoseDrawType>();

            //var proxy = new WalkProxy();

            //var strategy = runArgs.Strategy;

            var trace = runArgs.DoTrace;

            WalkState state = null;
            if (runArgs.CreateState != null)
                state = runArgs.CreateState(args);

            if(state == null)
                state = new WalkState(args);

            if (args.StopAtWinMode == WalkGameMode.StretchToSpike1 && !(args.DoLimitedStretchHands) && !(args.DoLimitedStretchInvestment))
                throw new InvalidOperationException("Stretch mode has to have a limit.");

            var seenSpike0p5 = false;
            var seenSpike1 = false;
            var seenSpike1p5 = false;
            var seenSpike2 = false;
            var seenSpike3 = false;

            var abandonAtHand = -1;

            var doStopInvesting = false;
            var stopInvesting = -1;

            player.StartPlaying(round, args, state);

            var hand = 0;
            while (true)
            {
                if (!(player.CanPlayHand(hand)))
                {
                    if (trace)
                        Console.WriteLine("Player can't play hand -- stopping...");

                    break;
                }

                if (trace)
                {
                    Console.WriteLine("-------------------");
                    Console.WriteLine($"#{vectors.Count + 1}: --> {state.Bankroll}, {state.Invested}, {state.Banked}");
                }

                // get the player decision...
                var decision = player.GetDecision(hand, round, args, state);

                // what are we playing? we don't need this really...
                var game = GameType.Baccarat;

                // do we have money?
                if (state.Bankroll < state.CurrentUnit)
                {
                    //if (invested >= args.MaxInvestment)
                    if (!(state.HasInvestables))
                    {
                        if (trace)
                            Console.WriteLine("Nothing to invest.");

                        state.StopReason = WalkStopReason.NothingToInvest;
                        break;
                    }

                    if (state.Bankroll > 0)
                    {
                        // we have to deal with odd amounts...
                        state.Banked += state.Bankroll;
                    }

                    state.Bankroll = 0;

                    var putIn = state.PutIn();

                    if (trace)
                        Console.WriteLine("Investing --> " + putIn);

                    if (doStopInvesting)
                        stopInvesting--;
                }
                else
                {
                    if (args.DoHailMary && state.BankrollUnits == args.TakeProfitMultiplier && state.HailMarysRemaining > 0 && state.Banked > 0)
                    {
                        if (this.ChooseHailMary())
                        {
                            if ((args.HailMaryMode == WalkHailMary.Double && state.InvestablesRemaining >= 4) ||
                                (args.HailMaryMode == WalkHailMary.Half && state.InvestablesRemaining >= 3) ||
                                (args.HailMaryMode == WalkHailMary.Single && state.InvestablesRemaining >= 2))
                            {
                                var until = 0;
                                if (args.HailMaryMode == WalkHailMary.Single)
                                    until = 2;
                                else if (args.HailMaryMode == WalkHailMary.Half)
                                    until = 3;
                                else if (args.HailMaryMode == WalkHailMary.Double)
                                    until = 4;
                                else
                                    throw new NotSupportedException($"Cannot handle '{args.HailMaryMode}'.");

                                for (var index = 0; index < until; index++)
                                {
                                    state.PutIn();
                                }

                                state.HailMarysRemaining--;
                            }
                        }
                    }
                }

                //var win = strategy.GetWin(rand);
                var win = round.GetResult(hand);

                var doSideBets = doSideBetsOverall && game != GameType.Roulette;

                var sideWin = SideBetWin.None;
                if (doSideBets)
                    sideWin = this.GetSideWin(game);

                try
                {
                    var theBaseBet = state.Bankroll;

                    //if (args.TopUp4Bet && state.BankrollUnits == 2 && state.HasInvestables && (args.MaxTopUp4Bets == 0 || state.TopUp4BetCount < args.MaxTopUp4Bets))
                    //{
                    //    theBaseBet += state.PutIn();
                    //    state.TopUp4BetCount++;
                    //}

                    if (doSideBets)
                        theBaseBet = (int)((decimal)state.Bankroll * args.SideSplit);

                    var theSideBet = state.Bankroll - theBaseBet;

                    if (trace)
                        Console.WriteLine($"Betting --> {theBaseBet} + {theSideBet}");

                    state.TotalWagered += (theBaseBet + theSideBet);

                    state.Bankroll = 0;

                    if (win == WinLoseDrawType.Win || sideWin != SideBetWin.None)
                    {
                        if (win == WinLoseDrawType.Win)
                            state.Bankroll += theBaseBet + theBaseBet;      // as in, evens (because we reset the bankroll to 0 before check the status...)

                        if (sideWin != SideBetWin.None)
                            state.Bankroll += theSideBet + this.GetSideWinValue(theSideBet, sideWin);

                        if (trace)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"*** {game} win *** --> now " + state.Bankroll + ", " + state.Banked + ", profit: " + state.Profit);
                        }

                        vectors.Add(WinLoseDrawType.Win);

                        state.AddWin();

                        if (state.Bankroll > state.TakeProfit)
                        {
                            var toTake = state.Bankroll - state.TakeProfit;

                            if (trace)
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine("Taking profit --> " + toTake);
                            }

                            state.Banked += toTake;
                            //bankroll = state.TakeProfit;
                            state.Bankroll -= toTake;

                            if (trace)
                            {
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Console.WriteLine($"After take --> bankroll: {state.Bankroll}, banked: {state.Banked}");
                            }
                        }
                    }
                    else if (win == WinLoseDrawType.Lose)
                    {
                        if (trace)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"*** {game} loss ***");
                        }

                        vectors.Add(WinLoseDrawType.Lose);

                        state.AddLoss();
                    }
                    else
                        throw new NotSupportedException($"Cannot handle '{win}'.");

                    if (runArgs.Step != null)
                    {
                        // get the vectors before...
                        var vectorsBefore = new List<WinLoseDrawType>(vectors);
                        vectorsBefore.RemoveAt(vectorsBefore.Count - 1);

                        runArgs.Step(hand, win, vectorsBefore, state);
                    }
                }
                finally
                {
                    if (trace)
                        Console.ResetColor();
                }

                lastGame = game;

                if (vectors.Count == 10)
                {
                    // call the ml...
                    //var response = proxy.Predict(vectors);

                    /*
                    bustProbability = response.BustProbability;

                    if (bustProbability >= 0.5M)
                        bustType = BustType.NotBusted;
                    else
                        bustType = BustType.Busted;

                    if (trace)
                        Console.WriteLine($"Bust probability --> {response.BustProbability}, {bustType}");
                    */

                    //if (response.BustProbability >= 0.7M)
                    //    break;
                    //else
                    /*
                    if (response.BustProbability <= .4M)
                    {
                        min = (int)((decimal)min * 1.5M);
                        //takeProfit = (int)((decimal)min * 1.5M);

                        if (trace)
                            Console.WriteLine("Bet level is now --> " + min);
                    }
                    */
                }

                hand++;

                // are we leaving early?
                if (state.IsAborted)
                {
                    if (trace)
                        Console.WriteLine("Aborted.");

                    break;
                }

                if (hand == args.MaxHands)
                {
                    if (trace)
                        Console.WriteLine($"Stopping at max hands ({args.MaxHands}).");

                    break;
                }

                /*
                if(!(seenSpike0p5) && state.Bankroll + state.Banked - state.Invested >= args.Spike0p5Win)
                {
                    // set...
                    seenSpike0p5 = true;

                    if (args.StopAtWinType == WalkStopType.ReachedSpike0p5)
                    {
                        state.StopReason = WalkStopReason.HitSpike0p5;
                        break;
                    }
                }
                */

                /*
                if (!(seenSpike1) && state.Bankroll + state.Banked - state.Invested >= args.Spike1Win)
                {
                    // set...
                    seenSpike1 = true;

                    if (args.StopAtWinType == WalkStopType.ReachedSpike1 || args.StopAtWinType == WalkStopType.StretchToSpike1)
                    {
                        state.StopReason = WalkStopReason.HitSpike1;
                        break;
                    }
                }
                */

                /*
                if (args.DoStopAtWin && args.StopAtWinMode == WalkStopMode.ReachedSpike0p5 &&
                    //(state.Bankroll + state.Banked + state.Investable) >= (2 * args.Spike0p5Win) &&
                    //state.Bankroll + state.Banked - state.Invested >= args.Spike0p5Win)
                    this.HasReached(state, args.Spike0p5Win))
                {
                    if (trace)
                        Console.WriteLine("Stopping at spike 0.5x.");

                    state.StopReason = WalkStopReason.HitSpike0p5;
                    break;
                }

                if (args.DoStopAtWin && args.StopAtWinMode == WalkStopMode.ReachedSpike1 && 
                    //(state.Bankroll + state.Banked + state.Investable) >= (2 * args.Spike1Win) &&
                    //state.Bankroll + state.Banked - state.Invested >= args.Spike1Win)
                    HasReached(state, args.Spike1Win))
                {
                    if (trace)
                        Console.WriteLine("Stopping at spike.");

                    state.StopReason = WalkStopReason.HitSpike1;
                    break;
                }
                */

                var atOrAboveSpike0p5 = this.HasReached(state, args.Spike0p5Win);
                var atOrAboveSpike1 = this.HasReached(state, args.Spike1Win);
                var atOrAboveSpike1p5 = this.HasReached(state, args.Spike1p5Win);
                var atOrAboveSpike2 = this.HasReached(state, args.Spike2Win);
                var atOrAboveSpike3 = this.HasReached(state, args.Spike3Win);

                Action<WalkStopReason> stopAtSpike1 = (reason) =>
                {
                    if (trace)
                        Console.WriteLine($"Stopping at '{reason}'.");

                    state.StopReason = reason;
                };

                var stopReason = WalkStopReason.None;
                if (args.DoStopAtWin)
                {
                    if (args.StopAtWinMode == WalkGameMode.ReachSpike0p5 || args.StopAtWinMode == WalkGameMode.ReachSpike1 ||
                        args.StopAtWinMode == WalkGameMode.ReachSpike2 || args.StopAtWinMode == WalkGameMode.ReachSpike3)
                    {
                        if (this.CheckStop(state, args, out stopReason))
                        {
                            stopAtSpike1(stopReason);
                            break;
                        }
                    }
                    else if(args.StopAtWinMode == WalkGameMode.StretchToSpike1)
                    {
                        /*
                        if (atOrAboveSpike0p5 || seenSpike0p5)
                        {
                            var remaining = args.MaxHands - hand;
                            var evaluation = this.Evaluate(args, state, remaining, setup, trace);

                            var bucket = setup.GetTag<EvaluationBucket>();
                            bucket.AddEvaluation(round, hand, state, evaluation);
                        }
                        */

                        if ((args.DoLimitedStretchHands || args.DoLimitedStretchInvestment) && this.HasReached(state, args.Spike0p5Win))
                        {
                            if (args.DoLimitedStretchHands && abandonAtHand == -1)
                                abandonAtHand = hand + args.LimitedStretchHands;

                            if (args.DoLimitedStretchInvestment && !(doStopInvesting))
                            {
                                doStopInvesting = true;
                                stopInvesting = args.LimitedStretchInvestments;
                            }
                        }
                    }
                }

                // set it...
                if (!(seenSpike0p5) && atOrAboveSpike0p5)
                    seenSpike0p5 = true;
                if (!(seenSpike1) && atOrAboveSpike1)
                    seenSpike1 = true;
                if (!(seenSpike1p5) && atOrAboveSpike1p5)
                    seenSpike1p5 = true;
                if (!(seenSpike2) && atOrAboveSpike2)
                    seenSpike2 = true;
                if (!(seenSpike3) && atOrAboveSpike3)
                    seenSpike3 = true;

                Action<string> logStop = (message) =>
                {
                    if (trace)
                        Console.WriteLine(message);
                };

                if(args.DoStopAtWin && args.StopAtWinMode == WalkGameMode.StretchToSpike1 && seenSpike1)
                {
                    logStop($"Hit Spike 1x.");
                    stopAtSpike1(WalkStopReason.HitSpike1);
                    break;
                }

                if (abandonAtHand > 0 && hand >= abandonAtHand)
                {
                    logStop($"Instructed to abandon at '{abandonAtHand}' hand.");
                    state.StopReason = WalkStopReason.DynamicHandsLimit;
                    break;
                }

                if (doStopInvesting && stopInvesting == 0)
                {
                    logStop($"Instructed to abandon after limited investment.");
                    state.StopReason = WalkStopReason.DynamicInvestmentLimit;
                    break;
                }

                /*
                if(args.StopAtWinMode == WalkGameMode.StretchToSpike1 && seenSpike0p5 &&
                    args.DoAbandonAtOrUnderChainScore && state.ChainScore <= args.AbandonAtOrUnderChainScore)
                {
                    logStop($"Negative chain score.");
                    state.StopReason = WalkStopReason.NegativeChainScore;
                    break;
                }
                */

                /*
                if (args.HasBadRunLimit && state.MaxLossChain > args.BadRunLimit)
                {
                    logStop($"Hit bad run");
                    state.StopReason = WalkStopReason.BadRun;
                    break;
                }

                if (args.HasGoodRunLimit && state.MaxWinChain > args.GoodRunLimit)
                {
                    logStop($"Hit good run");
                    state.StopReason = WalkStopReason.GoodRun;
                    break;
                }
                */

                /*
                if (args.DoStopAtMinusUnits && state.InvestedUnits >= args.StopAtMinusUnits && !(seenSpike0p5))
                {
                    logStop($"Stopped because bleeding.");
                    state.StopReason = WalkStopReason.Bleeding;
                    break;
                }
                */
            }

            var final = (state.Bankroll + state.Banked) - state.Invested;

            //if (args.AddInvestablesToFinal)
            //    final += state.Investable;

            //var minor = invested * .5;
            //var spike = invested;

            WalkOutcome outcome;
            var spikeType = WalkSpikeType.One;

            if (final < args.MinorBust)
                outcome = WalkOutcome.MajorBust;
            else if (final <= 0)
                outcome = WalkOutcome.MinorBust;
            else if (final < args.Spike0p5Win)
                outcome = WalkOutcome.Evens;
            else if (final < args.Spike1Win)
                outcome = WalkOutcome.Spike0p5;
            else
            {
                outcome = WalkOutcome.Spike1OrBetter;

                if (final >= args.Spike3Win)
                    spikeType = WalkSpikeType.ThreePlus;
                else if (final >= args.Spike2Win)
                    spikeType = WalkSpikeType.Two;
                else if (final >= args.Spike1p5Win)
                    spikeType = WalkSpikeType.OnePointFive;
                else
                    spikeType = WalkSpikeType.One;
            }

            if (trace)
            {
                Console.WriteLine("==================");
                Console.WriteLine("Invested --> " + state.Invested + ", count: " + state.NumInvestments);
                Console.WriteLine($"Cash in hand (bankroll, banked, remaining investment) --> {state.Bankroll} + {state.Banked} + {state.Investable} = {state.Bankroll + state.Banked + state.Investable}");
                Console.WriteLine("Final (ignoring what we didn't invest) --> " + final);
                Console.WriteLine("Final (ignoring what we didn't invest) [state] --> " + state.Profit);
            }

            var result = new WalkResult(state, round, vectors, outcome, spikeType, false, false, false, false);

            if (runArgs.Finished != null)
                runArgs.Finished(state, final);

            player.StopPlaying(round, args, state, result);

            return result;
        }

        private bool ChooseHailMary() => this.Random.Next(0, 999) >= 500;

        private bool CheckStop(WalkState state, WalkArgs args, out WalkStopReason stopReason)
        {
            stopReason = WalkStopReason.None;

            if (args.StopAtWinMode == WalkGameMode.ReachSpike0p5 && this.HasReached(state, args.Spike0p5Win))
            {
                stopReason = WalkStopReason.HitSpike0p5;
                return true;
            }
            else if ((args.StopAtWinMode == WalkGameMode.ReachSpike1) && this.HasReached(state, args.Spike1Win))
            {
                stopReason = WalkStopReason.HitSpike1;
                return true;
            }
            else if (args.StopAtWinMode == WalkGameMode.ReachSpike1p5 && this.HasReached(state, args.Spike1p5Win))
            {
                stopReason = WalkStopReason.HitSpike1p5;
                return true;
            }
            else if (args.StopAtWinMode == WalkGameMode.ReachSpike2 && this.HasReached(state, args.Spike2Win))
            {
                stopReason = WalkStopReason.HitSpike2;
                return true;
            }
            else if (args.StopAtWinMode == WalkGameMode.ReachSpike3 && this.HasReached(state, args.Spike3Win))
            {
                stopReason = WalkStopReason.HitSpike3Plus;
                return true;
            }
            else
                return false;
        }

        private bool HasReached(WalkState state, int win)
        {
            return (state.Bankroll + state.Banked + state.Investable) >= (2 * win) &&
                state.Bankroll + state.Banked - state.Invested >= win;
        }

        private WalkOutcomesBucket Evaluate(WalkArgs args, WalkState state, int remainingHands, WalkGameSetup setup, bool trace)
        {
            if (trace)
                Console.WriteLine($"Evaluating game with '{remainingHands}' hand(s)...");

            var rounds = WinLoseDrawRoundsBucket.GetAllWinLosePermutations(remainingHands, this.Random);

            var evalSetup = new WalkGameSetup(rounds, state.CurrentUnit);
            evalSetup.HandsPerRound = remainingHands;

            var evalArgs = new WalkArgs(evalSetup);
            evalArgs.StopAtWinMode = WalkGameMode.ReachSpike1;

            // evaluation always uses an automatic player...
            var player = new AutomaticWalkGamePlayer();

            var run = this.DoRun(evalSetup, player, (runArgs) =>
            {
                runArgs.CreateState = (theArgs) =>
                {
                    return state.Clone();
                };

            }, () =>
            {
                return evalArgs;
            });

            var outcomes = run.GetOutcomes();
            if (trace)
                outcomes.Dump();
            return outcomes;
        }

        private int GetSideWinValue(int sideBet, SideBetWin sideWin) => (int)((decimal)sideBet * sideWin.GetPayoutMultiplier()) - sideBet;

        /*
        private int PlayCraps(int bankroll, Random rand, bool trace)
        {
            if(trace)
                Console.WriteLine("Playing craps with " + bankroll);

            var places = new Dictionary<int, int>();
            for (var index = 2; index <= 12; index++)
                places[index] = 0;

            const int per = 5;

            while (true)
            {
                for (var index = 2; index <= 12; index++)
                {
                    if (index == 4 || index == 5 || index == 6 || index == 8 || index == 9 || index == 10)
                    {
                        places[index] += 5;
                        bankroll -= 5;
                    }

                    if (bankroll < per)
                        break;
                }

                if (bankroll < per)
                    break;
            }

            if (trace)
                Console.Write("Rolling --> ");

            var first = true;
            while (true)
            {
                var dice = new Dice(2);
                var score = dice.Roll(rand);

                if (trace)
                {
                    if (first)
                        first = false;
                    else
                        Console.Write(", ");
                    Console.Write(score.Score);
                }

                if (score.Score == 7)
                    break;

                if (places[score.Score] != 0)
                {
                    GameOdds odds = null;
                    if (score.Score == 4 || score.Score == 10)
                        odds = Craps.Place4Or10;
                    else if (score.Score == 5 || score.Score == 9)
                        odds = Craps.Place5Or9;
                    else if (score.Score == 6 || score.Score == 8)
                        odds = Craps.Place6Or8;
                    else
                        throw new NotSupportedException($"Cannot handle '{score.Score}'.");

                    var win = odds.GetWin(places[score.Score]);
                    bankroll += (int)Math.Floor(win);
                }
            }

            if (trace)
            {
                Console.WriteLine();
                Console.WriteLine("Craps winnings --> " + bankroll);
            }

            return bankroll;
        }
        */

        private SideBetWin GetSideWin(GameType game)
        {
            var rand = this.Random;
            var roll = rand.Next(1000); // 0–999

            if (game == GameType.Craps)
            {
                if (roll < 5)
                    return SideBetWin.Jackpot;   // 0.5% → 50x (All)
                if (roll < 15)
                    return SideBetWin.Major;     // 1% → 15x (Tall/Small)
                if (roll < 70)
                    return SideBetWin.Minor;     // 5.5% → 5x
                if (roll < 150)
                    return SideBetWin.Bonus;     // 8% → 2x
                else
                    return SideBetWin.None;      // 84%
            }
            else if (game == GameType.Baccarat)
            {
                if (roll < 10)
                    return SideBetWin.Jackpot;  // 1% → 30x
                if (roll < 25)
                    return SideBetWin.Major;    // 1.5% → 15x
                if (roll < 80)
                    return SideBetWin.Minor;    // 5.5% → 5x
                if (roll < 160)
                    return SideBetWin.Bonus;    // 8% → 2x                else
                else
                    return SideBetWin.None;      // 84%
            }
            else
            {
                if (roll < 10)   // 1%
                    return SideBetWin.Jackpot;
                if (roll < 25)   // next 1.5%
                    return SideBetWin.Major;
                if (roll < 80)   // next 5.5%
                    return SideBetWin.Minor;
                if (roll < 160)  // next 8%
                    return SideBetWin.Bonus;
                else
                    return SideBetWin.None;       // 84%
            }
        }

        /*
        internal void MeasureSpikes() => this.Measure(WalkOutcome.Spike);

        internal void MeasureMajorBusts() => this.Measure(WalkOutcome.MajorBust);

        internal void MeasureMinorBusts() => this.Measure(WalkOutcome.MinorBust);

        internal void MeasureMinors() => this.Measure(WalkOutcome.Minor);

        internal void MeasureEvens() => this.Measure(WalkOutcome.Evens);
        */

        internal void Measure(WalkOutcome outcome)
        {
            /*
            var args = GetDefaultArgs();

            var results = new List<WalkResult>();
            while (results.Count < 16)
            {
                var result = this.Walk(args);
                if (result.Outcome == outcome && result.Vectors.Count() == args.MaxHands)
                {
                    results.Add(result);
                }
            }

            var proxy = new WalkProxy()
            {
                LogRequests = true,
                LogJson = true
            };

            var probabilities = new Dictionary<int, Dictionary<int, decimal>>();

            for (var index = 0; index < results.Count; index++)
            {
                var result = results[index];

                probabilities[index] = new Dictionary<int, decimal>();

                var runArgs = new WalkRunArgs()
                {
                    Strategy = new PlaybackWalkStrategy(result.Vectors),
                    Step = (stepIndex, win, vectorsBefore, adjuster) =>
                    {
                        var vectorIndex = vectorsBefore.Count();

                        if (vectorIndex > StartAdvisingAt && vectorIndex < StopAdvisingAt)
                        {
                            this.LogInfo(() => $"Predicting '{outcome}' on run #{stepIndex}, step '{vectorIndex}'...");

                            // predict...
                            var response = proxy.Predict(outcome, vectorsBefore);
                            probabilities[index][vectorIndex] = response.Probabilities[1];

                        }
                    }
                };

                //this.Walk(args, runArgs);
                throw new NotImplementedException("This operation has not been implemented.");
            }

            // render...
            var table = new ConsoleTable();

            var header = table.AddHeaderRow();
            header.AddCell(string.Empty);

            this.RenderRow(header, results, (index, result) => index);

            this.RenderRow(table, "Outcome", results, (index, result) => result.Outcome);
            this.RenderRow(table, "Bankroll", results, (index, result) => result.Bankroll);
            
            for (var stepIndex = StartAdvisingAt; stepIndex < StopAdvisingAt; stepIndex++)
            {
                this.RenderRow(table, "P(o) #" + stepIndex, results, (index, result) =>
                {
                    var value = probabilities[index].GetValueOrDefault(stepIndex);

                    var prediction = this.GetPrediction(outcome, value);

                    var colour = ConsoleColor.Gray;
                    if (prediction == WalkPrediction.WillHit)
                        colour = ConsoleColor.Green;
                    else if (prediction == WalkPrediction.Neutral)
                        colour = ConsoleColor.Yellow;
                    else if (prediction == WalkPrediction.WillNotHit)
                        colour = ConsoleColor.Red;
                    else
                        throw new NotSupportedException($"Cannot handle '{prediction}'.");

                    return new ValueWithColour(value, colour);
                });
            }

            table.Render();
            */

            throw new NotImplementedException("This operation has not been implemented.");
        }

        /*
        private WalkPrediction GetPrediction(WalkOutcome outcome, decimal value)
        {
            if (outcome == WalkOutcome.Evens || outcome == WalkOutcome.Spike0p5)
            {
                if (value >= .5M)
                    return WalkPrediction.WillHit;
                else
                    return WalkPrediction.WillNotHit;
            }
            else
            {
                if (value >= .75M)
                    return WalkPrediction.WillHit;
                else if (value >= .5M)
                    return WalkPrediction.Neutral;
                else
                    return WalkPrediction.WillNotHit;
            }
        }
        */

        private void RenderRow<T>(ConsoleTable table, string name, IEnumerable<WalkRun> runs, Func<int, IEnumerable<WalkResult>, WalkArgs, T> callback)
        {
            var row = table.AddRow(name);
            this.RenderRow<T>(row, runs, callback); 
        }

        private void RenderRow<T>(ConsoleTableRow row, IEnumerable<WalkRun> runs, Func<int, IEnumerable<WalkResult>, WalkArgs, T> callback)
        {
            var index = 0;
            foreach(var run in runs)
            {
                var value = callback(index, run.Results, run.Args);

                if (value is ValueWithColour)
                {
                    var theValue = (ValueWithColour)(object)value;

                    string asString = null;
                    if (theValue.Value is decimal)
                        asString = ((decimal)theValue.Value).ToString("n3");    
                    else
                        asString = theValue.Value.ToString();

                    if (theValue.Flagged)
                        asString = this.Flag(asString);

                    var cell = row.AddCell(asString);
                    cell.ForegroundColor = theValue.colour;
                }
                else if (value is PercentageWithColour)
                {
                    var theValue = (PercentageWithColour)(object)value;

                    var asString = (theValue.Value * 100).ToString("n2") + "%";

                    if (theValue.Flagged)
                        asString = this.Flag(asString);

                    var cell = row.AddCell(asString);
                    cell.ForegroundColor = theValue.colour;
                }
                else
                    row.AddCell(value);

                index++;
            }
        }

        private string Flag(string value) => value + " ♦";
    }
}
