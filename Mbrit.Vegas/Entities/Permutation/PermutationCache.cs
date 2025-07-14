using BootFX.Common.Management;
using Mbrit.Vegas.Simulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas
{
    public class PermutationCache : Loggable
    {
		// configuration for this cache...
        private IPermutationSelector Selector { get; }
		private string Key { get; }
		private IEnumerable<PermutationIdAndKey> Keys { get; }
        private PrefixTree Tree { get; }
		private PermutationCacheLoader Loader { get; set; }

		// global shared cache -- shared across caches as the caches are just indexes...
		public static ReaderWriterLockSlim Lock { get; } = new ReaderWriterLockSlim();
		public static Dictionary<int, IPermutation> CachedItems { get; } = new Dictionary<int, IPermutation>();

		// cache cache...
		private static Dictionary<string, PermutationCache> Caches { get; } = new Dictionary<string, PermutationCache>();
        private static object _outerLock = new object();

		private PermutationCache(IPermutationSelector selector)
		{
			this.Selector = selector;

			var key = selector.GetKey();
			this.Key = key;

            this.LogInfo(() => $"Loading keys and ids for permutation cache build of '{key}'...");
			var keys = Permutation.GetKeysAndIdPairs(selector);    // don't copy -- we'll assume no one touches these...

			if (!(keys.Any()))
				throw new InvalidOperationException("The permutation cache didn't find any keys.");

			this.Keys = keys;

            var count = this.Keys.Count();
            this.LogInfo(() => $"Loaded '{count:n0}' key/id pairs...");

            this.LogInfo(() => "Building tree...");
            var tree = new PrefixTree();

			var logAt = DateTime.UtcNow.AddSeconds(2);
			var index = 0;
			foreach (var pair in this.Keys)
			{
				tree.Add(pair.Key, pair.Id);

				index++;
                if (DateTime.UtcNow >= logAt)
				{
					this.LogInfo(() => $"Added '{index:n0} of {count:n0}' to tree..");
                    logAt = DateTime.UtcNow.AddSeconds(2);
                }
            }

            this.LogInfo(() => "Finished building tree.");
            this.Tree = tree;

            this.LogInfo(() => "Initializing loader...");
			this.Loader = new PermutationCacheLoader(this.Keys, () =>
			{
				this.LogInfo(() => $"Loader for '{key}' signalled finished...");
				this.Loader = null;
			});

			this.LogInfo(() => "Finished cache setup -- now waiting for load on separate thread.");
        }

        /// <summary>
        /// Gets the singleton instance of <see cref="PermutationCache">PermutationCache</see>.
        /// </summary>
        internal static PermutationCache GetCache(IPermutationSelector selector)
		{
			lock(_outerLock)
			{
				var key = selector.GetKey();
				if (!(Caches.ContainsKey(key)))
					Caches[key] = new PermutationCache(selector);
				return Caches[key];
            }
		}

        private IEnumerable<int> GetIdsUnder(string key) => this.Tree.GetAllDownstreamIds(key);

        public IEnumerable<IPermutation> GetItemsUnder(string key)
        {
			var ids = this.GetIdsUnder(key);
			return GetOrFetchItemsFromCache(ids);
        }

        private static IEnumerable<IPermutation> GetOrFetchItemsFromCache(IEnumerable<int> ids)
        {
			var log = LogSet.GetLog<PermutationCache>();

			IEnumerable<int> misses = null;
			log.LogInfo(() => $"Requested '{ids.Count():n0}'...");

			Lock.EnterReadLock();
			try
			{
				misses = ids.Except(CachedItems.Keys).ToList();
            }
            finally
            {
                Lock.ExitReadLock();
            }

            if (misses.Any())
			{
                log.LogInfo(() => $"Loading '{misses.Count():n0}' missing item(s)...");

                // demand load here...
                var items = PermutationCacheLoader.GetPermutationByIds(misses);
				PermutationCache.PreloadItems(items);
            }

            log.LogInfo(() => $"Packaging...");

            // return...
            Lock.EnterReadLock();
			try
			{
				var results = new List<IPermutation>();
				var found = CachedItems.Keys.Intersect(ids);
				if (found.Any())
				{
					foreach (var id in found)
						results.Add(CachedItems[id]);
				}

				log.LogInfo(() => "Done.");

				return results;
			}
			finally
			{
				Lock.ExitReadLock();
			}
		}

		internal static void PreloadItems(IEnumerable<IPermutation> items)
        {
			Lock.EnterWriteLock();
			try
			{
				foreach (var item in items)
					CachedItems[item.PermutationId] = item;
			}
			finally
			{
				Lock.ExitWriteLock();
			}
        }

        public static void InitializeDefaultCache()
        {
			var selector = WalkGameDefaults.GetPermutationSelector();
			GetCache(selector);
        }
    }
}
