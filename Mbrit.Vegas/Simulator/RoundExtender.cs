using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
