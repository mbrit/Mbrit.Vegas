//using BootFX.Common;
//using BootFX.Common.Data.Text;
//using BootFX.Common.Management;
//using Mbrit.Vegas.Games;
//using System;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Data;
//using System.Diagnostics.Eventing.Reader;
//using System.Linq;
//using System.Net;
//using System.Net.Http.Headers;
//using System.Net.Mail;
//using System.Net.WebSockets;
//using System.Runtime.CompilerServices;
//using System.Runtime.ExceptionServices;
//using System.Runtime.InteropServices;
//using System.Security.Claims;
//using System.Security.Principal;
//using System.Text;
//using System.Threading.Tasks;

//namespace Mbrit.Vegas.Utility
//{
//    internal record WalkRun2(IEnumerable<WalkResult2> Results, WalkArgs Args, int Runs);

//    internal class WalkFoo2 : CliFoo
//    {
//        private static readonly ThreadLocal<Random> ThreadRandom = new(() => new Random(VegasRuntime.GetToken().GetHashCode()));

//        private int StartAdvisingAt = 10;
//        private int StopAdvisingAt = 24;

//        internal void DoMagicN()
//        {
//            var runs = this.GetValueWithDefault<int>("Number of runs");
//            this.DoMagic(runs);
//        }

//        internal static WalkArgs GetDefaultArgs(int unit = 125) => new WalkArgs(unit, 4, 12);

//        private string FormatPercentage(int count, int numRounds)
//        {
//            return (((decimal)count / (decimal)numRounds) * 100).ToString("n2") + "%";
//        }

//        internal void DoMagic(int numRounds = 25000)
//        {
//            this.LogInfo(() => $"Running '{numRounds}' simulation(s)...");

//            //const int min = 125; // * 4 * 6 * 2;
//            //const int maxInvestment = min * 12;
//            //const int takeProfit = (int)((decimal)min * 4M); //  500;
//            //const decimal sideSplit = 0; //  0.75M;

//            var args = GetDefaultArgs();

//            // get the rounds...
//            const decimal houseEdge = Game.AverageHouseEdge;
//            var rounds = WinLoseDrawRoundsBucket.GetWinLose(25000, args.MaxHands * 2, houseEdge, this.Random);

//            // from the analysis...
//            /*
//            const int min = 100;
//            const int putIns = 12;
//            const int maxInvestment = min * putIns;
//            const decimal takeProfitMultipler = 4M;
//            const int takeProfit = (int)((decimal)min * takeProfitMultipler); // 460;
//            const decimal sideSplit = 0; //  0.75M;

//            var baseBet = Math.Round(min * sideSplit);
//            while (baseBet % 5 != 0)
//                baseBet--;

//            var sideBet = min - baseBet;
//            */

//            Preamble(args);

//            var results = new List<WalkResult2>();
//            var _lock = new object();

//            var runArgs = new WalkRunArgs()
//            {
//                DoTrace = numRounds == 1
//            };

//            var logAt = DateTime.UtcNow.AddSeconds(2);
//            //for (var index = 0; index < runs; index++)
//            var index = 0;
//            foreach (var round in rounds)
//            {
//                var result = this.Walk(round, args, runArgs);

//                lock (_lock)
//                    results.Add(result);

//                if (DateTime.UtcNow >= logAt)
//                {
//                    this.LogInfo(() => $"Done {index + 1:n0} of {numRounds:n0}...");
//                    logAt = DateTime.UtcNow.AddSeconds(2);
//                }

//                index++;
//                if (index == numRounds)
//                    break;
//            }

//            var runs = new List<WalkRun2>();
//            runs.Add(new WalkRun2(results, args, numRounds));

//            var bankrolls = results.Select(v => v.Bankroll);
//            var wagereds = results.Select(v => v.TotalWagered);

//            Func<int, string> getPercentage = (count) => this.FormatPercentage(count, numRounds);

//            Console.WriteLine("=========================");
//            Console.WriteLine("Losses --> " + getPercentage(bankrolls.Count(v => v <= 0)));

//            var numMinorBusts = results.Count(v => v.Outcome == WalkOutcome.MinorBust);
//            Console.WriteLine($"Minor busts (> {args.MinorBust} < 0) [check] --> " + numMinorBusts + ", " + getPercentage(numMinorBusts));

//            var numMajorBusts = results.Count(v => v.Outcome == WalkOutcome.MajorBust);
//            Console.WriteLine($"Major busts (< {args.MinorBust}) [check] --> " + numMajorBusts + ", " + getPercentage(numMajorBusts));

//            var numBusts = numMajorBusts + numMinorBusts;
//            Console.WriteLine("Total busts [check] --> " + getPercentage(numBusts));

//            Console.WriteLine("=========================");
//            Console.WriteLine($"Average wagered --> " + wagereds.Average());
//            Console.WriteLine("=========================");

//            if (numRounds > 1)
//            {
//                // this works because the amount in "bankroll" is net of the investment...

//                // first lot...

//                var evens = args.InitialMaxInvestment * .5M;
//                var spike = args.InitialMaxInvestment;

//                Console.WriteLine("Evens --> " + evens);
//                Console.WriteLine("Spike --> " + spike);

//                var numEvens = results.Where(v => v.Outcome == WalkOutcome.Evens).Count();
//                Console.WriteLine($"Num wins over {0} (0x) [check] --> " + numEvens + ", " + getPercentage(numEvens));

//                var numMinor = results.Where(v => v.Outcome == WalkOutcome.Minor).Count();
//                Console.WriteLine($"Num wins over {args.InitialMaxInvestment * .5} (1x) [check] --> " + numMinor + ", " + getPercentage(numMinor));

//                var numSpike = results.Where(v => v.Outcome == WalkOutcome.Spike).Count();
//                Console.WriteLine($"Num wins over {args.InitialMaxInvestment} (2x) [check] --> " + numSpike + ", " + getPercentage(numSpike));

//                Console.WriteLine($"Total --> " + (numBusts + numEvens + numMinor + numSpike));
//                Console.WriteLine($"Total %age --> " + getPercentage(numBusts + numEvens + numMinor + numSpike));
//            }

//            Console.WriteLine($"Max win --> " + bankrolls.Max());
//            Console.WriteLine($"Average --> " + bankrolls.Where(v => v > 0).Average());
//        }

//        private void Preamble(WalkArgs args)
//        {
//            Console.WriteLine("Hands: " + args.MaxHands);
//            Console.WriteLine("Unit: " + args.Unit);
//            Console.WriteLine($"Side split: {args.SideSplit} ({args.BaseUnit}/{args.SideUnit})");
//            Console.WriteLine("Max investment: " + (args.Unit * args.MaxPutIns) + ", put-ins: " + args.MaxPutIns);
//            Console.WriteLine("Take profit: " + (args.Unit * args.TakeProfitMultiplier) + ", multipler: " + args.TakeProfitMultiplier);
//        }

//        private void DumpResults(IEnumerable<WalkRun2> runs)
//        {
//            var checkRounds = runs.SelectAndMerge(v => v.Results);
//            if (checkRounds.Count() != 1)
//                throw new InvalidOperationException("The number of runs have to have the same across the runs.");

//            var numRounds = checkRounds.Count();

//            var table = new ConsoleTable();
//            var header = table.AddHeaderRow();
//            header.AddCell("");
//            header.AddCell("Baseline");

//            Func<int, string> getPercentage = (count) => this.FormatPercentage(count, numRounds);

//            Action sep = () =>
//            {
//                const string sep = "········";
//                this.RenderRow(table, sep, runs, (index, results, args) => new ValueWithColour(sep, ConsoleColor.Cyan));
//            };

//            // config...
//            this.RenderRow(table, "Hands", runs, (index, results, args) => args.MaxHands);
//            this.RenderRow(table, "Unit", runs, (index, results, args) => args.Unit);
//            this.RenderRow(table, "Side split", runs, (index, results, args) => args.SideSplit);
//            this.RenderRow(table, "Initial max investment", runs, (index, results, args) => args.InitialMaxInvestment + ", put-ins: " + args.MaxPutIns);
//            this.RenderRow(table, "Take profit", runs, (index, results, args) => "???" + ", multiplier: " + args.TakeProfitMultiplier);

//            sep();

//            var baseline = runs.First().Args;

//            // metrics...
//            this.RenderRow(table, "Major Busts", runs, (index, results, args) =>
//            {
//                var count = results.Count(v => v.Outcome == WalkOutcome.MajorBust);
//                return getPercentage(count);
//            });

//            this.RenderRow(table, "Minor Busts", runs, (index, results, args) =>
//            {
//                var count = results.Count(v => v.Outcome == WalkOutcome.MinorBust);
//                return getPercentage(count);
//            });

//            this.RenderRow(table, "Busts", runs, (index, results, args) =>
//            {
//                var count = results.Count(v => v.DidBust);
//                return new ValueWithColour(getPercentage(count), ConsoleColor.Yellow);
//            });

//            sep();

//            this.RenderRow(table, "Evens", runs, (index, results, args) =>
//            {
//                var count = results.Count(v => v.Outcome == WalkOutcome.Evens);
//                return getPercentage(count);
//            });

//            this.RenderRow(table, "Minors", runs, (index, results, args) =>
//            {
//                var count = results.Count(v => v.Outcome == WalkOutcome.Minor);
//                return getPercentage(count);
//            });

//            this.RenderRow(table, $"Spikes ({baseline.InitialMaxInvestment})", runs, (index, results, args) =>
//            {
//                var count = results.Count(v => v.Outcome == WalkOutcome.Spike);
//                return new ValueWithColour(getPercentage(count), ConsoleColor.Yellow);
//            });

//            sep();

//            this.RenderRow(table, $"1x ({baseline.InitialMaxInvestment})", runs, (index, results, args) =>
//            {
//                var count = results.Count(v => v.Bankroll >= baseline.InitialMaxInvestment);
//                return new ValueWithColour(getPercentage(count), ConsoleColor.Gray);
//            });

//            this.RenderRow(table, $"1.5x ({baseline.InitialMaxInvestment * 1.5})", runs, (index, results, args) =>
//            {
//                var count = results.Count(v => v.Bankroll >= baseline.InitialMaxInvestment * 1.5);
//                return new ValueWithColour(getPercentage(count), ConsoleColor.Gray);
//            });

//            this.RenderRow(table, $"2x ({baseline.InitialMaxInvestment * 2})", runs, (index, results, args) =>
//            {
//                var count = results.Count(v => v.Bankroll >= baseline.InitialMaxInvestment * 2);
//                return new ValueWithColour(getPercentage(count), ConsoleColor.Gray);
//            });

//            this.RenderRow(table, $"3x ({baseline.InitialMaxInvestment * 3})", runs, (index, results, args) =>
//            {
//                var count = results.Count(v => v.Bankroll >= baseline.InitialMaxInvestment * 3);
//                return new ValueWithColour(getPercentage(count), ConsoleColor.Gray);
//            });

//            sep();

//            this.RenderRow(table, $"Max", runs, (index, results, args) =>
//            {
//                return new ValueWithColour(results.Max(v => v.Bankroll), ConsoleColor.Gray);
//            });

//            this.RenderRow(table, $"Average win", runs, (index, results, args) =>
//            {
//                var average = Math.Floor(results.Where(v => !(v.DidBust)).Average(v => v.Bankroll));
//                return new ValueWithColour(average + ", " + this.FormatPercentage((int)average, baseline.InitialMaxInvestment), ConsoleColor.Gray);
//            });

//            sep();

//            this.RenderRow(table, "Check all", runs, (index, results, args) =>
//            {
//                var count = results.Count();
//                return getPercentage(count);
//            });

//            table.Render();
//        }

//        private static void DumpProbabilities(int min, int maxInvestment, int takeProfit, IEnumerable<WalkResult2> results, int numHands)
//        {
//            var path = @$"c:\Mbrit\Casino\probabilities--{DateTime.UtcNow.ToString("yyyyMMdd-HHmmss")}--{results.Count()}.csv";
//            Runtime.Current.EnsureFolderForFileCreated(path);

//            using (var writer = new StreamWriter(path))
//            {
//                writer.WriteLine("Bankroll,Probability");
//                foreach (var result in results)
//                {
//                    writer.Write(result.Bankroll);
//                    writer.Write(",");
//                    writer.Write(result.WinLose);
//                }
//            }
//        }

//        private void DumpVectors(IEnumerable<WalkResult2> results, int numHands)
//        {
//            var byOutcome = results.ToDictionaryList(v => v.Outcome);
//            var min = byOutcome.Values.Select(v => v.Count()).Min();

//            // dump all of them...
//            var now = DateTime.UtcNow;
//            var path = @$"c:\Mbrit\Casino\vectors--{now.ToString("yyyyMMdd-HHmmss")}--{results.Count()}--all.csv";
//            this.DumpVectorsInternal(results, path, numHands, (result) =>
//            {
//                if (result.Outcome == WalkOutcome.MajorBust)
//                    return 0;
//                else if (result.Outcome == WalkOutcome.MinorBust)
//                    return 1;
//                else if (result.Outcome == WalkOutcome.Evens)
//                    return 2;
//                else if (result.Outcome == WalkOutcome.Minor)
//                    return 3;
//                else if (result.Outcome == WalkOutcome.Spike)
//                    return 4;
//                else
//                    throw new NotSupportedException($"Cannot handle '{result.Outcome}'.");
//            });

//            // go through the set...
//            var rand = ThreadRandom.Value;

//            foreach (var outcome in byOutcome.Keys)
//            {
//                this.LogInfo(() => $"Running '{outcome}' and 'Not {outcome}'...");

//                var isSet = this.GetSampleOutcomes(byOutcome[outcome], min);

//                var candidates = new List<WalkResult2>();
//                foreach (var walk in byOutcome.Keys)
//                {
//                    if (walk != outcome)
//                    {
//                        var candidatesForOutcome = this.GetSampleOutcomes(byOutcome[walk], min);
//                        candidates.AddRange(candidatesForOutcome);
//                    }
//                }

//                var isNotSet = this.GetSampleOutcomes(candidates, min);

//                var forDump = new List<WalkResult2>(isSet);
//                forDump.AddRange(isNotSet);

//                path = @$"c:\Mbrit\Casino\vectors--{now.ToString("yyyyMMdd-HHmmss")}--{results.Count()}--{outcome}.csv";
//                this.DumpVectorsInternal(forDump, path, numHands, (result) =>
//                {
//                    if (result.Outcome == outcome)
//                        return 1;
//                    else
//                        return 0;
//                });
//            }

//            this.LogInfo(() => "Finished dumping vectors.");
//        }

//        private IEnumerable<WalkResult2> GetSampleOutcomes(IEnumerable<WalkResult2> byOutcome, int min)
//        {
//            var forOutcome = byOutcome.ToList();

//            if (forOutcome.Count <= min)
//                return forOutcome;
//            else
//            {
//                forOutcome.Wash(this.Random);
//                return forOutcome.Take(min);
//            }
//        }

//        private Random Random => ThreadRandom.Value;

//        private void DumpVectorsInternal(IEnumerable<WalkResult2> results, string path, int numHands, Func<WalkResult2, int> getLabel)
//        {
//            Runtime.Current.EnsureFolderForFileCreated(path);

//            using (var writerx = new StreamWriter(path))
//            {
//                var csv = new CsvDataWriter(writerx);
//                csv.WriteValue("label");
//                csv.WriteValue("outcome");
//                csv.WriteValue("bankroll");
//                csv.WriteValue("count");

//                for (var index = 0; index < numHands; index++)
//                    csv.WriteValue("r" + index);

//                csv.WriteLine();

//                foreach (var result in results)
//                {
//                    var label = getLabel(result);
//                    csv.WriteValue(label);

//                    csv.WriteValue(result.Outcome);
//                    csv.WriteValue(result.Bankroll);

//                    csv.WriteValue(result.Vectors.Count());

//                    var asList = result.Vectors.ToList();

//                    for (var index = 0; index < numHands; index++)
//                    {
//                        if (index < asList.Count)
//                        {
//                            if (asList[index] == WinLoseDrawType.Win)
//                                csv.WriteValue(1);
//                            else if (asList[index] == WinLoseDrawType.Lose)
//                                csv.WriteValue(0);
//                            else
//                                throw new NotSupportedException($"Cannot handle '{asList[index]}'.");
//                        }
//                    }

//                    csv.WriteLine();
//                }
//            }

//            this.LogInfo(() => "Finished writing vectors --> " + path);
//        }

//        //public WalkResult2 Walk(int unit, int maxInvestment, int takeProfit, decimal sideSplit, int numHands, Random rand, bool trace = true)
//        public WalkResult2 Walk(Round<WinLoseDrawType> round, WalkArgs args, WalkRunArgs runArgs = null)
//        {
//            runArgs ??= new WalkRunArgs();

//            var allCasinos = Casino.GetCasinos();

//            var rand = this.Random;         // seeded per thread...

//            var casinos = new List<Casino>();
//            for (var index = 0; index < args.MaxHands; index++)
//                casinos.Add(allCasinos.PickRandom(rand));

//            var bankroll = 0;

//            var games = new List<GameType>()
//            {
//                GameType.Blackjack,
//                GameType.Baccarat,
//                GameType.Roulette,
//                GameType.Craps,
//            };

//            //const decimal crapsRatio = 0.5M;

//            var banked = 0;
//            //var invested = 0;
//            var totalWagered = 0;

//            var doSideBetsOverall = args.SideSplit > 0;

//            GameType? lastGame = null;

//            Chain winChain = null;
//            Chain loseChain = null;

//            var winChains = new List<Chain>();
//            var lossChains = new List<Chain>();
//            var vectors = new List<WinLoseDrawType>();

//            var proxy = new WalkProxy();

//            //var strategy = runArgs.Strategy;

//            var trace = runArgs.DoTrace;

//            var state = this.GetStateMachine(args);

//            var hand = 0;
//            while (true)
//            {
//                Casino casino = null;
//                if (hand < casinos.Count)
//                    casino = casinos[hand];
//                else
//                    casino = casinos.Last();

//                if (trace)
//                {
//                    Console.WriteLine("-------------------");
//                    Console.WriteLine($"#{vectors.Count + 1}: {casino.Name} --> {bankroll}, {state.Invested}, {banked}");
//                }

//                GameType? game = null;
//                while (true)
//                {
//                    game = games.PickRandom(rand);
//                    if (game != lastGame)
//                        break;
//                }

//                if (bankroll < state.CurrentUnit)
//                {
//                    //if (invested >= args.MaxInvestment)
//                    if (!(state.HasInvestables))
//                    {
//                        if (trace)
//                            Console.WriteLine("Stopped early.");

//                        break;
//                    }

//                    var putIn = state.PutIn();

//                    if (trace)
//                        Console.WriteLine("Investing --> " + putIn);

//                    //invested += toInvest;
//                    bankroll = putIn;
//                }

//                //var win = strategy.GetWin(rand);
//                var win = round.GetResult(hand);

//                var doSideBets = doSideBetsOverall && game != GameType.Roulette;

//                var sideWin = SideBetWin.None;
//                if (doSideBets)
//                    sideWin = this.GetSideWin(game.Value);

//                try
//                {
//                    var theBaseBet = bankroll;
//                    if (doSideBets)
//                        theBaseBet = (int)((decimal)bankroll * args.SideSplit);

//                    var theSideBet = bankroll - theBaseBet;

//                    if (trace)
//                        Console.WriteLine($"Betting --> {theBaseBet} + {theSideBet}");

//                    totalWagered += (theBaseBet + theSideBet);

//                    bankroll = 0;

//                    if (win == WinLoseDrawType.Win || sideWin != SideBetWin.None)
//                    {
//                        if (win == WinLoseDrawType.Win)
//                            bankroll += theBaseBet + theBaseBet;

//                        if (sideWin != SideBetWin.None)
//                            bankroll += theSideBet + this.GetSideWinValue(theSideBet, sideWin);

//                        if (trace)
//                        {
//                            Console.ForegroundColor = ConsoleColor.Green;
//                            Console.WriteLine($"*** {game} win *** --> now " + bankroll);
//                        }

//                        if (winChain == null)
//                        {
//                            winChain = new Chain();
//                            winChains.Add(winChain);
//                        }

//                        winChain.Increment();

//                        if (loseChain != null)
//                            loseChain = null;

//                        vectors.Add(WinLoseDrawType.Win);

//                        if (bankroll > state.TakeProfit)
//                        {
//                            var toTake = bankroll - state.TakeProfit;

//                            if (trace)
//                            {
//                                Console.ForegroundColor = ConsoleColor.Cyan;
//                                Console.WriteLine("Taking profit --> " + toTake);
//                            }

//                            banked += toTake;
//                            //bankroll = state.TakeProfit;
//                            bankroll -= toTake;
//                        }
//                    }
//                    else if (win == WinLoseDrawType.Lose)
//                    {
//                        if (trace)
//                        {
//                            Console.ForegroundColor = ConsoleColor.Red;
//                            Console.WriteLine($"*** {game} loss ***");
//                        }

//                        if (loseChain == null)
//                        {
//                            loseChain = new Chain();
//                            lossChains.Add(loseChain);
//                        }

//                        loseChain.Increment();

//                        if (winChain != null)
//                            winChain = null;

//                        vectors.Add(WinLoseDrawType.Lose);
//                    }
//                    else
//                        throw new NotSupportedException($"Cannot handle '{win}'.");

//                    if (runArgs.Step != null)
//                    {
//                        // get the vectors before...
//                        var vectorsBefore = new List<WinLoseDrawType>(vectors);
//                        vectorsBefore.RemoveAt(vectorsBefore.Count - 1);

//                        runArgs.Step(hand, win, vectorsBefore, state);
//                    }
//                }
//                finally
//                {
//                    if (trace)
//                        Console.ResetColor();
//                }

//                lastGame = game;

//                if (vectors.Count == 10)
//                {
//                    // call the ml...
//                    //var response = proxy.Predict(vectors);

//                    /*
//                    bustProbability = response.BustProbability;

//                    if (bustProbability >= 0.5M)
//                        bustType = BustType.NotBusted;
//                    else
//                        bustType = BustType.Busted;

//                    if (trace)
//                        Console.WriteLine($"Bust probability --> {response.BustProbability}, {bustType}");
//                    */

//                    //if (response.BustProbability >= 0.7M)
//                    //    break;
//                    //else
//                    /*
//                    if (response.BustProbability <= .4M)
//                    {
//                        min = (int)((decimal)min * 1.5M);
//                        //takeProfit = (int)((decimal)min * 1.5M);

//                        if (trace)
//                            Console.WriteLine("Bet level is now --> " + min);
//                    }
//                    */
//                }

//                hand++;

//                // are we leaving early?
//                if (state.IsAborted)
//                    break;

//                if (hand == args.MaxHands)
//                    break;
//            }

//            var final = (bankroll + banked) - state.Invested;

//            //var minor = invested * .5;
//            //var spike = invested;

//            WalkOutcome outcome;
//            if (final < args.MinorBust)
//                outcome = WalkOutcome.MajorBust;
//            else if (final <= 0)
//                outcome = WalkOutcome.MinorBust;
//            else if (final < args.MinorWin)
//                outcome = WalkOutcome.Evens;
//            else if (final < args.Spike1Win)
//                outcome = WalkOutcome.Minor;
//            else
//                outcome = WalkOutcome.Spike;

//            if (trace)
//            {
//                Console.WriteLine("==================");
//                Console.WriteLine("Investment --> " + state.Invested);
//                Console.WriteLine($"Cash in hand --> {bankroll} + {banked} = {bankroll + banked}");
//                Console.WriteLine("Final --> " + final);
//            }

//            return new WalkResult2(final, totalWagered, winChains, lossChains, vectors, outcome);
//        }

//        private WalkState GetStateMachine(WalkArgs args) => new WalkState(args);

//        private int GetSideWinValue(int sideBet, SideBetWin sideWin) => (int)((decimal)sideBet * sideWin.GetPayoutMultiplier()) - sideBet;

//        /*
//        private int PlayCraps(int bankroll, Random rand, bool trace)
//        {
//            if(trace)
//                Console.WriteLine("Playing craps with " + bankroll);

//            var places = new Dictionary<int, int>();
//            for (var index = 2; index <= 12; index++)
//                places[index] = 0;

//            const int per = 5;

//            while (true)
//            {
//                for (var index = 2; index <= 12; index++)
//                {
//                    if (index == 4 || index == 5 || index == 6 || index == 8 || index == 9 || index == 10)
//                    {
//                        places[index] += 5;
//                        bankroll -= 5;
//                    }

//                    if (bankroll < per)
//                        break;
//                }

//                if (bankroll < per)
//                    break;
//            }

//            if (trace)
//                Console.Write("Rolling --> ");

//            var first = true;
//            while (true)
//            {
//                var dice = new Dice(2);
//                var score = dice.Roll(rand);

//                if (trace)
//                {
//                    if (first)
//                        first = false;
//                    else
//                        Console.Write(", ");
//                    Console.Write(score.Score);
//                }

//                if (score.Score == 7)
//                    break;

//                if (places[score.Score] != 0)
//                {
//                    GameOdds odds = null;
//                    if (score.Score == 4 || score.Score == 10)
//                        odds = Craps.Place4Or10;
//                    else if (score.Score == 5 || score.Score == 9)
//                        odds = Craps.Place5Or9;
//                    else if (score.Score == 6 || score.Score == 8)
//                        odds = Craps.Place6Or8;
//                    else
//                        throw new NotSupportedException($"Cannot handle '{score.Score}'.");

//                    var win = odds.GetWin(places[score.Score]);
//                    bankroll += (int)Math.Floor(win);
//                }
//            }

//            if (trace)
//            {
//                Console.WriteLine();
//                Console.WriteLine("Craps winnings --> " + bankroll);
//            }

//            return bankroll;
//        }
//        */

//        private SideBetWin GetSideWin(GameType game)
//        {
//            var rand = this.Random;
//            var roll = rand.Next(1000); // 0–999

//            if (game == GameType.Craps)
//            {
//                if (roll < 5)
//                    return SideBetWin.Jackpot;   // 0.5% → 50x (All)
//                if (roll < 15)
//                    return SideBetWin.Major;     // 1% → 15x (Tall/Small)
//                if (roll < 70)
//                    return SideBetWin.Minor;     // 5.5% → 5x
//                if (roll < 150)
//                    return SideBetWin.Bonus;     // 8% → 2x
//                else
//                    return SideBetWin.None;      // 84%
//            }
//            else if (game == GameType.Baccarat)
//            {
//                if (roll < 10)
//                    return SideBetWin.Jackpot;  // 1% → 30x
//                if (roll < 25)
//                    return SideBetWin.Major;    // 1.5% → 15x
//                if (roll < 80)
//                    return SideBetWin.Minor;    // 5.5% → 5x
//                if (roll < 160)
//                    return SideBetWin.Bonus;    // 8% → 2x                else
//                else
//                    return SideBetWin.None;      // 84%
//            }
//            else
//            {
//                if (roll < 10)   // 1%
//                    return SideBetWin.Jackpot;
//                if (roll < 25)   // next 1.5%
//                    return SideBetWin.Major;
//                if (roll < 80)   // next 5.5%
//                    return SideBetWin.Minor;
//                if (roll < 160)  // next 8%
//                    return SideBetWin.Bonus;
//                else
//                    return SideBetWin.None;       // 84%
//            }
//        }

//        private void RenderRow<T>(ConsoleTable table, string name, IEnumerable<WalkRun2> runs, Func<int, IEnumerable<WalkResult2>, WalkArgs, T> callback)
//        {
//            var row = table.AddRow(name);
//            this.RenderRow<T>(row, runs, callback);
//        }

//        private void RenderRow<T>(ConsoleTableRow row, IEnumerable<WalkRun2> runs, Func<int, IEnumerable<WalkResult2>, WalkArgs, T> callback)
//        {
//            var index = 0;
//            foreach (var run in runs)
//            {
//                var value = callback(index, run.Results, run.Args);

//                if (value is ValueWithColour)
//                {
//                    var theValue = (ValueWithColour)(object)value;

//                    string asString = null;
//                    if (theValue.Value is decimal)
//                        asString = ((decimal)theValue.Value).ToString("n3");
//                    else
//                        asString = theValue.Value.ToString();

//                    var cell = row.AddCell(asString);
//                    cell.ForegroundColor = theValue.colour;
//                }
//                else
//                    row.AddCell(value);

//                index++;
//            }
//        }
//    }
//}
