using BootFX.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Runtime.Versioning;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Simulator
{
    public class WinLoseDrawRoundsBucket : RoundsBucket<WinLoseDrawType>, IWinLoseDrawRoundsBucket
    {
        private List<WinLoseDrawRoundWrapper> Wrappers { get; set; }

        private static BfxLookup<int, int, decimal, IWinLoseDrawRoundsBucket> Prebaked { get; set; }

        private const int NumPrebakedBuckets = 250;

        private WinLoseDrawRoundsBucket(int numRounds, int handsPerRound, decimal houseEdge, Func<int, WinLoseDrawType> callback, 
            Random rand, RoundsBucketFlags flags = RoundsBucketFlags.Default)
            : base(numRounds, handsPerRound, houseEdge, callback, rand, flags)
        {
        }

        static WinLoseDrawRoundsBucket()
        {
            var prebaked = new BfxLookup<int, int, decimal, IWinLoseDrawRoundsBucket>()
            {
                ExpirationPeriod = TimeSpan.FromHours(72)
            };
            prebaked.CreateItemValue += (sender, e) =>
            {
                var rand = new Random(VegasRuntime.GetToken().GetHashCode());
                e.NewValue = GetWinLoseBucket(e.Key1, e.Key2, e.Key3, rand);
            };

            Prebaked = prebaked;
        }

        protected override void Initialize(IEnumerable<Round<WinLoseDrawType>> vectors)
        {
            base.Initialize(vectors);

            var wrappers = new List<WinLoseDrawRoundWrapper>();
            
            var index = 0;
            foreach (var vector in vectors)
            {
                wrappers.Add(new WinLoseDrawRoundWrapper(index, vector));
                index++;
            }

            this.Wrappers  = wrappers;
        }

        private class WinLoseDrawRoundWrapper : IWinLoseDrawRound
        {
            public int Index { get; }
            private Round<WinLoseDrawType> Round { get; }

            public WinLoseDrawRoundWrapper(int index, Round<WinLoseDrawType> round)
            {
                this.Index = index;
                this.Round = round;
            }

            public int Count => this.Round.Count;

            public WinLoseDrawType GetResult(int hand) => this.Round.GetResult(hand);

            public string GetKey() => this.Round.GetKey();
        }

        public IWinLoseDrawRound this[int index] => this.Wrappers[index];

        public static IWinLoseDrawRoundsBucket GetWinLoseBucket(int numRounds, int handsPerRound, decimal houseEdge, Random rand, 
            RoundsBucketFlags flags = RoundsBucketFlags.Default)
        {
            var strategy = new RandomWalkStrategy(houseEdge);

            return new WinLoseDrawRoundsBucket(numRounds, handsPerRound, houseEdge, (hand) =>
            {
                return strategy.GetWin(rand);

            }, rand, flags);
        }

        public static IWinLoseDrawRoundsBucket GetAllWinLosePermutations(int handsPerRound, Random rand)
        {
            var strategy = new PermutationStrategy(handsPerRound);
            var vectors = strategy.GetVectors(rand);

            return new WinLoseDrawPermutationsBucket(strategy.Permutations, handsPerRound, 0, vectors);
        }

        internal static IWinLoseDrawRoundsBucket ReproduceWinLoseBucket(WalkGameSetup setup, Random rand) => 
            GetWinLoseBucket(setup.Rounds.Count, setup.HandsPerRound, setup.HouseEdge, rand);

        public static IWinLoseDrawRoundsBucket GetPrebakedWinLoseBucket(int numRounds, int numHands, decimal houseEdge, Random rand)
        {
            var index = rand.Next(0, NumPrebakedBuckets - 1);
            return Prebaked[numRounds, numHands, houseEdge];
        }

        public IEnumerable<IWinLoseDrawRound> ToEnumerable()
        {
            var results = new List<IWinLoseDrawRound>();
            for (var index = 0; index < this.Count; index++)
                results.Add(this[index]);
            return results;
        }

        public static IWinLoseDrawRoundsBucket Parse(string chain, decimal houseEdge, Random rand)
        {
            return new WinLoseDrawRoundsBucket(1, chain.Length, houseEdge, (hand) =>
            {
                var asString = new string(chain[hand], 1);
                if (asString == WinLoseDrawExtender.WinKey)
                    return WinLoseDrawType.Win;
                else if (asString == WinLoseDrawExtender.LoseKey)
                    return WinLoseDrawType.Lose;
                else
                    throw new NotSupportedException($"Cannot handle '{asString}'.");

            }, rand, RoundsBucketFlags.Exact);
        }
    }
}
