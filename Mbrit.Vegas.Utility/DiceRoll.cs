using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    internal class DiceRoll
    {
        internal IEnumerable<int> Dies { get; }
        internal int Score { get; }

        internal DiceRoll(IEnumerable<int> dies, int score)
        {
            this.Dies = new List<int>(dies);
            this.Score = score;
        }
    }
}
