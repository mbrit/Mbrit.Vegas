using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Games
{
    public class GameWin
    {
        public GameOdds Odds { get; }
        public decimal Percentage { get; }

        internal GameWin(GameOdds odds, decimal percentage)
        {
            if (percentage < 0 || percentage > 1)
                throw new ArgumentException("Percentage must be expressed as a decimal.");

            this.Odds = odds;
            this.Percentage = percentage;
        }

        public decimal GetWin(decimal bet) => this.Odds.GetWin(bet);

        public string Name => this.Odds.Name;
    }
}
