using BootFX.Common;
using BootFX.Common.Data;
using BootFX.Common.Management;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas
{
    internal class PermutationCacheLoader : Loggable
    {
        private IEnumerable<PermutationIdAndKey> Keys { get; }
        private Action Finished { get; }
        private Thread Thread { get; set; }

        internal PermutationCacheLoader(IEnumerable<PermutationIdAndKey> keys, Action finished)
        {
            this.Keys = keys;
            this.Finished = finished;

            this.Thread = new Thread(this.ThreadEntryPoint)
            {
                Name = "Permutation Cache Loader"
            };
            this.Thread.Start();
        }

        private void ThreadEntryPoint(object obj)
        {
            try
            {
                this.LogInfo(() => $"Loading '{this.Keys.Count():n0}' permutation(s) into cache...");
                var pages = this.Keys.SplitIntoPages(500);

                var index = 0;
                var count = pages.Count;
                var total = 0;

                var logAt = DateTime.UtcNow.AddSeconds(2);
                pages.ProcessItems((page) =>
                {
                    try
                    {
                        var ids = page.Select(v => v.Id);
                        var items = GetPermutationByIds(ids);
                        PermutationCache.PreloadItems(items);

                        total += items.Count();
                    }
                    finally
                    {
                        index++;
                        if (DateTime.UtcNow >= logAt)
                        {
                            this.LogInfo(() => $"Loaded '{index:n0} of {count:n0}' page(s), total items: {total:n0}...");
                            logAt = DateTime.UtcNow.AddSeconds(2);
                        }
                    }
                });

                this.LogInfo(() => "Finished loading permutations into cache.");
            }
            catch(Exception ex)
            {
                this.LogError(() => "The cache load failed.", ex);
            }
            finally
            {
                this.Finished();
            }
        }

        internal static IEnumerable<IPermutation> GetPermutationByIds(IEnumerable<int> ids)
        {
            var filter = new SqlFilter<Permutation>();

            var et = filter.EntityType;
            filter.Fields.Add(et.Fields["PermutationId"]);
            filter.Fields.Add(et.Fields["MajorBustSeen"]);
            filter.Fields.Add(et.Fields["MajorBustHand"]);
            filter.Fields.Add(et.Fields["MajorBustEv"]);
            filter.Fields.Add(et.Fields["MinorBustSeen"]);
            filter.Fields.Add(et.Fields["MinorBustHand"]);
            filter.Fields.Add(et.Fields["MinorBustEv"]);
            filter.Fields.Add(et.Fields["Spike0p5Seen"]);
            filter.Fields.Add(et.Fields["Spike0p5Hand"]);
            filter.Fields.Add(et.Fields["Spike0p5Ev"]);
            filter.Fields.Add(et.Fields["Spike1Seen"]);
            filter.Fields.Add(et.Fields["Spike1Hand"]);
            filter.Fields.Add(et.Fields["Spike1Ev"]);
            filter.Fields.Add(et.Fields["Score"]);

            filter.Constraints.AddArrayConstraint(v => v.PermutationId, ids);

            var table = Database.ExecuteDataTable(filter);

            var chips = new List<IPermutation>();
            foreach (DataRow row in table.Rows)
                chips.Add(new PermutationChip(row));

            return chips;
        }
    }
}
