using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Simulator
{
    public static class WinLoseDrawExtender
    {
        internal const string WinKey = "w";
        internal const string LoseKey = "l";

        public static string GetKey(this IEnumerable<WinLoseDrawType> wlds)
        {
            var builder = new StringBuilder();
            foreach(var wld in wlds)
            {
                if (wld == WinLoseDrawType.Win)
                    builder.Append(WinKey);
                else if (wld == WinLoseDrawType.Lose)
                    builder.Append(LoseKey);
                else
                    throw new NotSupportedException($"Cannot handle '{wld}'.");
            }

            return builder.ToString();
        }


        public static IWinLoseDrawRound GetRoundFromKey(string key)
        {
            var vectors = new List<WinLoseDrawType>();
            foreach (var c in key)
            {
                if (c == WinKey[0])
                    vectors.Add(WinLoseDrawType.Win);
                else if (c == LoseKey[0])
                    vectors.Add(WinLoseDrawType.Lose);
                else
                    throw new NotSupportedException($"Cannot handle '{c}'.");
            }

            return new RoundFromKeyWrapper(vectors);
        }

        internal static IWinLoseDrawRound GetRoundFromVectors(IEnumerable<WinLoseDrawType> vectors) => new RoundFromKeyWrapper(vectors);

        private class RoundFromKeyWrapper : Round<WinLoseDrawType>, IWinLoseDrawRound
        {
            internal RoundFromKeyWrapper(IEnumerable<WinLoseDrawType> vectors)
                : base(0, vectors)
            {
            }

            WinLoseDrawType IWinLoseDrawRound.GetResult(int hand) => this.GetResult(hand);
        }
    }
}
