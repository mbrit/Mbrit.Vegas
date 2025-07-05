using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mbrit.Vegas.Utility
{
    internal class RandomWalkStrategy : WalkStrategy
    {
        private decimal HouseEdge { get; set; }

        internal RandomWalkStrategy(decimal houseEdge = 0.015M)
        {
            if(houseEdge >= 0.08M)
                throw new ArgumentOutOfRangeException(nameof(houseEdge));

            this.HouseEdge = houseEdge;
        }

        internal override WinLoseDrawType GetWin(Random rand)
        {
            //var result = rand.Next(1000) <= 485;
            var win = 500 - (int)(this.HouseEdge * 1000);
            var result = rand.Next(1000) <= win;
            if (result)
                return WinLoseDrawType.Win;
            else
                return WinLoseDrawType.Lose;
        }
    }
}
