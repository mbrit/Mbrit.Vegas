using BootFX.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Simulator
{
    public class WinLoseDrawRoundsBucket : RoundsBucket<WinLoseDrawType>, IWinLoseDrawRoundsBucket
    {
        private List<WinLoseDrawRoundWrapper> Wrappers { get; set; }

        private WinLoseDrawRoundsBucket(int numRounds, int handsPerRound, decimal houseEdge, Func<int, WinLoseDrawType> callback, Random rand)
            : base(numRounds, handsPerRound, houseEdge, callback, rand)
        {
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
            private Round<WinLoseDrawType> Vector { get; }

            public WinLoseDrawRoundWrapper(int index, Round<WinLoseDrawType> vector)
            {
                this.Index = index;
                this.Vector = vector;
            }

            public WinLoseDrawType GetResult(int hand) => this.Vector.GetResult(hand);
        }

        public IWinLoseDrawRound this[int index] => this.Wrappers[index];

        public static IWinLoseDrawRoundsBucket GetWinLoseBucket(int numRounds, int handsPerRound, decimal houseEdge, Random rand)
        {
            var strategy = new RandomWalkStrategy(houseEdge);

            return new WinLoseDrawRoundsBucket(numRounds, handsPerRound, houseEdge, (hand) =>
            {
                return strategy.GetWin(rand);

            }, rand);
        }

        internal static IWinLoseDrawRoundsBucket GetAllWinLosePermutations(int handsPerRound, int maxPermutationSampleSize = 0, Random rand = null, bool doWash = true)
        {
            if (maxPermutationSampleSize > 0 && rand == null)
                throw new ArgumentNullException("rand");

            var strategy = new PermutationStrategy(handsPerRound);
            var vectors = strategy.GetVectors(maxPermutationSampleSize, rand);

            return new WinLoseDrawPermutationsBucket(strategy.Permutations, handsPerRound, 0, vectors);
        }

        internal static IWinLoseDrawRoundsBucket ReproduceWinLoseBucket(WalkGameSetup setup, Random rand) => 
            GetWinLoseBucket(setup.Rounds.Count, setup.HandsPerRound, setup.HouseEdge, rand);

        internal static IWinLoseDrawRoundsBucket GetWinLoseBucket(int numRounds, object numHands, object houseEdge, Random random)
        {
            throw new NotImplementedException();
        }
    }
}
