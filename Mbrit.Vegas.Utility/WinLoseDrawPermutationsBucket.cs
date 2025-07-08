
using System.Runtime.InteropServices.Marshalling;
using System.Security.Principal;

namespace Mbrit.Vegas.Utility
{
    internal class WinLoseDrawPermutationsBucket : IWinLoseDrawRoundsBucket
    {
        private int Permutations { get; }
        private int HandsPerRound { get; }
        public decimal HouseEdge { get; }
        private List<Round<char>> Vectors { get; }
        private List<WinLoseDrawPermutationWrapper> Wrappers { get; }

        internal WinLoseDrawPermutationsBucket(int permutations, int handsPerRound, decimal houseEdge, IEnumerable<Round<char>> vectors)
        {
            this.Permutations = permutations;
            this.HandsPerRound = handsPerRound;
            this.HouseEdge = houseEdge;
            this.Vectors = new List<Round<char>>(vectors);

            var wrappers = new List<WinLoseDrawPermutationWrapper>();
            
            var index = 0;
            foreach (var vector in vectors)
            {
                wrappers.Add(new WinLoseDrawPermutationWrapper(index, vector));
                index++;
            }

            this.Wrappers = wrappers;
        }

        private class WinLoseDrawPermutationWrapper : IWinLoseDrawRound
        {
            public int Index { get; }
            private Round<char> Vector { get; }

            public WinLoseDrawPermutationWrapper(int index, Round<char> vector)
            {
                this.Vector = vector;
                this.Index = index;
            }

            public WinLoseDrawType GetResult(int hand)
            {
                var c = this.Vector.GetResult(hand);
                if (c == '1')
                    return WinLoseDrawType.Win;
                else
                    return WinLoseDrawType.Lose;
            }
        }

        public int Count => this.Vectors.Count();

        public IWinLoseDrawRound this[int index] => this.Wrappers[index];
    }
}