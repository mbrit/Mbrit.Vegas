using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Simulator
{
    public static class IWinLoseDrawRoundExtender
    {
        public static IEnumerable<WinLoseDrawType> GetVectors(this IWinLoseDrawRound round)
        {
            var key = round.GetKey();

            var results = new List<WinLoseDrawType>();
            foreach(var c in key)
            {
                if (c == WinLoseDrawExtender.WinKey[0])
                    results.Add(WinLoseDrawType.Win);
                else if (c == WinLoseDrawExtender.LoseKey[0])
                    results.Add(WinLoseDrawType.Lose);
                else
                    throw new NotSupportedException($"Cannot handle '{c}'.");
            }

            return results;
        }
    }
}
