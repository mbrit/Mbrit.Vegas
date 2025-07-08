using BootFX.Common;
using BootFX.Common.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Mbrit.Vegas.Utility
{
    internal static class CollectionsExtender
    {
        internal static T PickRandom<T>(this IEnumerable<T> items, Random rand)
        {
            if (!(items.Any()))
                throw new InvalidOperationException("No items to choose from.");

            var asList = new List<T>(items);
            if (asList.Count > 1)
            {
                asList.Shuffle(rand);
                return asList.First();
            }
            else 
                return asList.First();
        }

        internal static void Wash<T>(this List<T> items, Random rand)
        {
            var loops = rand.Next(5, 10);

            var log = LogSet.GetLog(typeof(CollectionsExtender));

            var logAt = DateTime.UtcNow.AddSeconds(2);
            var hasLogged = false;

            for (var index = 0; index < loops; index++)
            {
                items.Shuffle(rand);

                if (DateTime.UtcNow >= logAt)
                {
                    log.LogInfo(() => $"Washing '{items.Count:n0}' -- done '{index}' of '{loops}'...");
                    hasLogged = true;
                    logAt = DateTime.UtcNow.AddSeconds(2);
                }
            }

            if(hasLogged)
                log.LogInfo(() => $"Wash complete.");
        }

        internal static decimal AverageSafe<T>(this IEnumerable<T> items, Func<T, decimal> eval)
        {
            if (items.Any())
                return items.Average(eval);
            else
                return 0M;
        }

        internal static decimal MinSafe<T>(this IEnumerable<T> items, Func<T, decimal> eval)
        {
            if (items.Any())
                return items.Min(eval);
            else
                return 0M;
        }

        internal static decimal MaxSafe<T>(this IEnumerable<T> items, Func<T, decimal> eval)
        {
            if (items.Any())
                return items.Max(eval);
            else
                return 0M;
        }

        internal static IEnumerable<T> Sample<T>(this IEnumerable<T> items, int maxResults, Random rand, bool doWash = true)
        {
            var asList = items.ToList();

            if (asList.Count <= maxResults)
                return asList;
            else
            {
                if (doWash)
                    asList.Wash(rand);
                else
                    asList.Shuffle();

                return asList.Take(maxResults);
            }
        }

        internal static decimal Percentage<T>(this IEnumerable<T> items, int total)
        {
            if (total > 0)
            {
                var count = items.Count();
                return (decimal)count / (decimal)total;
            }
            else
                return 0M;
        }
    }
}
