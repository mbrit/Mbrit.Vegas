using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas
{
    internal static class IPermutationSelectorExtender
    {
        internal static string GetKey(this IPermutationSelector selector)
        {
            var builder = new StringBuilder();
            builder.Append(selector.Mode);
            builder.Append("|");
            builder.Append(selector.Hands);
            builder.Append("|");
            builder.Append(selector.Investables);
            builder.Append("|");
            builder.Append(selector.UnitSize);
            builder.Append("|");
            builder.Append(selector.HouseEdge);
            builder.Append("|");

            if(selector.HailMaryMode == Simulator.WalkHailMary.None)
                builder.Append(selector.HailMaryMode);
            else
                throw new NotSupportedException($"Cannot handle '{selector.HailMaryMode}'.");

            return builder.ToString();
        }
    }
}
