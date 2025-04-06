using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Games
{
    public class Blackjack32 : Game
    {
        //private const decimal TheWinIncludingBonusPercentage = 42.5M;
        //private const decimal TheBonusWinPercentage = 4.75M;
        private const decimal TheLosePercentage = .49M;
        private const decimal TheDrawPercentage = .085M;
        private static decimal TheWinPercentage = 1 - TheLosePercentage - TheDrawPercentage;

        private const decimal TheStandardDeviation = 1.15M;

        private const int TheHandsPerHour = 70;

        private static decimal BlackjackWinPercentage => 0.11M;  // the player's wins with blackjack, not overall...

        public Blackjack32()
            : base(new List<GameWin>()
            {
                new GameWin(new GameOdds("Win", 1, 1), 1 - BlackjackWinPercentage),
                new GameWin(new GameOdds("Blackjack", 3, 2), BlackjackWinPercentage)

            }, TheLosePercentage, TheDrawPercentage, TheStandardDeviation, TheHandsPerHour)
        {
        }
    }
}
