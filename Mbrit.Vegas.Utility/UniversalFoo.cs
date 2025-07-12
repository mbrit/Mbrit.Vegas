
using BootFX.Common.Data;
using BootFX.Common.Management;
using Mbrit.Vegas.Simulator;
using System.Runtime.InteropServices;

namespace Mbrit.Vegas.Utility
{
    internal class UniversalFoo : Loggable
    {
        internal void DoMagic()
        {
            this.LogInfo(() => "Building universal set...");

            var rand = new Random(VegasRuntime.GetToken().GetHashCode());

            this.LogInfo(() => "Building permutations...");
            var rounds = WinLoseDrawRoundsBucket.GetAllWinLosePermutations(WalkGameDefaults.HandsPerRound, rand);

            const decimal houseEdge = 0M;       // at this point, we're a fair game...

            const int investables = 12;
            const int hands = 25;
            const int unitSize = 100;

            const WalkGameMode mode = WalkGameMode.Unrestricted;

            const int hailMaryCount = 0;

            // load the ones already there...
            var selector = new AdHocPermutationSelector(mode, investables, hands, unitSize);
            var existing = new HashSet<string>(Permutation.GetKeys(selector));

            // walk...
            var logAt = DateTime.UtcNow.AddSeconds(2);
            var count = rounds.Count;
            for(var index = 0; index < count; index++)
            {
                var toDo = rounds[index].WrapSingleton(houseEdge);
                var player = new AutomaticWalkGamePlayer();

                var runs = new WalkFoo().DoMagic(toDo, 1, player, (gameIndex, bucket) =>
                {
                    return WalkGameDefaults.GetSetup(mode, toDo, 100, hailMaryCount);

                }, false);

                // dump the run...
                if (runs.Count() != 1)
                    throw new InvalidOperationException("Invalid number of runs.");

                var run = runs.First();

                if (run.Results.Count() != 1)
                    throw new InvalidOperationException("Invalid number of results.");

                // the key may be duplicated... e.g. if we lose before the end, we'll already have the permutation...
                var result = run.Results.First();
                
                var key = result.Key;

                if (!(existing.Contains(key)))
                {
                    Permutation.AddPermutation(run.Results.First(), selector);

                    // store it...
                    existing.Add(key);
                }

                if(DateTime.UtcNow >= logAt)
                {
                    this.LogInfo(() => $"Built '{index:n0}' permutations of '{count:n0}'...");
                    logAt = DateTime.UtcNow.AddSeconds(2);
                }
            }

            this.LogInfo(() => "Finished writing permutations.");
        }
    }
}