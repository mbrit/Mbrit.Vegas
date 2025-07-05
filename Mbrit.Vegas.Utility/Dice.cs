using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    internal class Dice
    {
        private int NumDice { get; }

        internal Dice(int numDice)
        {
            this.NumDice = numDice;
        }

        internal DiceRoll Roll(Random rand)
        {
            var score = 0;

            var per = new List<int>();
            for(var index = 0; index < this.NumDice; index++)
            {
                var die = rand.Next(1, 7);

                per.Add(die);
                score += die;
            }

            return new DiceRoll(per, score);
        }
    }
}
