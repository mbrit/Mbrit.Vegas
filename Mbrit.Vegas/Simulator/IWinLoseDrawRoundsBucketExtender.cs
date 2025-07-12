using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Simulator
{
    public static class IWinLoseDrawRoundsBucketExtender
    {
        public static IWinLoseDrawRoundsBucket WrapSingleton(this IWinLoseDrawRound round, decimal houseEdge) => new SingletonWrapper(round, houseEdge);

        private class SingletonWrapper : IWinLoseDrawRoundsBucket
        {
            private IWinLoseDrawRound Round { get; }
            public decimal HouseEdge { get; set; }

            internal SingletonWrapper(IWinLoseDrawRound round, decimal houseEdge)
            {
                this.Round = round;
                this.HouseEdge = houseEdge;
            }

            public int Count => 1;

            public IWinLoseDrawRound this[int index]
            {
                get
                {
                    if (index == 0)
                        return this.Round;
                    else
                        throw new NotSupportedException($"Cannot handle '{index}'.");
                }
            }
        }
    }
}
