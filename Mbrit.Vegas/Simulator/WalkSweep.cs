// Sweeping version of WalkFoo.DoMagic for Excel-friendly output
using BootFX.Common;
using Mbrit.Vegas.Games;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Mbrit.Vegas.Simulator
{
    internal class WalkSweep
    {
        internal void DoMagic()
        {
            var rand = new Random();

            var sideSplits = new List<decimal>();
            for (var sideSplit = 0M; sideSplit < 1M; sideSplit += 0.05M)
                sideSplits.Add(sideSplit);

            var minUnits = new List<int>();
            for (var minUnit = 10; minUnit < 250; minUnit += 5)
                minUnits.Add(minUnit);

            var profitMultiples = new List<decimal>();
            for (var profitMultiple = 0.5M; profitMultiple <= 10M; profitMultiple += 0.5M)
                profitMultiples.Add(profitMultiple);

            //const int runs = 25000;

            /*
            var args = WalkFoo.GetDefaultArgs();

            var path = @$"c:\Mbrit\Casino\walk-results--{DateTime.UtcNow.ToString("yyyyMMdd-HHmmss")}.csv";
            Runtime.Current.EnsureFolderForFileCreated(path);

            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine("SideSplit,MinUnit,TakeProfit,WinRate,EvenRate,LossRate,AvgWagered,AvgWinOverProfit,Over1x,Over1.5x,Over2x,Over3x,MaxWin");

                foreach (var sideSplit in sideSplits)
                {
                    foreach (var min in minUnits)
                    {
                        foreach (var profitMult in profitMultiples)
                        {
                            var takeProfit = (int)(min * profitMult);
                            var maxInvestment = min * 12;

                            var results = new List<WalkResult>();
                            for (var i = 0; i < runs; i++)
                            {
                                //results.Add(new WalkFoo().Walk(args));
                                throw new NotImplementedException("This operation has not been implemented.");
                            }

                            var evens = (int)(takeProfit * 0.66M);
                            var bankrolls = results.Select(r => r.Bankroll).ToList();
                            var wagereds = results.Select(r => (decimal)r.TotalWagered).ToList();

                            var winRate = bankrolls.Count(x => x >= evens) / (decimal)runs;
                            var evenRate = bankrolls.Count(x => x > 0 && x < evens) / (decimal)runs;
                            var lossRate = bankrolls.Count(x => x <= 0) / (decimal)runs;
                            var averageWagered = wagereds.Average();
                            var averageOverProfit = bankrolls.Where(x => x >= evens).DefaultIfEmpty(0).Average();
                            var over1x = bankrolls.Count(x => x >= maxInvestment * 1M) / (decimal)runs;
                            var over1p5x = bankrolls.Count(x => x >= maxInvestment * 1.5M) / (decimal)runs;
                            var over2x = bankrolls.Count(x => x >= maxInvestment * 2M) / (decimal)runs;
                            var over3x = bankrolls.Count(x => x >= maxInvestment * 3M) / (decimal)runs;
                            int maxWin = bankrolls.Max();

                            var output = $"{sideSplit},{min},{takeProfit},{winRate:P2},{evenRate:P2},{lossRate:P2},{averageWagered:F2},{averageOverProfit:F2},{over1x:P2},{over1p5x:P2},{over2x:P2},{over3x:P2},{maxWin}";
                            Console.WriteLine(output);
                            writer.WriteLine(output);
                        }
                    }
                }
            }

            Console.WriteLine("Sweep complete. Results written to walk-results.csv");
            */
        }
    }
}
