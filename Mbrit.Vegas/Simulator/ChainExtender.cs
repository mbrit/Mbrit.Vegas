using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Simulator
{
    internal static class ChainExtender
    {
        public static decimal ScoreChains(this IEnumerable<Chain> chains, bool isWin)
        {
            if (chains == null) return 0m;

            return chains.Sum(chain => ScoreChain(chain.Steps, isWin));
        }

        public static decimal ScoreChain(int steps, bool isWin)
        {
            var winMultiplier = 1.0m;
            var lossMultiplier = 1.0m;

            decimal baseScore = steps switch
            {
                <= 0 => 0m,
                1 => 0.0001m,
                2 => 0.0005m,
                3 => 0.002m,
                4 => 0.005m,
                5 => 0.01m,
                6 => 0.02m,
                7 => 0.04m,
                _ => 0.08m
            };

            return isWin ? baseScore * winMultiplier : -baseScore * lossMultiplier;
        }

        public static int Max(this IEnumerable<Chain> items)
        {
            if (items.Any())
                return items.Select(v => v.Steps).Max();
            else
                return 0;
        }
    }
}
