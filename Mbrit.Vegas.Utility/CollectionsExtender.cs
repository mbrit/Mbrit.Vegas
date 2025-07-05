using BootFX.Common;
using BootFX.Common.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

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
    }
}
