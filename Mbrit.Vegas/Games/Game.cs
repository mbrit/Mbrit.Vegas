using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Games
{
    public abstract class Game
    {
        //public decimal WinIncludingBonusPercentage { get; }
        //public decimal BonusWinPercentage { get; }
        public IEnumerable<GameWin> Wins { get; }
        public decimal LosePercentage { get; }
        public decimal DrawPercentage { get; }

        public decimal StandardDeviation { get; }

        public int DecisionsPerHour { get; }

        //protected Game(decimal winIncludingBonusPercentage, decimal bonusWinPercentage, decimal losePercentage, decimal drawPercentage, 
        //    decimal standardDeviation, int handsPerHour)
        protected Game(IEnumerable<GameWin> wins, decimal losePercentage, decimal drawPercentage,
            decimal standardDeviation, int decisionsPerHour)
        {
            //this.WinIncludingBonusPercentage = winIncludingBonusPercentage;
            //this.BonusWinPercentage = bonusWinPercentage;

            if (losePercentage < 0 || losePercentage > 1)
                throw new ArgumentException("Lose percentage must be expressed as a decimal.");
            if (drawPercentage < 0 || drawPercentage > 1)
                throw new ArgumentException("Draw percentage must be expressed as a decimal.");

            if (wins.Sum(v => v.Percentage) != 1)
                throw new InvalidOperationException("The wins percentages must add exactly to 1.");

            this.Wins = new List<GameWin>(wins);

            this.LosePercentage = losePercentage;
            this.DrawPercentage = drawPercentage;
            this.StandardDeviation = standardDeviation;
            this.DecisionsPerHour = decisionsPerHour;
        }

        public decimal WinPercentage => 1 - this.LosePercentage - this.DrawPercentage;
    }
}
