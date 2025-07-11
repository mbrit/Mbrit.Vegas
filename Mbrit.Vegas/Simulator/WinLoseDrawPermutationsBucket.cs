
using System.Runtime.InteropServices.Marshalling;
using System.Security.Principal;

namespace Mbrit.Vegas.Simulator
{
    internal class WinLoseDrawPermutationsBucket : IWinLoseDrawRoundsBucket
    {
        private int Permutations { get; }
        private int HandsPerRound { get; }
        public decimal HouseEdge { get; }
        private List<Round<WinLoseDrawType>> Rounds { get; }

        internal WinLoseDrawPermutationsBucket(int permutations, int handsPerRound, decimal houseEdge, IEnumerable<Round<WinLoseDrawType>> rounds)
        {
            this.Permutations = permutations;
            this.HandsPerRound = handsPerRound;
            this.HouseEdge = houseEdge;
            this.Rounds = new List<Round<WinLoseDrawType>>(rounds);
        }

        public int Count => this.Rounds.Count;

        public IWinLoseDrawRound this[int index] => new RoundWrapper(index, this.Rounds[index]);

        private class RoundWrapper : IWinLoseDrawRound
        {
            public int Index { get; }
            private Round<WinLoseDrawType> Round { get; }

            public RoundWrapper(int index, Round<WinLoseDrawType> round)
            {
                this.Index = index;
                this.Round = round;
            }

            public WinLoseDrawType GetResult(int hand) => this.Round.GetResult(hand);

            public string GetKey() => this.Round.GetKey();
        }
    }
}