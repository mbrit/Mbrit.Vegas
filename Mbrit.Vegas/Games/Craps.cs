using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Games
{
    public class Craps
    {
        public static GameOdds Place4Or10 = new GameOdds("Place4Or10", 9, 5);
        public static GameOdds Place5Or9 = new GameOdds("Place5Or9", 7, 5);
        public static GameOdds Place6Or8 = new GameOdds("Place6Or8", 7, 6);
    }
}
