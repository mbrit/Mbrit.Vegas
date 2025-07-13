using BootFX.Common;
using Mbrit.Vegas.Simulator;
using System.Runtime.InteropServices;

namespace Mbrit.Vegas.Web.Api.Controllers
{
    public class ManualGameRounds : IWinLoseDrawRoundsBucket
    {
        internal IEnumerable<GameRunHand> Hands { get; }
        public WalkGameMode Mode { get; }
        public decimal HouseEdge { get; }

        public ManualGameRounds(IEnumerable<GameRunHand> hands, GameRun game)
        {
            var asList = hands.ToList();
            asList.Sort((a, b) => a.Hand.CompareTo(b.Hand));
            this.Hands = asList;

            this.Mode = game.Mode;
            this.HouseEdge = game.HouseEdge;
        }

        public int Count => 1;

        public IWinLoseDrawRound this[int index]
        {
            get
            {
                if (index == 0)
                    return new InnerRound(index, this);
                else
                    throw new NotSupportedException($"Cannot handle '{index}'.");
            }
        }

        private class InnerRound : IWinLoseDrawRound
        {
            public int Index { get; }

            private List<WinLoseDrawType> Vectors { get; }

            internal InnerRound(int index, ManualGameRounds owner)
            {
                this.Index = index;

                var vectors = new List<WinLoseDrawType>();
                foreach (var hand in owner.Hands)
                    vectors.Add(hand.Outcome);
                this.Vectors = vectors;
            }

            public int Count => this.Vectors.Count;

            public string GetKey() => this.Vectors.GetKey();
            
            public WinLoseDrawType GetResult(int hand) => this.Vectors[hand];
        }

        public IWalkGamePlayer GetPlayer() => new ManualPlayer(this[0], this.Mode);

        public IEnumerable<IWinLoseDrawRound> ToEnumerable() => this[0].WrapInEnumerable();
    }
}