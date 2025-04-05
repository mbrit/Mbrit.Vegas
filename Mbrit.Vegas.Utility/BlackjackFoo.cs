using BootFX.Common;
using BootFX.Common.Management;
using System.Net.Mail;
using System.Numerics;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace Mbrit.Vegas.Utility
{
    internal class BlackjackFoo : CliFoo
    {
        const decimal WinIncludingBonusPercentage = 42.5M;
        const decimal BonusWinPercentage = 4.75M;
        const decimal LosePercentage = 49M;
        const decimal DrawPercentage = 8.5M;

        internal void DoMagic()
        {
            var rand = new RandomWrapper();

            var unitSize = this.GetValueWithDefault<decimal>("Unit size");

            var games = this.GetValueWithDefault<int>("Num games");

            var render = this.GetValueWithDefault<bool>("Render each");

            var handsPerHour = 80;
            var hours = 1M;
            var totalHands = (int)(handsPerHour * hours);

            var sequences = new List<WldSequence>();
            for (var index = 0; index < games; index++)
                sequences.Add(new WldSequence(WinIncludingBonusPercentage, BonusWinPercentage, LosePercentage, DrawPercentage, totalHands, rand));

            var primary = new GameResult(0);

            var outcomes = new Dictionary<GameOutcome, int>();
            foreach (var outcome in Enum.GetValues<GameOutcome>())
                outcomes[outcome] = 0;

            var settings = new GameSettings(unitSize, 1.15M, totalHands, .5M, 1M);

            Console.WriteLine();

            var playThroughProfits = new List<decimal>();
            var quitProfits = new List<decimal>();
            var quitTracks = new List<int>();
            var handsPlayed = new List<int>();

            var recoveries = new Dictionary<int, int>();
            var recoveryIndexes = new List<int>();
            var startRecovery = settings.FloorUnits + 6;
            for (var index = startRecovery; index >= startRecovery - 12; index--)
            {
                recoveryIndexes.Add(index);
                recoveries[index] = 0;
            }

            sequences.ProcessItems((sequence) =>
            {
                var results = this.PlayGame(sequence, primary, settings, rand, render);
                outcomes[results.Outcome]++;

                if (results.Outcome == GameOutcome.PlayedThrough)
                {
                    playThroughProfits.Add(results.FinalProfit);

                    if (results.LowestTrack <= startRecovery)
                        recoveries[results.LowestTrack]++;
                }
                else if (results.Outcome == GameOutcome.Quit)
                {
                    quitProfits.Add(results.FinalProfit);
                    quitTracks.Add(results.FinalTrack);
                }

                handsPlayed.Add(results.NumHands);

                var trend = this.GetTrend(results);
                Console.WriteLine(results.FinalProfit + "\t" + trend);
            });

            Console.WriteLine();

            var table = new ConsoleTable();
            table.AddHeaderRow("Setting", "Value");
            table.AddRow("Unit size", settings.UnitSize);
            table.AddRow("Bankroll", settings.StartBank);
            table.AddRow("SD", settings.SingleStandardDeviation);
            table.AddRow("Ceiling", settings.StandardDeviationHigh, settings.CeilingUnits, settings.CeilingMultipler);
            table.AddRow("Floor", settings.StandardDeviationLow, settings.FloorUnits, settings.FloorMultipler);
            table.AddRow("Quit", settings.StandardDeviationLow, settings.QuitUnits);
            table.Render();

            Console.WriteLine();

            this.DumpResults(primary);

            Console.WriteLine();

            table = new ConsoleTable();
            table.AddHeaderRow("Recovery", "Count");
            foreach(var index in recoveryIndexes)
                table.AddRow(index, recoveries[index]);
            table.Render();

            Console.WriteLine();

            table = new ConsoleTable();
            table.AddHeaderRow("Outcome", "Count", "%age");
            foreach (var outcome in Enum.GetValues<GameOutcome>())
                table.AddRow(outcome, outcomes[outcome], this.FormatPercent2((decimal)outcomes[outcome] / (decimal)sequences.Count));
            table.Render();

            Console.WriteLine();

            table = new ConsoleTable();
            table.AddHeaderRow("Outcome", "Value");
            if (playThroughProfits.Any())
            {
                table.AddRow("Min profit on play-throughs", playThroughProfits.Min());
                table.AddRow("Max profit on play-throughs", playThroughProfits.Max());
                table.AddRow("Average profit on play-throughs", playThroughProfits.Average());
            }
            if (quitProfits.Any())
            {
                table.AddRow("Min profit on quits", quitProfits.Min());
                table.AddRow("Max profit on quits", quitProfits.Max());
                table.AddRow("Average profit on quits", quitProfits.Average());
                table.AddRow("Average track on quits", quitTracks.Select(v => (decimal)v).Average());
            }
            table.AddRow("Average hands played", handsPlayed.Average());
            table.Render();
        }

        private decimal GetTrend(GameResult result)
        {
            //double[] yValues = { 10, 12, 15, 20, 25 }; // elevati
            //on or Y values
            var yValues = result.Select(v => (decimal)v.TrackAfter).Take(35).ToList();

            var xStart = 0;
            var xEnd = yValues.Count - 1;

            var rise = yValues[xEnd] - yValues[xStart];
            var run = xEnd - xStart; // assuming evenly spaced x-values like 0, 1, 2...

            var angleRadians = Math.Atan2((double)rise, run);
            var angleDegrees = angleRadians * (180.0 / Math.PI);
            return (decimal)angleDegrees;
        }

        private void DumpResults(GameResult results)
        {
            var table = new ConsoleTable();
            table.AddHeaderRow("Result", "Count", "%age", "ideal");
            table.AddRow(WinLoseDraw.Win, results.NumWins, this.FormatPercent2(results.PercentWins), this.FormatPercent2(WinIncludingBonusPercentage / 100));
            table.AddRow(WinLoseDraw.Bonus, results.NumBonuses, this.FormatPercent2(results.PercentBonuses), this.FormatPercent2(BonusWinPercentage / 100));
            table.AddRow(WinLoseDraw.Lose, results.NumLosses, this.FormatPercent2(results.PercentLosses), this.FormatPercent2(LosePercentage / 100));
            table.AddRow(WinLoseDraw.Draw, results.NumDraws, this.FormatPercent2(results.PercentDraws), this.FormatPercent2(DrawPercentage / 100));
            table.Render();
        }

        private string FormatPercent2(decimal value) => (value * 100).ToString("n2") + "%";

        internal GameResult PlayGame(WldSequence sequence, GameResult primary, GameSettings settings, IRng rand, bool render = true)
        {
            var results = new GameResult(settings.StartBank);

            var bank = settings.StartBank;

            var track = 0;
            var trackUp = 1;
            var trackDown = 1;

            var warn = settings.FloorUnits + 2;

            foreach (var theResult in sequence)
            {
                if (bank < settings.UnitSize)
                    break;

                // reduce the balance to play...
                var bet = settings.UnitSize;
                bank -= bet;

                var result = BlackjackHandResult.Unknown;
                if (theResult == WinLoseDraw.Win)
                {
                    result = BlackjackHandResult.PlayerWins;

                    if (render)
                    {
                        ConsoleHelper.WriteColor("↑", ConsoleColor.Green);
                        this.RenderTrack((track + 1), ConsoleColor.Green, warn);
                    }
                }
                else if (theResult == WinLoseDraw.Bonus)
                {
                    result = BlackjackHandResult.PlayerWinsWithBlackjack;

                    if (render)
                    {
                        ConsoleHelper.WriteColor("↑", ConsoleColor.Cyan);
                        this.RenderTrack((track + 1), ConsoleColor.Cyan, warn);
                    }
                }
                else if (theResult == WinLoseDraw.Lose)
                {
                    result = BlackjackHandResult.DealersWins;

                    if (render)
                    { 
                        ConsoleHelper.WriteColor("↓", ConsoleColor.Red);
                        this.RenderTrack((track - 1), ConsoleColor.Red, warn);
                    }
                }
                else if (theResult == WinLoseDraw.Draw)
                {
                    result = BlackjackHandResult.Push;

                    if (render)
                    {
                        Console.Write("·");
                        this.RenderTrack(track, ConsoleColor.Gray, warn);
                    }
                }
                else
                    throw new NotSupportedException($"Cannot handle '{result}'.");

                if (result == BlackjackHandResult.Push)
                    bank += bet;
                else
                {
                    if (result == BlackjackHandResult.PlayerWinsWithBlackjack || result == BlackjackHandResult.PlayerWins)
                    {
                        if (result == BlackjackHandResult.PlayerWins)
                            bank += bet * 2;
                        else
                            bank += bet * 2.5M;

                        result = BlackjackHandResult.PlayerWins;

                        track += trackUp;
                    }
                    else
                        track -= trackDown;
                }

                results.AddHand(theResult, bank, track);
                primary.AddHand(theResult, bank, track);

                if (track == settings.CeilingUnits)
                {
                    results.Outcome = GameOutcome.TookProfit;
                    break;
                }
                else if (track == settings.FloorUnits)
                {
                    results.Outcome = GameOutcome.CrashedOut;
                    break;
                }

                // are we at 35 hands?
                if(results.NumHands == 25 && track <= settings.QuitUnits)
                {
                    results.Outcome = GameOutcome.Quit;
                    break;
                }

                if (render && results.Count() % 40 == 0)
                    Console.WriteLine();
            }

            if (render)
            {
                Console.Write("\r\n\tt:");
                Console.Write(results.NumHands);
                Console.Write(", w:");
                Console.Write(results.NumWins);
                Console.Write(", l:");
                Console.Write(results.NumLosses);
                Console.Write(", p:");
                Console.Write(results.FinalProfit);
                Console.WriteLine();
                Console.WriteLine();
            }

            return results;
        }

        private void RenderTrack(int track, ConsoleColor color, int warnLevel)
        {
            var background = ConsoleColor.Black;
            if(track <= warnLevel)
                background = ConsoleColor.Magenta;

            var builder = new StringBuilder();
            if (track > 0)
                builder.Append(" ");
            builder.Append(track);
            while (builder.Length < 3)
                builder.Append(" ");

            ConsoleHelper.WriteColor(builder.ToString(), color, background);
        }

        private decimal GetPercentage(int value, int hands)
        {
            if (hands > 0)
                return (decimal)value / (decimal)hands;
            else
                return 0M;
        }

        /*
        private BlackjackHandResult PlayHand(Random rand)
        {
            // standard metrics...
            var outcomes = new List<BlackjackHandResult>();

            //Final Result    Approx %
            //Player Wins ~42 %
            //Dealer Wins ~49 %
            //Push    ~9 %
            // Roughly 11.3% of all player wins in blackjack come from natural blackjacks.

            Action<BlackjackHandResult> add = (result) =>
            {
                outcomes.Add(result);
            };

            // this adjusts the player to be a %age off of perfect in basic
            // strategy -- a value of "15" should mean "5%"...
            var wobble = 15;

            for (var index = 0; index < 420 - 113 - wobble; index++)
                add(BlackjackHandResult.PlayerWins);

            for (var index = 0; index < 113; index++)
                add(BlackjackHandResult.PlayerWinsWithBlackjack);

            for (var index = 0; index < 490 + wobble; index++)
                add(BlackjackHandResult.DealersWins);

            for (var index = 0; index < 90; index++)
                add(BlackjackHandResult.Push);

            var next = rand.Next(100, 200);
            for (var index = 0; index < next; index++)
                outcomes.Shuffle();

            return outcomes.First();
        }
        */

        /*
        internal BlackjackHandResult PlayHand(Shoe shoe)
        { 
            var p1 = shoe.Deal();
            var d1 = shoe.Deal();
            var p2 = shoe.Deal();
            var d2 = shoe.Deal();

            if (this.IsBlackjack(d1, d2))
                return BlackjackHandResult.DealerBlackjack;

            if (this.IsBlackjack(p1, p2))
                return BlackjackHandResult.PlayerBlackjack;

            var r1 = 0;
            var r2 = 0;
            this.ResolveTotals(p1, p2, ref r1, ref r2);

            var dr1 = 0;
            var dr2 = 0;
            this.ResolveTotals(d1, null, ref dr1, ref dr2);

            this.LogInfo(() => $"Playing hand --> player: {r1}, {r2}, dealer: {dr1}, {dr2}...");

            // resolve the player hand...
            Console.WriteLine("Taking for player...");
            if (!(this.Take(shoe, this.ResolveRank(d1), true, ref r1, ref r2)))
                return BlackjackHandResult.PlayerBust;

            // resolve the dealer hand...
            Console.WriteLine("Taking for dealer...");
            if (!(this.Take(shoe, 0, false, ref dr1, ref dr2)))
                return BlackjackHandResult.DealerBust;

            Console.WriteLine($"Player {r1} against dealer {dr1}...");
            if (r1 > dr1)
                return BlackjackHandResult.PlayerWin;
            else if (r1 < dr1)
                return BlackjackHandResult.DealerWin;
            else
                return BlackjackHandResult.Push;
        }

        private void ResolveTotals(Card c1, Card c2, ref int h1, ref int h2)
        {
            if (c2 != null)
            {
                if (c1.Rank == 1 && c2.Rank == 1)
                    throw new NotImplementedException("This operation has not been implemented.");
                else if (c1.Rank == 1)
                    throw new NotImplementedException("This operation has not been implemented.");
                else if (c2.Rank == 1)
                    throw new NotImplementedException("This operation has not been implemented.");
                else
                {
                    h1 = this.ResolveRank(c1) + this.ResolveRank(c2);
                    h2 = 0;
                }
            }
            else
            {
                if (c1.Rank == 1)
                {
                    h1 = 1;
                    h2 = 11;
                }
                else
                {
                    h1 = c1.Rank;
                    h2 = 0;
                }
            }
        }

        private bool Take(Shoe shoe, int against, bool isPlayer, ref int h1, ref int h2)
        {
            Console.WriteLine($"Initial --> {h1} against {against}...");

            while (true)
            {
                if (h2 == 0)
                {
                    var doHit = false;
                    if(isPlayer)
                    {
                        if (h1 < 12 || (h1 == 12 && against <= 3))
                            doHit = true;
                        else if (h1 < 17)
                        {
                            if (against >= 7)
                                doHit = true;
                        }
                    }
                    else
                    {
                        if (h1 <= 17)
                            doHit = true;
                    }

                    if (doHit)
                    {
                        var hit = shoe.Deal();
                        Console.WriteLine("Hit --> " + hit);

                        if (hit.Rank == 1)
                            throw new NotImplementedException("This operation has not been implemented.");
                        else
                            h1 += this.ResolveRank(hit);

                        Console.WriteLine("Total now --> " + h1);

                        if (h1 > 21)
                            return false;
                    }
                    else
                    {
                        Console.WriteLine("Stand");
                        return true;
                    }
                }
                else
                {
                    throw new NotImplementedException("This operation has not been implemented.");
                }
            }

            throw new NotImplementedException("This operation has not been implemented.");
        }

        private bool IsBlackjack(Card p1, Card p2)
        {
            var a = this.ResolveRank(p1);
            var b = this.ResolveRank(p2);
            return (a == 1 && b == 10) || (b == 1 && a == 10);
        }

        private int ResolveRank(Card c)
        {
            if (c.Rank >= 10)
                return 10;
            else
                return c.Rank;
        }
        */
    }
}