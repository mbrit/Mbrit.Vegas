using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    internal class WinLoseDrawRoundsBucket : RoundsBucket<WinLoseDrawType>
    {
        private WinLoseDrawRoundsBucket(int numRounds, int handsPerRound, Func<WinLoseDrawType> callback, Random rand)
            : base(numRounds, handsPerRound, callback, rand)
        {
        }

        internal static WinLoseDrawRoundsBucket GetWinLose(int numRounds, int handsPerRound, decimal houseEdge, Random rand)
        {
            var strategy = new RandomWalkStrategy(houseEdge);

            return new WinLoseDrawRoundsBucket(numRounds, handsPerRound, () =>
            {
                return strategy.GetWin(rand);

            }, rand);
        }
    }
}
