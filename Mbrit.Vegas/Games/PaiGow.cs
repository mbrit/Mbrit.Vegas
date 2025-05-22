using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Games
{
    public class PaiGow : Game
    {
        private const decimal TheLosePercentage = .27M;
        private const decimal TheDrawPercentage = .49M;

        private const decimal TheStandardDeviation = .61M;

        private const int TheHandsPerHour = 45;

        //private static decimal BlackjackWinPercentage => 0.11M;  // the player's wins with blackjack, not overall...

        public PaiGow()
            : base(new List<GameWin>()
            {
                new GameWin(new GameOdds("Win", 1, 1), 1),
                //new GameWin(new GameOdds("Blackjack", 3, 2), BlackjackWinPercentage)

            }, TheLosePercentage, TheDrawPercentage, TheStandardDeviation, TheHandsPerHour)
        {
        }
    }
}
